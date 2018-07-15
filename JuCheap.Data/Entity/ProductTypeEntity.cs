


using System;
using System.Collections.Generic;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 单页
    /// </summary>
    public class ProductTypeEntity : BaseEntity
    {

        public ProductTypeEntity()
        {
            Roles = new List<RoleEntity>();
        }

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

        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleEntity> Roles { get; set; }
        public IList<ProductEntity> Products { get; set; }
    }
}
