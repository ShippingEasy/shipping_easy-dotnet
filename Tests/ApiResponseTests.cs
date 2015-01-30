using System;
using NUnit.Framework;
using ShippingEasy.Responses;

namespace Tests
{
    [TestFixture]
    public class ApiResponseTests
    {
        [Test]
        public void ErrorsContainsStringVersionOfErrorObject()
        {
            var response = new ApiResponse {ErrorObject = new[]{1, 2, 3}};
            Assert.AreEqual("System.Int32[]", response.Errors);
        }

        [Test]
        public void SuccessIsTrueWhenNoErrorsOrParseException()
        {
            var response = new ApiResponse();
            Assert.IsTrue(response.Success);
        }

        [Test]
        public void SuccessIsFalseWhenErrors()
        {
            var response = new ApiResponse { ErrorObject = new[] { 1, 2, 3 } };
            Assert.IsFalse(response.Success);            
        }

        [Test]
        public void SuccessIsFalseWhenParseException()
        {
            var response = new ApiResponse {ParseException = new ApplicationException()};
            Assert.IsFalse(response.Success);
        }
    }
}