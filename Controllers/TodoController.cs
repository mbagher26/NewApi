
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
    public async Task<IActionResult> GetAll()
    {
    try{    var connection = MysqlConnect.GetConnection();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, i.Created_At, i.Update_At, i.PriorityID, p.Title, i.Description, i.StatusID, s.TitleStatus
                                FROM Items i
                                INNER JOIN Status s ON i.StatusID = s.StatusID
                                INNER JOIN Priority p ON i.PriorityID = p.PriorityID ORDER BY i.ItemsID;";

            List<TodoItemviewModel> items = new List<TodoItemviewModel>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    items.Add(new TodoItemviewModel
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
            return Ok(items);
        }
        catch(Exception e)
        {
            return StatusCode(500,"خطایی در سرور رخ داده است");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {   
            try
            {
                var connection = MysqlConnect.GetConnection();
                var cmd = connection.CreateCommand();
                
                    cmd.CommandText = $"SELECT COUNT(*) FROM Items WHERE ItemsID = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    var count = await cmd.ExecuteScalarAsync();
                    //استفاده از متد ExecuteScalar()
                    //این متد معمولا زمانی استفاده می شود که انتظار یک مقدار واحد را دارد مثل زمانی که از 
                    // متدهای کانت و سام و اوریج در جمله اس کیو ال استفاده کردیم 

                    // عبارت Async 
                    // باعث می شود که این متد به صورت نا همزمان اجرا شود برای حفظ پاسخگویی و مقیاس پذیری استفاده می شود. 
                    if (count != null)
                    {       if((Int64)count == 0)
                            {   var errorViewModel = new MessageViewModel
                                    {
                                        StatusCode = 404,
                                        Message = $"رکوردی با این شماره آیدی وجود ندارد:{id}"
                                    };                              
                                    return NotFound(errorViewModel); 
                            }                       
                    }
                    cmd.CommandText = @"SELECT i.ItemsID, i.name, i.IsCompelete, i.IsDelete, i.Created_At, i.Update_At, i.PriorityID, i.Description, i.StatusID, s.TitleStatus,p.Title
                                        FROM Items i
                                        INNER JOIN Status s ON i.StatusID = s.StatusID
                                        INNER JOIN Priority p ON i.PriorityID = p.PriorityID                         
                                        WHERE ItemsID=@id;";
                    List<TodoItemviewModel> items = new ();
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            items.Add(new TodoItemviewModel
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
            catch(Exception e)
            {   
                var errorViewModel = new MessageViewModel
                {
                    StatusCode = 500,
                    Message = "خطایی در سرور رخ داده"
                };
                return StatusCode(500,errorViewModel);
            }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromForm]TodoItemPostModel model)
    {   
        if(string.IsNullOrWhiteSpace(model.Name))
        {
            var messge = new PostResponseViewModel
            {
                StatusCode = 400,
                Message = "نام وارد شده معتبر نیست"
            };
            return BadRequest(messge);
        }

        if(model.Image == null || model.Image.Length == 0)
        {
            var messge = new PostResponseViewModel
            {
                StatusCode = 400,
                Message = "تصویر ارسال شده معتبر نیست"
            };
        }
      
        string imagesFolderPath = "/home/mohamad/imagefolder/";
        string uniqueFileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
        string imagePath = Path.Combine(imagesFolderPath, uniqueFileName);

        using(var stream = new FileStream(imagePath, FileMode.Create))
        {
            await model.Image.CopyToAsync(stream);
        }

        var connection = MysqlConnect.GetConnection();
        using var cmd = connection.CreateCommand();
        try{
        cmd.CommandText = @"INSERT INTO `Items` (name,PriorityID,StatusID,Description,Image) VALUE(@Name,@PriorityID,@StatusID,@Description,@Image);";
        cmd.Parameters.AddWithValue("@Name", model.Name);
        cmd.Parameters.AddWithValue("@PriorityID", model.PriorityID);
        cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
        cmd.Parameters.AddWithValue("@Description", model.Description);
        cmd.Parameters.AddWithValue("@Image",imagePath);



        await cmd.ExecuteNonQueryAsync();
        var insertedItemId = (int)cmd.LastInsertedId;

        var responsiveViewModel = new PostResponseViewModel
        {   
            StatusCode = 200,
            Id = insertedItemId,
            Message = "ثبت با موفقیت انجام شد"
        };
        return Ok(responsiveViewModel);

        }catch(Exception e){
            return BadRequest(e.Message);

        }
    }
            // var errorViewModel = new MessageViewModel
            // {
            //     StatusCode = 400,
            //     Message = "خطا در ثبت اطلاعات"
            // };
            // return BadRequest(errorViewModel);

    [HttpPut]
    public async Task<IActionResult> Update([FromForm]TodoItemUpdateModel model)
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
                var messge = new MessageViewModel
                {
                    StatusCode = 404,
                    Message = $"رکوردی با این شماره آیدی وجود ندارد:{model.ItemsID}"
                };
                return NotFound(messge);
            }
        }

        try{

        string imagesFolderPath = "/home/mohamad/UpdateImageFolder/";
        string uniqueFileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
        string imagePath = Path.Combine(imagesFolderPath, uniqueFileName);

        using(var stream = new FileStream(imagePath, FileMode.Create))
        {
            await model.Image.CopyToAsync(stream);
        }

        cmd.CommandText = @"UPDATE Items SET name = @name , Description = @Description, PriorityID = @PriorityID, StatusID = @StatusID, Image = @Image  WHERE ItemsID = @id;";
        cmd.Parameters.AddWithValue("@name", model.Name);
        cmd.Parameters.AddWithValue("@Description", model.Description);
        cmd.Parameters.AddWithValue("@PriorityID", model.PriorityID);
        cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
        cmd.Parameters.AddWithValue("@Image", imagePath);


        cmd.ExecuteNonQuery();
        var messge = new MessageViewModel
        {
            StatusCode = 200,
            Message = "بروزرسانی با موفقیت انجام شد"
        };
        return Ok(messge);

        }catch(Exception){
            var error = new MessageViewModel
            {
                StatusCode = 400,
                Message = "خطا در بروزرسانی اطلاعلت"
            };
            return BadRequest(error);
        }
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
            {
                var error = new MessageViewModel
                {
                    StatusCode = 404,
                    Message = $"No record found with ID: {id}"
                };
                return NotFound(error);
            }
        }

        cmd.CommandText =@"SELECT IsDelete FROM Items WHERE ItemsID = @id";
        var Isdelete = (bool) await cmd.ExecuteScalarAsync();
        if(Isdelete)
        {
            var Message = new MessageViewModel
            {
                StatusCode = 200,

                Message = "رکورد با این شماره آیدی قبلا حذف شده است"
            };
            return Ok(Message);
        }
        try
        {
            cmd.CommandText = @"UPDATE Items SET IsDelete = @isdelete WHERE ItemsID = @id;";
            cmd.Parameters.AddWithValue("@isdelete", true);
            await cmd.ExecuteNonQueryAsync();
            var messge = new MessageViewModel
            {
                StatusCode = 200,
                Message = "عملیات با موفقیت انجام شد"
            };
            return Ok(messge);
        }
        catch (Exception)
        {   
            var error = new MessageViewModel
            {
                StatusCode = 404,
                Message = "خطا در اجرای عملیات حذف"
            };
            return BadRequest(error);
        }
    }


    [HttpPut("[action]")]
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
            {   var error = new MessageViewModel
                {
                    StatusCode = 404,
                    Message = $"رکوردی با این شماره آیدی یافت نشد:{model.ItemsID}"
                };
                return NotFound(error);
            }
        }
        cmd.CommandText = @"SELECT  StatusID FROM Items WHERE ItemsID = @id;";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@id",model.ItemsID);
        using (var reader = cmd.ExecuteReader())
        
        if(reader.Read())
        {   
            var currentStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            if(currentStatusID == model.StatusID)
            {
                var responsiveViewModel = new PostResponseViewModel
                    {   
                        Id = model.ItemsID,

                        Message = $"قبلا در این وضعیت قرار گرفته است:{model.StatusID}"
                    };
                return Ok(responsiveViewModel);
            }
            reader.Close();
        }
        try
        {
            cmd.CommandText = $"UPDATE Items SET  StatusID = @StatusID WHERE ItemsID = @id;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@StatusID", model.StatusID);
            cmd.Parameters.AddWithValue("@id", model.ItemsID);

            await cmd.ExecuteNonQueryAsync();
            // از ExecuteNonQuery
            // زمانی جمله اس کیو ال پس از اجرا مقداری را بر نمیگرداند استفاده می شود مثل دیلیت و آپدیت و اینزرت
            var messge =new MessageViewModel
                {
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            return Ok(messge);
        }
        catch (Exception )
        {
            var error = new MessageViewModel
            {
                StatusCode = 404,
                Message = "عملیات موفقیت آمیز نبود"
            };
            return BadRequest(error);
        }        
    }
}


