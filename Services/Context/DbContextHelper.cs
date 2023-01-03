using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Context
{
    public static class DbContextHelper
    {
        public static DbContextOptions<RegisterContext> GetDbContextOptions()
        {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            return new DbContextOptionsBuilder<RegisterContext>()
                  .UseSqlServer(new SqlConnection(configuration.GetConnectionString("local"))).Options;

        }
    }
}
