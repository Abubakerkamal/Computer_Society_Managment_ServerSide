using CMSDataAccsessLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBusinessLayer
{
    public class clsTask
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        public TaskDTO TDTO
        {
            get
            {
                return new TaskDTO(
                    this.TaskID,
                    this.TaskTitle,
                    this.Piorty,
                    this.Description,
                    this.DueDate
                );
            }
        }

        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public short Piorty { get; set; }
        public DateTime DueDate { get; set; }

        public clsTask(TaskDTO TaskDTO, enMode Mode = enMode.AddNew)
        {
            this.TaskID = TaskDTO.TaskID;
            this.Piorty = TaskDTO.Piorty;
            this.TaskTitle = TaskDTO.TaskTitle;
            this.Description = TaskDTO.Description;
            this.DueDate = TaskDTO.DueDate;

            this.Mode = Mode;
        }

        public static List<TaskDTO> GetAllTasks()
        {
            return TaskData.GetAllTask();
        }

        public static clsTask? Find(int TaskID)
        {
            TaskDTO? dto = TaskData.GetTaskByID(TaskID);

            if (dto == null)
                return null;

            return new clsTask(dto, enMode.Update);
        }

        private bool _AddNew()
        {
            this.TaskID = TaskData.AddTask(TDTO);
            return (this.TaskID > 0);
        }

        private bool _Update()
        {
            return TaskData.UpdateTask(TDTO);
        }

        public static bool Delete(int TaskID)
        {
            return TaskData.DeleteTask(TaskID);
        }

        // Save (Smart Function)
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _Update();
            }

            return false;
        }
    }
}

