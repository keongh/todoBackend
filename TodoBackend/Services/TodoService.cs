using TodoBackend.Models;

namespace TodoBackend.Services
{
    public class TodoService
    {
        public Todo CreateTodo(string newTask)
        {
            Todo newTodo = new Todo
            {
                Task = newTask,
                Completed = false
            };
            return newTodo;
        } 
    }
}