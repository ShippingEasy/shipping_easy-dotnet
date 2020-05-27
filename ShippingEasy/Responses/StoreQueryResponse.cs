using System.Collections.Generic;

namespace ShippingEasy.Responses
{
    public class StoreQueryResponse : ApiResponse
    {
        private readonly IList<Store> _stores = new List<Store>();

        public IList<Store> Stores
        {
            get { return _stores; }
        }
    }
}
