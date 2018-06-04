using System.Collections.Generic;
using JuCheap.Models;
using System.Threading.Tasks;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 代理Ip服务接口
    /// </summary>
    public interface IProxyIpService
    {
        /// <summary>
        /// 如果不存在则添加代理ip
        /// </summary>
        /// <param name="models"></param>
        Task<bool> AddIfNotExists(List<ProxyIpDto> models);

        /// <summary>
        /// 获取待验证的代理Ip
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<IList<ProxyIpDto>> GetWaitValidateIps(int number);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="success">代理ip是否有效</param>
        Task<bool> Update(string id, bool success);

        /// <summary>
        /// 删除失效的代理Ip地址
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteDisabledProxyIps();
    }
}
