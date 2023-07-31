namespace newApi.Models
{
    public class TodoItem
    {
        public int  ItemsID { get; set; }

        public string Name { get;set;} = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsComplete { get; set; }
        
        public bool IsDelete { get; set;}
        
        public DateTime Created_At { get; set;}

        public DateTime Update_At { get; set;}

        public int PriorityID { get; set;}

        public String? Title { get; set;}

        public int StatusID { get; set;}

        public String? TitleStatus { get; set;}
    }


    public class TodoItemPostModel
    {
        public string? Name { get;set;}

        public string Description { get; set; } = string.Empty;

        public IFormFile? Image {get; set;}

        public int? PriorityID { get; set;}

        public int? StatusID { get; set;}
    }

    public class TodoItemUpdateModel
    {
        public int  ItemsID { get; set; }

        public string Name { get;set;} = string.Empty;
        public IFormFile? Image {get; set;}

        public string Description { get; set; } = string.Empty;

        public int? PriorityID { get; set;}

        public int? StatusID { get; set;}

    }

    public class TodoItemActivateMethod
    {
        public int  ItemsID { get; set; }
        public int StatusID { get; set;}

    }
    public class TodoItemChangeCompeletModel
    {
        public int ItemsID { get; set;}
        public bool? IsComplete { get; set;}

        public bool? IsDelete { get; set;}
    }

    public class TodoItemviewModel
    {
        public int  ItemsID { get; set; }

        public string Name { get;set;} = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsComplete { get; set; }
        
        public bool IsDelete { get; set;}
        
        public DateTime Created_At { get; set;}

        public DateTime Update_At { get; set;}

        public int PriorityID { get; set;}

        public String? Title { get; set;}

        public int StatusID { get; set;}

        public String? TitleStatus { get; set;}
    }

    public class MessageViewModel
    {
        public int StatusCode {get; set;}

        public string Message {get; set;}
    }

    public class PostResponseViewModel
    {
        public int Id {get; set;}

        public int StatusCode {get; set;}
        public string Message {get; set;}
    }
}
