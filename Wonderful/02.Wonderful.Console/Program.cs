using System;
using System.Text;
using Wonderful.Util.Helper;

namespace Wonderful.Console1
{
    class Program
    {
        static void Main(string[] args)
        {
            //方法一，使用命令行打印
            //current directory
            string cd = System.IO.Directory.GetCurrentDirectory().getPath();
            //template folder in current directory
            string tmpInCdPath = new StringBuilder(cd).Append("PIMTemplate").ToString().getPath();
            //template file name
            string fileName = "zaokuai.btw";
            //template file path in template folder
            string fileInTmpPath = new StringBuilder(tmpInCdPath).Append(fileName).ToString();
            //xml file name
            string xmlFileName = "zaokuai.xml";
            //xml file path in template folder
            string xmlFileInTmpPath = new StringBuilder(tmpInCdPath).Append(xmlFileName).ToString();
            //bartend.exe path
            string bartendExePath = @"c:\PROGRA~2\Seagull\BARTEN~1\bartend.exe";
            Console.Write(new StringBuilder()
                            .AppendLine("Call bartend from command line:")
                            .AppendLine(
                                DosCommandOutputHelper.Execute(
                                    new StringBuilder("")
                                    .Append(bartendExePath)
                                    //.Append(" /F=").Append(fileInTmpPath) //File
                                    .Append(" /XMLScript=").Append(xmlFileInTmpPath)//XMLScript File
                                    .Append(" /X")//Close File
                                    .ToString(), 3000)
                            )
                            .ToString());
            Console.ReadLine();
        }
    }

}
