# Wonderful项目 #
是一个使用谷歌插件和谷歌应用采集硬件数据控制硬件的开源项目。
# Wonderful项目在GitHub上的地址 #
https://github.com/liangb4009/Wonderful。
# Wonderful项目代码结构 #
-  01.Wonderful.Util
    - 这个项目存放了帮助类
-  02.Wonderful.Console
    - 这个项目是一个控制台应用，测试Bartender命令行套打条码
- 03.Wonderful.Framework.Console
    - 这个项目是一个控制台应用，测试Bartender Print SDK套打条码
- 04.Wonderful.WebApi.PrintService
    - 这个项目是一个Web服务，提供网站打印功能
- 05.Wonderful.WebApi.ScaleService
    - 这个项目是一个Web服务，提供网站读取电子秤功能
- 06.Wonderful.Chrome.Extension.Scale
    - 这个项目是一个Chrome扩展，允许网站启动名字为“Web Scale Application”的Chrome应用
- 07.Wonderful.Chrome.Application.Scale
    - 这个项目是一个Chrome应用，安装之后是名字为“Web Scale Application”的Chrome应用，读取梅特勒电子秤
- 08.Wonderful.WebApi.ScheduleService
    - 这个项目是一个Web服务，提供网站调用Windows定时任务功能
- 09.Wonderful.WebApi.ImageService
    - 这个项目是一个Web服务，提供网站拍照功能
- 10.Wonderful.Chrome.Extension.Camera
    - 这个项目是一个Chrome扩展，允许网站启动摄像头拍照
- 11.Wonderful.Chrome.Extension.HautosProduct
    - 这个项目是一个Chrome扩展，允许网站启动名字为“Web Thermo-Hygrometer Application”的Chrome应用
- 12.Wonderful.Chrome.Application.HautosProduct
    - 这个项目是一个Chrome应用，安装之后是名字为“Web Thermo-Hygrometer Application”的Chrome应用，读取华图温湿度数据
- 13.Wonderful.WebApi.HautosProductService
    - 这个项目是一个Web服务，提供网站读取温湿度功能
# Wonderful项目的调试和使用 #
- 1 下载安装软件
    - 1.1 下载地址
https://github.com/liangb4009/Wonderful/tree/master/relate_softwares
    - 1.2 下载内容说明
        - 1.2.1 名字为“ChromeSetup.exe”的软件
            - chrome浏览器在线安装包
        - 1.2.2 名字为“dotnet-sdk-2.2.202-win-x64.exe”的软件
            - 可以在：https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-2.2.202-windows-x64-installer
            地址下载得到
        - 1.2.3 名字为“GoogleChromeEnterpriseBundle64.zip”的软件
            - chrome浏览器离线安装包
        - 1.2.4 名字
            - “vs_community__760131582.1548741376.exe”的软件
            - visual studio 2019社区版
- 2 安装Chrome扩展和应用
    - 2.1 打开谷歌浏览器，要求版本(71.0.3554.0或以上版本)
    - 2.2 在地址栏输入：chrome://extensions/，打开“扩展应用”页面，打开“开发者模式”
    - 2.3 安装Chrome扩展和应用
        - 2.3.1 点击“加载已解压的扩展程序”，选择名字为“06.Wonderful.Chrome.Extension.Scale”的项目所在程序路径，点击“选择文件夹”
如果“06.Wonderful.Chrome.Extension.Scale”安装成功，会出现名字为“Web Scale Extension”的谷歌扩展，目前版本号是“1.3”
        - 2.3.2 点击“加载已解压的扩展程序”，选择名字为“07.Wonderful.Chrome.Application.Scale”的项目所在程序路径，点击“选择文件夹”
如果“07.Wonderful.Chrome.Application.Scale”安装成功，会出现名字为“Web Scale Application”的谷歌应用，目前版本号是“1.4”
        - 2.3.3 点击“加载已解压的扩展程序”，选择名字为“10.Wonderful.Chrome.Extension.Camera”的项目所在程序路径，点击“选择文件夹”
如果“10.Wonderful.Chrome.Extension.Camera”安装成功，会出现名字为“Web Camera Extension”的谷歌扩展，目前版本号是“1.4”
        - 2.3.4 点击“加载已解压的扩展程序”，选择名字为“11.Wonderful.Chrome.Extension.HautosProduct”的项目所在程序路径，点击“选择文件夹”
