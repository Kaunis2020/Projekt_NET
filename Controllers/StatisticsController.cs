using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("api/")]
    public class StatisticsController : ControllerBase
    {
        private MediaManager mediamanager = null;

        public StatisticsController(CompanyDatabasContext _con)
        {
            mediamanager = new MediaManager(_con);
        }

        /// <summary>
        /// API-Controller, som skickar alla data som JSON;
        /// Data fångas upp av JQuery och visas i tabeller;
        /// </summary>
        /// <returns>JSON</returns>
        [HttpGet]
        [Route("media/slag/0")]
        public async Task<Object> GetMedia()
        {
            var data = await mediamanager.MediaStatistics();
            return data;
        }

        [HttpGet]
        [Route("media/slag/1")]
        public async Task<Object> BookStatistics()
        {
            var data = await mediamanager.BookStatistics();
            return data;
        }

        [HttpGet]
        [Route("media/slag/2")]
        public async Task<Object> CDStatistics()
        {
            var data = await mediamanager.CDStatistics();
            return data;
        }

        [HttpGet]
        [Route("media/slag/3")]
        public async Task<Object> DVDStatistics()
        {
            var data = await mediamanager.DVDStatistics();
            return data;
        }
    }
}
