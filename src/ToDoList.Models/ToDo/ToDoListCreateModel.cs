using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.ToDo;

public record ToDoListCreateModel
{
	[Required]
	public string Title { get; init; }
	public string Description { get; init; }
}