如果“11.Wonderful.Chrome.Extension.HautosProduct”安装成功，会出现名字为“Web Thermo-Hygrometer Extension”的谷歌扩展，目前版本号是“1.1”
        - 2.3.5 点击“加载已解压的扩展程序”，选择名字为“12.Wonderful.Chrome.Application.HautosProduct”的项目所在程序路径，点击“选择文件夹”
如果“12.Wonderful.Chrome.Application.HautosProduct”安装成功，会出现名字为“Web Thermo-Hygrometer Application”的谷歌应用，目前版本号是“1.0”
安装完成后的结果，参考图片《01-安装Chrome扩展和应用》![《01-安装Chrome扩展和应用》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/01-%E5%AE%89%E8%A3%85Chrome%E6%89%A9%E5%B1%95%E5%92%8C%E5%BA%94%E7%94%A8.jpg?raw=true)
- 3 安装数据库和运行数据库脚本
    - 3.1 安装数据库
我使用的是SQLServer2012数据库
可以从网上自行获取SQLServerExpress最新版本安装并下载
下载地址：https://go.microsoft.com/fwlink/?linkid=866658
    - 3.2 运行数据库脚本
下载地址：https://github.com/liangb4009/Wonderful/tree/master/publish
名字为：“SQLServer_DataStructure.sql”的数据库脚本
安装完成后的结果，参考图片《02-安装数据库和运行脚本》![《02-安装数据库和运行脚本》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/02-%E5%AE%89%E8%A3%85%E6%95%B0%E6%8D%AE%E5%BA%93%E5%92%8C%E8%BF%90%E8%A1%8C%E8%84%9A%E6%9C%AC.jpg?raw=true)
- 4 调试和启动Web服务
    - 4.1 点击名字为“04.Wonderful.WebApi.PrintService”的项目，右键菜单，选择“启动新实例”
如果启动成功，会自动打开打印服务的测试页面，你可以修改URL地址，进入API说明页面。
注意：这个测试页面只能显示测试结果列表不能用作测试，测试方法参考“5 测试Web服务”
    - 4.2 点击名字为“05.Wonderful.WebApi.ScaleService”的项目，右键菜单，选择“启动新实例”
如果启动成功，会自动打开称重服务的测试页面，你可以修改URL地址，进入API说明界面
注意：这个测试页面只能显示测试结果列表不能用作测试，测试方法参考“5 测试Web服务”
    - 4.3 点击名字为“08.Wonderful.WebApi.ScheduleService”的项目，右键菜单，选择“启动新实例”
如果启动成功，但是不会打开定时任务服务的测试页面，因为没有开发测试页面，你可以修改URL地址，在API说明页面测试调用定制任务
注意：这个测试页面只能显示测试结果列表不能用作测试，测试方法参考“5 测试Web服务”
    - 4.4 点击名字为“09.Wonderful.WebApi.ImageService”的项目，右键菜单，选择“启动新实例”
如果启动成功，会自动打开拍照服务的测试页面，你可以修改URL地址，进入API说明页面
注意：这个测试页面只能显示测试结果列表不能用作测试，测试方法参考“5 测试Web服务”
    - 4.5 点击名字为“13.Wonderful.WebApi.HautosProductService”的项目，右键菜单，选择“启动新实例”
如果启动成功，会自动打开华图温湿度计服务的测试页面，你也可以修改URL地址，进入API说明页面
注意：这个测试页面只能显示测试结果列表不能用作测试，测试方法参考“5 测试Web服务”
启动Web服务结果，参考图片
        - 《03-启动打印服务》![《03-启动打印服务》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/03-%E5%90%AF%E5%8A%A8%E6%89%93%E5%8D%B0%E6%9C%8D%E5%8A%A1.jpg?raw=true)
        - 《04-启动称重服务》![《04-启动称重服务》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/04-%E5%90%AF%E5%8A%A8%E7%A7%B0%E9%87%8D%E6%9C%8D%E5%8A%A1.jpg?raw=true)
        - 《05-启动拍照服务》![《05-启动拍照服务》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/05-%E5%90%AF%E5%8A%A8%E6%8B%8D%E7%85%A7%E6%9C%8D%E5%8A%A1.jpg?raw=true)
        - 《06-启动温湿度计服务》![《06-启动温湿度计服务》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/06-%E5%90%AF%E5%8A%A8%E6%B8%A9%E6%B9%BF%E5%BA%A6%E8%AE%A1%E6%9C%8D%E5%8A%A1.jpg?raw=true)
