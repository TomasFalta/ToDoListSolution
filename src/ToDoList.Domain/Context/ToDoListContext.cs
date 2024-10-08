﻿using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;

namespace ToDoList.Domain.Context;

public class ToDoListContext : DbContext
{
	public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}

	public DbSet<ToDoListEntity> ToDoList { get; set; }
}