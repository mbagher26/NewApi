using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace newApi.Models;


[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{


    [HttpGet]
    public IActionResult Get()
    {
        
        DatabaseConnection.TestConnection(); 

        // /* Include this "using" directive...
        // using MySql.Data.MySqlClient;
        // */

        // string connectionString = "Server=localhost;Port=3306;Database=db;Uid=user;Pwd=qaz";

        // // Best practice is to scope the MySqlConnection to a "using" block
        // using (MySqlConnection conn = new MySqlConnection(connectionString))
        // {
        // // Connect to the database
        // conn.Open();

        // // Read rows
        // MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM MyTable", conn);
        // MySqlDataReader results = selectCommand.ExecuteReader();
    
        // // Enumerate over the rows
        // while(results.Read())
        // {
        //     // Console.WriteLine("Column 0: {0} Column 1: {1}", results[0], results[1]);
            
        //     Console.WriteLine(results);
            
        // }
        // }   

        // Implement logic to retrieve data
        return Ok("Hello World");
    }

    [HttpPost]
    public IActionResult Post([FromBody] string value)
    {
        // Implement logic to save data
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] string value)
    {
        // Implement logic to update data
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Implement logic to delete data
        return Ok();
    }
}