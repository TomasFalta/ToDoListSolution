using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Business.Exceptions
{
	/// <summary>
	/// 400
	/// </summary>
	public class BadRequestException : Exception
	{
		public BadRequestException()
		{
		}

		public BadRequestException(string message) : base(message)
		{
		}
	}
}
