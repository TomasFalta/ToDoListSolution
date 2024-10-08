using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.ToDo;

public record ToDoListUpdateModel
{
	[Required]
	public string Title { get; init; }
	public string Description { get; init; }
	public bool IsCompleted { get; init; }
	public DateTime? DateCompleted { get; init; }
}