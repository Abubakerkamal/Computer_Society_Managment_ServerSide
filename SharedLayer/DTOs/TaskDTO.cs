using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.DTOs
{
    public class TaskDTO
    {
       public int TaskID { get; set; }
       public  string TaskTitle { get; set; }
        
       public short Piorty { get; set; }

       public string Description { get; set; }

       public DateTime DueDate { get; set; }

        public TaskDTO() { }
        public TaskDTO(int taskID, string taskTitle, short piorty, string description, DateTime dueDate)
        {
            TaskID = taskID;
            TaskTitle = taskTitle;
            Piorty = piorty;
            Description = description;
            DueDate = dueDate;
        }
    }
}
