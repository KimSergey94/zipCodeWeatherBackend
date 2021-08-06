using System;
using System.Collections.Generic;
using System.Text;
using zipCodeWeather.DLL.Models;

namespace zipCodeWeather.DLL
{
    public class ZipCodeWeatherContext : System.Data.Entity.DbContext
    {
        public ZipCodeWeatherContext()
        {
            this.Database.CreateIfNotExists();
            Database.SetInitializer<DoczContext>(new CreateDatabaseIfNotExists<DoczContext>());
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += FixDates;
        }

        public DbSet<Query> Queries { get; set; }

    }
}
