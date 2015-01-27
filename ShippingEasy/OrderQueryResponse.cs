using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class OrderQueryResponse
    {
        private readonly IList<ApiError> _errors = new List<ApiError>();
        private readonly IList<Order> _orders = new List<Order>();

        public IList<Order> Orders { get { return _orders; } }
        public string RawJson { get; private set; }

        public bool Success
        {
            get { return Errors.Count == 0; }
        }

        [JsonProperty("meta")]
        public ApiMetaData Meta { get; private set; }

        public IList<ApiError> Errors { get { return _errors; } }
    }

    public class ApiError
    {
        [JsonProperty]
        public string Message { get; private set; }
        [JsonProperty]
        public int? Status { get; private set; }
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