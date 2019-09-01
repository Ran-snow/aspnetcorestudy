using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;

namespace DI.AutofacModule
{
    public class BLLModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var assemblys = AppDomain.CurrentDomain.BaseDirectory;
            //DirectoryInfo theFolder = new DirectoryInfo(assemblys);
            //foreach (var item in theFolder.GetFiles("*.dll"))
            //{
               
            //}

            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(t => t.Name.EndsWith("BLL"))
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }
    }
}
