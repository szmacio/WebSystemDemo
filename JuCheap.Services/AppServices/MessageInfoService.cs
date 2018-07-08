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

        public Task<string> Add(MessageInfoDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<MessageInfoDto> Find(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MessageInfoDto>> Search(PageFilter filters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(MessageInfoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
