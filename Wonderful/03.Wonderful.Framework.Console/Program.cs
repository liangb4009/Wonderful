using System;
using System.Collections.Generic;
using System.Text;
using Seagull.BarTender.Print;

namespace Wonderful.Framework.Console1
{
    class Program
    {
        static void Main(string[] args)
        {
            //方法二，使用Bartender .Net Print SDK
            //current directory
            string cd = getPath(System.IO.Directory.GetCurrentDirectory());
            //template folder in current directory
            string tmpInCdPath = getPath(new StringBuilder(cd).Append("PIMTemplate").ToString());
            //xml file name
            string xmlFileName = "zaokuai.xml";
            //xml file path in template folder
            string xmlFileInTmpPath = new StringBuilder(tmpInCdPath).Append(xmlFileName).ToString();
            // Initialize a new BarTender print engine 
            using (Engine btEngine = new Engine())
            {
                // Start the BarTender print engine 
                btEngine.Start();
                // Run XML Script as a file 
                string XMLResponse = btEngine.XMLScript(xmlFileInTmpPath, XMLSourceType.ScriptFile);
                // Display returned XML response 
                Console.WriteLine(XMLResponse);
                // Stop the BarTender print engine 
                btEngine.Stop();
            }
            Console.Read();
        }
        /// <summary>
        /// 获得以"\"结尾的路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>以"\"结尾的路径</returns>
        public static string getPath(string path)
        {
            return (path.Substring(path.Length - 1, 1) == @"\") ? path : new StringBuilder(path).Append(@"\").ToString();
        }
    }
}
