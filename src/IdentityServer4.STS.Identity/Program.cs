﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using IdentityServer4.STS.Identity.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.STS.Identity
{
    public class Program
    {
        private const string SeedArgs = "/seed";

        public static async Task Main(string[] args)
        {
            var seed = args.Any(x => x == SeedArgs);
            if (seed) args = args.Except(new[] { SeedArgs }).ToArray();

            var host = BuildWebHost(args);

            // Uncomment this to seed upon startup, alternatively pass in `dotnet run /seed` to seed using CLI
            // await DbMigrationHelpers.EnsureSeedData<IdentityServerConfigurationDbContext, AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, UserIdentity, UserIdentityRole>(host);
            if (seed)
            {
                await DbMigrationHelpers.EnsureSeedData<IdentityServerConfigurationDbContext, AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, UserIdentity, UserIdentityRole>(host);
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel(c => c.AddServerHeader = false)
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .Build();
    }    
}
