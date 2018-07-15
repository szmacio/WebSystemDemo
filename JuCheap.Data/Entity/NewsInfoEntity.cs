


using System;
using System.Collections.Generic;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 单页
    /// </summary>
    public class NewsInfoEntity : BaseEntity
    {

        public NewsInfoEntity()
        {
            Roles = new List<RoleEntity>();
        }

        #region Model
        private string _newstitle;
        private string _write;
        private string _newstypeid;
        private DateTime? _fatime;
        private string _imageurl;
        private int? _hitnum;
        private string _newscontent;
        /// <summary>
        /// 
        /// </summary>
        public string NewsTitle
        {
            set { _newstitle = value; }
            get { return _newstitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Write
        {
            set { _write = value; }
            get { return _write; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewsTypeID
        {
            set { _newstypeid = value; }
            get { return _newstypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Fatime
        {
            set { _fatime = value; }
            get { return _fatime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageURL
        {
            set { _imageurl = value; }
            get { return _imageurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HitNum
        {
            set { _hitnum = value; }
            get { return _hitnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NewsContent
        {
            set { _newscontent = value; }
            get { return _newscontent; }
        }
        #endregion Model
        /// <summary>
        /// 所属角色
        /// </summary>newsType
        public IList<RoleEntity> Roles { get; set; }
        public virtual NewsTypeEntity newsType { get; set; }
    }
}
