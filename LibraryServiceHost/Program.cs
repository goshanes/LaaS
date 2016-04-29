using ServiceAssemblyBuilder;
using System;
using System.Linq;
using System.ServiceModel;

namespace LibraryServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceType = BuildMathService();
            
            using (ServiceHost host = new ServiceHost(serviceType))
            {
                host.Open();

                Console.WriteLine("The service is available at");
                Console.WriteLine(string.Join(Environment.NewLine, host.BaseAddresses.Select(ba => ba.AbsoluteUri)));
                Console.WriteLine("Press Enter to quit.");

                Console.ReadLine();

                host.Close();
            }
        }

        /// <summary>
        /// Builds sample service proxy for class System.Math
        /// </summary>
        /// <returns>Service type.</returns>
        static Type BuildMathService()
        {
            return ServiceBuilder.CreateFromType(typeof(Math));
        }

        /// <summary>
        /// Builds sample service proxy for class System.String
        /// </summary>
        /// <returns>Service type.</returns>
        static Type BuildStringService()
        {
            return ServiceBuilder.CreateFromType(typeof(string));
        }
    }
}
