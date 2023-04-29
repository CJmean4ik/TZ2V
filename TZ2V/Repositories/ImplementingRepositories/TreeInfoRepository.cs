using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class TreeInfoRepository : ITreeInfoRepository
    {
        public SqlConnection connection { get; init; }

        public TreeInfoRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(TreeInfo entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToTreeInfo";

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    SqlValue = entity.TreeName,
                    ParameterName = "@TreeInfo",
                    Direction = System.Data.ParameterDirection.Input

                });

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    SqlValue = entity.TreeCode,
                    ParameterName = "@TreeCode",
                    Direction = System.Data.ParameterDirection.Input

                });

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@IdInsertedItem",
                    Direction = System.Data.ParameterDirection.Output

                });

                await command.ExecuteNonQueryAsync();
                return (int)command.Parameters["@IdInsertedItem"].Value;
            }

           
        }
        public IEnumerable<TreeInfo> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
