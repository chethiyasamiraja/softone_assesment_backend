using Application.Wrappers;
using AuthBackend.Interfaces;
using AuthBackend.Modal; 
using System.Data;
using System.Data.SqlClient;


namespace AuthBackend.Services
{
    public class TaskService : ITaskService
    {
        private readonly string _connectionString;

        public TaskService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }



        public async Task<Responses<List<TaskEntity>>> GetAllTasksAsync()
        {
            const string spQuery = "sp_GetAllTasks";
            var tasks = new List<TaskEntity>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new SqlCommand(spQuery, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                     
                    using (var reader = await cmd.ExecuteReaderAsync())
                    { 
                        while (await reader.ReadAsync())
                        {
                            tasks.Add(new TaskEntity
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull("Description")
                                    ? null
                                    : reader.GetString("Description"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                DueDate = reader.IsDBNull("DueDate")
                                    ? (DateTime?)null
                                    : reader.GetDateTime("DueDate")
                            });
                        }
                         
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            var message = reader["Message"]?.ToString();
                            var code = Convert.ToInt32(reader["ResponseCode"]);

                            return new Responses<List<TaskEntity>>
                            {
                                Succeeded = code == 200,
                                Message = message!,
                                Data = tasks,
                                ResponseCode = code
                            };
                        }
                    }
                }
            }

            return new Responses<List<TaskEntity>>
            {
                Succeeded = false,
                Message = "Failed to retrieve tasks.",
                ResponseCode = 500
            };
        }



        public async Task<Responses<TaskEntity>> GetTaskByIdAsync(int id)
        {
            const string spQuery = "sp_GetTaskById";
            TaskEntity? task = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(spQuery, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    { 
                        if (await reader.ReadAsync())
                        {
                            task = new TaskEntity
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                DueDate = reader.IsDBNull("DueDate") ? (DateTime?)null : reader.GetDateTime("DueDate")
                            };
                        }
                         
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            var message = reader["Message"]?.ToString();
                            var code = Convert.ToInt32(reader["ResponseCode"]);

                            return new Responses<TaskEntity>
                            {
                                Succeeded = code == 200,
                                Message = message!,
                                Data = task,
                                ResponseCode = code
                            };
                        }
                    }
                }
            }

            return new Responses<TaskEntity>
            {
                Succeeded = false,
                Message = "Unknown error occurred.",
                ResponseCode = 500
            };
        }

        public async Task<Responses<TaskEntity>> CreateTaskAsync(TaskEntity task)
        {
            const string spQuery = "sp_CreateTask";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(spQuery, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", (object)task.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    cmd.Parameters.AddWithValue("@DueDate", (object)task.DueDate ?? DBNull.Value);

                    TaskEntity? savedTask = null;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    { 
                        if (await reader.ReadAsync())
                        {
                            savedTask = new TaskEntity
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                DueDate = reader.IsDBNull("DueDate") ? (DateTime?)null : reader.GetDateTime("DueDate")
                            };
                        }
                         
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            var message = reader["Message"]?.ToString();
                            var code = Convert.ToInt32(reader["ResponseCode"]);

                            return new Responses<TaskEntity>
                            {
                                Succeeded = code == 201 || code == 200,
                                Message = message!,
                                Data = savedTask,
                                ResponseCode = code
                            };
                        }
                    }
                }
            }

            return new Responses<TaskEntity>
            {
                Succeeded = false,
                Message = "Failed to create task.",
                ResponseCode = 500
            };
        }

        public async Task<Responses<TaskEntity>> UpdateTaskAsync(int id, TaskEntity task)
        {
            const string spQuery = "sp_UpdateTask";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(spQuery, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", (object)task.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    cmd.Parameters.AddWithValue("@DueDate", (object)task.DueDate ?? DBNull.Value);

                    TaskEntity? updatedTask = null;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    { 
                        if (await reader.ReadAsync())
                        {
                            updatedTask = new TaskEntity
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                DueDate = reader.IsDBNull("DueDate") ? (DateTime?)null : reader.GetDateTime("DueDate")
                            };
                        }
                         
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            var message = reader["Message"]?.ToString();
                            var code = Convert.ToInt32(reader["ResponseCode"]);

                            return new Responses<TaskEntity>
                            {
                                Succeeded = code == 200,
                                Message = message!,
                                Data = updatedTask,
                                ResponseCode = code
                            };
                        }
                    }
                }
            }

            return new Responses<TaskEntity>
            {
                Succeeded = false,
                Message = "Unknown error occurred.",
                ResponseCode = 500
            };
        }

        public async Task<Responses<bool>> DeleteTaskAsync(int id)
        {
            const string spQuery = "sp_DeleteTask";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(spQuery, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            var message = reader["Message"]?.ToString();
                            var code = Convert.ToInt32(reader["ResponseCode"]);

                            return new Responses<bool>
                            {
                                Succeeded = code == 200,
                                Message = message!,
                                Data = code == 200,
                                ResponseCode = code
                            };
                        }
                    }
                }
            }

            return new Responses<bool>
            {
                Succeeded = false,
                Message = "Unknown error occurred.",
                ResponseCode = 500
            };
        }


        public async Task<LoginResponse> ValidateUserAsync(string username, string password)
        {
            const string spQuery = "sp_ValidateUser";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 

                    using (var cmd = new SqlCommand(spQuery, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username ?? "");
                        cmd.Parameters.AddWithValue("@Password", password ?? "");

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            { 

                                return new LoginResponse
                                {
                                    IsValid = Convert.ToBoolean(reader["IsValid"]),
                                    Message = reader["Message"]?.ToString() ?? "Unknown error.",
                                    ResponseCode = reader.GetInt32("ResponseCode")
                                };
                            }
                        }
                    }
                }
                 
                return new LoginResponse
                {
                    IsValid = false,
                    Message = "Invalid username or password.",
                    ResponseCode = 401
                };
            }
            catch (SqlException ex)
            { 
                Console.WriteLine($"SQL Error: {ex.Number} - {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return new LoginResponse
                {
                    IsValid = false,
                    Message = $"Database error: {ex.Message}",
                    ResponseCode = 500
                };
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation: {ex.Message}");
                return new LoginResponse
                {
                    IsValid = false,
                    Message = "Operation error: " + ex.Message,
                    ResponseCode = 500
                };
            }
            catch (Exception ex)
            {
                // General fallback
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");

                return new LoginResponse
                {
                    IsValid = false,
                    Message = "Internal server error. Check logs.",
                    ResponseCode = 500
                };
            }
        }








    }
}