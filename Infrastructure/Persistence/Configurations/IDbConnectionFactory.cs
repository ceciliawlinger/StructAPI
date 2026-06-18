using System.Data;

namespace StructAPI.Infrastructure.Persistence.Configurations
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
