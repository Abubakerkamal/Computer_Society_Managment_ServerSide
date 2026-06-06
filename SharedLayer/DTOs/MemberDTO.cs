using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.DTOs
{
    public class MemberDTO
    {
        public int MemberID { get; set; }

        public DateTime JoinDate { get; set; }

        public int CreatedBy { get; set; }

        public int UserID { get; set; }

        public MemberDTO() { }

        public MemberDTO(int memberID, DateTime joinDate, int createdBy, int userID)
        {
            MemberID = memberID;
            JoinDate = joinDate;
            CreatedBy = createdBy;
            UserID = userID;
        }
    }
}
