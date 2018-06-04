using System.Threading.Tasks;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 数据库初始化契约
    /// </summary>
    public interface IDatabaseInitService
    {
        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        void Init();

        /// <summary>
        /// 初始化路径码
        /// </summary>
        Task<bool> InitPathCode();

        /// <summary>
        /// 初始化省市区数据
        /// </summary>
        /// <returns></returns>
        Task<bool> InitAreas();
    }
}
