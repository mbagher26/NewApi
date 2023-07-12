using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace newApi.Models;


[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    [HttpGet]
    public  IActionResult GetAll()
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Items;";
        using( var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
                {
                    Console.WriteLine(string.Format(
                    "Reading from table=({0}, {1}, {2})",
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2)));
                }
        }
        return Ok("Hello World");
    }

    [HttpGet("{id}")]
    public IActionResult Get()
    {
        // var connection = MysqlConnect.GetConnection();
        // using var cmd = connection.CreateCommand();
        // cmd.CommandText = @"SELECT * FROM items;";
        // await cmd.ExecuteNonQueryAsync();
        
        return Ok("Hello World");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string value)
    {

        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `Items` (name,IsCompelete) VALUE(@value,1);";
        cmd.Parameters.AddWithValue("@value", value);
        await cmd.ExecuteNonQueryAsync();
        var id = (int) cmd.LastInsertedId;
        Console.WriteLine(id);

        
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

    // public static void BindParams(MySqlCommand cmd)
    // {
    //     // cmd.Parameters.Add(new MySqlParameter
    //     // {
    //     //     ParameterName = "@title",
    //     //     DbType = DbType.String,
    //     //     Value = Title,
    //     // });
    //     // cmd.Parameters.Add(new MySqlParameter
    //     // {
    //     //     ParameterName = "@content",
    //     //     DbType = DbType.String,
    //     //     Value = Content,
    //     // });
    // }
}


