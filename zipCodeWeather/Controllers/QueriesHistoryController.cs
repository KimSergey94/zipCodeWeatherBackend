using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using zipCodeWeather.DLL.Interfaces;
using zipCodeWeather.DLL.Models;

namespace zipCodeWeather.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class QueriesHistoryController : ApiController
    {
        private readonly IQueriesRepository _queriesRepository;
        public QueriesHistoryController(IQueriesRepository queriesRepository)
        {
            _queriesRepository = queriesRepository;
        }

        // GET: api/QueriesHistory/GetAllQueriesList
        [ResponseType(typeof(List<Query>))]
        public HttpResponseMessage GetAllQueriesList()
        {
            var queries = _queriesRepository.GetAllQueries();
            var res = Request.CreateResponse(HttpStatusCode.OK, queries);
            res.Content = new StringContent(new JavaScriptSerializer().Serialize(queries), Encoding.UTF8, "application/json");
            return res; 
        }
    }
}
