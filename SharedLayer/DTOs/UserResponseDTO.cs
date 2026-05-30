using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.DTOs
{
    public class UserResponseDTO
    {
   
        public int UserID { get; set; }
        public string Username { get; set; }
        public int StudentID { get; set; }

        public UserResponseDTO() { }

        public UserResponseDTO(int userID, string username, int studentID)
        {
            this.UserID = userID;
            this.Username = username;
            this.StudentID = studentID;
        }
     }

    public class UserDTO
    {

        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int StudentID { get; set; }

        public UserDTO() { }

        public UserDTO(int userID, string username, string passwordHash, int studentID)
        {
            this.UserID = userID;
            this.Username = username;
            this.PasswordHash = passwordHash;
            this.StudentID = studentID;
        }
    }
}

