using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{


    public class ProductDto
    {

        #region Model
        private string _proname;
        private int? _protypeid;
        private string _imageurl;
        private int? _proxingid;
        private DateTime? _prochutime;
        private DateTime? _fatime;
        private int? _toujian;
        private int? _hitnum;
        private string _procontent;
        private string _promonery;
        [Display(Name = "产品名称")]
        /// <summary>
        /// 
        /// </summary>
        public string ProName
        {
            set { _proname = value; }
            get { return _proname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProTypeID
        {
            set { _protypeid = value; }
            get { return _protypeid; }
        }
        [Display(Name = "产品图片")]
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
        public int? ProXingID
        {
            set { _proxingid = value; }
            get { return _proxingid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Prochutime
        {
            set { _prochutime = value; }
            get { return _prochutime; }
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
        public int? Toujian
        {
            set { _toujian = value; }
            get { return _toujian; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HitNum
        {
            set { _hitnum = value; }
            get { return _hitnum; }
        }
        [Display(Name = "产品介绍")]
        /// <summary>
        /// 
        /// </summary>
        public string Procontent
        {
            set { _procontent = value; }
            get { return _procontent; }
        }
        [Display(Name = "产品价格")]
        /// <summary>
        /// 
        /// </summary>
        public string ProMonery
        {
            set { _promonery = value; }
            get { return _promonery; }
        }
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        #endregion Model
    }
}
