using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace DI.AutofacModule
{
    public class ControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册所有Controller
            var controllersTypesInAssembly = 
                typeof(Startup).Assembly.GetExportedTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();

            builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
        }
    }
}
