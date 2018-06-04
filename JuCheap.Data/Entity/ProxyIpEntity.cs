using System;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// Ip代理库
    /// </summary>
    public class ProxyIpEntity : BaseEntity
    {
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        public int UseTimes { get; set; }

        /// <summary>
        /// 使用成功的次数
        /// </summary>
        public int SuccessTimes { get; set; }

        /// <summary>
        /// 成功率
        /// </summary>
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// 上一次更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
    }
}
