﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JuCheap.Infrastructure;
using JuCheap.Models;
using JuCheap.Models.Filters;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 日志契约
    /// </summary>
    public interface IMessageInfoService
    {

        /// <summary>
        /// 添加单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<string> Add(MessageInfoDto dto);
        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<MessageInfoDto>> Search(PageFilter filters);

        /// <summary>
        /// 更新单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<bool> Update(MessageInfoDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<MessageInfoDto> Find(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<string> ids);


    }
}