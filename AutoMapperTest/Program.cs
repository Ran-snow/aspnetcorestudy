using System.Diagnostics;
using AutoMapper;
using AutoMapperTest;

var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDO, UserEntity>());
var mapper = new Mapper(config);

UserDO userDO = new()
{
    UserAge = 1,
    UserName = "123",
    UserNick = "666"
};

Stopwatch stopwatch = Stopwatch.StartNew();
List<UserEntity> users = new();

for (int i = 0; i < 1_000_000; i++)
{
    users.Add(new()
    {
        UserName = userDO.UserName,
        UserAge = userDO.UserAge,
        UserNick = userDO.UserNick,
    });
}

Console.WriteLine(stopwatch.ElapsedMilliseconds);
Console.WriteLine(users.Count);
stopwatch = Stopwatch.StartNew();
users = new();

for (int i = 0; i < 1_000_000; i++)
{
    users.Add(mapper.Map<UserEntity>(userDO));
}

Console.WriteLine(stopwatch.ElapsedMilliseconds);
Console.WriteLine(users.Count);

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.ReadLine();
