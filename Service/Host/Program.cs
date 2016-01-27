using System;
using System.ServiceModel;

namespace Host
{
    class Program
    {
        private static void Main()
        {
            using (var categoryHost = new ServiceHost(typeof(CategoryController)))
            using (var todoHost = new ServiceHost(typeof(TodoController)))
            {
                categoryHost.Open();
                todoHost.Open();
                Console.Write($"Category service started at {DateTime.Now}");
                Console.ReadLine();
            }
        }
    }
}
