using System;
using System.Data;
using Microsoft.Data.SqlClient;
using SharedLayer.DTOs;

namespace CMSDataAccsessLayer
{
    public class StudentData
    {
        public static List<StudentDTO> GetAllStudents()
        {
            var studentsList = new List<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentsList.Add(new StudentDTO
                            {
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                ST = reader.GetString(reader.GetOrdinal("ST")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                                ThirdName = reader.GetString(reader.GetOrdinal("ThirdName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Major = reader.GetByte(reader.GetOrdinal("Major")),
                                Semester = reader.GetByte(reader.GetOrdinal("Semester")),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           

            return studentsList;
        }

        public static int AddStudent(StudentDTO student)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (var command = new SqlCommand("SP_AddStudent", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ST", student.ST);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@SecondName", student.SecondName);
                    command.Parameters.AddWithValue("@ThirdName", student.ThirdName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)student.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Major", student.Major);
                    command.Parameters.AddWithValue("@Semester", student.Semester);
                    command.Parameters.AddWithValue("@ImagePath", (object?)student.ImagePath ?? DBNull.Value);

                    var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    conn.Open();
                    command.ExecuteNonQuery();

                    return (int)outputIdParam.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;

            
        }

        public static bool UpdateStudent(StudentDTO student)
        {
            int rowsAffected = 0;
            try
            {

                using (var connection = new SqlConnection(ConnectionString.connectionString))
                using (var command = new SqlCommand("SP_UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    command.Parameters.AddWithValue("@ST", student.ST);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@SecondName", student.SecondName);
                    command.Parameters.AddWithValue("@ThirdName", student.ThirdName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)student.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Major", student.Major);
                    command.Parameters.AddWithValue("@Semester", student.Semester);
                    command.Parameters.AddWithValue("@ImagePath", (object?)student.ImagePath ?? DBNull.Value);

                    var rowsAffectedParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(rowsAffectedParam);

                    connection.Open();
                    command.ExecuteNonQuery();
                    rowsAffected = (int)rowsAffectedParam.Value;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

                return rowsAffected > 0;
        }
        

        public static bool DeleteStudent(int studentID)
        {

            try
            {
                using (var connection = new SqlConnection(ConnectionString.connectionString))
                using (var command = new SqlCommand("SP_DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentID);
                    var rowsAffectedParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(rowsAffectedParam);

                    connection.Open();
                    command.ExecuteScalar();
                    int rowsAffected = (int)rowsAffectedParam.Value;
                    return (rowsAffected == 1);


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;

           
        }

        public static StudentDTO? GetStudentById(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            using (SqlCommand command = new SqlCommand("SP_GetStudentByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentID", studentId);

                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {



                            return new StudentDTO
                            {
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                ST = reader.GetString(reader.GetOrdinal("ST")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                                ThirdName = reader.GetString(reader.GetOrdinal("ThirdName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Major = reader.GetByte(reader.GetOrdinal("Major")),
                                Semester = reader.GetByte(reader.GetOrdinal("Semester")),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

    
            }

            return null; //If student not found
        }
    }
}
