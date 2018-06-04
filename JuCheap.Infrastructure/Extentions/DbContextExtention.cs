using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JuCheap.Infrastructure.Extentions
{
    /// <summary>
    /// DbContext扩展
    /// </summary>
    public static class DbContextExtention
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="db">数据库对象实例</param>
        /// <param name="sql">需要执行的sql语句</param>
        /// <param name="parameters">自定义参数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public static PagedResult<T> SqlPagerQuery<T>(this Database db, string sql, SqlParameter[] parameters, int pageIndex = 1, int pageSize = 15, string orderBy = "Id")
        {
            var basePageSql = @"SET NOCOUNT ON;
                                SELECT @Total = COUNT(1) FROM({#SQL#}) AS T
                                SELECT r.* FROM(SELECT ROW_NUMBER() OVER(ORDER BY {#OrderBy#}) as RowId, t.* FROM ({#SQL#}) AS t) r
                                WHERE r.RowId >= @StartID AND r.RowId <= @EndID";

            sql = basePageSql.Replace("{#SQL#}", sql).Replace("{#OrderBy#}", orderBy);

            var queryParam = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Total", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int, Size = 4},
                new SqlParameter {ParameterName = "@StartID", Value = (pageIndex - 1)*pageSize + 1, SqlDbType = SqlDbType.Int,Size = 4},
                new SqlParameter {ParameterName = "@EndID", Value = pageIndex * pageSize, SqlDbType = SqlDbType.Int,Size = 4}
            };
            if (parameters.AnyOne())
                queryParam.AddRange(parameters);

            var query = db.SqlQuery<T>(sql, queryParam.ToArray());

            return new PagedResult<T>
            {
                rows = query.ToList(),
                records = Convert.ToInt32(queryParam[0].Value),
                page = pageIndex,
                pagesize = pageSize
            };
        }

        /// <summary>
        /// 根据主键加载数据，如果没有则抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbset"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Load<T>(this DbSet<T> dbset, string id) where T : class
        {
            var entity = dbset.Find(id);
            if (entity == null)
            {
                throw new Exception(string.Format("记录未找到{0}：id={1}", typeof (T).FullName, id));
            }
            return entity;
        }

        /// <summary>
        /// 根据主键异步加载数据，如果没有则抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbset"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(this DbSet<T> dbset, string id) where T : class
        {
            var entity = await dbset.FindAsync(id);
            if (entity == null)
            {
                throw new Exception("记录未找到：id=" + id);
            }
            return entity;
        }
    }
}
