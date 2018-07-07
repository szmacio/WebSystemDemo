

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 登录日志表配置
    /// </summary>
    public class MessageInfoConfig : EntityTypeConfiguration<MessageInfoEntity>
    {
        public MessageInfoConfig()
        {
            ToTable("MessageInfo");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            Property(item => item.MessTitle).HasColumnType("varchar").IsRequired().HasMaxLength(200);
   
        }
    }
}
