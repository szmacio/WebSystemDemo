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

        Task<bool> ISinglePageService.Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        Task<SinglePageDto> ISinglePageService.Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                 var entity = await db.SinglePages.LoadAsync(id);
                var dto = _mapper.Map<SinglePageDto, SinglePageEntity>(entity);
                if (entity.Department != null)
                {
                    dto.DepartmentName = entity.Department.FullName;
                }
                return dto;
            }
        }

        Task<bool> ISinglePageService.Update(SinglePageDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
