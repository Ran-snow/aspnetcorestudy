namespace WebApplicationUnitTestDemo.Repositories.impl
{
    public class ReservedRepository : IReservedRepository
    {
        public string GetSummary(string seed)
        {
            return "Freeze";
        }
    }
}
