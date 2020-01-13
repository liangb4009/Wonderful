using System;
using System.Collections.Generic;
using System.Text;
using Spire.Xls;
using Spire.Pdf;
using System.Drawing.Printing;

namespace Wonderful.Util.Helper
{
    public class ExcelHelper
    {
        public static string ExcelToPdf(string excelFile)
        {
            string pdfFileName, pdfFile;
            pdfFileName  = excelFile.Substring(excelFile.LastIndexOf("\\") + 1);
            pdfFileName = pdfFileName.Substring(0, pdfFileName.LastIndexOf(".")) + ".pdf";
            pdfFile = excelFile.Substring(0, excelFile.LastIndexOf("\\") + 1) + pdfFileName;

            if (!ExcelToPdfWithFreeSpire(excelFile, pdfFile))
            {
                ExcelToPdfWithExcel(excelFile, pdfFile);
            }

            return pdfFileName;
        }

        public static void PrintExcel(string excelFile, string printerName, int printCount, string pdfFile = null)
        {
            if (!PrintExcelWithFreeSpire(excelFile, printerName, printCount, pdfFile))
            {
                PrintExcelWithExcel(excelFile, printerName, printCount);
            }
        }

        private static bool ExcelToPdfWithFreeSpire(string excelFile, string pdfFile)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(excelFile);
                workbook.SaveToFile(pdfFile, Spire.Xls.FileFormat.PDF);

                //判断是否超过免费版限制，3页pdf
                PdfDocument pdfDocument = new PdfDocument(pdfFile);
                if (pdfDocument.Pages.Count > 3)
                {
                    pdfDocument.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static bool ExcelToPdfWithExcel(string excelFile, string pdfFile)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = false;
                workbooks = application.Workbooks;
                workbook = workbooks.Open(excelFile, false, true);
                workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, pdfFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }
                if (workbooks != null)
                {
                    workbooks.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                    application = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return true;
        }

        private static bool PrintExcelWithFreeSpire(string excelFile, string printerName, int printCount, string pdfFile)
        {
            try
            {
                //判断是否超过免费版限制，3页pdf
                if (string.IsNullOrWhiteSpace(pdfFile)) return false;
                PdfDocument pdfDocument = new PdfDocument(pdfFile);
                if (pdfDocument.Pages.Count > 3)
                {
                    pdfDocument.Close();
                    return false;
                }

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(excelFile);
                foreach (var worksheet in workbook.Worksheets)
                {
                    worksheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
                }

                PrintDocument printDocument = workbook.PrintDocument;
                printDocument.DocumentName = excelFile;
                printDocument.PrinterSettings.PrinterName = printerName;
                printDocument.PrinterSettings.Copies = (short)printCount;
                printDocument.Print();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static bool PrintExcelWithExcel(string excelFile, string printerName, int printCount)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = false;
                application.DisplayAlerts = false;
                application.AlertBeforeOverwriting = true;
                workbooks = application.Workbooks;
                workbook = workbooks.Open(excelFile);
                //worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                //worksheet.Activate();
                workbook.PrintOutEx(Type.Missing, Type.Missing, printCount, Type.Missing, printerName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (worksheet != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    worksheet = null;
                }
                if (workbook != null)
                {
                    workbook.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }
                if (workbooks != null)
                {
                    workbooks.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (application != null)
                {
                    application.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
                    application = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return true;
        }
    }
}
