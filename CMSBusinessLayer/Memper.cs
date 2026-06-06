using CMSDataAccsessLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBusinessLayer
{
   
        public class Member
        {
            public enum enMode { AddNew = 0, Update = 1 }
            private enMode Mode;

            public MemberDTO MDTO
            {
                get
                {
                    return new MemberDTO(
                        this.MemberID,
                        this.JoinDate,
                        this.CreatedBy,
                        this.UserID
                    );
                }
            }

            public int MemberID { get; set; }
            public int UserID { get; set; }
            public int CreatedBy { get; set; }
            public DateTime JoinDate { get; set; }

            public Member(MemberDTO memberDTO, enMode Mode = enMode.AddNew)
            {
                this.MemberID = memberDTO.MemberID;
                this.UserID = memberDTO.UserID;
                this.CreatedBy = memberDTO.CreatedBy;
                this.JoinDate = memberDTO.JoinDate;

                this.Mode = Mode;
            }

            public static List<MemberDTO> GetAllMembers()
            {
                return MemberData.GetAllMember();
            }

            public static Member? Find(int MemberID)
            {
                MemberDTO? dto = MemberData.GetMemberByID(MemberID);

                if (dto == null)
                    return null;

                return new Member(dto, enMode.Update);
            }

            private bool _AddNew()
            {
                this.MemberID = MemberData.AddMember(MDTO);
                return (this.MemberID > 0);
            }

            private bool _Update()
            {
                return MemberData.UpdateMember(MDTO);
            }

            public static bool Delete(int MemberID)
            {
                return MemberData.DeleteMember(MemberID);
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

