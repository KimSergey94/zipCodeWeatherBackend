using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zipCodeWeather.DLL;
using zipCodeWeather.DLL.Interfaces;
using zipCodeWeather.DLL.Models;

namespace zipCodeWeather.Repos
{
    public class QueriesRepository : IQueriesRepository
    {
        public List<Query> GetAllQueries()
        {
            using (var db = new ZipCodeWeatherContext())
            {
                return db.Queries.ToList();
            }
        }


    }
}