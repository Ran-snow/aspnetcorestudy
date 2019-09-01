using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.BLL
{
    public class TestBLL : ITestBLL
    {
        public string SayHello()
        {
            return "Hello AutoFac";
        }
    }
}
