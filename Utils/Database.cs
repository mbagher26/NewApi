using System;
using System.Threading.Tasks;
using MySqlConnector;
namespace newApi;
using System.Data;

public static class NewBaseType
{
    public static string Connectionstring = "Server=localhost;Port=3306;Database=newapi;Uid=user;Pwd=qaz";



    private static MySqlConnectionStringBuilder? _instance = null;

    public static MySqlConnectionStringBuilder GetConnection()
    {
        if (_instance != null)
        {
            return _instance;
        }
        _instance = new MySqlConnectionStringBuilder(Connectionstring);
        return _instance;
    }
}

public static class DatabaseConnection 
{
    public static string Connectionstring = "Server=localhost;Port=3306;Database=newapi;Uid=user;Pwd=qaz";
    public static void TestConnection ()
        {

        try
        {
                var connection = new MySqlConnection(Connectionstring);
                connection.Open();
                if(connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("openinig connection");
                }
                else{
                    Console.WriteLine(connection.State);
                    // throw new MySqlError(connection.State);
                }
        }
        catch(Exception e)
            {
                Console.WriteLine("error opening connection" + e.Message);
            }
            finally
            {
                new MySqlConnection(Connectionstring).Close();
            }
        }
}
   
