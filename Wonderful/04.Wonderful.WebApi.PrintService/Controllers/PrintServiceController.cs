using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Wonderful.WebApi.PrintService.Models;
using Wonderful.Util.Helper;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using ExcelReport;
using ExcelReport.Driver.NPOI;
using System.IO;
using System.Text;

namespace Wonderful.WebApi.PrintService.Controllers
{
    /// <summary>
    /// 打印服务控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PrintServiceController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly string TokenId = "00000000-0000-0000-0000-000000000001";
        private readonly string TaskId = "00000000-0000-0000-0000-000000000001";
        private readonly string FileName = "blank";
        private readonly string PrintName = "HPC Floor1 Zebra";
        private readonly string TemplateFolder = "PIMTemplate";
        private readonly string OutputFolder = "PIMOutput";
        private readonly string TemplateFolder_Excel = "PIMTemplate\\Excel";
        private readonly string OutputFolder_Excel = "PIMOutput\\Excel";
        private readonly string BartendExePath = ConfigHelper.GetValue("BartendExePath");
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        public PrintServiceController(TodoContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { TokenId = this.TokenId, TaskId = this.TaskId, PrintName = this.PrintName, FileName = this.FileName, BartendExePath=this.BartendExePath, CurrentDirectory = hostingEnvironment.WebRootPath, TemplateFolder= this.TemplateFolder, IsComplete = true});
                _context.SaveChanges();
            }

            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 请求打印
        /// </summary>
        /// <param name="printRequest">请求打印请求</param>
        /// <returns>请求打印结果</returns>
        [EnableCors]
        [HttpPost("PostPrint")]
        public ActionResult<PrintResponse> PostPrint(PrintRequest printRequest)
        {
            //生成TaskId
            string TaskId = Guid.NewGuid().ToString();
            //生成TodoItem并保存
            _context.TodoItems.Add(new TodoItem { TokenId = printRequest.TokenId, TaskId = TaskId, BartendExePath = this.BartendExePath, CurrentDirectory=_hostingEnvironment.WebRootPath, TemplateFolder=this.TemplateFolder, IsComplete=false });
            _context.SaveChanges();
            //返回结果
            PrintResponse printReponse = new PrintResponse { TokenId = printRequest.TokenId, TaskId = TaskId, Result = true, Msg = "" };
            return printReponse;
        }

        /// <summary>
        /// 执行打印
        /// </summary>
        /// <param name="executePrintRequest">执行打印请求</param>
        /// <returns>执行打印响应</returns>
        [EnableCors]
        [HttpPost("ExecutePrint")]
        public ActionResult<ExecutePrintResponse> PostExecutePrint(ExecutePrintRequest executePrintRequest)
        {
            //更新TodoItem
            string taskId = executePrintRequest.TaskId;
            string printName = executePrintRequest.PrintName;
            string printType = executePrintRequest.PrintType;
            int printCount = executePrintRequest.PrintCount;
            string fileName = executePrintRequest.FileName;
            ExecutePrintResponse executePrintResponse = new ExecutePrintResponse { TaskId = taskId, FileName = fileName, PrintName = printName, PrintType = printType, PrintCount = printCount, Result = true, Msg = "PreLoad Success" };

            if (PrinterHelper.GetPrinterStatus(printName) == -1)
            {
                executePrintResponse.Result = false;
                executePrintResponse.Msg = "Not Such Printer In The Server";
            }
            else
            {
                TodoItem todoItem = _context.TodoItems.Find(taskId);
                todoItem.PrintName = printName;
                todoItem.PrintType = printType;
                todoItem.PrintCount = printCount;
                todoItem.FileName = fileName;
                todoItem.IsComplete = true;
                _context.Entry(todoItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                if (printType == "Bartend")
                {
                    //生成预加载Bartender任务
                    string preLoadBartender = executePrintRequest.PreLoadBartender(_hostingEnvironment.WebRootPath, this.TemplateFolder, this.BartendExePath);
                    LogHelper.WriteLog(preLoadBartender, new Exception("PostExecutePrint"));
                    //预加载Bartender
                    //DosCommandOutputHelper.Execute(preLoadBartender, 3000);
                }
                else if (printType == "Excel")
                {
                    Configurator.Put(".xlsx", new WorkbookLoader());
                }
            }

            //执行打印结果
            return executePrintResponse;
        }

        /// <summary>
        /// 执行打印任务
        /// </summary>
        /// <param name="printTaskRequest">执行打印任务请求</param>
        /// <returns>执行打印任务响应</returns>
        [EnableCors]
        [HttpPost("PrintTask")]
        public async Task<ActionResult<PrintTaskResponse>> PostPrintTask(PrintTaskRequest printTaskRequest)
        {
            //更新TodoItem
            string taskId = printTaskRequest.TaskId;
            string fileName = printTaskRequest.FileName;
            string printName = printTaskRequest.PrintName;
            string printType = printTaskRequest.PrintType;
            int printCount = printTaskRequest.PrintCount;
            TodoItem todoItem = _context.TodoItems.Find(printTaskRequest.TaskId);
            todoItem.PrintName = printName;
            todoItem.PrintType = printType;
            todoItem.PrintCount = printCount;
            todoItem.FileName = fileName;
            todoItem.IsComplete = true;
            _context.Entry(todoItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            bool result = true;
            string msg= "Success to print";
            if (printType == "Bartend")
            {
                //生成打印任务
                string printTask = printTaskRequest.GeneratePrintTask(taskId, _hostingEnvironment.WebRootPath, this.TemplateFolder, this.OutputFolder, fileName, this.BartendExePath, printName);
                LogHelper.WriteLog(printTask, new Exception("PostPrintTask"));
                //发送条码打印任务
                await Task.Run(() =>
                {
                    //需要执行Dos命令，
                    //举例：C:\PROGRA~2\Seagull\BARTEN~1\bartend.exe /XMLScript=E:\OA_HOME\Web\OA\AmwayFramework\PrintService.Webapi\wwwroot\PIMOutput\f3d12df1-8d85-4ece-ac24-607e675293eb_ontest.xml /X
                    //方法一，问题是部署到服务器上无法执行，所以采用方法二
                    DosCommandOutputHelper.Execute(printTask, 5000);
                    //方法二，使用System.Diagnostics.Process.Start执行
                    //string[] cmd = printTask.Split(" ");
                    //元素1 C:\PROGRA~2\Seagull\BARTEN~1\bartend.exe 
                    //元素2 XMLScript=E:\OA_HOME\Web\OA\AmwayFramework\PrintService.Webapi\wwwroot\PIMOutput\f3d12df1-8d85-4ece-ac24-607e675293eb_ontest.xml 
                    //元素3 /X
                    //string cmd2 = new StringBuilder(cmd[1]).Append(" ").Append(cmd.Length > 2 ? cmd[2] : "").ToString();
                    //var psi = new System.Diagnostics.ProcessStartInfo(cmd[0], cmd2);
                    //System.Diagnostics.Process.Start(psi);
                    //Thread.Sleep(5000);
                });
                //生成打印任务图片结果
                result = false;
                msg = "Fail to print";
                if (printTaskRequest.ExportPrintPreviewToImage(taskId, _hostingEnvironment.WebRootPath, this.TemplateFolder, fileName, this.BartendExePath, printName) == true)
                {
                    result = true;
                    msg = "Success to print";
                }
            }
            else if (printType == "Excel")
            {
                //根据模板和数据生成Excel文件
                if (!printTaskRequest.GeneratePrintExcel(taskId, _hostingEnvironment.WebRootPath, this.TemplateFolder_Excel, this.OutputFolder_Excel, fileName, out msg))
                {
                    result = false;
                }
                else
                {
                    //根据Excel文件生成Pdf文件
                    string pdfFileName = ExcelHelper.ExcelToPdf(msg);

                    //保存
                    Uri location = new Uri($"{Request.Scheme}://{Request.Host}");
                    todoItem.GenerateExcelFile = msg;
                    todoItem.GeneratePdfFile = $"{location.AbsoluteUri}{this.OutputFolder_Excel.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)}/{pdfFileName}";
                    _context.Entry(todoItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            //请求打印任务结果
            PrintTaskResponse printTaskResponse = new PrintTaskResponse { TaskId = taskId, Result = result, Msg = msg };
            return printTaskResponse;
        }

        /// <summary>
        /// 执行打印任务
        /// </summary>
        /// <param name="printTaskRequest">执行打印任务请求</param>
        /// <returns>执行打印任务响应</returns>
        [EnableCors]
        [HttpPost("PrintTaskExcel")]
        public async Task<ActionResult<PrintTaskResponse>> PostPrintTaskExcel(PrintTaskRequest printTaskRequest)
        {
            string taskId = printTaskRequest.TaskId;
            bool result = true;
            string msg = "Success to print";

            TodoItem todoItem = _context.TodoItems.Find(taskId);
            await Task.Run(() =>
            {
                ExcelHelper.PrintExcel(todoItem.GenerateExcelFile, todoItem.PrintName, todoItem.PrintCount, todoItem.GenerateExcelFile.Replace(".xlsx", ".pdf"));
            });

            //请求打印任务结果
            PrintTaskResponse printTaskResponse = new PrintTaskResponse { TaskId = taskId, Result = result, Msg = msg };
            return printTaskResponse;
        }

        /// <summary>
        /// 获得代办列表
        /// GET: api/printservice
        /// </summary>
        /// <returns>待办列表</returns>
        [EnableCors]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.OrderByDescending(m => m.CreateDate).ToListAsync();
        }

        /// <summary>
        /// 获得一个待办项目
        /// </summary>
        /// <param name="request">待办项目请求</param>
        /// <returns>待办项目</returns>
        [EnableCors]
        [HttpPost("TodoItem")]
        public ActionResult<TodoItem> PostTodoItem(TodoItemRequest request)
        {
            TodoItem todoItem = _context.TodoItems.Find(request.TaskId);
            return todoItem;
        }

    }
}
