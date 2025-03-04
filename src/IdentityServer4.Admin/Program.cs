﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using IdentityServer4.Admin.Helpers;

namespace IdentityServer4.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .UseSerilog()
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}