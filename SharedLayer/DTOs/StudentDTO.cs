using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.DTOs
{
    public class StudentDTO
    {
            public int StudentID { get; set; }
            public string ST { get; set; }

            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string ThirdName { get; set; }
            public string LastName { get; set; }

            public string FullName { get {  return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }

            public string? Email { get; set; }
            public string? Phone { get; set; }

            public byte Major { get; set; }
            public byte Semester { get; set; }

            public string? ImagePath { get; set; }


            public StudentDTO()
            {

            }


            public StudentDTO(int StudentID, string ST, string FirstName, string SecondName, string ThirdName, string LastName, string? Email
                , string? Phone, byte Major, byte Semester, string? ImagePath)
            {
                this.StudentID = StudentID;
                this.ST = ST;
                this.FirstName = FirstName;
                this.SecondName = SecondName;
                this.ThirdName = ThirdName;
                this.LastName = LastName;
                this.Email = Email;
                this.Phone = Phone;
                this.Major = Major;
                this.Semester = Semester;
                this.ImagePath = ImagePath;

            }
        }

    
}
