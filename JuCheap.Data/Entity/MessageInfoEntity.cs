


using System;
using System.Collections.Generic;

namespace JuCheap.Data.Entity
{
    /// <summary>
    /// 单页
    /// </summary>
    public class MessageInfoEntity : BaseEntity
    {

        public MessageInfoEntity()
        {
            Roles = new List<RoleEntity>();
        }

        #region Model
        private string _messtitle;
        private string _username;
        private int? _sex;
        private DateTime? _datetime;
        private string _address;
        private string _linkphoto;
        private string _email;
        private string _messcontent;
        /// <summary>
        /// 
        /// </summary>
        public string MessTitle
        {
            set { _messtitle = value; }
            get { return _messtitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Linkphoto
        {
            set { _linkphoto = value; }
            get { return _linkphoto; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Messcontent
        {
            set { _messcontent = value; }
            get { return _messcontent; }
        }
        #endregion Model
        /// <summary>
        /// 所属角色
        /// </summary>
        public IList<RoleEntity> Roles { get; set; }
    }
}
