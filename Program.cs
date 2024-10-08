using AppBuilderDataAPI.Controllers;

namespace AppBuilderDataAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder
                        .ConfigureServices()
                        .ConfigurePipeline();

            app.Run();
        }
    }
}
