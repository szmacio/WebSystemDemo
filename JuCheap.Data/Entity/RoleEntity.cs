/*******************************************************************************
* Copyright (C) JuCheap.Com
* 
* Author: dj.wong
* Create Date: 09/04/2015 11:47:14
* Description: Automated building by service@JuCheap.com 
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/


using System.Collections.Generic;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class RoleEntity : BaseEntity
    {
        public RoleEntity()
        {
            Users = new List<UserEntity>();
            Menus = new List<MenuEntity>();
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户角色关系
        /// </summary>
        public virtual IList<UserEntity> Users { get; set; } 

        /// <summary>
        /// 角色菜单关系
        /// </summary>
        public virtual IList<MenuEntity> Menus { get; set; } 
    }
}
