namespace ToDoList.Models.Filters
{
	public record ToDoListFilter
	{
		public string? FilterTitle { get; init; }
		public bool? FilterIsCompleted { get; init; }
	}
}
