using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;

namespace GeneralBusinessSystem
{
    public class Program
    {
   

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json", optional: true)
        .Build();
            //支持中文
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            BuildWebHost(args, config).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
               .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();
    }
}
