using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{



    public class NewsTypeDto
    {
        public DateTime CreateDateTime { get; set; }
        public string Id { get; set; }
        [Display(Name = "新闻类别")]
        public string NewsTypeTitle { get; set; }

    }
}
