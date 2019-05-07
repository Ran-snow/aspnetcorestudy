using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace NewtonsoftStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string json = File.ReadAllTextAsync("appsetting.json").Result;

            JToken _ = JObject.Parse(json).GetValue("employees");

            var o = JsonConvert.DeserializeObject<StackCollection>(json);

            Console.WriteLine(o["Gates"]);

            //var config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build().GetSection("employees");
        }
    }


    public class StackCollection
    {
        [JsonProperty(PropertyName= "employees")]
        private readonly List<EmployeesItem> list = new List<EmployeesItem>();

        public string this[string index]
        {
            get
            {
                return list.Where(x=>x.firstName == index).FirstOrDefault()?.lastName;
            }
        }
    }

    public class EmployeesItem
    {
        public string firstName { get; set; }


        public string lastName { get; set; }

    }
}
