using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{

    public class MessageInfoDto
    {
        #region Model
        private string _messtitle;
        private string _username;
        private int? _sex;
        private DateTime? _datetime;
        private string _address;
        private string _linkphoto;
        private string _email;
        private string _messcontent;
        [Display(Name = "留言内容")]
        public string MessTitle
        {
            set { _messtitle = value; }
            get { return _messtitle; }
        }
        [Display(Name = "称谓")]
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        [Display(Name = "性别")]
        /// <summary>
        /// 
        /// </summary>
        public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
   
        public DateTime? DateTime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }
        [Display(Name = "地址")]
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
        [Display(Name = "留言内容")]
        public string Messcontent
        {
            set { _messcontent = value; }
            get { return _messcontent; }
        }

        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }

        #endregion Model
    }
}
