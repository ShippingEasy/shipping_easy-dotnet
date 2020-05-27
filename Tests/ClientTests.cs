using System;
using System.Net;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void GetOrdersReturnsAListOfOrders()
        {
            var client = new Client(ResponseFromFile("get_orders_success"));

            var response = client.GetOrders();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Orders.Count);
            Assert.AreEqual("ABC-123", response.Orders[0].ExternalOrderIdentifier);
            Assert.AreEqual("ABC-789", response.Orders[1].ExternalOrderIdentifier);
        }

        [Test]
        public void GetOrdersReturnsAListOfOrdersShipments()
        {
            var client = new Client(ResponseFromFile("get_orders_success_shipment"));

            var response = client.GetOrders();
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Orders.Count);
            Assert.AreEqual("8888888888888888888888", response.Orders[0].Shipments[0].TrackingNumber);
            Assert.AreEqual("internal order notes", response.Orders[0].Recipients[0].InternalNotes);
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
            var client = new Client(ResponseFromFile("fail_authentication"));

            var response = client.GetOrders();
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("Access denied"));
        }

        [Test]
        public void GetOrdersReturnsInvalidJson()
        {
            var client = new Client(ResponseFromFile("invalid_json_response"));

            var response = client.GetOrders();
            Assert.IsFalse(response.Success);
            Assert.That(response.HttpResponse.Body, Is.StringContaining("Internal Server Error"));
        }

        [Test]
        public void CreateOrderSuccess()
        {
            var client = new Client(ResponseFromFile("create_order_response"));

            var response = client.CreateOrder("abc", new Order());
            Assert.IsTrue(response.Success);
            Assert.AreEqual(DateTimeOffset.Parse("2015-01-23T20:13:32Z"), response.Order.OrderedAt);
        }

        [Test]
        public void CreateOrderWithValidationError()
        {
            var client = new Client(ResponseFromFile("create_order_fail_validation"));

            var response = client.CreateOrder("abc", new Order());
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("ordered_at"));
            Assert.That(response.Errors, Is.StringContaining("recipients"));
        }
        
        [Test]
        public void CreateOrderExpiredRequestSignature()
        {
            var client = new Client(ResponseFromFile("create_order_fail_expired"));

            var response = client.CreateOrder("abc", new Order());
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("The request has expired"));
        }

        [Test]
        public void CancelOrderSuccess()
        {
            var client = new Client(ResponseFromFile("cancel_order_success"));

            var response = client.CancelOrder("abc", "ABC-100");
            Assert.IsTrue(response.Success);
            Assert.AreEqual("cleared", response.Order.OrderStatus);            
        }

        [Test]
        public void CancelOrderAlreadyCancelled()
        {
            var client = new Client(ResponseFromFile("cancel_order_already_cancelled"));

            var response = client.CancelOrder("abc", "ABC-100");
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("cannot be cancelled"));
        }   

        [Test]
        public void CancelOrderUnknownOrder()
        {
            var client = new Client(ResponseFromFile("cancel_order_fail_unknown_order"));

            var response = client.CancelOrder("abc", "ABC-45");
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("Order not found"));
        }

        [Test]
        public void CancelOrderAuthenticationError()
        {
            var client = new Client(ResponseFromFile("fail_authentication"));

            var response = client.CancelOrder("abc", "ABC-45");
            Assert.IsFalse(response.Success);
            Assert.That(response.Errors, Is.StringContaining("Access denied"));
        }

        [Test]
        public void CreateJsonFromOrder()
        {
            var order = new Order
            {
                ExternalOrderIdentifier = "ABC-100",
                OrderedAt = new DateTimeOffset(2014, 1, 16, 14, 37, 56, new TimeSpan(-6, 0, 0))
            };
            var recipient = new Recipient { FirstName = "Colin", LastName = "Smith", Address = "1600 Pennsylvania Ave" };
            var lineItem = new LineItem
            {
                ItemName = "Sprocket",
                Quantity = 7,
                ProductOptions = { { "color", "blue" }, { "size", "3" } }
            };
            recipient.LineItems.Add(lineItem);
            order.Recipients.Add(recipient);

            var json = Client.OrderToJson(order);
            const string serializedOrder = @"{
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
            Assert.AreEqual(serializedOrder, json);
        }

        private static Connection ResponseFromFile(string filename)
        {
            var contents = Fixture.Json(filename);
            return FakeConnection(returns => new HttpResponse{Body = contents});
        }

        private static Connection FakeConnection(Func<WebRequest, HttpResponse> response)
        {
            var fakeConnection = new Connection("fakeKey", "fakeSecret", "http://localhost")
            {
                RequestRunner = response,
                SetRequestBody = (request, body) => {}
            };
            return fakeConnection;
        }
    }
}