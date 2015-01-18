using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void SignatureWithoutBody()
        {
            var apiKey = "444888222";
            var apiSecret = "zxy98761234dcba";
            var apiTimestamp = 1421551594;
            var body = "";
            var method = "Get";
            var path = "/api/orders";

            var parameters = new Dictionary<string, string>{{"api_key", apiKey},
                {"page", "1"}, {"count", "200"},
                {"api_timestamp", apiTimestamp.ToString()}
            };
            var signature = Client.MakeSignature(apiSecret, method, path, parameters, body);
            Assert.AreEqual("0d3ff01aa8c2bd5b9d8a5148b9ac4c9acbd01c9cdd53e01d630bce6cb59a4d20", signature);

        }
    }
}
