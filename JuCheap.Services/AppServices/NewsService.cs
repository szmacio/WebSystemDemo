using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using Mehdime.Entity;
using AutoMapper;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 日志契约实现
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public NewsService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        async Task<string> INewsService.AddType(NewsTypeDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<NewsTypeDto, NewsTypeEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.NewsTypeInfos.Add(entity);

                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        public Task<bool> Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        async Task<NewsTypeDto> INewsService.FindType(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.NewsTypeInfos.LoadAsync(id);
                var dto = _mapper.Map<NewsTypeEntity, NewsTypeDto>(entity);
                return dto;
            }
        }

        async Task<PagedResult<NewsTypeDto>> INewsService.SearchType(PageFilter filters)
        {
            if (filters == null)
                return new PagedResult<NewsTypeDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.NewsTypeInfos
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.NewsTypeTitle.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new NewsTypeDto
                    {
                        Id = x.Id,
                        NewsTypeTitle = x.NewsTypeTitle,
                        CreateDateTime = x.CreateDateTime,

                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        async Task<bool> INewsService.Update(NewsInfoDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.NewsInfos.LoadAsync(dto.Id);
                entity.NewsTitle = dto.NewsTitle;
                entity.NewsContent = dto.NewsContent;
                entity.Write = dto.Write;
                await scope.SaveChangesAsync();
                return true;
            }
        }
        async Task<bool> INewsService.UpdateType(NewsTypeDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.NewsTypeInfos.LoadAsync(dto.Id);
                entity.NewsTypeTitle = dto.NewsTypeTitle;

                await scope.SaveChangesAsync();
                return true;
            }
        }
        async Task<string> INewsService.Add(NewsInfoDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<NewsInfoDto, NewsInfoEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.NewsInfos.Add(entity);
                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        async Task<PagedResult<NewsInfoDto>> INewsService.Search(PageFilter filters)
        {
            if (filters == null)
                return new PagedResult<NewsInfoDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.NewsInfos
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.NewsTitle.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new NewsInfoDto
                    {
                        Id = x.Id,
                        NewsTypeID = x.newsType.NewsTypeTitle,
                        NewsTitle = x.NewsTitle,
                        CreateDateTime = x.CreateDateTime,

                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        async Task<NewsInfoDto> INewsService.Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.NewsInfos.LoadAsync(id);
                var dto = _mapper.Map<NewsInfoEntity, NewsInfoDto>(entity);
                return dto;
            }
        }
        async Task<List<NewsInfoDto>>INewsService.FindallNews()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.NewsInfos.ToListAsync();
                var dto = _mapper.Map<List<NewsInfoEntity>, List<NewsInfoDto>>(entity);
                return dto;
            }
        }
        async  Task<List<NewsTypeDto>> INewsService.GetType()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = await db.NewsTypeInfos.ToListAsync();
                var result = _mapper.Map<List<NewsTypeEntity>, List<NewsTypeDto>>(list);
                return result;
            }
        }
    }
}
