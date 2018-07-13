using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{


  
    public class NewsInfoDto
    {
        #region Model
        private string _newstitle;
        private string _write;
        private int? _newstypeid;
        private DateTime? _fatime;
        private string _imageurl;
        private int? _hitnum;
        private string _newscontent;
        [Display(Name = "新闻标题")]
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
        [Display(Name = "新闻类别")]
        /// <summary>
        /// 
        /// </summary>
        public int? NewsTypeID
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
        [Display(Name = "新闻图片")]
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
        [Display(Name = "新闻内容")]
        /// <summary>
        /// 
        /// </summary>
        public string NewsContent
        {
            set { _newscontent = value; }
            get { return _newscontent; }
        }

        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        #endregion Model

    }
}
