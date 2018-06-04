using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuCheap.Infrastructure
{
    /// <summary>
    /// 导出数据对象列名
    /// </summary>
    public class ExportColumn
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    /// 导出数据对象
    /// </summary>
    public class ExportData
    {
        public List<ExportColumn> Columns { get; set; }

        public List<dynamic> Values { get; set; } 
    }

    public class ExportDataValue
    {
        public string Value { get; set; }

        public Type Type { get; set; }
    }
}
