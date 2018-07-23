﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JuCheap.Infrastructure;
using JuCheap.Models;
using JuCheap.Models.Filters;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 日志契约
    /// </summary>
    public interface IProductService
    {

        #region 产品类别
       Task<List<ProductDto>> GetAllProducts(); 
       Task<string> Add(ProductDto dto);
        Task<string> AddType(ProductTypeDto dto);
        Task<PagedResult<ProductDto>> Search(PageFilter filters);
        Task<PagedResult<ProductTypeDto>> SearchType(PageFilter filters);
        #endregion
        Task<bool> Update(ProductDto dto);
        Task<bool> UpdateType(ProductTypeDto dto);
        Task<ProductDto> Find(string id);
        Task<ProductTypeDto> FindType(string id);
        Task<List<ProductTypeDto>> GetType();
        Task<bool> Delete(IEnumerable<string> ids);
        Task<List<ProductDto>> GetProductsByType(string id);


    }
}
