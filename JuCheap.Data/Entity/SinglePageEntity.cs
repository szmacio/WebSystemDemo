/*******************************************************************************
* Copyright (C) TOM
* 
* Author: TOM
* Create Date: 05/06/2018 11:47:14
* Description: Automated building by TOM
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/


using System;
using System.Collections.Generic;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 单页
    /// </summary>
    public class SinglePageEntity : BaseEntity
    {

        public SinglePageEntity()
        {
            Roles = new List<RoleEntity>();
        }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 页面内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleEntity> Roles { get; set; }
    }
}
