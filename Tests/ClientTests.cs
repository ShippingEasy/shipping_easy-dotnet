using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
          ""quantity"": 7,
          ""product_options"": {
            ""color"": ""blue"",
            ""size"": ""3""
          }
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
            var lineItem = new LineItem
            {
                ItemName = "Sprocket",
                Quantity = 7,
                ProductOptions = { {"color", "blue"}, {"size", "3"}}
            };
            recipient.LineItems.Add(lineItem);
            order.Recipients.Add(recipient);

            var json = Client.OrderToJson(order);
            Assert.AreEqual(OrderJson, json);
        }

        [Test]
        public void GetOrdersReturnsAListOfOrders()
        {
            var client = new Client(ResponseFromFile("get_orders_success"));

            var response = client.GetOrders();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Orders.Count);
            Assert.AreEqual("ABC-123", response.Orders[0].OrderIdentifier);
            Assert.AreEqual("ABC-789", response.Orders[1].OrderIdentifier);
        }

        [Test]
        public void GetOrdersReturnsCountAndPagingDetails()
        {
            var client = new Client(ResponseFromFile("get_orders_success"));

            var response = client.GetOrders();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(3, response.Meta.CurrentPage);
            Assert.AreEqual(2, response.Meta.PreviousPage);
            Assert.AreEqual(4, response.Meta.NextPage);
            Assert.AreEqual(5, response.Meta.TotalPages);
            Assert.AreEqual(9, response.Meta.TotalCount);
            Assert.AreEqual(DateTimeOffset.Parse("01/26/2015 15:44:34 -06:00"), response.Meta.LastUpdated);
        }

        [Test]
        public void GetOrdersReturnsEmptyResult()
        {
            var client = new Client(ResponseFromFile("get_orders_none_returned"));

            var response = client.GetOrders();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Meta.CurrentPage);
            Assert.AreEqual(null, response.Meta.PreviousPage);
            Assert.AreEqual(null, response.Meta.NextPage);
            Assert.AreEqual(0, response.Meta.TotalPages);
            Assert.AreEqual(0, response.Meta.TotalCount);
            Assert.AreEqual(DateTimeOffset.Parse("01/20/2015 21:59:50 +00:00"), response.Meta.LastUpdated);
        }

        [Test]
        public void GetOrdersAuthenticationError()
        {
            var client = new Client(ResponseFromFile("get_orders_fail_authentication"));

            var response = client.GetOrders();
            Assert.IsFalse(response.Success);
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual(401, response.Errors[0].Status);
            Assert.AreEqual("Access denied.", response.Errors[0].Message);
        }

        [Test, Ignore("Need to normalize the errors node in response")]
        public void CreateOrderWithValidationError()
        {
            var client = new Client(ResponseFromFile("create_order_fail_validation"));

            var response = client.GetOrders();
            Assert.IsFalse(response.Success);
            Assert.AreEqual("foo", response.Errors);
        }

        private static Connection ResponseFromFile(string filename)
        {
            var contents = Fixture.Json(filename);
            return FakeConnection(returns => new HttpResponse{Body = contents});
        }

        private static Connection FakeConnection(Func<WebRequest, HttpResponse> response)
        {
            return new Connection("fakeKey", "fakeSecret") {RequestRunner = response};
        }
    }
}