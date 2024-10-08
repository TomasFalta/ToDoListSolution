using System.Linq.Expressions;

namespace ToDoList.Business.Extensions;

public static class LinqExtension
{
	#region WhereIf

	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
	{
		return condition ? source.Where(predicate) : source;
	}
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
	{
		return condition ? query.Where(predicate) : query;
	}

	public static IEnumerable<T> WhereIfStringNotNull<T>(this IEnumerable<T> source, string value, Func<T, bool> predicate)
	{
		return !string.IsNullOrEmpty(value) ? source.Where(predicate) : source;
	}

	public static IQueryable<T> WhereIfStringNotNull<T>(this IQueryable<T> query, string value, Expression<Func<T, bool>> predicate)
	{
		return !string.IsNullOrEmpty(value) ? query.Where(predicate) : query;
	}

	public static IEnumerable<T> WhereIfNullableNotNull<T, TU>(this IEnumerable<T> source, TU? nullable, Func<T, bool> predicate) where TU : struct
	{
		return nullable.HasValue ? source.Where(predicate) : source;
	}

	public static IQueryable<T> WhereIfNullableNotNull<T, TU>(this IQueryable<T> query, TU? nullable, Expression<Func<T, bool>> predicate) where TU : struct
	{
		return nullable.HasValue ? query.Where(predicate) : query;
	}

	#endregion


	public static IQueryable<TU> Project<T, TU>(this IQueryable<T> query, Expression<Func<T, TU>> selector)
	{
		return query.Select(selector);
	}

	public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> selector, bool? orderDesc)
	{
		return orderDesc.HasValue && orderDesc.Value ? source.OrderByDescending(selector) : source.OrderBy(selector);
	}
}