using System.Data.SqlClient;
using TZ2V.Data;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class BrandModelRepository : IBrandModelRepository
    {
        public SqlConnection connection { get; init; }
        
        public BrandModelRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(BrandModel entity)
        {
            if (entity == null) throw new NullReferenceException("entity type AboutModel is null");

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SP_InsertDataToBrandModel";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter 
                {
                  Direction = System.Data.ParameterDirection.Output,
                  SqlDbType = System.Data.SqlDbType.Int,
                  ParameterName = "@IdInsertedItem",
                });

                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Input,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@Name",
                    Value = entity.NameModel,

                });               

                await command.ExecuteNonQueryAsync();
                return (int)command.Parameters["@IdInsertedItem"].Value;
            }          
        }

        public IEnumerable<BrandModel> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
