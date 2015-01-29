using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using ShippingEasy;
using ShippingEasy.Responses;

namespace Tests
{
    [TestFixture]
    public class CreateOrderResponseTests
    {
        [Test]
        public void DeserializeTheCreatedOrder()
        {
            var body = Fixture.Json("create_order_response");
            var response = new HttpResponse {Body = body};
            var createOrderResponse = new ResponseHandler().Build<CreateOrderResponse>(response);
            var order = createOrderResponse.Order;
            Assert.AreEqual("ABC-6355", order.OrderIdentifier);
            var date = new DateTimeOffset(2015, 1, 23, 20, 13, 32, new TimeSpan());
            Assert.AreEqual(date, order.OrderedAt);
            Assert.AreEqual("ready_for_shipment", order.Status);
            Assert.AreEqual(0.01m, order.TotalIncludingTax);
            Assert.AreEqual(0.02m, order.TotalExcludingTax);
            Assert.AreEqual(0.03m, order.DiscountAmount);
            Assert.AreEqual(0.04m, order.CouponDiscount);
            Assert.AreEqual(0.05m, order.SubtotalIncludingTax);
            Assert.AreEqual(0.06m, order.SubtotalExcludingTax);
            Assert.AreEqual(0.07m, order.SubtotalTax);
            Assert.AreEqual(0.08m, order.TotalTax);
            Assert.AreEqual(0.09m, order.BaseShippingCost);
            Assert.AreEqual(0.10m, order.ShippingCostIncludingTax);
            Assert.AreEqual(0.11m, order.ShippingCostExcludingTax);
            Assert.AreEqual(0.12m, order.ShippingCostTax);
            Assert.AreEqual(0.13m, order.BaseHandlingCost);
            Assert.AreEqual(0.14m, order.HandlingCostExcludingTax);
            Assert.AreEqual(0.15m, order.HandlingCostIncludingTax);
            Assert.AreEqual(0.16m, order.HandlingCostTax);
            Assert.AreEqual(0.17m, order.BaseWrappingCost);
            Assert.AreEqual(0.18m, order.WrappingCostExcludingTax);
            Assert.AreEqual(0.19m, order.WrappingCostIncludingTax);
            Assert.AreEqual(0.20m, order.WrappingCostTax);
            Assert.AreEqual("The Bills", order.BillingCompany);
            Assert.AreEqual("Jim", order.BillingFirstName);
            Assert.AreEqual("Kelly", order.BillingLastName);
            Assert.AreEqual("100 Cold Lake Way", order.BillingAddress);
            Assert.AreEqual("Suite #0", order.BillingAddress2);
            Assert.AreEqual("Buffalo", order.BillingCity);
            Assert.AreEqual("NY", order.BillingState);
            Assert.AreEqual("USA", order.BillingCountry);
            Assert.AreEqual("19901", order.BillingPostalCode);
            Assert.AreEqual("2124443333", order.BillingPhoneNumber);
            Assert.AreEqual("bills@example.com", order.BillingEmail);
            Assert.AreEqual("c71dc6da574eea04e2c926906bcb4eab", order.StoreApiKey);
            var updated = new DateTimeOffset(2015, 1, 23, 20, 13, 33, new TimeSpan());
            Assert.AreEqual(updated, order.UpdatedAt);


            Assert.AreEqual(1, order.Recipients.Count);
            var recipient = order.Recipients[0];

            Assert.AreEqual("Colin", recipient.FirstName);
            Assert.AreEqual("Smith", recipient.LastName);
            Assert.AreEqual("1600 Pennsylvania Ave", recipient.Address);
            Assert.AreEqual("Suite # 2", recipient.Address2);
            Assert.AreEqual("c/o White House", recipient.Address3);
            Assert.AreEqual("Washington", recipient.City);
            Assert.AreEqual("DC", recipient.State);
            Assert.AreEqual(true, recipient.Residential);
            Assert.AreEqual("Province", recipient.Province);
            Assert.AreEqual("United States", recipient.Country);
            Assert.AreEqual("12345", recipient.PostalCode);
            Assert.AreEqual("5250", recipient.PostalCodePlus4);
            Assert.AreEqual("5553339999", recipient.PhoneNumber);
            Assert.AreEqual("colin@example.com", recipient.Email);
            Assert.AreEqual(0.00m, recipient.BaseCost);
            Assert.AreEqual(1.00m, recipient.CostExcludingTax);
            Assert.AreEqual(2.00m, recipient.CostIncludingTax);
            Assert.AreEqual(3.00m, recipient.CostTax);
            Assert.AreEqual(4.00m, recipient.BaseHandlingCost);
            Assert.AreEqual(5.00m, recipient.HandlingCostExcludingTax);
            Assert.AreEqual(6.00m, recipient.HandlingCostIncludingTax);
            Assert.AreEqual(7.00m, recipient.HandlingCostTax);
            Assert.AreEqual(4, recipient.ShippingZoneId);
            Assert.AreEqual("North", recipient.ShippingZoneName);
            Assert.AreEqual(88, recipient.ItemsTotal);
            Assert.AreEqual("Fast", recipient.ShippingMethod);
            Assert.AreEqual(66, recipient.ItemsShipped);
            Assert.AreEqual(33, recipient.ExtShippingDetailId);


            Assert.AreEqual(2, recipient.LineItems.Count);

            var line1 = recipient.LineItems[0];
            Assert.AreEqual("Sprocket", line1.ItemName);
            Assert.AreEqual(null, line1.Sku);
            Assert.AreEqual("W4", line1.BinPickingNumber);
            Assert.AreEqual(16.5m, line1.WeightInOunces);
            Assert.AreEqual(7, line1.Quantity);
            Assert.AreEqual(1.01m, line1.TotalExcludingTax);
            Assert.AreEqual(1.02m, line1.PriceExcludingTax);
            Assert.AreEqual(1.03m, line1.UnitPrice);
            Assert.AreEqual("998", line1.ExtLineItemId);
            Assert.AreEqual("887", line1.ExtProductId);
            Assert.AreEqual(0, line1.ProductOptions.Count);
            var line2 = recipient.LineItems[1];
            Assert.AreEqual("Widget", line2.ItemName);
            Assert.AreEqual("AB-321", line2.Sku);
            Assert.AreEqual(null, line2.BinPickingNumber);
            Assert.AreEqual(4.01m, line2.TotalExcludingTax);
            Assert.AreEqual(3, line2.ProductOptions.Count);
            Assert.AreEqual("blue", line2.ProductOptions["color"]);
            Assert.AreEqual("2", line2.ProductOptions["size"]);
            Assert.AreEqual("True", line2.ProductOptions["wide"]);
        }
    }
}