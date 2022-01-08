using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjsClassLibrary.DTO;

namespace ObjsClassLibrary.DO
{
    public sealed class UserDO : IMap<UserDTO>
    {
        public string UserName { get; set; }
    }
}
