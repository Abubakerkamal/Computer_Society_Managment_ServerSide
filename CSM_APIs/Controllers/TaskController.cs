using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLayer.DTOs;
using CMSBusinessLayer;

namespace CSM_APIs.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/Task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet(Name = "GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<TaskDTO>> GetAllTasks()
        {
            List<TaskDTO> tasksList = clsTask.GetAllTasks();

            if (tasksList.Count == 0) return NotFound("No tasks found");

            return Ok(tasksList);
        }

        [HttpGet("{TaskID}", Name = "GetTaskByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TaskDTO> GetTaskByID(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("Invalid ID");
            }

            clsTask? task = clsTask.Find(TaskID);

            if (task == null)
            {
                return NotFound($"Task with ID {TaskID} not found.");
            }

            return Ok(task.TDTO);
        }

        [HttpPost(Name = "AddTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TaskDTO> AddTask(TaskDTO newTask)
        {
            if (newTask == null)
            {
                return BadRequest("Invalid Data");
            }

            clsTask task = new clsTask(newTask, clsTask.enMode.AddNew);

            if (!task.Save())
            {
                return BadRequest("Invalid Data");
            }

            return CreatedAtRoute("GetTaskByID", new { TaskID = task.TaskID }, task.TDTO);
        }

        [HttpPut("{TaskID}", Name = "UpdateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TaskDTO> UpdateTask(int TaskID, TaskDTO taskUpdate)
        {
            if (TaskID < 1 || taskUpdate == null)
            {
                return BadRequest("Invalid Data");
            }

            clsTask? task = clsTask.Find(TaskID);

            if (task == null)
            {
                return NotFound($"Task not found with ID = {TaskID}");
            }

            task.TaskTitle = taskUpdate.TaskTitle;
            task.Description = taskUpdate.Description;
            task.Piorty = taskUpdate.Piorty;
            task.DueDate = taskUpdate.DueDate;

            if (!task.Save())
            {
                return BadRequest("Invalid Data");
            }

            return Ok($"Task with ID {TaskID} has been updated.");
        }

        [HttpDelete("{TaskID}", Name = "DeleteTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteTask(int TaskID)
        {
            if (TaskID < 1)
            {
                return BadRequest("Invalid ID");
            }

            if (!clsTask.Delete(TaskID))
            {
                return NotFound($"Task with ID {TaskID} not found.");
            }

            return Ok($"Task with ID {TaskID} has been deleted.");
        }
    }
}
