

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 产品分类
    /// </summary>
    public class ProductTypeConfig : EntityTypeConfiguration<ProductTypeEntity>
    {
        public ProductTypeConfig()
        {
            ToTable("ProductType");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(200);
            Property(item => item.ProTypeTitle).HasColumnType("varchar").IsRequired().HasMaxLength(20);
     
        }
    }
}
