using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class TileRepository : ITileRepository
    {
        public SqlConnection connection { get; init; }

        public TileRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(Tile entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToTiles";

                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.VarBinary,
                    SqlValue = entity.ImageScheme,
                    ParameterName = "@Image",
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

        public IEnumerable<Tile> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
