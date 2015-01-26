using NUnit.Framework;
using ShippingEasy;
using System;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class CreateOrderResponseTests
    {
        [Test]
        public void DeserializeTheCreatedOrder()
        {
            var response = File.ReadAllText(@"fixtures\create_order_response.json");

            var createOrderResponse = new ResponseParser().Parse<CreateOrderResponse>(response);
            var order = createOrderResponse.Order;
            Assert.AreEqual("ABC-6355", order.OrderIdentifier);
            var date = new DateTimeOffset(2015, 1, 23, 20, 13, 32, new TimeSpan());
            Assert.AreEqual(date, order.OrderedAt);

            Assert.AreEqual(1, order.Recipients.Count);
            var recipient = order.Recipients[0];

            Assert.AreEqual("Colin", recipient.FirstName);
            Assert.AreEqual("Smith", recipient.LastName);
            Assert.AreEqual(1, recipient.LineItems.Count);
            var lineItem = recipient.LineItems[0];

            Assert.AreEqual("Sprocket", lineItem.ItemName);
            Assert.AreEqual(7, lineItem.Quantity);
        }
    }
}