using System;
using System.Collections.Generic;

namespace ShippingEasy
{
    public class OrderQuery
    {
        public string Status { get; set; }
        public int? Page { get; set; }
        public int? ResultsPerPage { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public string StoreKey { get; set; }

        public IDictionary<string, string> ToDictionary()
        {
            var options = new Dictionary<string,string>();
            if (Status != null)
            {
                options.Add("status", Status);
            }
            if (Page.HasValue)
            {
                options.Add("page", Page.ToString());
            }
            if (ResultsPerPage.HasValue)
            {
                options.Add("per_page", ResultsPerPage.ToString());
            }
            if (LastUpdated.HasValue)
            {
                options.Add("last_updated_at", LastUpdated.Value.ToString("O"));
            }
            return options;
        }
    }
}