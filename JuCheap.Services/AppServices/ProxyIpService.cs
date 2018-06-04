using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 代理Ip服务
    /// </summary>
    public class ProxyIpService : IProxyIpService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public ProxyIpService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="models">模型</param>
        /// <returns></returns>
        public async Task<bool> AddIfNotExists(List<ProxyIpDto> models)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                models.ForEach(model =>
                {
                    if (!db.ProxyIps.Any(x => x.Ip == model.Ip && x.Port == model.Port))
                    {
                        var entity = new ProxyIpEntity
                        {
                            Ip = model.Ip,
                            Port = model.Port == 0 ? 80 : model.Port
                        };
                        entity.Create();
                        db.ProxyIps.Add(entity);
                    }
                });
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 获取待验证的代理Ip
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<IList<ProxyIpDto>> GetWaitValidateIps(int number)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                return await db.ProxyIps.OrderBy(x => x.LastUpdateTime).Take(number)
                    .Select(x => new ProxyIpDto
                    {
                        Id = x.Id,
                        Ip = x.Ip,
                        Port = x.Port
                    }).ToListAsync();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="success">代理ip是否有效</param>
        public async Task<bool> Update(string id, bool success)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var ip = await db.ProxyIps.FindAsync(id);
                if (ip != null)
                {
                    ip.UseTimes += 1;
                    if (success)
                    {
                        ip.SuccessTimes += 1;
                    }
                    if (ip.UseTimes > 0)
                    {
                        ip.SuccessRate = Convert.ToDecimal(ip.SuccessTimes) / Convert.ToDecimal(ip.UseTimes);
                    }
                    ip.LastUpdateTime = DateTime.Now;
                }
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 删除失效的代理Ip地址
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDisabledProxyIps()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var sql = @"Delete ProxyIps Where UseTimes>1 And SuccessRate=0;
                            Delete ProxyIps Where UseTimes>2 And SuccessRate<0.5;";
                return await db.Database.ExecuteSqlCommandAsync(sql);
            }
        }
    }
}
