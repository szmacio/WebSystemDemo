
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 登录日志表配置
    /// </summary>
    public class ProductConfig : EntityTypeConfiguration<ProductEntity>
    {
        public ProductConfig()
        {
            ToTable("Product");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            HasOptional(x => x.productType).WithMany(x => x.Products).HasForeignKey(x => x.ProTypeID);
    

        }
    }
}
