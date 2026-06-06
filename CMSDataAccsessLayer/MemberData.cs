using Microsoft.Data.SqlClient;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSDataAccsessLayer
{
    public class MemberData
    {
        public static List<MemberDTO> GetAllMember()
        {
            var list = new List<MemberDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllMembers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MemberDTO
                            {
                                MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                JoinDate = reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public static MemberDTO? GetMemberByID(int MemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetMemberByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemberID", MemberID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MemberDTO
                            {

                                MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                JoinDate = reader.GetDateTime(reader.GetOrdinal("JoinDate")),
                            };

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static int AddMember(MemberDTO Member)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_AddMember", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CreatedBy", Member.CreatedBy);
                    cmd.Parameters.AddWithValue("@JoinDate", Member.JoinDate);
                    cmd.Parameters.AddWithValue("@UserID", Member.UserID);


                    var outputId = new SqlParameter("@NewMemberID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(outputId);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputId.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        public static bool UpdateMember(MemberDTO member)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_UpdateMember", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MemberID", member.MemberID);
                    cmd.Parameters.AddWithValue("@JoinDate", member.JoinDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", member.CreatedBy);
                    cmd.Parameters.AddWithValue("@UserID", member.CreatedBy);

                    var rowsParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(rowsParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    rowsAffected = (int)rowsParam.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rowsAffected > 0;
        }

        public static bool DeleteMember(int MemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteMember", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MemberID", MemberID);

                    var rowsParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(rowsParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int rows = (int)rowsParam.Value;

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
