using CMSBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMSBusinessLayer;
using SharedLayer.DTOs;

namespace CSM_APIs.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<UserResponseDTO>> GetAll()
        {
            
            var list = clsUser.GetAllUsers();

            if (list.Count == 0)
                return NotFound("No users found");

            return Ok(list);
        }

        [HttpGet("{userID}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserResponseDTO> GetUserById(int userID)
        {
            if (userID < 1)
                return BadRequest("Invalid ID");

            var user = clsUser.Find(userID);

            if (user == null)
                return NotFound();

            return Ok(user.UResponseDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserResponseDTO> AddUser(UserDTO userDTO)
        {
            var user = new clsUser(userDTO);

            if(user == null)
            {
                return BadRequest("Invalid Data");

            }
            if (!user.Save())
                return BadRequest("Invalid Data");

            return CreatedAtRoute("GetUserById", new { userID = user.UserID }, user.UResponseDTO);
        }


        [HttpPut("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateUser(int userID,UserDTO userDTO)
        {
            clsUser user = clsUser.Find(userID);

            if (user == null || userID < 1)
            {
                return BadRequest("Invalid data!");

            }
            if (user == null)
            {
                return NotFound($"Student not found with ID = {userID}");
            }
            user.Username = userDTO.Username;
            user.PasswordHash = userDTO.PasswordHash;

            if (!user.Save())
            {
                return Problem("Faild to save");
            }

            return Ok($"User with ID = {userID} updated successfuly");
        }

        [HttpDelete("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int userID)
        {
            if (!clsUser.Delete(userID))
                return NotFound();

            return Ok("Deleted");
        }
    }
}
