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
    /// 省市区表配置
    /// </summary>
    public class AreaConfig : EntityTypeConfiguration<AreaEntity>
    {
        public AreaConfig()
        {
            ToTable("Area");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(item => item.ParentId).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.FullSpelling).HasColumnType("varchar").HasMaxLength(100);
            Property(item => item.SimpleSpelling).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.PathCode).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Enabled).IsRequired();
        }
    }
}
