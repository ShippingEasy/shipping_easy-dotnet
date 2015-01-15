using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ShippingEasy.Client();
            System.Console.WriteLine(client.Greet());
        }
    }
}
