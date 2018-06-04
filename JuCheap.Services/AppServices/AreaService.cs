using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Exceptions;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 省市区契约服务
    /// </summary>
    public class AreaService : IAreaService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public AreaService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto">模型</param>
        /// <returns></returns>
        public async Task<string> Add(AreaDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = _mapper.Map<AreaDto, AreaEntity>(dto);
                entity.Id = BaseIdGenerator.Instance.GetId();
                entity.PathCode = await GetPathCode(db, entity.ParentId);
                entity.SimpleSpelling = entity.Name.GetChineseSpell();
                db.Areas.Add(entity);
                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto">模型</param>
        /// <returns></returns>
        public async Task<bool> Update(AreaDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = db.Areas.Load(dto.Id);
                entity.Name = dto.Name;
                entity.SimpleSpelling = dto.SimpleSpelling;
                entity.FullSpelling = dto.FullSpelling;
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<AreaDto> Find(string id)
        {
            if (id.IsBlank())
                return null;
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Areas.LoadAsync(id);
                var result = _mapper.Map<AreaEntity, AreaDto>(entity);
                if (result.ParentId.IsNotBlank() && result.ParentId != "0")
                {
                    var parent = db.Areas.Find(entity.ParentId);
                    if (parent != null)
                        result.ParentName = parent.Name;
                }
                return result;
            }
        }

        /// <summary>
        /// 根据父ID查询
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        public async Task<IList<TreeDto>> FindByParentId(string parentId)
        {
            parentId = parentId.WithDefaultValueIfEmpty("0");
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Areas.Where(x => x.Enabled && x.ParentId == parentId);
                return await query.Select(x => new TreeDto
                {
                    id = x.Id,
                    name = x.Name,
                    pId = x.ParentId,
                    isParent = db.Areas.Any(c => c.ParentId == x.Id)
                }).ToListAsync();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> Delete(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var areas = await db.Areas.Where(x => ids.Contains(x.Id)).ToListAsync();

                if (!areas.AnyOne())
                {
                    throw new TipInfoException("没有找到任何需要删除的数据");
                }

                if (await db.Areas.AnyAsync(x => x.Enabled && ids.Contains(x.ParentId)))
                {
                    throw new TipInfoException("该节点下还有子节点，请删除子节点");
                }

                foreach (var area in areas)
                {
                    area.IsDeleted = true;
                }
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="id">区域id，可以为空</param>
        /// <param name="name">区域名称</param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id, string name)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Areas.Where(x => x.Name == name)
                    .WhereIf(id.IsNotBlank(), x => x.Id != id && x.ParentId == id);

                return await query.AnyAsync();
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<AreaDto>> Search(BaseFilter filters)
        {
            if (filters == null)
                return new PagedResult<AreaDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Areas
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.Name.Contains(filters.keywords));
                
                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new AreaDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        FullSpelling = item.FullSpelling,
                        SimpleSpelling = item.SimpleSpelling,
                        Enabled = item.Enabled,
                        Level = item.Level,
                        DisplaySequence = item.DisplaySequence
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 更新路劲码
        /// </summary>
        public async Task<bool> UpdatePathCodes()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                while (await db.Areas.AnyAsync(x =>x.PathCode == string.Empty))
                {
                    var query = db.Areas.Where(item =>item.PathCode == string.Empty);
                    var list = query.AsNoTracking().Take(100).ToList();
                    foreach (var area in list)
                    {
                        var entity = db.Areas.Find(area.Id);
                        if (entity != null)
                        {
                            var parent = db.Areas.Find(entity.ParentId);
                            if (parent != null && parent.PathCode.IsNotBlank())
                            {
                                entity.PathCode = await GetPathCode(db, entity.ParentId);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 获取路径码
        /// </summary>
        /// <param name="db"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private async Task<string> GetPathCode(JuCheapContext db, string parentId)
        {
            //顶级页面
            List<string> existCodes;
            var parentPathCode = string.Empty;
            if (parentId.IsBlank())
            {
                var list = await db.Areas
                    .Where(x => (x.ParentId == null || x.ParentId == string.Empty))
                    .Select(x => x.PathCode).ToListAsync();
                existCodes = list.Select(x => x.Trim()).ToList();
            }
            else
            {
                var area = await db.Areas.LoadAsync(parentId);
                parentPathCode = area.PathCode;

                var list = await db.Areas.Where(x => x.ParentId == parentId && x.PathCode != string.Empty)
                    .Select(x => x.PathCode).ToListAsync();
                existCodes = list.Select(x => x.Substring(area.PathCode.Trim().Length, 2)).ToList();
            }
            var pathCode = await db.PathCodes
                    .OrderBy(x => x.Code)
                    .FirstOrDefaultAsync(x => !existCodes.Contains(x.Code));
            return parentPathCode.Trim() + pathCode.Code.Trim();
        }
    }
}
