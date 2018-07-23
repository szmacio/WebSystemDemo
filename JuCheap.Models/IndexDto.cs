using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{



    public class IndexDto
    {
        public IList<ProductTypeDto> productTypes { get; set; }
        public IList<ProductDto> products { get; set; }
        public IList<NewsInfoDto> newsInfos { get; set; }
        public IList<NewsTypeDto> newsTypes { get; set; }
        public IList<SinglePageDto> SinglePages { get; set; }
        public IList<MessageInfoDto> Messages { get; set; }
        public IPagedList<ProductDto> Pagedproducts { get; set; }
        public IPagedList<NewsInfoDto> PagednewsInfos { get; set; }
    }
}
