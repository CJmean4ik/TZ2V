using System.Data.Common;
using System.Data.SqlClient;
using TZ2V.Data;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class AboutModelRepository : IAboutModelRepository
    {
        public SqlConnection connection { get; init; }

        public AboutModelRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(AboutModel entity)
        {
            if (entity == null) throw new NullReferenceException("entity type AboutModel is null");

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SP_InsertDataToAboutModel";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Input,
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@BrandModelID",
                    Value = entity.BrandModelId,
                });

                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Input,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@Code",
                    Value = entity.Code,

                });

                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Input,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@DataRange",
                    Value = entity.DataRange,

                });              
               
                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Input,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    ParameterName = "@CodeModel",
                    Value = entity.CodeModel,
                });
                command.Parameters.Add(new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Output,
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@IdInsertedItem",
                });

                await command.ExecuteNonQueryAsync();
                return (int)command.Parameters["@IdInsertedItem"].Value;
            }
        }

        public IEnumerable<AboutModel> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
