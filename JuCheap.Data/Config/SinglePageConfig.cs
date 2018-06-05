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

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 登录日志表配置
    /// </summary>
    public class SinglePageConfig : EntityTypeConfiguration<SinglePageEntity>
    {
        public SinglePageConfig()
        {
            ToTable("SinglePage");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.PageName).HasColumnType("varchar").IsRequired().HasMaxLength(20);
            Property(item => item.Author).HasColumnType("varchar").IsOptional().HasMaxLength(20);
            Property(item => item.Title).HasColumnType("varchar").IsOptional().HasMaxLength(20);
            Property(item => item.Content).HasColumnType("varchar").IsOptional().HasMaxLength(2000);
        }
    }
}
