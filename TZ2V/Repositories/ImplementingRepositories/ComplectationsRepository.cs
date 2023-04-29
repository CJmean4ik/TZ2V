using System.Data;
using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Repositories.IRepositories;

namespace TZ2V.Repositories.ImplementingRepositories
{
    public class ComplectationsRepository : IComplectationsRepository
    {
        public SqlConnection connection { get; init; }

        public ComplectationsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Create(Complectations entity)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_InsertDataToComplectations";

              
                command.Parameters.Add(new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@IdInsertedItem",
                    Direction = System.Data.ParameterDirection.Output

                });

                AddParametrs(entity, command);
                await command.ExecuteNonQueryAsync();
                return (int)command.Parameters["@IdInsertedItem"].Value;

            }
        }
        public void AddParametrs(Complectations complectations,SqlCommand command)
         {
            var properties = typeof(Complectations).GetProperties().SkipLast(2).ToList();

            for (int i = 0; i < properties.Count(); i++)
            {
                object propValue = properties[i].GetValue(complectations);

                if (propValue != null)
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@" + properties[i].Name;
                    parameter.SqlDbType = properties[i].PropertyType.Name == "String" ? SqlDbType.NVarChar : SqlDbType.Int;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = properties[i].GetValue(complectations);
                    command.Parameters.Add(parameter);
                }
            }
            
        }
        public IEnumerable<Complectations> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
