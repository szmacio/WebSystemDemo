using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using Mehdime.Entity;
using Newtonsoft.Json;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    public class DatabaseInitService : IDatabaseInitService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public DatabaseInitService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            InitData.Init();
        }

        /// <summary>
        /// 初始化路径码
        /// </summary>
        public async Task<bool> InitPathCode()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = InitData.GetPathCodes();
                var pathCodes = db.PathCodes.ToList();
                db.PathCodes.RemoveRange(pathCodes);
                db.PathCodes.AddRange(list);
                return await scope.SaveChangesAsync() > 0;
            }
        }

        /// <summary>
        /// 初始化省市区数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitAreas()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                //删除以前的数据
                var olds = await db.Areas.ToListAsync();
                db.Areas.RemoveRange(olds);
                var filePath = string.Format("{0}areas-json.json", AppDomain.CurrentDomain.BaseDirectory);
                if (File.Exists(filePath))
                {
                    var areas = JsonConvert.DeserializeObject<IList<AreaEntity>>(File.ReadAllText(filePath));
                    if (areas.AnyOne())
                    {
                        areas.ForEach(x => x.CreateDateTime = DateTime.Now);
                    }
                    db.Areas.AddRange(areas);
                    await scope.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
    }
}
