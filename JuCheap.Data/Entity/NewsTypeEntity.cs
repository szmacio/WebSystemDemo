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
    public class NewsTypeEntity : BaseEntity
    {

        public NewsTypeEntity()
        {
            Roles = new List<RoleEntity>();
        }

        public string NewsTypeTitle { get; set; }
        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleEntity> Roles { get; set; }
    }
}
