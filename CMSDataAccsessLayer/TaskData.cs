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
    public class TaskData
    {
        public static List<TaskDTO> GetAllTask()
        {
            var list = new List<TaskDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllTasks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TaskDTO
                            {
                                TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                                TaskTitle = reader.GetString(reader.GetOrdinal("TaskTitle")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Piorty = Convert.ToInt16(  reader.GetInt32(reader.GetOrdinal("Piorty"))),
                                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
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

        public static TaskDTO? GetTaskByID(int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetTaskByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TaskID", TaskID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaskDTO
                            {

                                TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                                TaskTitle = reader.GetString(reader.GetOrdinal("TaskTitle")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Piorty = Convert.ToInt16(reader.GetInt32(reader.GetOrdinal("Piorty"))),
                                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
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

        public static int AddTask(TaskDTO Task)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_AddNewTask", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TaskTitle", Task.TaskTitle);
                    cmd.Parameters.AddWithValue("@Piorty", Task.Piorty);
                    cmd.Parameters.AddWithValue("@Description", Task.Description);
                    cmd.Parameters.AddWithValue("@DueDate", Task.DueDate);


                    var outputId = new SqlParameter("@NewTaskID", SqlDbType.Int)
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

        public static bool UpdateTask(TaskDTO Task)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_UpdateTask", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TaskID", Task.TaskID);
                    cmd.Parameters.AddWithValue("@Piorty", Task.Piorty);
                    cmd.Parameters.AddWithValue("@TaskTitle", Task.TaskTitle);
                    cmd.Parameters.AddWithValue("@Description", Task.Description);
                    cmd.Parameters.AddWithValue("@DueDate", Task.DueDate);


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

        public static bool DeleteTask(int TaskID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString.connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteTask", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TaskID", TaskID);

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
