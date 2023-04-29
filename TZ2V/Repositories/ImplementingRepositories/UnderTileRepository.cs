using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class UnderTileRepository : IUnderTileRepository
    {
        public SqlConnection connection { get; init; }

        public UnderTileRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(UnderTile entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToUnderTile";


                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@Name",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.NameTile
                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@TileID",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.TileId

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

        public IEnumerable<UnderTile> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
