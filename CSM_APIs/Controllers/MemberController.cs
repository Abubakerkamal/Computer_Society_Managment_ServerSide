using CMSBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLayer.DTOs;

namespace CSM_APIs.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/Member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet(Name = "GetAllMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<MemberDTO>> GetAllMembers()
        {
            List<MemberDTO> membersList = Member.GetAllMembers();

            if (membersList.Count == 0) return NotFound("No members found");

            return Ok(membersList);
        }

        [HttpGet("{MemberID}", Name = "GetMemberByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MemberDTO> GetMemberByID(int MemberID)
        {
            if (MemberID < 1)
            {
                return BadRequest("Invalid ID");
            }

            Member? member = Member.Find(MemberID);

            if (member == null)
            {
                return NotFound($"Member with ID {MemberID} not found.");
            }

            return Ok(member.MDTO);
        }

        [HttpPost(Name = "AddMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MemberDTO> AddMember(MemberDTO newMember)
        {
            if (newMember == null)
            {
                return BadRequest("Invalid Data");
            }

            Member member = new Member(newMember, Member.enMode.AddNew);

            if (!member.Save())
            {
                return BadRequest("Invalid Data");
            }

            return CreatedAtRoute("GetMemberByID", new { MemberID = member.MemberID }, member.MDTO);
        }

        [HttpPut("{MemberID}", Name = "UpdateMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MemberDTO> UpdateMember(int MemberID, MemberDTO memberUpdate)
        {
            if (MemberID < 1 || memberUpdate == null)
            {
                return BadRequest("Invalid Data");
            }

            Member? member = Member.Find(MemberID);

            if (member == null)
            {
                return NotFound($"Member not found with ID = {MemberID}");
            }

            member.UserID = memberUpdate.UserID;
            member.CreatedBy = memberUpdate.CreatedBy;
            member.JoinDate = memberUpdate.JoinDate;

            if (!member.Save())
            {
                return BadRequest("Invalid Data");
            }

            return Ok($"Member with ID {MemberID} has been updated.");
        }

        [HttpDelete("{MemberID}", Name = "DeleteMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteMember(int MemberID)
        {
            if (MemberID < 1)
            {
                return BadRequest("Invalid ID");
            }

            if (!Member.Delete(MemberID))
            {
                return NotFound($"Member with ID {MemberID} not found.");
            }

            return Ok($"Member with ID {MemberID} has been deleted.");
        }
    }
}
