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
    public class SinglePageService : ISinglePageService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public SinglePageService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        public async Task<PagedResult<SinglePageDto>> Search(PageFilter filters)
        {
            if (filters == null)
                return new PagedResult<SinglePageDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.SinglePages
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.PageName.Contains(filters.keywords) ||
                                                                 x.Title.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new SinglePageDto
                    {
                        Id = x.Id,
                        PageName = x.PageName,
                        Title = x.Title,
                        Content = x.Content,
                        Author = x.Author,
                        CreateDateTime = x.CreateDateTime,

                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        async Task<string> ISinglePageService.Add(SinglePageDto singlepage)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<SinglePageDto, SinglePageEntity>(singlepage);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.SinglePages.Add(entity);

                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        async Task<bool> ISinglePageService.Delete(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entities = await db.SinglePages.Where(item => ids.Contains(item.Id)).ToListAsync();
                foreach (var menuEntity in entities)
                {
                    menuEntity.IsDeleted = true;
                }
                await scope.SaveChangesAsync();
                return true;
            }
        }

         async Task<SinglePageDto> ISinglePageService.Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                 var entity = await db.SinglePages.LoadAsync(id);
                var dto = _mapper.Map<SinglePageEntity,SinglePageDto>(entity);
                return dto;
            }
        }
        /// <summary>
        /// 更新单页
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
     async Task<bool> ISinglePageService.Update(SinglePageDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.SinglePages.LoadAsync(dto.Id);
                entity.PageName = dto.PageName;
                entity.Author = dto.Author;
                entity.Content = dto.Content;
                await scope.SaveChangesAsync();
                return true;
            }
        }
    }
}
