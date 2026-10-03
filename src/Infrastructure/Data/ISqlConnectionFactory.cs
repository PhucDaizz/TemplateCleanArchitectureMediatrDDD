using System.Data;

namespace AuthService.Infrastructure.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
