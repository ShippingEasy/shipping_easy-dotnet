using System;
using System.Collections.Generic;

namespace ShippingEasy
{
    public class OrderQuery
    {
        /// <summary>
        /// The status of the orders you would like to return. 'shipped' or 'ready_for_shipment'
        /// <remarks>You can specify multiple states by separating with a comma. Ex: 'shipped,ready_for_shipment'</remarks>
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// The 1-based number of the page to retrieve, when the results span multiple pages
        /// </summary>
        public int? Page { get; set; }
        /// <summary>
        /// The number of orders to return in each request (page)
        /// </summary>
        public int? ResultsPerPage { get; set; }
        /// <summary>
        /// A date indicating how far back you want to retrieve orders
        /// <remarks>Only orders modified after this date will be returned.</remarks>
        /// </summary>
        public DateTimeOffset? LastUpdated { get; set; }
        /// <summary>
        /// A Store API Key if you want to limit results to orders from a single store.
        /// </summary>
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