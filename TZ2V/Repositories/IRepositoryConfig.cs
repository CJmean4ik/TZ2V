using System.Data.SqlClient;

namespace TZ2V.Repositories
{
    internal interface IRepositoryConfig
    {
        public SqlConnection connection { get; init; }
    }
}
