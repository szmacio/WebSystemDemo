using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{


 
    public class ProductTypeDto
    {

        #region Model
        private string _protypetitle;
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
