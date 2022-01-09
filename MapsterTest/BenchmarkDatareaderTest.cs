using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Data;
using BenchmarkDotNet.Attributes;
using FastExpressionCompiler;
using Mapster;

namespace MapsterTest
{
    public class BenchmarkDataReaderTest
    {
        //Mapster
        readonly TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        //AutoMapper
        readonly Mapper autoMapper = new(new MapperConfiguration(cfg =>
        {
            cfg.AddDataReaderMapping(false);
            cfg.CreateMap<IDataRecord, UserDO>();
            cfg.CreateMap<UserDO, UserDTO>();
        }));

        readonly SqlConnection sqlConnection = new("Data Source=192.168.0.107;Initial Catalog=model;User ID=sa;Password=Pass@w0rd");

        public BenchmarkDataReaderTest()
        {
            //Mapster
            config.Compiler = exp => exp.CompileFast();
        }

        //[Benchmark]
        //public void Mapster()
        //{
        //    //UserDTO userDTO = userDO.Adapt<UserDTO>(
        //    //    config.Fork(forked => forked.ForType<UserDO, UserDTO>().Map("UserName", "UserName")));
        //    UserDTO userDTO = userDO.Adapt<UserDTO>();
        //}

        //[Benchmark]
        //public void AutoMapper()
        //{
        //    UserDTO userDTO = autoMapper.Map<UserDTO>(userDO);
        //}

        [Benchmark]
        public async Task NativeDataReader()
        {
            List<UserDO> userDOs = await Func(async reader =>
             {
                 List<UserDO> userDOs = new();

                 while (await reader.ReadAsync())
                 {
                     userDOs.Add(new UserDO
                     {
                         UserName = reader[0].ToString(),
                         UserAddr = reader[1].ToString(),
                     });
                 }

                 return userDOs;
             });
        }

        [Benchmark]
        public async Task AutoMapperDataReader()
        {
            List<UserDO> userDOs = await Func(reader =>
            {
                return Task.FromResult(autoMapper.Map<IDataReader, IEnumerable<UserDO>>(reader).ToList());
            });
        }

        public delegate void SetPropertyValue(object Value);

        public static SetPropertyValue CreateSetPropertyValueDelegate<S>(S foo, string propName)
        {
            return (SetPropertyValue)Delegate.CreateDelegate(typeof(SetPropertyValue), foo, typeof(S).GetProperty(propName).GetSetMethod());
        }

        //[Benchmark]
        public async Task SchemaTableDataReader()
        {
            List<UserDO> userDOs = await Func(async reader =>
            {
                List<UserDO> userDOs = new();

                DataTable SchemaTable = reader.GetSchemaTable();
                DataRowCollection rows = SchemaTable.Rows;

                var user = new UserDO();
                foreach (DataRow row in rows)
                {
                    Console.WriteLine(row["ColumnName"] + "=" + row["DataType"] + "=" + row["AllowDBNull"] + "=");

                    Type typeT = Type.GetType(row["DataType"].ToString());
                    if (typeT != typeof(DateTime))
                        CreateSetPropertyValueDelegate(user, row["ColumnName"].ToString())(Convert.ChangeType("1234", typeT));
                }

                while (await reader.ReadAsync())
                {
                    userDOs.Add(new UserDO
                    {
                        UserName = reader[0].ToString(),
                        UserAddr = reader[1].ToString(),
                    });
                }

                return userDOs;
            });
        }

        private async Task<List<UserDO>> Func(Func<SqlDataReader, Task<List<UserDO>>> func)
        {
            try
            {
                await sqlConnection.OpenAsync();
                await using SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select * from [dbo].[User]";

                await using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                return await func.Invoke(reader);
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }
    }
}
