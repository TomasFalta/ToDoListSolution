using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;
using ToDoList.Api.Middleware.Models;
using ToDoList.Business.Exceptions;

namespace ToDoList.Api.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			if (httpContext.Request.Path.StartsWithSegments("/api"))
			{
				try
				{
					await _next(httpContext);
					return;
				}
				catch (CodeMessageException ex)
				{
					await HandleCodeMessageExceptionAsync(httpContext, ex);
					return;
				}
				catch (BadRequestException ex)
				{
					//Log.Error(ex, "Validation Exception");
					await HandleValidationExceptionAsync(httpContext, ex);
					return;
				}
				catch (NotFoundException ex)
				{
					//Log.Warning(ex.Message);
					await HandleNotFoundExceptionAsync(httpContext, ex);
					return;
				}
				catch (Exception ex)
				{
					//Log.Fatal(ex, "Unknown Exception");
					await HandleExceptionAsync(httpContext, ex);
					return;
				}
			}

			await _next(httpContext);
		}

		private static async Task HandleCodeMessageExceptionAsync(HttpContext context, CodeMessageException exception)
		{
			// todo: log if necessary

			context.Response.StatusCode = (int)exception.HttpStatusCode;
			await WriteCodeMessage(context, exception.ErrCode, exception.ErrMessage);
			await Task.FromResult(context);
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			await WriteCodeMessage(context, ErrorStatusCode.Exception, exception.Message);
			await Task.FromResult(context);
		}

		private static async Task HandleNotFoundExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			await WriteCodeMessage(context, ErrorStatusCode.Exception, exception.Message);
			await Task.FromResult(context);
		}

		private static async Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			await WriteCodeMessage(context, ErrorStatusCode.Exception, exception.Message);
			await Task.FromResult(context);
		}

		private static async Task HandleForbiddenAccessExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
			await WriteCodeMessage(context, ErrorStatusCode.Exception, exception.Message);
			await Task.FromResult(context);
		}

		private static async Task HandleValidationExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			await WriteCodeMessage(context, ErrorStatusCode.Exception, exception.Message);
			await Task.FromResult(context);
		}

	
		private static async Task WriteCodeMessage(HttpContext context, ErrorStatusCode errCode, string errMessage)
		{
			var response = new CodeMessageModel
			{
				ErrCode = (int)errCode,
				ErrMessage = errMessage
			};
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
		}
	}

	public static class ExceptionMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
