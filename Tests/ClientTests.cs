using System;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ClientTests
    {
        private const string OrderJson = @"{
  ""external_order_identifier"": ""ABC-100"",
  ""ordered_at"": ""2014-01-16T14:37:56-06:00"",
  ""recipients"": [
    {
      ""first_name"": ""Colin"",
      ""last_name"": ""Smith"",
      ""address"": ""1600 Pennsylvania Ave"",
      ""line_items"": [
        {
          ""item_name"": ""Sprocket"",
          ""quantity"": 7
        }
      ]
    }
  ]
}";

        [Test]
        public void CreateJsonFromOrder()
        {
            var order = new Order
            {
                OrderIdentifier = "ABC-100",
                OrderedAt = new DateTimeOffset(2014, 1, 16, 14, 37, 56, new TimeSpan(-6, 0, 0))
            };
            var recipient = new Recipient {FirstName = "Colin", LastName = "Smith", Address = "1600 Pennsylvania Ave"};
            var lineItem = new LineItem {ItemName = "Sprocket", Quantity = 7};
            recipient.LineItems.Add(lineItem);
            order.Recipients.Add(recipient);

            var json = Client.OrderToJson(order);
            Assert.AreEqual(OrderJson, json);
        }
    }
}