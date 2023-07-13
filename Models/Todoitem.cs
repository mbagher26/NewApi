using System;

namespace newApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get;set;} = string.Empty;
        // public string Title { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        // public DateTime CreatedAt { get; set; }
        
        public bool IsDelete { get; set;}
        
        public DateTime Created_At { get; set;}

        public String Titele { get; set;}
    }

    
}
