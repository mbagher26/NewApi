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
        

        // Implement logic to retrieve data
        return Ok("Hello World");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string value)
    {

        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `Items` (name,IsCompelete) VALUE('ahmad',1);";
        await cmd.ExecuteNonQueryAsync();
        var id = (int) cmd.LastInsertedId;
        Console.WriteLine(id);

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

    public static void BindParams(MySqlCommand cmd)
    {
        // cmd.Parameters.Add(new MySqlParameter
        // {
        //     ParameterName = "@title",
        //     DbType = DbType.String,
        //     Value = Title,
        // });
        // cmd.Parameters.Add(new MySqlParameter
        // {
        //     ParameterName = "@content",
        //     DbType = DbType.String,
        //     Value = Content,
        // });
    }
}


