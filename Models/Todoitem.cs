using System;

namespace newApi.Models
{
    public class TodoItem
    {
        public int  ItemsID { get; set; }
        public string Name { get;set;} = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        // public DateTime CreatedAt { get; set; }
        
        public bool IsDelete { get; set;}
        
        public DateTime Created_At { get; set;}

        public DateTime Update_At { get; set;}

        public int PriorityID { get; set;}
        public String? Titele { get; set;}

        
    }

    public class TodoItemModel
    {
        public int  ItemsID { get; set; }
        public string Name { get;set;} = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        // public DateTime CreatedAt { get; set; }
        
        public bool IsDelete { get; set;}
        
        public DateTime Created_At { get; set;}

        public DateTime Update_At { get; set;}

        public int PriorityID { get; set;}
    }
}
