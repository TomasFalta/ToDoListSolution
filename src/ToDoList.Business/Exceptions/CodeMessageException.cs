using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Business.Exceptions
{
	public class CodeMessageException : Exception
	{
		public CodeMessageException(ErrorStatusCode errCode, string errMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
		{
			ErrCode = errCode;
			ErrMessage = errMessage;
			HttpStatusCode = httpStatusCode;
		}

		public ErrorStatusCode ErrCode { get; set; }
		public string ErrMessage { get; set; }
		public HttpStatusCode HttpStatusCode { get; set; }
	}
}
