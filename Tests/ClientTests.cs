using System;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void CreateInstanceWithValidUrl()
        {
            Assert.DoesNotThrow(() =>
            {
                var client = new Client("apiKey", "apiSecret", "https://api.example.com:8080");
                Assert.IsInstanceOf<Client>(client);
            });
        }

        [Test]
        public void CreateInstanceWithInvalidUrl()
        {
            Assert.Catch<UriFormatException>(() =>
            {
                var client = new Client("apiKey", "apiSecret", "badexample");
                Assert.IsInstanceOf<Client>(client);
            });
        }
    }
}