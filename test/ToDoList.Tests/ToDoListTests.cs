using Microsoft.EntityFrameworkCore;
using ToDoList.Business.Exceptions;
using ToDoList.Business.Interfaces;
using ToDoList.Business.Services;
using ToDoList.Domain.Context;
using ToDoList.Domain.Entities;
using ToDoList.Models.Filters;
using ToDoList.Models.ToDo;

namespace ToDoList.Tests
{
	public class ToDoListTests
	{
		private ToDoListContext GetInMemoryDbContext()
		{
			DbContextOptions<ToDoListContext> options = new DbContextOptionsBuilder<ToDoListContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			return new ToDoListContext(options);
		}

		[Fact]
		public async Task GetListAsync_ShouldReturnFilteredResults()
		{
			ToDoListContext context = GetInMemoryDbContext();
			context.ToDoList.AddRange(
				new ToDoListEntity { Id = Guid.NewGuid(), Title = "Task 1", IsCompleted = false },
				new ToDoListEntity { Id = Guid.NewGuid(), Title = "Task 2", IsCompleted = true }
			);
			await context.SaveChangesAsync();

			IToDoListService service = new ToDoListService(context);
			ToDoListFilter filter = new ToDoListFilter { FilterIsCompleted = true };
			
			IList<ToDoListModel> result = await service.GetListAsync(filter);
			
			Assert.Single(result);
			Assert.Equal("Task 2", result.First().Title);
		}

		[Fact]
		public async Task GetAsync_ShouldReturnEntity_WhenEntityExists()
		{
			ToDoListContext context = GetInMemoryDbContext();
			ToDoListEntity entity = new ToDoListEntity { Id = Guid.NewGuid(), Title = "Task 1" };
			context.ToDoList.Add(entity);
			await context.SaveChangesAsync();

			IToDoListService service = new ToDoListService(context);
			
			ToDoListModel result = await service.GetAsync(entity.Id);
			
			Assert.NotNull(result);
			Assert.Equal(entity.Title, result.Title);
		}

		[Fact]
		public async Task GetAsync_ShouldThrowNotFoundException_WhenEntityDoesNotExist()
		{
			ToDoListContext context = GetInMemoryDbContext();
			IToDoListService service = new ToDoListService(context);

			await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsync(Guid.NewGuid()));
		}

		[Fact]
		public async Task CreateAsync_ShouldAddNewEntity()
		{
			ToDoListContext context = GetInMemoryDbContext();
			IToDoListService service = new ToDoListService(context);
			ToDoListCreateModel model = new ToDoListCreateModel { Title = "New Task", Description = "Description" };
			
			ToDoListModel result = await service.CreateAsync(model);

			Assert.NotNull(result);
			Assert.Equal(model.Title, result.Title);
			Assert.Equal(model.Description, result.Description);
		}

		[Fact]
		public async Task UpdateAsync_ShouldUpdateExistingEntity()
		{
			ToDoListContext context = GetInMemoryDbContext();
			ToDoListEntity entity = new ToDoListEntity { Id = Guid.NewGuid(), Title = "Task 1", IsCompleted = false };
			context.ToDoList.Add(entity);
			await context.SaveChangesAsync();

			IToDoListService service = new ToDoListService(context);
			ToDoListUpdateModel model = new ToDoListUpdateModel { Title = "Updated Task", Description = "Updated Description", IsCompleted = true, DateCompleted = DateTime.Now };
			
			ToDoListModel result = await service.UpdateAsync(entity.Id, model);
			
			Assert.NotNull(result);
			Assert.Equal(model.Title, result.Title);
			Assert.Equal(model.Description, result.Description);
			Assert.Equal(model.IsCompleted, result.IsCompleted);
			Assert.Equal(model.DateCompleted, result.DateCompleted);
		}

		[Fact]
		public async Task UpdateAsync_ShouldThrowNotFoundException_WhenEntityDoesNotExist()
		{
			ToDoListContext context = GetInMemoryDbContext();
			IToDoListService service = new ToDoListService(context);
			ToDoListUpdateModel model = new ToDoListUpdateModel { Title = "Updated Task", Description = "Updated Description", IsCompleted = true, DateCompleted = DateTime.Now };

			await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(Guid.NewGuid(), model));
		}

		[Fact]
		public async Task DeleteAsync_ShouldRemoveEntity()
		{
			ToDoListContext context = GetInMemoryDbContext();
			ToDoListEntity entity = new ToDoListEntity { Id = Guid.NewGuid() };
			context.ToDoList.Add(entity);
			await context.SaveChangesAsync();

			IToDoListService service = new ToDoListService(context);
			
			bool result = await service.DeleteAsync(entity.Id);
			
			Assert.True(result);
			Assert.Null(await context.ToDoList.FindAsync(entity.Id));
		}

		[Fact]
		public async Task DeleteAsync_ShouldThrowNotFoundException_WhenEntityDoesNotExist()
		{
			ToDoListContext context = GetInMemoryDbContext();
			IToDoListService service = new ToDoListService(context);

			await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(Guid.NewGuid()));
		}
	}
}
