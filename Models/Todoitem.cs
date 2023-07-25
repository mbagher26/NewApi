using System;

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
        public int  ItemsID { get; set; }

        public string Name { get;set;} = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? Created_At { get; set;}

        public int? PriorityID { get; set;}

        public int? StatusID { get; set;}
    }

    public class TodoItemUpdateModel
    {
        public int  ItemsID { get; set; }

        public string Name { get;set;} = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? Update_At { get; set;}

        public int? PriorityID { get; set;}

        public int? StatusID { get; set;}

    }

    public class TodoItemActivateMethod
    {
        public int  ItemsID { get; set; }

        public bool IsDelete { get; set;}
        public int StatusID { get; set;}

    }

}
