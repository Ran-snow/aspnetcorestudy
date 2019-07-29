using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class HTTPController : Controller
    {
        private IHttpContextAccessor _accessor;

        public HTTPController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            var sss = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return View();
        }
    }
}