namespace ToDoList.Business.Exceptions;

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