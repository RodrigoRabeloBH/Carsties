﻿using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces.Repository;
using AuctionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AuctionService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly ILogger<BaseRepository<T>> _logger;

        protected readonly AuctionDbContext _context;

        public BaseRepository(ILogger<BaseRepository<T>> logger, AuctionDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool>Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task Create(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"[BASE REPOSITORY][CREATE]: {ex.Message}", ex);

                throw;
            }
        }

        public async Task<bool> DeleteById(Guid id)
        {
            try
            {
                var entity = await GetById(id);

                _context.Set<T>().Remove(entity);

                return await _context.SaveChangesAsync() != 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[BASE REPOSITORY][DELETE]: {ex.Message}", ex);

                throw;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _context.Set<T>()
                     .AsNoTracking()
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[BASE REPOSITORY][GET All]: {ex.Message}", ex);

                throw;
            }
        }

        public async Task<T> GetById(Guid id)
        {
            try
            {
                var entity = await _context.Set<T>()
                     .AsNoTracking()
                     .FirstOrDefaultAsync(e => e.Id == id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[BASE REPOSITORY][GET BY ID]: {ex.Message}", ex);

                throw;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);

                return await _context.SaveChangesAsync() != 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[BASE REPOSITORY][ÚPDATE]: {ex.Message}", ex);

                throw;
            }
        }
    }
}
