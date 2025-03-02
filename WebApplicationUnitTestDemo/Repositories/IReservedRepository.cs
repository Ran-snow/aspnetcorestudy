namespace WebApplicationUnitTestDemo.Repositories
{
    public interface IReservedRepository : IRepository
    {
        public string GetSummary(string seed);
    }
}
