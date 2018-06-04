using System;

namespace JuCheap.Infrastructure.Attributes
{
    /// <summary>
    /// 导出属性
    /// </summary>
    public class ExportPropertyAttribute : Attribute
    {
        private string _name;
        private int _order;

        /// <summary>
        /// 导出列名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public ExportPropertyAttribute()
        {
        }

        public ExportPropertyAttribute(string name,int order)
        {
            _name = name;
            _order = order;
        }
    }
}
