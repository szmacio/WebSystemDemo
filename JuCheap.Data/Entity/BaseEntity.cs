using System;
using JuCheap.Infrastructure.Utilities;

namespace JuCheap.Data.Entity
{
    public class BaseEntity : IsDeleteFilter
    {
        public BaseEntity()
        {
            IsDeleted = false;
            CreateDateTime = DateTime.Now;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 是否删除的过滤器
    /// </summary>
    public interface IsDeleteFilter
    {
        bool IsDeleted { get; set; }
    }

    public static class BaseEntityExtention
    {
        public static void Create(this BaseEntity entity)
        {
            entity.Id = BaseIdGenerator.Instance.GetId();
            entity.CreateDateTime = DateTime.Now;
        }
    }
}
