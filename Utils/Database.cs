using System;
using System.Threading.Tasks;
using MySqlConnector;
namespace newApi;
using System.Data;


public class MysqlConnect
{
    public static string Connectionstring = "Server=localhost;Port=3306;Database=newapi;Uid=user;Pwd=qaz";



    protected  MySqlConnection? _instance = null;

    public MySqlConnection GetConnection()
    {
        if (_instance != null)
        {
            return _instance;
        }
        _instance = new MySqlConnection(Connectionstring);
        return _instance;
    }

    public static void CreateCommand (string sqlSatement)
    {
        MySqlCommand command = new MySqlCommand(sqlSatement);

    }
    

    // public static object Command { get => command; set => command = value; }
}

public class DatabaseConnection : MysqlConnect
{
    
    public void TestConnection ()
        {
            try
            {
                    
                    if(_instance?.State == ConnectionState.Open)
                    {
                        Console.WriteLine("openinig connection");
                    }
                    else
                    {
                            Console.WriteLine("error",_instance?.State);
                       
                    }
            }
            catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {  
                    new MySqlConnection(Connectionstring).Close();
                    Console.WriteLine("closeing connection");
                }

            // try
            // {
            //         var connection = new MySqlConnection(Connectionstring);
            //         connection.Open();
            //         if(connection.State == ConnectionState.Open)
            //         {
            //             Console.WriteLine("openinig connection");
            //         }
            //         else{
            //                 Console.WriteLine("error");
                       
            //             }
            // }
            // catch(Exception e)
            //     {
            //         Console.WriteLine("error opening connection" + e.Message);
            //     }
            //     finally
            //     {  
            //         new MySqlConnection(Connectionstring).Close();
            //         // if(Connection.State == ConnectionState.Closed)
            //         Console.WriteLine("closeing connection");
            //     }
        }
}
   
