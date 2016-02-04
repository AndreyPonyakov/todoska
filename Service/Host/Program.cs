using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;

using Model.SqlCe;

namespace Host
{
    /// <summary>
    /// Class with enter point of wcf host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Enter point of wcf host
        /// </summary>
        private static void Main()
        {
            using (var categoryHost = new ServiceHost(typeof(CategoryController)))
            using (var todoHost = new ServiceHost(typeof(TodoController)))
            {
                categoryHost.Open();
                todoHost.Open();
                Console.Write($"TodoSystem service started at {DateTime.Now}");
                Console.ReadLine();
            }
        }
    }
}
