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
    /// 新闻分类
    /// </summary>
    public class NewsTypeConfig : EntityTypeConfiguration<NewsTypeEntity>
    {
        public NewsTypeConfig()
        {
            ToTable("NewsType");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.NewsTypeTitle).HasColumnType("varchar").IsRequired().HasMaxLength(20);

        }
    }
}
