using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Business.Exceptions
{
	/// <summary>
	/// 404
	/// </summary>
	public class NotFoundException : Exception
	{
		public NotFoundException()
		{
		}

		public NotFoundException(string message) : base(message)
		{
		}
	}
}
