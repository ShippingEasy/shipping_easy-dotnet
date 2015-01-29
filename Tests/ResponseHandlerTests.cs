using System.Collections.Generic;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ResponseHandlerTests
    {
        [Test]
        public void PopulatesPropertiesFromJson()
        {
            var response = new HttpResponse
            {
                Body = @"{""status"": 23, ""part"": {""name"": ""Left blade"", ""Weight"": 38}}"
            };

            var handler = new ResponseHandler();
            var parsedResponse = handler.Build<FakeResponse>(response);

            Assert.AreEqual("Left blade", parsedResponse.Part.Name);
            Assert.AreEqual(38, parsedResponse.Part.Weight);
            Assert.AreEqual(23, parsedResponse.Status);
            Assert.IsNull(parsedResponse.SomethingElse);
        }

        [Test]
        public void PopulatesCollectionPropertiesFromJson()
        {
            var response = new HttpResponse
            {
                Body = @"{""parts"": [{""name"": ""Left blade"", ""Weight"": 38},{""weight"": 16, ""name"": ""Right blade""}]}"
            };
            var handler = new ResponseHandler();
            var parsedResponse = handler.Build<FakeResponse2>(response);

            Assert.AreEqual(2, parsedResponse.Parts.Count);
            Assert.AreEqual("Left blade", parsedResponse.Parts[0].Name);
            Assert.AreEqual(38, parsedResponse.Parts[0].Weight);
            Assert.AreEqual("Right blade", parsedResponse.Parts[1].Name);
            Assert.AreEqual(16, parsedResponse.Parts[1].Weight);
        }

        [Test]
        public void PopulatesSpecialHttpResponseProperty()
        {
            var response = new HttpResponse
            {
                Status = 200,
                Body = @"{""status"": 23, ""part"": {""name"": ""Left blade"", ""Weight"": 38}}"
            };
            var handler = new ResponseHandler();
            var parsedResponse = handler.Build<FakeResponse3>(response);
            Assert.AreEqual(response, parsedResponse.HttpResponse);
        }

        public class FakeResponse
        {
            public Widget Part { get; set; }
            public string SomethingElse { get; set; }
            public int Status { get; set; }
        }

        public class FakeResponse2
        {
            public IList<Widget> Parts { get; set; }
        }

        public class FakeResponse3
        {
            public Widget Part { get; set; }
            public HttpResponse HttpResponse { get; private set; }
        }

        public class Widget
        {
            public string Name { get; set; }
            public int Weight { get; set; }
        }
    }
}