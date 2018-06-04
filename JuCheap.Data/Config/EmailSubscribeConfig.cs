/*******************************************************************************
* Copyright (C) JuCheap.Com
* 
* Author: dj.wong
* Create Date: 09/04/2017 11:47:14
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
    /// 邮件订阅表配置
    /// </summary>
    public class EmailSubscribeConfig : EntityTypeConfiguration<EmailSubscribeEntity>
    {
        public EmailSubscribeConfig()
        {
            ToTable("EmailSubscribes");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Email).HasColumnType("varchar").IsRequired().HasMaxLength(50);
        }
    }
}
