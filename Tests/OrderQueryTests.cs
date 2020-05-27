using System;
using NUnit.Framework;
using ShippingEasy;

namespace Tests
{
    [TestFixture]
    public class OrderQueryTests
    {
        [Test]
        public void BuildsDictionaryForPropertiesWithValues()
        {
            var orderQuery = new OrderQuery {Page = 2, Status = "shipped", OrderNumber = "1"};
            var options = orderQuery.ToDictionary();
            Assert.That(options.Keys, Is.EquivalentTo(new[] { "page", "status" }));
            Assert.AreEqual("2", options["page"]);
            Assert.AreEqual("1", options["order_number"]);
            Assert.AreEqual("shipped", options["status"]);
        }

        [Test]
        public void PassesDatesInIso8601Format()
        {
            var cstTimeZone = new TimeSpan(-6, 0, 0);
            var date = new DateTimeOffset(2014, 1, 22, 14, 30, 0, cstTimeZone);
            var orderQuery = new OrderQuery { ResultsPerPage = 20, LastUpdated = date };
            var options = orderQuery.ToDictionary();
            Assert.That(options.Keys, Is.EquivalentTo(new[] { "per_page", "last_updated_at" }));
            Assert.AreEqual("20", options["per_page"]);
            Assert.AreEqual("2014-01-22T14:30:00.0000000-06:00", options["last_updated_at"]);
        }
    }
}