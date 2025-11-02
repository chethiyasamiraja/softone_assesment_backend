using Application.Wrappers;
using AuthBackend.Modal;

namespace AuthBackend.Interfaces
{

    public interface ITaskService
    {
        Task<Responses<List<TaskEntity>>> GetAllTasksAsync();
        Task<Responses<TaskEntity>> GetTaskByIdAsync(int id);
        Task<Responses<TaskEntity>> CreateTaskAsync(TaskEntity task);
        Task<Responses<TaskEntity>> UpdateTaskAsync(int id, TaskEntity task);
        Task<Responses<bool>> DeleteTaskAsync(int id);
        Task<LoginResponse> ValidateUserAsync(string username, string password);
    }

}
