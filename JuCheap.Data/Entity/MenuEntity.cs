﻿/*******************************************************************************
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
    /// 菜单
    /// </summary>
    public class MenuEntity : BaseEntity
    {
        public MenuEntity()
        {
            Roles = new List<RoleEntity>();
        }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 路径码=(上级的路径码+当前的Code)
        /// </summary>
        public string PathCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleEntity> Roles { get; set; }
    }
}
