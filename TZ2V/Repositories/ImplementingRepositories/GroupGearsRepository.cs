using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class GroupGearsRepository : IGroupGearsRepository
    {
        public SqlConnection connection { get; init; }

        public GroupGearsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }
        
        public async Task<int> Create(GroupGears entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToGroupGears";


                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@Name",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.NameGroup
                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@UnderTileID",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.UnderTileId

                });
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@ComplectationsID",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.ComplectationsID
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

        public IEnumerable<GroupGears> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
