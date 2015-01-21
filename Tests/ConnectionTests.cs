using System;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ConnectionTests
    {
        [Test]
        public void CreateInstanceWithValidUrl()
        {
            Assert.DoesNotThrow(() =>
            {
                var client = new Connection("apiKey", "apiSecret", "https://api.example.com:8080");
                Assert.IsInstanceOf<Connection>(client);
            });
        }

        [Test]
        public void CreateInstanceWithInvalidUrl()
        {
            Assert.Catch<UriFormatException>(() =>
            {
                var client = new Connection("apiKey", "apiSecret", "badexample");
                Assert.IsInstanceOf<Connection>(client);
            });
        }
    }
}