using Microsoft.EntityFrameworkCore;
using OTE.Data.EFCore.Contexts;
using System.ComponentModel.DataAnnotations;

namespace OTE.Data.EFCore.Repositories;

/// <summary>Abstract class that implements virtual repository methods.</summary>
/// <param name="context">The `OteContext` to use for database CRUD.</param>
/// <typeparam name="TEntity">The entity type the repository uses.</typeparam>
public abstract class AbstractRepo<TEntity>(OteContext context)
    where TEntity : class
{
    protected DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>Gets all entities in the table.</summary>
    /// <returns>The entities in the table.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>Inserts an entity into the table.</summary>
    /// <param name="entity">The `TEntity` containing the data to insert into the table.</param>
    /// <returns>The number of entities in the table that got inserted.</returns>
    public virtual async Task<int> Insert(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    /// <summary>Updates an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to update.</param>
    /// <param name="entity">The `TEntity` containing the data to replace the table entity with.</param>
    /// <returns>The number of entities in the table that got updated.</returns>
    public virtual async Task<int> Update(object key, TEntity entity)
    {
        TEntity? target = await _dbSet.FindAsync(key);
        if (target == null)
            return 0;

        foreach (var property in typeof(TEntity).GetProperties())
        {
            if (property.GetCustomAttributes(typeof(KeyAttribute), false).Any())
                continue;

            property.SetValue(target, property.GetValue(entity));
        }

        return await context.SaveChangesAsync();
    }

    /// <summary>Deletes an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to delete.</param>
    /// <returns>The number of entities in the table that got deleted.</returns>
    public virtual async Task<int> Delete(object key)
    {
        TEntity? dbEntity = await _dbSet.FindAsync(key);
        if (dbEntity == null)
            return 0;

        _dbSet.Remove(dbEntity);

        return await context.SaveChangesAsync();
    }
}
