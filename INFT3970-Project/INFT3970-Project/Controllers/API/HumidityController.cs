﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using INFT3970Project.Helpers;
using INFT3970Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace INFT3970Project.Controllers
{
    [Produces("application/json")]
    [Route("api/Humidity")]
    public class HumidityController : Controller
    {
        private readonly IConfiguration configuration;
        private DatabaseHelper _databaseHelper;

        public HumidityController(IConfiguration configuration)
        {
            this.configuration = configuration;
            _databaseHelper = new DatabaseHelper(configuration);
        }

        /// <summary>
        /// Implementation of method to create a record async via post request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Create([FromBody] HumidityModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            try
            {
                model.Timestamp = DateTime.Now;
                _databaseHelper.CreateRecordAsync(model);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            return response;
        }
    }
}