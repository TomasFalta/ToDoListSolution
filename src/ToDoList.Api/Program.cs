using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Middleware;
using ToDoList.Business.Interfaces;
using ToDoList.Business.Services;
using ToDoList.Domain.Context;
using ToDoList.Models.Filters;
using ToDoList.Models.ToDo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoListContext>(options => { options.UseInMemoryDatabase("ToDoList"); });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IToDoListService, ToDoListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseExceptionMiddleware();

RouteGroupBuilder group = app.MapGroup("/api/v1/todo");

group.MapGet("/list", (IToDoListService toDoListService, [FromQuery] string? filterTitle = null, [FromQuery] bool? filterCompleted = null) =>
	{
		return toDoListService.GetListAsync(new ToDoListFilter
		{
			FilterTitle = filterTitle,
			FilterIsCompleted = filterCompleted
		});
	})
.WithName("GetList");

group.MapGet("{id}", (IToDoListService toDoListService, Guid id) =>
{
	return toDoListService.GetAsync(id);
}).WithName("Get");

group.MapPost("", (IToDoListService toDoListService, ToDoListCreateModel model) =>
{
	return toDoListService.CreateAsync(model);
}).WithName("Post");

group.MapPut("{id}", (IToDoListService toDoListService, Guid id, ToDoListUpdateModel model) =>
{
	return toDoListService.UpdateAsync(id, model);
}).WithName("Put");

group.MapDelete("{id}", (IToDoListService toDoListService, Guid id) =>
{
	return toDoListService.DeleteAsync(id);
}).WithName("Delete");

app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/openapi/v1.json", "v1");
});


app.Run();

