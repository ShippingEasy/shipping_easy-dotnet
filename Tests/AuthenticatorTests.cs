using System;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class AuthenticatorTests
    {
        private string _apiSecret;
        private string _signature;
        private string _queryString;
        private string _requestBody;
        private string _path;
        private string _httpMethod;
        private Authenticator _authenticator;

        [SetUp]
        public void Setup()
        {
            _apiSecret = "a54b67c89";
            _signature = "7b3a2231201b361caa294ce482fe87e536c7498d87ddb3e09c3626096ca83cf7";
            // querystring = Request.QueryString.ToString(); or Request.QueryString.ToString();
            _queryString = string.Format("api_signature={0}&color=blue", _signature);
            // body = new System.IO.StreamReader(Request.InputStream).ReadToEnd(); 
            _requestBody = @"{""success"": true}";
            _path = "/shipments/callback";
            _httpMethod = "POST";
            _authenticator = new Authenticator(_apiSecret);
        }

        [Test]
        public void SuccessfulIfSignatureCanBeVerified()
        {
            Assert.IsTrue(_authenticator.Verify(_httpMethod, _path, _queryString, _requestBody));
        }

        [Test]
        public void FailsIfSignatureDoesNotMatch()
        {
            _requestBody = @"{""different"": ""contents""}";
            Assert.IsFalse(_authenticator.Verify(_httpMethod, _path, _queryString, _requestBody));
        }

        [Test]
        public void ThrowsIfSignatureNotProvided()
        {
            _queryString = "color=blue&size=2"; // no api_signature
            Assert.Throws<ArgumentException>(() =>
            {
                _authenticator.Verify(_httpMethod, _path, _queryString, _requestBody);
            });
        }
    }
}