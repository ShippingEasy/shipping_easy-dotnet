using System;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = new Client();
                System.Console.WriteLine(client.GetOrders()     );
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            
        }
    }
}
