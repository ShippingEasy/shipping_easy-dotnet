using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingEasy
{
    public class Client
    {
        public string Greet()
        {
            var url = "172.16.65.1:5000/api/orders?api_key=f9a7c8ebdfd34beaf260d9b0296c7059&api_timestamp=1421359500&api_signature=28ecb63dc43dc1246dabacf517c758152c5131f5ab3dda75b0cce6e61ba5d7d9";
            var client = new System.Net.WebClient();
            var query = "?api_key=f9a7c8ebdfd34beaf260d9b0296c7059&api_timestamp=1421362783&api_signature=325faf574c1c767cb0628968c12543de2f2c27488afab05962abd98b91849eb8";
            var uri = new UriBuilder("http", "172.16.65.1", 5000, "/api/orders",query).ToString();
            var response = client.DownloadString(uri);
            return response;
        }
    }
}
