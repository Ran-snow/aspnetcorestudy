namespace WebApplicationUnitTestDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddRepositories(); // 编译时生成的代码，零反射
            //<ProjectReference Include="..\SourceGenerator\SourceGenerator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
