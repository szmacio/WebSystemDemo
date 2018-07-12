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
    public class MessageInfoService : IMessageInfoService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public MessageInfoService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        async Task<string> IMessageInfoService.Add(MessageInfoDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<MessageInfoDto, MessageInfoEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.MessageInfos.Add(entity);
                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        public Task<bool> Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<MessageInfoDto> Find(string id)
        {
            throw new NotImplementedException();
        }

        async Task<PagedResult<MessageInfoDto>> IMessageInfoService.Search(PageFilter filters)
        {
            if (filters == null)
                return new PagedResult<MessageInfoDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.MessageInfos
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.MessTitle.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new MessageInfoDto
                    {
                        Id = x.Id,
                        MessTitle = x.MessTitle,
                        Username = x.Username,
                        Messcontent = x.Messcontent,
                        Linkphoto = x.Linkphoto,
                        CreateDateTime = x.CreateDateTime,

                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        public Task<bool> Update(MessageInfoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
