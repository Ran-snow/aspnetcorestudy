﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPIHttp.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 接收报文测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetJson([FromBody]JObject value)
        //public object GetJson([FromBody]dynamic value)
        //public object GetJson(user value)
        {
            object json = "{\"hello\":\"test\"}";

            //if (string.IsNullOrEmpty(value))
            //{
            //    return "{\"失败\":\"失败\"}";
            //}

            var http = HttpContext;

            JsonSerializerSettings js = new JsonSerializerSettings();
            js.ObjectCreationHandling = ObjectCreationHandling.Replace;
            js.Formatting = Formatting.Indented;

            return json;
        }
    }
}