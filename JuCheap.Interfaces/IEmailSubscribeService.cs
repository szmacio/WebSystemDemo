using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuCheap.Interfaces
{
    public interface IEmailSubscribeService
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        Task<bool> Subscribe(string email);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="email"></param>
        Task<bool> UnSubscribe(string email);

        /// <summary>
        /// 获取所有的订阅人员
        /// </summary>
        /// <returns></returns>
        Task<IList<string>> GetAll();
    }
}
