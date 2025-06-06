﻿using Microsoft.EntityFrameworkCore;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly OnlineRestaurantDbContext _databaseContext;
        public DbSet<T> DbSet { get; }

        public BaseRepository(OnlineRestaurantDbContext databaseContext)
        {
            _databaseContext = databaseContext;
            DbSet = databaseContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(bool includeDeletedEntities = false)
        {
            return await GetRecords(includeDeletedEntities).ToListAsync();
        }

        public List<T> GetAll(bool includeDeletedEntities = false)
        {
            return GetRecords(includeDeletedEntities).ToList();
        }

        public async Task<T?> GetFirstOrDefaultAsync(int primaryKey, bool includeDeletedEntities = false)
        {
            var records = GetRecords(includeDeletedEntities);
            return await records.FirstOrDefaultAsync(record => record.Id == primaryKey);
        }

        public void Insert(params T[] records)
        {
            DbSet.AddRange(records);
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        public async Task SoftDelete(params T[] records)
        {
            foreach (var baseEntity in records)
            {
                baseEntity.DeletedAt = DateTime.UtcNow;
            }
            await Update(records);
        }

        public async Task SoftDeleteByIdAsync(int id)
        {
            var entity = await GetFirstOrDefaultAsync(id);
            if(entity != null)
            {
                await SoftDelete(entity); 
            }
            await SaveChangesAsync();
        }

        public async Task Update(params T[] records)
        {
            foreach (var baseEntity in records)
            {
                baseEntity.ModifiedAt = DateTime.UtcNow;
            }
            DbSet.UpdateRange(records);
            await SaveChangesAsync();
        }

        protected IQueryable<T> GetRecords(bool includeDeletedEntities = false)
        {
            var result = DbSet.AsQueryable();
            if (includeDeletedEntities is false)
            {
                result = result.Where(r => r.DeletedAt == null).OrderBy(r => r.Id);
            }
            return result;
        }
    }
}
