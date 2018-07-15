

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using JuCheap.Data.Entity;

namespace JuCheap.Data.Config
{
    /// <summary>
    /// 新闻信息
    /// </summary>
    public class NewsInfoConfig : EntityTypeConfiguration<NewsInfoEntity>
    {
        public NewsInfoConfig()
        {
            ToTable("NewsInfo");
            HasKey(item => item.Id);
            HasOptional(x => x.newsType).WithMany(x => x.newsInfos).HasForeignKey(x => x.NewsTypeID);

        }
    }
}
