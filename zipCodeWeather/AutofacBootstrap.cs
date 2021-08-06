using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zipCodeWeather.DLL.Interfaces;
using zipCodeWeather.Repos;

namespace zipCodeWeather
{
    public class AutofacBootstrap
    {
        internal static void Init(ContainerBuilder builder)
        {
            builder.RegisterInstance(new QueriesRepository()).As<IQueriesRepository>();
        }
    }
}