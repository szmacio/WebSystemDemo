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
    /// 代理Ip表配置
    /// </summary>
    public class ProxyIpConfig : EntityTypeConfiguration<ProxyIpEntity>
    {
        public ProxyIpConfig()
        {
            ToTable("ProxyIps");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.Ip).HasColumnType("varchar").IsRequired().HasMaxLength(20);
            Property(item => item.Port).IsRequired();
            Property(item => item.UseTimes).IsRequired();
            Property(item => item.SuccessTimes).IsRequired();
            Property(item => item.SuccessRate).HasPrecision(18, 2);
            Property(item => item.LastUpdateTime).IsOptional();
        }
    }
}
