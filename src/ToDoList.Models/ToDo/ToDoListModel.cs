using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Models.ToDo
{
	public record ToDoListModel
	{
		public Guid Id { get; init; }
		[Required]
		public string Title { get; init; }
		public string Description { get; init; }
		public bool IsCompleted { get; init; }
		public DateTime DateCreated { get; init; }
		public DateTime? DateCompleted { get; init; }


		public static Expression<Func<ToDoListEntity, ToDoListModel>> Projection
		{
			get
			{
				return x => new ToDoListModel
				{
					Id = x.Id,
					Title = x.Title,
					Description = x.Description,
					IsCompleted = x.IsCompleted,
					DateCreated = x.DateCreated,
					DateCompleted = x.DateCompleted
				};
			}
		}
	}
}
