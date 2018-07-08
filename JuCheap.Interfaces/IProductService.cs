using System.Collections.Generic;
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


        Task<string> Add(ProductDto dto);
   
        Task<PagedResult<ProductDto>> Search(PageFilter filters);


        Task<bool> Update(ProductDto dto);


        Task<ProductDto> Find(string id);

   
        Task<bool> Delete(IEnumerable<string> ids);


    }
}
