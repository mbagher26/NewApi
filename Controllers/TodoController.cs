
using MySqlConnector;
using System;
namespace newApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


[Area("V1")]
[ApiController]
[Route("api/[Area]/todo")]
public class MyController : Controller
{
    [HttpGet("[action]")]
    public  IActionResult GetAll()
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, p.Titele, i.Created_At, i.Update_At, i.PriorityID
                            FROM Items i
                            INNER JOIN Priority p ON i.PriorityID = p.PriorityID;";
                            
        List<TodoItem> items = new List<TodoItem>(); 
        using( var reader = cmd.ExecuteReader())
        {
            

                    while(reader.Read())
                    {
                   
                    Console.WriteLine(string.Format(
                    "Reading from table=({0}, {1}, {2},{3},{4},{5},{6},{7})",
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetBoolean(2),
                    reader.GetBoolean(3),
                    reader.GetString(4),                    
                    reader.GetDateTime(5),
                    reader.GetDateTime(6),
                    reader.GetInt32(7)
                    

                    ));
                    items.Add(new TodoItem {
                                            ItemsID = reader.GetInt32(reader.GetOrdinal("ItemsID")),
                                            Name = reader.GetString(reader.GetOrdinal("name")),
                                            IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")),
                                            IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
                                            Titele = reader.GetString(reader.GetOrdinal("Titele")),                                       
                                            Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                                            Update_At_At = reader.GetDateTime(reader.GetOrdinal("Update_At")),
                                            PriorityID = reader.GetInt32(reader.GetOrdinal("PriorityID"))
                                            });


                    }
                    
        }

        return Ok(items.OrderBy(x => x.ItemsID));
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id",id);
        var count = await cmd.ExecuteScalarAsync();
        if(count != null)
        {
            var number = (long)count;
            if(number == 0){
                return NotFound($"رکوردی با این شماره آیدی وجود ندارد:{id}");
            }
        }

        cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, p.Titele, i.Created_At, i.Update_At, i.PriorityID
                            FROM Items i
                            INNER JOIN Priority p ON i.PriorityID = p.PriorityID WHERE ItemsID=@id;";
        List<TodoItem> items = new List<TodoItem>();
        using(var reader = cmd.ExecuteReader())
        {
            while(reader.Read())
            {
                items.Add(new TodoItem {
                                        ItemsID = reader.GetInt32(reader.GetOrdinal("ItemsID")) , 
                                        Name = reader.GetString(reader.GetOrdinal("name")), 
                                        IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")), 
                                        IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
                                        Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                                        PriorityID = reader.GetInt32(reader.GetOrdinal("PriorityID")),
                                        Update_At_At = reader.GetDateTime(reader.GetOrdinal("Update_At")),  
                                        Titele = reader.GetString(reader.GetOrdinal("Titele"))
                                        });

            }
        }
        
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] int id, string value , bool IsCompelet , bool IsDelete, int PriorityID )
    {

        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `Items` (ItemsID,name,IsCompelete,IsDelete,PriorityID) VALUE(NULL,@value,@IsCompelete,@IsDelete,@PriorityID);";
        cmd.Parameters.AddWithValue("@value", value);
        cmd.Parameters.AddWithValue("@IsCompelete", IsCompelet);
        cmd.Parameters.AddWithValue("@IsDelete", IsDelete);
        cmd.Parameters.AddWithValue("@PriorityID", PriorityID);
        await cmd.ExecuteNonQueryAsync();
        
        
        return Ok();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] string Name ,bool IsCompelet ,int PriorityID)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id",id);
        var count = await cmd.ExecuteScalarAsync();
        if(count != null)
        {
            var number = (long)count;
            if(number == 0){
                return NotFound($"رکوردی با این شماره آیدی وجود ندارد:{id}");
            }
        }

        cmd.CommandText = @"UPDATE Items SET name = @name , IsCompelete = @IsCompelete , PriorityID = @PriorityID WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@name",Name);
        cmd.Parameters.AddWithValue("@IsCompelete",IsCompelet);
        cmd.Parameters.AddWithValue("PriorityID",PriorityID);     
        cmd.ExecuteNonQuery();

        
        
        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id ,bool isdelete)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id" , id);
        var count = await cmd.ExecuteScalarAsync();
        if(count != null) {
            var number = (long) count;
            if(number.Equals(0))
                return NotFound($"No record found with ID: {id}");
        }

        cmd.CommandText = @"UPDATE Items SET IsDelete = @isdelete WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@isdelete" , isdelete);
        await cmd.ExecuteNonQueryAsync();

        return Ok(id);
    }
    



}


