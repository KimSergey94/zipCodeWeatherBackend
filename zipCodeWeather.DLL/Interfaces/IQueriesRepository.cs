using System;
using System.Collections.Generic;
using System.Text;
using zipCodeWeather.DLL.Models;


namespace zipCodeWeather.DLL.Interfaces
{
    public interface IQueriesRepository
    {
        List<Query> GetAllQueries();
        void SaveQuery(Query query);

    }
}
