﻿using System;
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
    public class ProductService : IProductService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public ProductService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }
  
        async  Task<string> IProductService.AddType(ProductTypeDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<ProductTypeDto, ProductTypeEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.ProductTypes.Add(entity);

                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }
        public Task<string> Add(ProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> Find(string id)
        {
            throw new NotImplementedException();
        }
        async Task<ProductTypeDto> IProductService.FindType(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.ProductTypes.LoadAsync(id);
                var dto = _mapper.Map<ProductTypeEntity, ProductTypeDto>(entity);
                return dto;
            }
        }
        public Task<PagedResult<ProductDto>> Search(PageFilter filters)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedResult<ProductTypeDto>> SearchType(PageFilter filters)
        {
            if (filters == null)
                return new PagedResult<ProductTypeDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.ProductTypes
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.ProTypeTitle.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new ProductTypeDto
                    {
                        Id = x.Id,
                        ProTypeTitle = x.ProTypeTitle,
                        CreateDateTime = x.CreateDateTime,

                    }).PagingAsync(filters.page, filters.rows);
            }
        }
        public Task<bool> Update(ProductDto dto)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IProductService.UpdateType(ProductTypeDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.ProductTypes.LoadAsync(dto.Id);
                entity.ProTypeTitle = dto.ProTypeTitle;
         
                await scope.SaveChangesAsync();
                return true;
            }
        }

    }
}
