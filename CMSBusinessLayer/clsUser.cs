using CMSDataAccsessLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBusinessLayer
{
    public class clsUser
    {

            public enum enMode { AddNew = 0, Update = 1 }

            private enMode Mode;

            public int UserID { get; set; }
            public string Username { get; set; }
            public string PasswordHash { get; set; }
            public int StudentID { get; set; }
            
            public Student Student { get; set; }

            public UserResponseDTO UResponseDTO
            {
                get
                {
                    return new UserResponseDTO(UserID, Username, StudentID);
                }
            }

            public UserDTO UDTO
        {
                get
                {
                    return new UserDTO(UserID, Username, PasswordHash, StudentID);
                }
        }

        public clsUser(UserDTO dto, enMode Mode = enMode.AddNew)
            {
                this.UserID = dto.UserID;
                this.Username = dto.Username;
                this.PasswordHash = dto.PasswordHash;
                this.StudentID = dto.StudentID;
                this.Mode = Mode;
                this.Student = Student.Find(StudentID);
            }


            public static List<UserResponseDTO> GetAllUsers()
            {
                return UserData.GetAllUsers();
            }

            public static clsUser? Find(int id)
            {
                var dto = UserData.GetUserById(id);

                if (dto == null) return null;

                return new clsUser(dto,enMode.Update);
            }

            private bool _AddNew()
            {
                UserID = UserData.AddUser(UDTO);
                return UserID > 0;
            }

            private bool _Update()
            {
                return UserData.UpdateUser(UDTO);
            }

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

            public static bool Delete(int id)
            {
                return UserData.DeleteUser(id);
            }
        }
    }

