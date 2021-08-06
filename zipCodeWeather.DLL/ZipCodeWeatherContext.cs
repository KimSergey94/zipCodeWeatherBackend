using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using zipCodeWeather.DLL.Models;

namespace zipCodeWeather.DLL
{
    public class ZipCodeWeatherContext : DbContext
    {
        public ZipCodeWeatherContext()
        {
            this.Database.CreateIfNotExists();
            Database.SetInitializer<ZipCodeWeatherContext>(new CreateDatabaseIfNotExists<ZipCodeWeatherContext>());
        }

        public DbSet<Query> Queries { get; set; }
    }
}
