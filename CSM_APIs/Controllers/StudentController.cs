using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMSDataAccsessLayer;
using CMSBusinessLayer;
using SharedLayer.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedLayer.DTOs;

namespace CSM_APIs.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet(Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> studentsList = Student.GetAllStudent();

            if (studentsList.Count == 0) return NotFound("No students found");
            
            return Ok(studentsList); 
               
            
        }
        [HttpGet("{StudentID}",Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentByID(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest("Invalid ID");
            }

            Student? student = Student.Find(StudentID);
            
            if (student == null)
            {
                return NotFound($"Student with ID {StudentID} not found.");
            }

            return Ok(student.SDTO);
        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public ActionResult<StudentDTO> AddStudent(StudentDTO newStudent)
        {
            if (newStudent == null)
            {
                return BadRequest("Invalid Data");
            }

            Student student = new Student(newStudent);

            if (!student.Save())
            {
                return BadRequest("Invalid Data");
            }


            return CreatedAtRoute("GetStudentByID", new { StudentID = student.StudentID }, student.SDTO);
        }


        [HttpPut("{StudentID}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent(int StudentID,StudentDTO studentUpdate)
        {
            if (StudentID < 1||studentUpdate == null)
            {
                return BadRequest("Invalid Data");
            }

            Student? student =  Student.Find(StudentID);
            if (student == null)
            {
                return NotFound($"Student not found with ID = {StudentID}");
            }
            student.ST = studentUpdate.ST;
            student.FirstName = studentUpdate.FirstName;
            student.SecondName = studentUpdate.SecondName;
            student.ThirdName = studentUpdate.ThirdName;
            student.LastName = studentUpdate.LastName;
            student.Major = studentUpdate.Major;
            student.Email = studentUpdate.Email;
            student.Phone = studentUpdate.Phone;
            student.ImagePath = studentUpdate.ImagePath;

            if (!student.Save())
            {
                return BadRequest("Invalid Data");
            }
            

            return Ok($"Student with ID {StudentID} has been updated.");
        }

        [HttpDelete("{StudentID}",Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteStudent(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest("Invalid ID");
            }
            if (!Student.Delete(StudentID))
            {
                return NotFound($"Student with ID {StudentID} not found.");
            }


            return Ok($"Student with ID {StudentID} has been deleted.");
        }

    }
}
