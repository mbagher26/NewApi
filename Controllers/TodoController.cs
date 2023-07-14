
using MySqlConnector;
using System;
namespace newApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("[controller]")]
public class MyController : Controller
{
    [HttpGet]
    public  IActionResult GetAll()
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT *
                            FROM Items i
                            INNER JOIN Priority p ON i.ItemsID = p.PriorityID;";
                            
        List<TodoItem> items = new List<TodoItem>(); 
        using( var reader = cmd.ExecuteReader())
        {
            

                    if(reader.Read())
                    {
                   
                    Console.WriteLine(string.Format(
                    "Reading from table=({0}, {1}, {2},{3},{4})",
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetBoolean(2),
                    reader.GetBoolean(3),
                    reader.GetDateTime(4),
                    reader.GetString(5)

                    ));
                    items.Add(new TodoItem {Id = reader.GetInt32(reader.GetOrdinal("id")) ,
                                            Name = reader.GetString(reader.GetOrdinal("name")),
                                            IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")),
                                            IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
                                            Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                                            Titele = reader.GetString(reader.GetOrdinal("Titele"))

                                            });
                    }
                    else
                    {
                        Console.WriteLine("can`t read!");
                    }
        }
        return Ok(items);
    }




    // [HttpGet]
    // public  IActionResult GetAll()
    // {
    //     var connection = MysqlConnect.GetConnection();
    //     using var cmd = connection.CreateCommand();
    //     cmd.CommandText = @"SELECT * FROM Items;";
    //     List<TodoItem> items = new List<TodoItem>(); 
    //     using( var reader = cmd.ExecuteReader())
    //     {
    //         while (reader.Read())
    //             {
    //                 Console.WriteLine(string.Format(
    //                 "Reading from table=({0}, {1}, {2},{3},{4})",
    //                 reader.GetInt32(0),
    //                 reader.GetString(1),
    //                 reader.GetBoolean(2),
    //                 reader.GetBoolean(3),
    //                 reader.GetDateTime(4)

    //                 ));
    //                 items.Add(new TodoItem {Id = reader.GetInt32(reader.GetOrdinal("id")) ,
    //                                         Name = reader.GetString(reader.GetOrdinal("name")),
    //                                         IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")),
    //                                         IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
    //                                         Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At"))
    //                                         });
    //             }
    //     }
    //     return Ok(items);
    // }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Items;";
        List<TodoItem> items = new List<TodoItem>();
        using(var reader = cmd.ExecuteReader())
        {
            while(reader.Read())
            {
                items.Add(new TodoItem {Id = reader.GetInt32(reader.GetOrdinal("id")) , 
                                        Name = reader.GetString(reader.GetOrdinal("name")), 
                                        IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")), 
                                        IsDelete = reader.GetBoolean("IsDelete"),
                                        Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At"))
                                        });

            }
        }
        
        return Ok(items.FirstOrDefault(x => x.Id == id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string value , bool IsCompelet , int PriorityID)
    {

        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `Items` (name,IsCompelete,PriorityID) VALUE(@value,@IsCompelete,@PriorityID);";
        cmd.Parameters.AddWithValue("@value", value);
        cmd.Parameters.AddWithValue("@IsCompelete", IsCompelet);
        cmd.Parameters.AddWithValue("@PriorityID", PriorityID);


        await cmd.ExecuteNonQueryAsync();
        var id = (int) cmd.LastInsertedId;
        Console.WriteLine(id);

        
        return Ok();
    }


    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] string Name ,bool IsCompelet)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"UPDATE Items SET name = @name , IsCompelete = @IsCompelete WHERE id = @id;";
        cmd.Parameters.AddWithValue("@name",Name);
        cmd.Parameters.AddWithValue("@IsCompelete",IsCompelet);
        cmd.Parameters.AddWithValue("@id",id);
        cmd.ExecuteNonQuery();

        
        
        return Ok();
    }



    [HttpDelete("{id}")]
    public void Delete(int id, [FromBody] bool isdelete)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"UPDATE Items SET IsDelete = @isdelete WHERE id = @id;";
        cmd.Parameters.AddWithValue("@isdelete" , isdelete);
        cmd.Parameters.AddWithValue("@id" , id);
        cmd.ExecuteNonQuery();


    }



        // [HttpDelete("{id}")]
    // public IActionResult Delete(int id)
    // {
    //     var connection = MysqlConnect.GetConnection();
    //     using var cmd = connection.CreateCommand();
    //     cmd.CommandText = @"DELETE FROM Items WHERE id = @id;";
    //     cmd.Parameters.AddWithValue("@id",id);
    //     cmd.ExecuteNonQuery();
    //     return Ok();
    // }



   
}


