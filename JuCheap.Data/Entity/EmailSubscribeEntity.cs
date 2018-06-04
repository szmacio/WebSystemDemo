namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 邮件订阅
    /// </summary>
    public class EmailSubscribeEntity : BaseEntity
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
    }
}
