using KolumbusRouteApi;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");



            RouteApi api = new RouteApi();

            api.GetVehicles();


        }
    }
}