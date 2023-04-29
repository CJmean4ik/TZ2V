using System.Data.SqlClient;
using TZ2V.Entity;
using TZ2V.Parser;
using TZ2V.Repositories.ImplementingRepositories;

namespace TZ2V
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Uri uri = new Uri("https://www.ilcats.ru/toyota/?function=getModels&market=EU");
            string dir = @"C:\Users\Стас\OneDrive\Документы\Shemes\";
            string conn = @"Data Source=СТАС-ПК\SQLEXPRESS;Initial Catalog=Cars;Integrated Security=True";
            HtmlCarsParser htmlParser = new HtmlCarsParser(ConfigurationType.Loader, uri, dir);

              var cars = await htmlParser.ParseAsync(1);

            
              MsSqlDataBase dataBase = new MsSqlDataBase(conn);
              dataBase.OpenSqlConnection();
              await dataBase.InsertDataToDataBase(cars);
            
            

            SqlConnection connection = new SqlConnection(conn);
            
            
            dataBase.CloseSqlConnection();

        }

    }
}