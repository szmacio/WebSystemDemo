using System;
using System.Collections.Generic;
using System.IO;
using JuCheap.Infrastructure.Extentions;
using Spire.Pdf;
using Spire.Xls;
using Spire.Xls.Converter;

namespace JuCheap.Infrastructure
{
    /// <summary>
    /// Spire帮助类
    /// </summary>
    public class SpireHelper
    {
        /// <summary>
        /// 导出Excel，并输出到Response中供用户下载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">名称，不带文件后缀名</param>
        /// <param name="sourceDatas"></param>
        /// <returns></returns>
        public static void ExportToExcel<T>(string name, List<T> sourceDatas) where T : class
        {
            if (!sourceDatas.AnyOne())
                return;
            var book = GetWorkBook(sourceDatas);
            book.SaveToHttpResponse(name + ".xlsx", System.Web.HttpContext.Current.Response);
        }

        /// <summary>
        /// 导出Excel，并保存到本地
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">名称，不带文件后缀名</param>
        /// <param name="sourceDatas">数据源</param>
        /// <returns>返回本地文件的路径，如D:\a.xlsx</returns>
        public static string SaveToExcel<T>(string name, List<T> sourceDatas) where T : class
        {
            if (!sourceDatas.AnyOne())
                return string.Empty;
            var book = GetWorkBook(sourceDatas);
            var now = DateTime.Now;
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}ExcelFiles\\{now.Year}\\{now.Month}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, $"{name}-{now.ToString("yyyyMMddHHmmssfff")}.xlsx");
            book.SaveToFile(filePath);
            return filePath;
        }

        /// <summary>
        /// 导出Pdf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">名称，不带文件后缀名</param>
        /// <param name="pdfWidth">pdf页面宽度</param>
        /// <param name="isView">是否是在线浏览</param>
        /// <param name="pdfHeight">pdf页面高度</param>
        /// <param name="sourceDatas">数据源</param>
        public static void ExportToPdf<T>(string name, List<T> sourceDatas, bool isView = false, int pdfWidth = 0, int pdfHeight = 0) where T : class
        {
            if (!sourceDatas.AnyOne())
                return;
            var book = GetWorkBook(sourceDatas);
            var pdfDocument = new PdfDocument
            {
                PageSettings =
                {
                    Orientation = PdfPageOrientation.Landscape
                }
            };
            //设置宽度和高度
            if (pdfWidth > 0)
            {
                pdfDocument.PageSettings.Width = pdfWidth;
            }
            if (pdfHeight > 0)
            {
                pdfDocument.PageSettings.Height = pdfHeight;
            }
            var settings = new PdfConverterSettings
            {
                TemplateDocument = pdfDocument,
                FitSheetToOnePage = FitToPageType.ScaleWidthDifferentFactor
            };
            pdfDocument = book.SaveToPdf(settings);
            pdfDocument.SaveToHttpResponse(name + ".pdf", System.Web.HttpContext.Current.Response,
                isView ? HttpReadType.Open : HttpReadType.Save);
        }

        /// <summary>
        /// 获取WorkBook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceDatas"></param>
        /// <returns></returns>
        private static Workbook GetWorkBook<T>(List<T> sourceDatas) where T : class
        {
            var book = new Workbook();
            if (!sourceDatas.AnyOne())
                return book;
            //获取要导出的列名
            var columns = sourceDatas[0].GetExportAttribute();
            
            var sheet = book.Worksheets[0];
            var titleStyle = GetTitleStyle(sheet);
            var contentStyle = GetContentStyle(sheet);
            var iCellCount = 1;
            //1.设置表头
            foreach (var col in columns)
            {
                var cell = sheet.Range[1, iCellCount++];
                cell.Text = col.Name;
                cell.Style = titleStyle;
                cell.AutoFitColumns();
                cell.AutoFitRows();
            }
            //2.构造表数据
            for (var i = 0; i < sourceDatas.Count; i++)
            {
                iCellCount = 1;
                var values = sourceDatas[i].GetPropertyValues();
                foreach (var val in values)
                {
                    var cell = sheet.Range[i + 2, iCellCount++];
                    cell.Text = val.Value;
                    cell.Style = contentStyle;
                    cell.AutoFitColumns();
                    cell.AutoFitRows();
                }
            }
            return book;
        }

        /// <summary>
        /// 获取标题通用样式
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private static CellStyle GetTitleStyle(Worksheet worksheet)
        {
            var cellStyle = worksheet.Workbook.Styles.Add("titleStyle");//创建一个style并命名为"titleStyle"
            cellStyle.Font.IsBold = true;//设置字体加粗
            cellStyle.HorizontalAlignment = HorizontalAlignType.Center;//设置文本水平居中
            SetThinBorder(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// 获取内容通用样式
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private static CellStyle GetContentStyle(Worksheet worksheet)
        {
            var cellStyle = worksheet.Workbook.Styles.Add("contentStyle");//创建一个style并命名为"contentStyle"
            SetThinBorder(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// 设置border
        /// </summary>
        /// <param name="cellStyle"></param>
        private static void SetThinBorder(CellStyle cellStyle)
        {
            cellStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
            cellStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
            cellStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
            cellStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        }
    }
}