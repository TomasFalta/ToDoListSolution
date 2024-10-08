using ToDoList.Models.Filters;
using ToDoList.Models.ToDo;

namespace ToDoList.Business.Interfaces;

public interface IToDoListService
{
	Task<IList<ToDoListModel>> GetListAsync(ToDoListFilter filter);

	Task<ToDoListModel> GetAsync(Guid id);

	Task<ToDoListModel> CreateAsync(ToDoListCreateModel model);

	Task<ToDoListModel> UpdateAsync(Guid id, ToDoListUpdateModel model);

	Task<bool> DeleteAsync(Guid id);
}