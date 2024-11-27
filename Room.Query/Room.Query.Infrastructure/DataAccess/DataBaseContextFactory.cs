using Microsoft.EntityFrameworkCore;

namespace Room.Query.Infrastructure.DataAccess
{
    public class DataBaseContextFactory 
    {
        private readonly Action<DbContextOptionsBuilder> _config;

        public DataBaseContextFactory(Action<DbContextOptionsBuilder> config)
        {
            _config = config;
        }

        public DataBaseContext CreateDbContext() { 
        
            var Option = new DbContextOptionsBuilder<DataBaseContext>();
            _config(Option);
            return new DataBaseContext(Option.Options);
        }
    }
}
