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
    public class UserData
    {
        public static List<UserResponseDTO> GetAllUsers()
        {
            var list = new List<UserResponseDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new UserResponseDTO
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID"))
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

        public static UserDTO? GetUserById(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetUserByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDTO
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID"))
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

        public static int AddUser(UserDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_Add_User", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@StudentID", user.StudentID);

                    var outputId = new SqlParameter("@NewUserID", SqlDbType.Int)
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

        public static bool UpdateUser(UserDTO user)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

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

        public static bool DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", userId);

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

