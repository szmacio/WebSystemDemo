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

using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 菜单表配置
    /// </summary>
    public class DepartmentConfig : EntityTypeConfiguration<DepartmentEntity>
    {
        public DepartmentConfig()
        {
            ToTable("Department");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(item => item.FullName).HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
        }
    }
}