- 5 测试Web服务
    - 5.1 打开Web服务测试页面
        - 需要重新打开一个Chrome浏览器，输入下面地址完成测试
        - 打印测试网页：https://localhost:44351
        - 称重测试网页：https://localhost:44344
        - 拍照测试页面：https://localhost:44333
        - 温湿度计测试页面：https://localhost:44358
    - 5.2 打开API的说明页面
            - 打印: https://localhost:44351/swagger/index.html
            - 称重: https://localhost:44344/swagger/index.html
            - 拍照: https://localhost:44333/swagger/index.html
            - 温湿度计: https://localhost:44358/swagger/index.html
            - Window定时任务: https://localhost:44332/swagger/index.html
        - 打开API页面结果，参考图片
            - 《07-打开打印API页面》![《07-打开打印API页面》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/07-%E6%89%93%E5%BC%80%E6%89%93%E5%8D%B0API%E9%A1%B5%E9%9D%A2.jpg?raw=true)
            - 《08-打开称重API页面》![《08-打开称重API页面》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/08-%E6%89%93%E5%BC%80%E7%A7%B0%E9%87%8DAPI%E9%A1%B5%E9%9D%A2.jpg?raw=true)
            - 《09-打开拍照API页面》![《09-打开拍照API页面》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/09-%E6%89%93%E5%BC%80%E6%8B%8D%E7%85%A7API%E9%A1%B5%E9%9D%A2.jpg?raw=true)
            - 《10-打开温湿度计API页面》![《10-打开温湿度计API页面》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/10-%E6%89%93%E5%BC%80%E6%B8%A9%E6%B9%BF%E5%BA%A6%E8%AE%A1API%E9%A1%B5%E9%9D%A2.jpg?raw=true)
            - 《11-打开Windows定时任务API页面》![《11-打开Windows定时任务API页面》](https://github.com/liangb4009/Wonderful/blob/master/doc/02-pictures/11-%E6%89%93%E5%BC%80Windows%E5%AE%9A%E6%97%B6%E4%BB%BB%E5%8A%A1API%E9%A1%B5%E9%9D%A2.jpg?raw=true)
    - 5.3 测试打印
        - 测试方法，可以参考[《01-PIMNext Web Print》](https://mp.weixin.qq.com/s/IgRTRBTfYJvvzmHBS8GU9Q)
        - 注意：测试打印需要安装Bartender
        - 安装Bartender方法，可以参考文章《01-安装Bartender》
    - 5.4 测试称重
        - 测试方法，可以参考[《02-PIMNext Web Scale》](https://mp.weixin.qq.com/s/KsOB33tCHApX4hfS4ek-Sg)
        - 注意：测试称重需要安装梅特勒电子秤
        - 安装梅特勒电子秤，可以参考[《02-网页读取梅特勒电子秤数据》](https://mp.weixin.qq.com/s/Br9LvZrPgfRQ0hA9sIqrBQ)
    - 5.5 测试拍照
        - 测试方法，可以参考[《03-PIMNext Web Camera》](https://mp.weixin.qq.com/s/0DxyigQiqK9lTGBsCTkNbQ)
    - 5.6 测试温湿度计
        - 测试方法，可以参考[《04-PIMNext Web Thermo-Hygrometer》](https://mp.weixin.qq.com/s/eOXY6pbkb32osApg7jClUA)
        - 注意：测试温湿度计需要安装华图温湿度计，
        - 安装华图温湿度计，可以参考[《03-华图温湿度计配置》](https://mp.weixin.qq.com/s/vjcvutt3nxSVD1_a3IXwaQ)
    - 5.7 测试定时任务
        - 注意：测试定时任务需要安装定时任务执行程序，可以参考文章《04-安装定时任务执行程序》
        - 测试方法：
            - 打开“https://localhost:44332/swagger/index.html”
            - 选择名字为“/api/AmwaySchedule/UpdateSchedule 更新计划任务API”
            - 输入下面参数：
                > {
                >   "scheduleId": "2ff9e139-e836-4c14-8098-16b80af94440",
                >   "scheduleCode": "10012",
                >   "systemCode": "PIM",
                >   "descr": "定时统计干燥间负荷任务",
                >   "startDate": "2019-03-25 14:56:04.000",
                >   "endDate": "2023-03-31 14:55:09.000",
                >   "period": "15",
                >   "retryCount": "5",
                >   "scheduleStatus": "1"
                > }
            - 点击“Execute”
            - 返回200



