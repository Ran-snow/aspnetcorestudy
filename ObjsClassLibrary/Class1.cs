using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjsClassLibrary.DO;

namespace ObjsClassLibrary
{
    internal class Class1
    {
        internal void xxx()
        {
            UserDO userDO = new UserDO();
            userDO.MapToUserDTO();
        }
    }
}
