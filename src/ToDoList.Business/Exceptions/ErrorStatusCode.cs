using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Business.Exceptions
{
	public enum ErrorStatusCode
	{
		OK = 0,
		Exception = 1,
		ParametersNotSpecified = 2,
		// ...
	}
}
