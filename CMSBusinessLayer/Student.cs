using CMSDataAccsessLayer;
using SharedLayer.DTOs;
namespace CMSBusinessLayer
{
    public class Student
    {


       public enum enMode { AddNew= 0, Update=1}
        enMode Mode;    
        public StudentDTO SDTO
        {
            get
            {
                return new StudentDTO(this.StudentID, this.ST, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
           this.Email, this.Phone, this.Major, this.Semester, this.ImagePath);
            }
        }
   
        public int StudentID { get; set; }
        public string ST { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }   

      

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public byte Major { get; set; }
        public byte Semester { get; set; }

        public string? ImagePath { get; set; }


        public Student(StudentDTO student, enMode Mode = enMode.Update)
        {
            this.StudentID = student.StudentID;
            this.ST = student.ST;
            this.FirstName = student.FirstName;
            this.SecondName = student.SecondName;
            this.ThirdName = student.ThirdName;
            this.LastName = student.LastName;
            this.Email = student.Email;
            this.Phone = student.Phone;
            this.Major = student.Major;
            this.Semester = student.Semester;
            this.ImagePath = student.ImagePath;
        }

        static public List<StudentDTO> GetAllStudent()
        {
            return StudentData.GetAllStudents();
        }

        static public Student? Find(int StudentID)
        {
            StudentDTO? studentDTO = StudentData.GetStudentById(StudentID);

            if (studentDTO == null) {
                return null;
            }
            return new Student(studentDTO,enMode.Update);
        }
        private bool _AddNew()
        {
            this.StudentID = StudentData.AddStudent(SDTO);
             
            return (this.StudentID > 0);
        }
        private bool _Update()
        {
            return StudentData.UpdateStudent(SDTO);
        }

        static public bool Delete(int StudentID)
        {
            return StudentData.DeleteStudent(StudentID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew()) 
                    {
                        Mode = enMode.Update; return true;
                    }
                    return false;   
                case enMode.Update:
                    if(_Update()) return true;
                    return false;
               
            }
            return false;
        }
    }
}
