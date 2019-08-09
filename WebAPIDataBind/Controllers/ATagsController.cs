using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPIDataBind.Model;

namespace WebAPIDataBind.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ATagsController : ControllerBase
    {
        [HttpPost]
        [HttpGet]
        public object FromSimple(string name)
        {
            return "res "+name;
        }

        [HttpPost]
        [HttpGet]
        public object FromSimpleFrom([FromForm]string name)
        {
            return "res "+name;
        }

        [HttpPost]
        [HttpGet]
        public object RawSimple(string name)
        {
            return "res "+name;
        }

        [HttpPost]
        public object RawSimpleFromBody([FromBody]string name)
        {
            return "res "+name;
        }

        [HttpGet]
        public object RawSimpleFromQuery([FromBody]string name)
        {
            return "res "+name;
        }

        //--------------

        [HttpPost]
        [HttpGet]
        public object FromComplex(User user)
        {
            return "res "+user;
        }

        [HttpPost]
        [HttpGet]
        public object FromComplexFrom([FromForm]User user)
        {
            return "res "+user;
        }

        [HttpPost]
        [HttpGet]
        public object RawComplex(User user)
        {
            return "res "+user;
        }

        [HttpPost]
        public object RawComplexFromBody([FromBody]User user)
        {
            return "res "+user;
        }

        [HttpGet]
        public object RawComplexFromQuery([FromQuery]User user)
        {
            return "res "+user;
        }
    }
}
