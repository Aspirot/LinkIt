namespace LinkIt.Models.DAO
{
    public class LegacyDbContext
    {
        private string connectionString;
        private IConfiguration _configuration;

        public LegacyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MyDb");
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}
