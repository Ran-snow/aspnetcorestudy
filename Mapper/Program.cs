using System;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace MapClass
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var myAssembly = typeof(Program).Assembly;

            Order order = new Order
            {
                Name = "Test"
            };

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>());
            var config = new MapperConfiguration(cfg => cfg.AddMaps(myAssembly));

            var mapper = new Mapper(config);
            OrderDto dto = mapper.Map<OrderDto>(order);

            Console.WriteLine(dto.Name666);

            Console.ReadKey();
        }
    }

    [AutoMap(typeof(Order))]
    internal class OrderDto
    {
        [SourceMember(nameof(Order.Name))]
        public string Name666 { get; set; }
    }

    internal class Order
    {
        public string Name { get; set; }
    }
}
