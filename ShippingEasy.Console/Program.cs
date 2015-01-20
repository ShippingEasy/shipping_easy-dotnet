using System;
using System.Linq;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = (args.FirstOrDefault() ?? "ORDERS").ToUpper();
            try
            {
                var client = new Client();
                string response;
                switch (command)
                {
                    case "ORDERS":
                        response = client.GetOrders();
                        break;
                    case "STORE_ORDERS":
                        response = client.GetStoreOrders();
                        break;
                    case "CREATE_ORDER":
                        response = client.CreateOrder();
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command: " + command);
                }
                System.Console.WriteLine(response);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            
        }
    }
}
