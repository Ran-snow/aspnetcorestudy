using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Models
{
    /// <summary>
    /// 自定义用户
    /// </summary>
    public class SDUserModel: IdentityUser
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [PersonalData]
        public string Nick { get; set; }
    }
}
