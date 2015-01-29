using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShippingEasy.Responses
{
    public class OrderQueryResponse : ApiResponse
    {
        private readonly IList<Order> _orders = new List<Order>();

        public IList<Order> Orders
        {
            get { return _orders; }
        }

        [JsonProperty("meta")]
        public ApiMetaData Meta { get; private set; }
    }

    public class ApiMetaData
    {
        [JsonProperty]
        public int CurrentPage { get; private set; }
        [JsonProperty]
        public int TotalPages { get; private set; }
        [JsonProperty]
        public int TotalCount { get; private set; }
        [JsonProperty("last_updated_at")]
        public DateTimeOffset LastUpdated { get; private set; }
        [JsonProperty("prev_page")]
        public int? PreviousPage { get; private set; }
        [JsonProperty("next_page")]
        public int? NextPage { get; private set; }

    }
}