using System.Data.SqlClient;
using TZ2V.Data;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class TreeRepository : ITreeRepository
    {
        public SqlConnection connection { get; init; }

        public TreeRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(Tree entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToTree";

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    SqlValue = entity.Code,
                    ParameterName = "@Code",
                    Direction = System.Data.ParameterDirection.Input

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    SqlValue = entity.Count,
                    ParameterName = "@Count",
                    Direction = System.Data.ParameterDirection.Input

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    SqlValue = entity.Data,
                    ParameterName = "@Data",
                    Direction = System.Data.ParameterDirection.Input

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    SqlValue = entity.Info,
                    ParameterName = "@Info",
                    Direction = System.Data.ParameterDirection.Input

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    SqlValue = entity.TreeInfoId,
                    ParameterName = "@TreeInfoID",
                    Direction = System.Data.ParameterDirection.Input

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    SqlValue = entity.TileId,
                    ParameterName = "@TileID",
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
        public IEnumerable<Tree> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
