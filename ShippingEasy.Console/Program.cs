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
                        response = client.GetAllOrders();
                        break;
                    case "STORE_ORDERS":
                        response = client.GetStoreOrders("c71dc6da574eea04e2c926906bcb4eab");
                        break;
                    case "CREATE_ORDER":
                        response = client.CreateOrder("c71dc6da574eea04e2c926906bcb4eab", new Order
                        {
                            OrderIdentifier = string.Format("ABC-{0}", DateTime.Now.Ticks),
                            OrderedAt = DateTime.Now,
                            Recipients =
                            {
                                new Recipient
                                {
                                    FirstName = "Colin",
                                    LastName = "Smith",
                                    Address = "1600 Pennsylvania Ave",
                                    Address2 = "Suite # 2",
                                    LineItems = { new LineItem { ItemName = "Sprocket", Quantity = 7 } }
                                }
                            }
                        });
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
