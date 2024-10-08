using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Entities
{
	public class ToDoListEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateCompleted { get; set; }
	}
}
