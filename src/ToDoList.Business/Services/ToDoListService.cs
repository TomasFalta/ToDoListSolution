using Microsoft.EntityFrameworkCore;
using ToDoList.Business.Exceptions;
using ToDoList.Business.Extensions;
using ToDoList.Business.Interfaces;
using ToDoList.Domain.Context;
using ToDoList.Domain.Entities;
using ToDoList.Models.Filters;
using ToDoList.Models.ToDo;

namespace ToDoList.Business.Services;

public class ToDoListService(ToDoListContext context) : IToDoListService
{
	public async Task<IList<ToDoListModel>> GetListAsync(ToDoListFilter filter)
	{
		return await context.ToDoList
			.WhereIf(filter.FilterIsCompleted.HasValue, x => x.IsCompleted)
			.WhereIf(!string.IsNullOrEmpty(filter.FilterTitle), x => x.Title.Contains(filter.FilterTitle))
			.Select(ToDoListModel.Projection)
			.ToListAsync();
	}

	public async Task<ToDoListModel> GetAsync(Guid id)
	{
		ToDoListModel? entity = await context.ToDoList.AsNoTracking()
			.Where(x => x.Id == id)
			.Select(ToDoListModel.Projection)
			.FirstOrDefaultAsync();

		if (entity == null)
			throw new NotFoundException("Entity not found");

		return entity;
	}

	public async Task<ToDoListModel> CreateAsync(ToDoListCreateModel model)
	{
		ToDoListEntity entity = new()
		{
			Id = Guid.NewGuid(),
			Title = model.Title,
			Description = model.Description,
			IsCompleted = false,
			DateCreated = DateTime.Now,
			DateCompleted = null
		};

		context.ToDoList.Add(entity);
		await context.SaveChangesAsync();

		return await GetAsync(entity.Id);
	}

	public async Task<ToDoListModel> UpdateAsync(Guid id, ToDoListUpdateModel model)
	{
		if (model.IsCompleted && model.DateCompleted == null)
			throw new ArgumentException("DateCompleted is required when IsCompleted is true", nameof(model.DateCompleted));

		if (model.DateCompleted.HasValue && model.IsCompleted == false)
			throw new ArgumentException("IsCompleted must be true when DateCompleted is set", nameof(model.IsCompleted));

		ToDoListEntity? entity = await context.ToDoList.FirstOrDefaultAsync(x => x.Id == id);
		if (entity == null)
			throw new NotFoundException("Entity not found");

		entity.Title = model.Title;
		entity.Description = model.Description;
		entity.IsCompleted = model.IsCompleted;
		entity.DateCompleted = model.DateCompleted;

		await context.SaveChangesAsync();

		return await GetAsync(entity.Id);
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		ToDoListEntity? entity = await context.ToDoList.FirstOrDefaultAsync(x => x.Id == id);

		if (entity == null)
			throw new NotFoundException("Entity not found");

		context.ToDoList.Remove(entity);
		int result = await context.SaveChangesAsync();

		return result > 0;
	}
}