using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{


 
    public class ProductTypeDto
    {

        #region Model
        private string _protypetitle;

        public DateTime CreateDateTime { get; set; }
        public string Id { get; set; }
        [Display(Name = "产品栏目")]
        /// <summary>
        /// 
        /// </summary>

        public string ProTypeTitle
        {
            set { _protypetitle = value; }
            get { return _protypetitle; }
        }
        #endregion Model
    }
}
