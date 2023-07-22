
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
    public IActionResult GetAll()
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, i.Created_At, i.Update_At, i.PriorityID, p.Title, i.Description, i.StatusID, s.TitleStatus
                            FROM Items i
                            INNER JOIN Status s ON i.StatusID = s.StatusID
                            INNER JOIN Priority p ON i.PriorityID = p.PriorityID ;";

        List<TodoItem> items = new List<TodoItem>();
        using (var reader = cmd.ExecuteReader())
        {

            while (reader.Read())
            {

                Console.WriteLine(string.Format(
                "Reading from table=({0}, {1}, {2},{3},{4},{5},{6},{7},{8},{9},{10})",
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetBoolean(2),
                reader.GetBoolean(3),
                reader.GetDateTime(4),
                reader.GetDateTime(5),
                reader.GetInt32(6),
                reader.GetString(7),
                reader.GetString(8),
                reader.GetInt32(9),
                reader.GetString(10)

                ));
                items.Add(new TodoItem
                {
                    ItemsID = reader.GetInt32(reader.GetOrdinal("ItemsID")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")),
                    IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    Update_At = reader.GetDateTime(reader.GetOrdinal("Update_At")),
                    PriorityID = reader.GetInt32(reader.GetOrdinal("PriorityID")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    StatusID = reader.GetInt32(reader.GetOrdinal("StatusID")),
                    TitleStatus = reader.GetString(reader.GetOrdinal("TitleStatus"))
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
        cmd.Parameters.AddWithValue("@id", id);
        var count = await cmd.ExecuteScalarAsync();
        if (count != null)
        {
            if ((Int64)count == 0)
            {
                return NotFound($"رکوردی با این شماره آیدی وجود ندارد:{id}");
            }
        }

        cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, i.Created_At, i.Update_At, i.PriorityID, i.Description, i.StatusID, s.TitleStatus,p.Title
                            FROM Items i
                            INNER JOIN Status s ON i.StatusID = s.StatusID
                            INNER JOIN Priority p ON i.PriorityID = p.PriorityID                         

                            WHERE ItemsID=@id;";
        List<TodoItem> items = new List<TodoItem>();
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                items.Add(new TodoItem
                {
                    ItemsID = reader.GetInt32(reader.GetOrdinal("ItemsID")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    IsComplete = reader.GetBoolean(reader.GetOrdinal("IsCompelete")),
                    IsDelete = reader.GetBoolean(reader.GetOrdinal("IsDelete")),
                    Created_At = reader.GetDateTime(reader.GetOrdinal("Created_At")),
                    PriorityID = reader.GetInt32(reader.GetOrdinal("PriorityID")),
                    Update_At = reader.GetDateTime(reader.GetOrdinal("Update_At")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    TitleStatus = reader.GetString(reader.GetOrdinal("TitleStatus")),
                    StatusID = reader.GetInt32(reader.GetOrdinal("StatusID"))

                });

            }
        }

        return Ok(items);
    }


    [HttpPost]
    public async Task<IActionResult> Post(TodoItemPostModel model)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `Items` (name,Created_At,PriorityID,StatusID,Description) VALUE(@Name,@Created_At,@PriorityID,@StatusID,@Description);";
        cmd.Parameters.AddWithValue("@Name", model.Name);
        cmd.Parameters.AddWithValue("@Created_At", model.Created_At);
        cmd.Parameters.AddWithValue("@PriorityID", model.PriorityID);
        cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
        cmd.Parameters.AddWithValue("@Description", model.Description);


        await cmd.ExecuteNonQueryAsync();

        return Ok("ثبت با موفقیت انجام شد");
    }


    [HttpPut]
    public async Task<IActionResult> Update(TodoItemUpdateModel model)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id", model.ItemsID);
        var count = await cmd.ExecuteScalarAsync();
        if (count != null)
        {
            
            if ((Int64)count == 0)
            {
                return NotFound($"رکوردی با این شماره آیدی وجود ندارد:{model.ItemsID}");
            }
        }

        cmd.CommandText = @"UPDATE Items SET name = @name , Description = @Description, PriorityID = @PriorityID, StatusID = @StatusID, Update_At = @Update_At  WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@name", model.Name);
        cmd.Parameters.AddWithValue("@Description", model.Description);
        cmd.Parameters.AddWithValue("@PriorityID", model.PriorityID);
        cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
        cmd.Parameters.AddWithValue("@Update_At", model.Update_At);



        cmd.ExecuteNonQuery();
        return Ok("بروزرسانی با موفقیت انجام شد");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id", id);
        var count = await cmd.ExecuteScalarAsync();
        if (count != null)
        {
            
            if ((Int64)count == 0)
                return NotFound($"No record found with ID: {id}");
        }
        try
        {
            cmd.CommandText = @"UPDATE Items SET IsDelete = @isdelete WHERE ItemsID = @id;";
            cmd.Parameters.AddWithValue("@isdelete", true);
            await cmd.ExecuteNonQueryAsync();
            return Ok("عملیات با موفقیت انجام شد");
        }
        catch (Exception e)
        {
            return Ok(e.Message + "عملیات ناموفق");
        }
    }


    [HttpDelete("[action]")]
    public async Task<IActionResult> Activate(TodoItemActivateMethod model)
    {
        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@id", model.ItemsID);
        var count = await cmd.ExecuteScalarAsync();
        if (count != null)
        {
            if ((Int64)count == 0)
            {
                return NotFound($"رکوردی با این شماره آیدی یافت نشد:{model.ItemsID}");
            }
        }
        try
        {
            cmd.CommandText = $"UPDATE Items SET IsDelete = @isdelete WHERE ItemsID = @id;";
            cmd.Parameters.AddWithValue("IsDelete", model.IsDelete);
            await cmd.ExecuteNonQueryAsync();
            return Ok("عملیات با موفقیت انجام شد");
        }
        catch (Exception e)
        {
            return Ok(e.Message + "عملیات ناموفق ");
        }        
    }
}


