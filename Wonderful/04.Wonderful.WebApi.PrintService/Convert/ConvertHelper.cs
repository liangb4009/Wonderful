using ExcelReport;
using ExcelReport.Renderers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderful.Util.Extention;
using Wonderful.Util.Helper;
using Wonderful.WebApi.PrintService.Models;

namespace Wonderful.WebApi.PrintService.Controllers
{
    /// <summary>
    /// 内部静态类，提供路径转换公用函数
    /// </summary>
    internal static class CommonConvertHelper
    {
        /// <summary>
        /// 获得以"\"结尾的路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>以"\"结尾的路径</returns>
        public static string getPath(this string path)
        {
            return (path.Substring(path.Length - 1, 1) == @"\") ? path : new StringBuilder(path).Append(@"\").ToString();
        }

        /// <summary>
        /// 获得NameSubString，去掉最后回车换行
        /// </summary>
        /// <param name="nameSubStrings"></param>
        /// <returns></returns>
        public static string getNameSubStrings(this string nameSubStrings)
        {
            return (nameSubStrings.Substring(nameSubStrings.Length - 2, 2) == "\r\n") ? nameSubStrings.Substring(0,nameSubStrings.Length-2) :nameSubStrings;
        }

        public static string getDuplicates(this string duplicate, int count)
        {
            StringBuilder rtn = new StringBuilder(duplicate);
            for (int i = 1; i < count; i++)
            {
                rtn.Append(duplicate);
            }
            return rtn.ToString();
        }

        /// <summary>
        /// 生成打印任务
        /// </summary>
        /// <param name="printTaskRequest">打印任务请求</param>
        /// <param name="TaskId">任务Id</param>
        /// <param name="CurrentDirectory">当前路径</param>
        /// <param name="TemplateFolder">模板文件夹名</param>
        /// <param name="OutputFolder">输出文件夹名</param>
        /// <param name="FileName">文件名</param>
        /// <param name="BartendExePath">Bartender执行路径</param>
        /// <param name="PrintName">打印机名</param>
        /// <returns></returns>
        public static string GeneratePrintTask(this PrintTaskRequest printTaskRequest, string TaskId, string CurrentDirectory, string TemplateFolder, string OutputFolder, string FileName, string BartendExePath, string PrintName)
        {
            //Step-1，产生NameSubStrings
            StringBuilder NamedSubStrings = new StringBuilder();
            foreach (NamedSubString item in printTaskRequest.TaskContent)
            {
                NamedSubStrings
                    .Append("\t".getDuplicates(3)).Append(string.Format(@"<NamedSubString Name=""{0}"">", item.Name)).Append("\r\n")
                    .Append("\t".getDuplicates(4)).Append(string.Format(@"<Value>{0}</Value>", item.Value)).Append("\r\n")
                    .Append("\t".getDuplicates(3)).Append("</NamedSubString>").Append("\r\n");

            }
            string nameSubStrings = NamedSubStrings.ToString();
            //Step-2，生成图片
            StringBuilder ExportPrintPreviewToImage = new StringBuilder();
            string exportPrintPreviewToImage = ExportPrintPreviewToImage
                .Append("\t".getDuplicates(2)).Append(@"<ExportPrintPreviewToImage ReturnImageInResponse=""false"">").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<Format>").Append("\r\n")
                .Append("\t".getDuplicates(4)).Append(string.Format(@"{0}{1}{2}{3}", CurrentDirectory.getPath(), TemplateFolder.getPath(), FileName, ".btw")).Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("</Format>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<Folder>").Append("\r\n")
                .Append("\t".getDuplicates(4)).Append(string.Format(@"{0}{1}", CurrentDirectory.getPath(), OutputFolder.getPath())).Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("</Folder>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<FileNameTemplate>").Append("\r\n")
                .Append("\t".getDuplicates(4)).Append(string.Format(@"{0}_{1}_{2}{3}", TaskId,"Preview_Label","%PageNumber%",".jpg")).Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("</FileNameTemplate>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<ImageFormatType>").Append("JPG").Append("</ImageFormatType>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<Colors>").Append("btColors24Bit").Append("</Colors>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<DPI>").Append("300").Append("</DPI>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<Overwrite>").Append("true").Append("</Overwrite>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<IncludeMargins>").Append("true").Append("</IncludeMargins>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<IncludeBorder>").Append("true").Append("</IncludeBorder>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<BackgroundColor>").Append("16777215").Append("</BackgroundColor>").Append("\r\n")
                .Append("\t".getDuplicates(2)).Append("</ExportPrintPreviewToImage>").Append("\r\n")
                .ToString();
            //Step-3，产生XmlScripts
            string xmlScripts = new StringBuilder(@"<?xml version=""1.0"" encoding=""utf-8""?>").Append("\r\n")
                .Append(@"<XMLScript Version=""2.0"">").Append("\r\n")
                .Append("\t".getDuplicates(1)).Append(string.Format(@"<Command Name=""{0}"">", TaskId)).Append("\r\n")
                .Append("\t".getDuplicates(2)).Append("<Print>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<Format>").Append("\r\n")
                .Append("\t".getDuplicates(4)).Append(string.Format(@"{0}{1}{2}{3}", CurrentDirectory.getPath(), TemplateFolder.getPath(), FileName,".btw")).Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("</Format>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("<PrintSetup>").Append("\r\n")
                .Append("\t".getDuplicates(4)).Append("<Printer>").Append(PrintName).Append("</Printer>").Append("\r\n")
                .Append("\t".getDuplicates(3)).Append("</PrintSetup>").Append("\r\n")
                .Append(string.Format("{0}", nameSubStrings.getNameSubStrings())).Append("\r\n")
                .Append("\t".getDuplicates(2)).Append("</Print>").Append("\r\n")
                .Append(string.Format("{0}", exportPrintPreviewToImage))
                .Append("\t".getDuplicates(1)).Append("</Command>").Append("\r\n")
                .Append(@"</XMLScript>")
                .ToString();
            //Step-4, 产生XmlScripts写入xml文件
            string path = string.Format(@"{0}{1}{2}_{3}{4}", CurrentDirectory.getPath(), OutputFolder.getPath(),TaskId, FileName, ".xml");
            if (FileHelper.CreateFile(path)) FileHelper.Write(path, xmlScripts);
            //Step-5, 产生打印任务
            StringBuilder sb = new StringBuilder("")
                        .Append(BartendExePath)
                        .Append(" /XMLScript=").Append(path);
            //Step-6，是否打印下一个标签
            if(!printTaskRequest.PrintNext) sb.Append(" /X");
            string rtn = sb.ToString();
            return rtn;
        }
        
        /// <summary>
        /// 预加载Bartender
        /// </summary>
        /// <param name="executePrintRequest">执行打印请求</param>
        /// <param name="CurrentDirectory">当前路径</param>
        /// <param name="TemplateFolder">模板文件夹名</param>
        /// <param name="BartendExePath">Bartender执行路径</param>
        /// <returns></returns>
        public static string PreLoadBartender(this ExecutePrintRequest executePrintRequest, string CurrentDirectory, string TemplateFolder, string BartendExePath)
        {
            //Step-1，产生预加载任务
            string rtn = new StringBuilder("")
                        .Append(BartendExePath)
                        .Append(" /F=").Append(string.Format(@"{0}{1}{2}{3}", CurrentDirectory.getPath(), TemplateFolder.getPath(), "blank", ".btw"))
                        .ToString();
            return rtn;
        }
        
        /// <summary>
        /// 生成打印任务图片结果
        /// </summary>
        /// <param name="executePrintRequest">执行打印请求</param>
        /// <param name="CurrentDirectory">当前路径</param>
        /// <param name="TemplateFolder">模板文件夹名</param>
        /// <returns></returns>
        public static bool ExportPrintPreviewToImage(this ExecutePrintRequest executePrintRequest, string CurrentDirectory, string TemplateFolder)
        {
            bool rtn = false;
            string path = string.Format(@"{0}{1}{2}_{3}_{4}{5}", CurrentDirectory.getPath(), TemplateFolder.getPath(), executePrintRequest.TaskId, "Preview_Label", "1", ".jpg");
            if (FileHelper.Exists(path)) rtn = true;
            return rtn;
        }
        
        /// <summary>
        /// 生成打印任务图片结果
        /// </summary>
        /// <param name="printTaskRequest">打印任务请求</param>
        /// <param name="TaskId">任务Id</param>
        /// <param name="CurrentDirectory">当前路径</param>
        /// <param name="TemplateFolder">模板文件夹名</param>
        /// <param name="FileName">文件名</param>
        /// <param name="BartendExePath">Bartender执行路径</param>
        /// <param name="PrintName">打印机名</param>
        /// <returns></returns>
        public static bool ExportPrintPreviewToImage(this PrintTaskRequest printTaskRequest, string TaskId, string CurrentDirectory, string TemplateFolder, string FileName, string BartendExePath, string PrintName)
        {
            bool rtn = false;
            string path = string.Format(@"{0}{1}{2}_{3}_{4}{5}", CurrentDirectory.getPath(), TemplateFolder.getPath(), TaskId, "Preview_Label", "1", ".jpg");
            if (FileHelper.Exists(path)) rtn = true;
            return rtn;
        }

        public static string ToDebugString(this TodoItem item)
        {
            string rtn = "";
            StringBuilder sb = new StringBuilder();
            foreach (System.Reflection.PropertyInfo p in item.GetType().GetProperties())
            {
                sb.Append(string.Format("{0}:{1}", p.Name, p.GetValue(item))).Append("\r\n");
            }
            rtn = sb.ToString();
            return rtn;
        }

        /// <summary>
        /// 生成待打印的Excel
        /// </summary>
        /// <param name="printTaskRequest">打印任务请求</param>
        /// <param name="taskId">任务Id</param>
        /// <param name="currentDirectory">当前路径</param>
        /// <param name="templateFolder">模板文件夹名</param>
        /// <param name="outputFolder">输出文件夹名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">输出信息</param>
        /// <returns></returns>
        public static bool GeneratePrintExcel(this PrintTaskRequest printTaskRequest, string taskId, string currentDirectory, string templateFolder, string outputFolder, string fileName, out string msg)
        {
            try
            {
                List<SheetRenderer> list = new List<SheetRenderer>();

                foreach (var sheet in printTaskRequest.ExcelTaskContent)
                {
                    List<IElementRenderer> elementRenderers = new List<IElementRenderer>();
                    //单元格内容
                    foreach (var cell in sheet.CellContent)
                    {
                        elementRenderers.Add(new ParameterRenderer(cell.Name, cell.Value));
                    }
                    //表格内容
                    foreach (var table in sheet.TableContent)
                    {
                        List<JObject> obj = new List<JObject>();
                        foreach(var content in table.Content)
                        {
                            obj.Add(content.ToString().ToJObject());
                        }

                        if (obj.Count == 0) continue;

                        List<ParameterRenderer<JObject>> parameterRenderers = new List<ParameterRenderer<JObject>>();
                        var properties = obj[0].Properties();
                        foreach (var property in properties)
                        {
                            parameterRenderers.Add(new ParameterRenderer<JObject>(property.Name, m => m[property.Name].ToString()));
                        }
                        elementRenderers.Add(new RepeaterRenderer<JObject>(table.TableName, obj, parameterRenderers.ToArray()));
                    }

                    list.Add(new SheetRenderer(sheet.SheetName, elementRenderers.ToArray()));
                }

                if (list.Count > 0)
                {
                    string templateExcel = Path.Combine(currentDirectory, templateFolder, $"{fileName}.xlsx");
                    string outputExcel = Path.Combine(currentDirectory, outputFolder, $"{taskId}_{fileName}.xlsx");
                    ExportHelper.ExportToLocal(templateExcel, outputExcel, list.ToArray());

                    msg = outputExcel;
                    return true;
                }
                else
                {
                    msg = "Fail to generate excel, bad excel content";
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
        }
    }
}
