using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.ToDo
{
	public record ToDoListUpdateModel
	{
		[Required]
		public string Title { get; init; }
		public string Description { get; init; }
		public bool IsCompleted { get; init; }
		public DateTime? DateCompleted { get; init; }
	}
}
