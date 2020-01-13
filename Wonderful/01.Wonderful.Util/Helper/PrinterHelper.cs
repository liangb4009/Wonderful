using System;
using System.Drawing.Printing;
using System.Management;

namespace Wonderful.Util.Helper
{
    public class PrinterHelper
    {
        public static int GetPrinterStatus(string printerDevice)
        {
            try
            {
                string path = @"win32_printer.DeviceId='" + printerDevice + "'";
                ManagementObject printer = new ManagementObject(path);
                printer.Get();
                return Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
