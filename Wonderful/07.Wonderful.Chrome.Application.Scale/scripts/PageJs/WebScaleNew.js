var app = app || {
    //url: "https://localhost:44344/api/ScaleService/PostScaleSaveValue",//本地环境
    url: "http://10.158.14.35:8089/api/ScaleService/PostScaleSaveValue",//QA环境
    pimupdateurl: "http://10.143.172.99/OA/AmwayFramework/MESEFORM/Handlers/QUAToiletSoapCheckListScan.ashx",
    scalepimparm: {},//pim皂块称重参数   
    pimupdaterequestparm: {},//pim皂块称重请求参数
    soapcutvalidbarcodelist:[],//初始化皂块切割有效的条码
    result: false,
    resultstr: "请选择端口",
    state: "close",
    port: "COM1",
    extentionid: "",
    taskid: "",
    module: "",
    moduletype:"",
    scaletype: "manual", //称重类型：manual/auto
    /**
     * options为端口配置选项，完整的结构如下：
     * {
            persistent: 应用关闭时连接是否保持打开状态,
            name: 与连接相关联的字符串,
            bufferSize: 用于接收数据的缓冲区大小,
            bitrate: 打开连接时请求的比特率,
            dataBits: 默认为"eight",
            parityBit: 默认为"no",
            stopBits: 默认为"one",
            ctsFlowControl: 是否启用RTS/CTS硬件流控制,
            receiveTimeout: 等待新数据的最长时间，以毫秒为单位,
            sendTimeout: 等待send操作完成的最长时间，以毫秒为单位
        }
     * */
    options: {bitrate: 9600 },
    weight: "0.00",
    unit: "g",
    count: 0,
    subcount: 0,
    connectionId: 0,
    tmpstr: "",
    ScaleValueId: 0,
    soapcutValueId:0,//初始化时保存皂块称重切割类型传过来的数据中最大的id,之后用于增加新的条码保存新增的数据的valueid
    icount: null,
    alreadyAddListener: false,
    
    init: function () {
        //get webscale extentionid
     

        chrome.serial.getConnections(function (info) {
            if (info.length > 0) {
                info.forEach(function (val, index, array) {
                    console.log(val.connectionId);
                    if (val.connectionId) {
                        chrome.serial.send(val.connectionId,
                            app.convertStringToArrayBuffer('S'),
                            function (sendInfo) { });
                    }
                });
            }
        });
       
        chrome.storage.local.get('webscaleshowinfo', function (result) {      
            //来自于demo的启动
            if (result.webscaleshowinfo.module == "scaledemo") {
                $("#modulescaledemo").show();
                if (result.webscaleshowinfo.scalesaveurl) {
                    app.url = result.webscaleshowinfo.scalesaveurl;
                }
                $('.scalecountvalue').empty().html(result.webscaleshowinfo.scalecount);  
            } else//来自于pim皂块的称重启动
                if (result.webscaleshowinfo.module == "soap") {
                    //初始化pim称重模块parm
                    scalepimparm = {
                        m: result.webscaleshowinfo.scaleparams.m, amc: result.webscaleshowinfo.scaleparams.amc, cupboardno: result.webscaleshowinfo.scaleparams.cupboardno, surplusquality: result.webscaleshowinfo.scaleparams.surplusquality,
                        batchquality: result.webscaleshowinfo.scaleparams.batchquality, headerid: result.webscaleshowinfo.scaleparams.headerid, batchid: result.webscaleshowinfo.scaleparams.batchid,
                        tokenid: result.webscaleshowinfo.scaleparams.tokenid, currEmpnumber: result.webscaleshowinfo.scaleparams.currEmpnumber
                    };
                    app.scalepimparm = scalepimparm;
                    if (result.webscaleshowinfo.scaleparams.pimsoapupdateurl) {
                        app.pimupdateurl = result.webscaleshowinfo.scaleparams.pimsoapupdateurl;                        
                    }
                    for (var i = 0; i < 15; i++) {
                        var id = i + 1;
                        id = id < 10 ? ("0" + id.toString()) : id.toString();
                        app.soapcutvalidbarcodelist.push(scalepimparm.amc + scalepimparm.cupboardno + id);
                    }

                    $('.scaleamcvalue').empty().html(result.webscaleshowinfo.scaleparams.amc);
                    $('.scalecupboardnovalue').empty().html(result.webscaleshowinfo.scaleparams.cupboardno);
                    $("#modulesoap").show();
                }
            app.port = result.webscaleshowinfo.scaleparams.scalePort;
            app.scaletype = result.webscaleshowinfo.scaletype;
            app.extentionid = result.webscaleshowinfo.webscaleextentionid;
            app.taskid = result.webscaleshowinfo.taskid;
            app.module = result.webscaleshowinfo.module;
            app.moduletype = result.webscaleshowinfo.moduletype;
            if (app.module != 'soap') {
                result.webscaleshowinfo.scalevalues.forEach(function (scalevalue, index, array) {
                    app.addorupdaterow(scalevalue.scaleValueId, scalevalue.name, scalevalue.value, "重新加载第" + (index + 1).toString() + "个数据!");
                });
            }
            app.soapcutValueId = result.webscaleshowinfo.scalevalues.length;

            //加载串口列表
            chrome.serial.getDevices(function (ports) {
                for (var i = ports.length - 1; i >= 0; i--) {
                    $('.splsts').append('<option value="' + ports[i].path + '"' + (ports[i].path == app.port ? " selected" : "") + '>' + ports[i].path + '</option>');
                }
            });

            //绑定打印信息
            $('.taskidvalue').empty().html(result.webscaleshowinfo.taskid);
            $('.scaletypevalue').empty().html(result.webscaleshowinfo.scaletype);
            $('.scalenameevalue').empty().html(result.webscaleshowinfo.scalename);
            $('.scalefrequencyvalue').empty().html(result.webscaleshowinfo.scalefrequency);                    
            var paramstr = "<br>scaleparamid:" + result.webscaleshowinfo.scaleparams.scaleParamId + "<br>"
                + "scalename:" + result.webscaleshowinfo.scaleparams.scaleName + "<br>"
                + "scaleport:" + result.webscaleshowinfo.scaleparams.scalePort + "<br>"
                + "scalebitrate:" + result.webscaleshowinfo.scaleparams.scaleBitrate;
            $('.scaleparamsvalue').empty().html(paramstr);

            if (app.scaletype == "auto") {
                app.openserialport();
            }

            app.bindEvent();
        });
    },
    bindEvent: function () {

        $('.splsts').on('change', function () {
            app.port = $(this).val();
            app.resultstr = '你选择了:[' + app.port + '],请打开端口';
        });

        $('.opensp').on('click', function () {
            if ($('.splsts').val() == 'COM0') {
                app.resultstr = '请选择端口';
                return;
            }
            app.openserialport();
        });
        $('.closesp').on('click', function () { app.closeserialport(); });
        $('.returnvalue').on('mouseenter', function () { app.rvmouseenter(); });
        $('.returnvalue').on('mouseleave', function () { app.rvmouseleave(); });
        $('.returnvalue').on('click', function () { app.returnvalue(); });

        $('.txtscanbarcode').focus();
        $('.txtscanbarcode').keyup(function (event) {
            if (event.keyCode == "13")
                app.scanbarcode();           
        });
    },
    rvmouseenter: function () {
        $('.returnvalue').removeClass("normal").addClass("highlight");
    },
    rvmouseleave: function () {
        $('.returnvalue').removeClass("highlight").addClass("normal");
    },
    scanbarcode: function () {
        var curbarcode = $('.txtscanbarcode').val();

        var scalevalueid = $('input[value="' + curbarcode + '"]').parent().parent().children().eq(1).find('a').html();
        //如果是来自于pim皂块称重
        if ((app.module == "soap" && app.moduletype == 'cut') || (app.module == "soap" && app.moduletype == 'dry')) {
            if (typeof (scalevalueid) == "undefined") {
                app.soapcutValueId++;
                scalevalueid = app.soapcutValueId;
            }
            if ($.inArray(curbarcode.toUpperCase(), app.soapcutvalidbarcodelist) < 0) {
                app.resultstr = "你扫描条码:[" + curbarcode + "]不在扫描范围内!";
                return;
            }
           
        } else {
            if (typeof (scalevalueid) == "undefined") {
                app.resultstr = "你扫描条码:[" + curbarcode + "]不在扫描范围内!";
                return;
            }

        }

        var taskid = $('.taskidvalue').html();
        var weightvalue = app.weight.toString();
        if (app.module =='soap') {
            app.pimupdaterequestparm["m"] = app.scalepimparm.m;
            app.pimupdaterequestparm["amc"] = app.scalepimparm.amc;
            app.pimupdaterequestparm["cupboardno"] = app.scalepimparm.cupboardno;
            app.pimupdaterequestparm["surplusquality"] = app.scalepimparm.surplusquality;
            app.pimupdaterequestparm["batchquality"] = app.scalepimparm.batchquality;
            app.pimupdaterequestparm["headerid"] = app.scalepimparm.headerid;
            app.pimupdaterequestparm["batchid"] = app.scalepimparm.batchid;
            app.pimupdaterequestparm["curbarcode"] = curbarcode;
            app.pimupdaterequestparm["weightvalue"] = weightvalue;
            app.pimupdaterequestparm["currEmpnumber"] = app.scalepimparm.currEmpnumber;
            //var xhr = new XMLHttpRequest();
            //xhr.open('POST', app.pimupdateurl + "?TokenID=" + app.scalepimparm.tokenid, true);
            //xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            //xhr.send(app.pimupdaterequestparm);
            //xhr.onreadystatechange = function () {
            //    if (xhr.readyState == 4 && xhr.status == 200) {
            //        var tmp = $.trim(xhr.responseText);
            //        var jsondata = $.parseJSON(tmp);
            //        app.addorupdaterow(scalevalueid, curbarcode, weightvalue, jsondata.msg);
            //        app.sorttablebybarcodevalue();
            //        app.resort();
            //        $('.txtscanbarcode').val("");
            //    }
            //};
            if (weightvalue == "0.00") {
                app.addorupdaterow(scalevalueid, curbarcode, weightvalue, "重量不能为0");
                $('.txtscanbarcode').val("");
                return;
            }

            $.ajax({
                url: app.pimupdateurl + "?TokenID=" + app.scalepimparm.tokenid,
                method: 'post',
                dataType: 'json',
                data: app.pimupdaterequestparm
            }).done(function (rs) {
                    
                app.addorupdaterow(scalevalueid, curbarcode, weightvalue, rs.resultmsg);
                app.sorttablebybarcodevalue();
                app.resort();
                $('.txtscanbarcode').val("");
            });



        } else if (app.module == 'scaledemo'){
            var request = {
                "TaskId": taskid,
                "ScaleValue": {
                    "ScaleValueId": scalevalueid,
                    "Name": curbarcode,
                    "Value": weightvalue,
                    "DateTime": app.getDate(2),
                    "TaskId": taskid
                }
            };
            //var xhr = new XMLHttpRequest();
            //xhr.open('POST', app.url, true);
            //xhr.setRequestHeader("Content-Type", "application/json");
            //xhr.send(JSON.stringify(request));
            //xhr.onreadystatechange = function () {
            //    if (xhr.readyState == 4 && xhr.status == 200) {
            //        var tmp = $.trim(xhr.responseText);
            //        var jsondata = $.parseJSON(tmp);
            //        app.addorupdaterow(scalevalueid, curbarcode, weightvalue, jsondata.msg);
            //        app.sorttablebybarcodevalue();
            //        app.resort();
            //        $('.txtscanbarcode').val("");
            //    }
            //};

            $.ajax({
                url: app.url,
                type: 'POST',
                data: JSON.stringify(request),
                dataType: 'json',
                contentType:"application/json; charset=utf-8",
               success: function (res) {
                   if (res && res.result) {
                       app.addorupdaterow(scalevalueid, curbarcode, weightvalue, res.msg);
                       app.sorttablebybarcodevalue();
                       app.resort();
                       $('.txtscanbarcode').val("");
                   }
               }
            });

            
        }
    },
    isexistname: function (name) {
        var rtn = false;
        $('.barcodevalue').each(
            function () {
                if ($(this).val() == name) rtn = true;
            }
        );
        return rtn;
    },
    addorupdaterow: function (valueid, name, value, resultmsg) {
        var isexist = app.isexistname(name);
        if (!isexist) app.addrow(valueid, name, value, resultmsg);
        else app.updaterow(name, value, resultmsg);
    },
    addrow: function (valueid, name, value, resultmsg) {
        app.count = app.count + 1;
        const tr = $("<tr></tr>")
            .append('<td><a class="RowNo">' + app.count.toString() + '</a></td>')
            .append('<td><a class="scalevalueid">' + valueid + '</a></td>')
            .append('<td><input class="barcodevalue" type="textbox" value="' + name + '"></td>')
            .append('<td><input class="weightvalue" type="textbox" value="' + value + app.unit.toString() + '"></td>')
            .append('<td><a class="updateresult">' + resultmsg + '</a></td>');
           
        const tbody = $('.scaleresultsrows');
        tr.appendTo(tbody);
        $('.tagSub').on('click', function () {
            app.rmrow();
        });
        app.resultstr = '你刚刚扫描了条码:[' + name + '],你总共扫描了:[' + app.count.toString() + ']';
    },
    updaterow: function (name, value, resultmsg) {
        $('input[value="' + name + '"]').parent().parent().children().eq(3).find('input').val(value + app.unit);
        $('input[value="' + name + '"]').parent().parent().children().eq(4).find('a').html(resultmsg);
        app.resultstr = '你刚刚扫描了条码:[' + name + '],你总共扫描了:[' + app.count.toString() + ']';
    },
    rmrow: function () {
        $('tr .tagSub').click(function () {
            $(this).parent().parent().remove();
            app.count = app.count - 1;
        });
        app.resort();
        app.resultstr = '你刚刚删除了皂块';
    },
    resort: function () {
        var i = 1;
        $('.RowNo').each(
            function () {
                $(this).empty().html(i++);
            }
        );
    },
    gettrlist: function () {
        return $("tr:has('.RowNo')")
    },
    sorttablebybarcodevalue: function () {
        var trlist = app.gettrlist(); //获得原来tr元素
        for (var i = 0; i < trlist.length - 1; i++) {//对原来tr元素按照barcodevalue的升序进行重拍序
            for (var j = 0; j < trlist.length - 1 - i; j++) {
                var barcodevalue1 = trlist[j].children[1].children[0].value; //tr->td->input->value的顺序获取元素值
                var barcodevalue2 = trlist[j + 1].children[1].children[0].value;
                if (barcodevalue1 > barcodevalue2) {
                    var temp = trlist[j];
                    trlist[j] = null;
                    trlist[j] = trlist[j + 1];
                    trlist[j + 1] = null;
                    trlist[j + 1] = temp;
                }
            }
        }

        $('.scanresults tbody').html('');
        $('.scanresults tbody').append(trlist); //重新加载排序后tr元素
        $('.tagSub').on('click', function () {//重新注册tagSub的点击事件
            app.rmrow();
        });
    },
    returnvalue: function () {
        //        if (window.confirm("确认完成扫描么？") == true) {
        //            window.returnValue = app.getJsondata();
        //            app.closeserialport();
        //            window.close();
        //        }
        //        var current_window = chrome.app.window.current();
        //        current_window.close();
        chrome.runtime.sendMessage(app.extentionid, { getTargetData: true, msg: "scan_complete", taskid: app.taskid },
            function (response) {

            });
    },
    getJsondata: function () {
        var rtn = "";
        var index = 0;
        $('tr').each(function () {
            var tdAddr = $(this).children()
            if (tdAddr.eq(0).find('a').hasClass('RowNo')) {
                index = index + 1;
            }
        })
        rtn = "{getJsondataCount: '" + index + "'}";
        return rtn;
    },
    chg2close: function () {
        $('.opensp').removeClass('normal').addClass('highlight');
        $('.closesp').removeClass('highlight').addClass('normal');
        $('.splsts').removeAttr('disabled');
        app.state = 'close';
    },
    chg2open: function () {
        $('.opensp').removeClass('highlight').addClass('normal');
        $('.closesp').removeClass('normal').addClass('highlight');
        $('.splsts').removeAttr('disabled').attr('disabled', 'disabled');
        app.state = 'open';
    },
    getWeightAfterSendCmdReadStableData: function () {
        if (app.connectionId) {
            chrome.serial.send(app.connectionId,
                app.convertStringToArrayBuffer('S\n\r'),
                function (sendInfo) { });
        }
    },
    convertStringToArrayBuffer: function (str) {
        var buf = new ArrayBuffer(str.length);
        var bufView = new Uint8Array(buf);
        for (var i = 0; i < str.length; i++) {
            bufView[i] = str.charCodeAt(i);
        }
        return buf;
    },
    convertArrayBufferToString: function (buf) {
        var bufView = new Uint8Array(buf);
        var encodedString = String.fromCharCode.apply(null, bufView);
        return encodedString;
    },
    onReceiveCallback: function (info) {
        var tmpstr = app.convertArrayBufferToString(info.data);
        if (tmpstr == 'g') {
            var reg = new RegExp('S', "g","D");
            //if (app.tmpstr.indexOf('D') > -1) {
            //    console.log(app.tmpstr);
            //}
            app.weight = app.tmpstr.replace(reg, '').replace('D', '').toString();
            if (app.weight) {
                app.weight = app.weight.trim();                
            }
            app.tmpstr = "";
        }
        else {
            app.tmpstr = app.tmpstr + tmpstr;
        }

    },
    onReceiveErrorCallback: function (info) {
        if (app.connectionId) {
            chrome.serial.disconnect(app.connectionId, app.onDisconnectCallback);
        }
    },
    onDisconnectCallback: function (result) {
        if (result) {
            console.log("Disconnected from the serial port");
        }
        else {
            console.log("connectionId：" + app.connectionId);
            console.log("Disconnect failed");
        }

        app.result = result;
        app.resultstr = app.result ? '关闭端口成功' : '关闭端口失败';
        if (app.result) {
            app.alreadyAddListener = false;
            app.chg2close();
            if (app.scaletype == "auto") {
                clearInterval(app.icount);
            }
        }
        app.weight = "0.00";
    },
    onConnectCallback: function (connectionInfo) {
        //console.log(chrome.runtime.lastError, connectionInfo);//输出连接信息

        if (connectionInfo) {
            app.resultstr = '打开端口成功';
            app.result = true;
            app.connectionId = connectionInfo.connectionId;

            chrome.storage.local.set({ "serialportConnectionID": connectionInfo.connectionId});

            if (!app.alreadyAddListener) {
                app.alreadyAddListener = true;
                //add once
                //当接收到数据时，会触发onReceive事件
                chrome.serial.onReceive.addListener(app.onReceiveCallback);
                chrome.serial.onReceiveError.addListener(app.onReceiveErrorCallback);
            }

            app.chg2open();
            if (app.scaletype == "auto") {
                var count = 0;
                var maxcount = parseInt($('.scalecountvalue').html());
                app.icount = setInterval(function () {
                    count = count + 1;
                    if (count <= maxcount) {
                        $('.txtscanbarcode').val(count);
                        app.scanbarcode();
                    }
                }, parseInt($('.scalefrequencyvalue').html()) * 1000);
            };            
        }
        else {
            app.result = false;
            app.resultstr = '端口连接失败';
        }
       
    },
    openserialport: function () {
        if (app.state == 'close') {
            chrome.serial.connect(app.port, app.options, app.onConnectCallback);
        }
        else {
            app.resultstr = '需要关闭端口才能打开';
        }
    },
    closeserialport: function () {
        if (app.connectionId) {
            chrome.serial.flush(app.connectionId, function (result) {
                console.log(result);
            });

            chrome.serial.disconnect(app.connectionId, app.onDisconnectCallback);
        }
    },
    updateresult: function () {
        $('.spstatus').empty().append(app.resultstr);
    },
    updateweights: function () {
        $('.weights span').html(app.weight + app.unit.toString());
    },
    regTimeout: function () {
        setInterval(app.updateresult, 1000);
        setInterval(app.updateweights, 1000);
    },
    getDate: function (fmt) {
        /**
        * format=1表示获取年月日
        * format=2表示获取年月日时分秒
        * **/
        var _time;
        var now = new Date();
        var year = now.getFullYear();
        var month = now.getMonth() + 1;
        var date = now.getDate();
        var day = now.getDay();//得到周几
        var hour = now.getHours();//得到小时
        var minu = now.getMinutes();//得到分钟
        var sec = now.getSeconds();//得到秒
        if (fmt == 1) {
            _time = year + "-" + month + "-" + date
        }
        else if (fmt == 2) {
            _time = year + "-" + month + "-" + date + " " + hour + ":" + minu + ":" + sec;
        }
        return _time;
    }
};

$(function () {
    app.init();
    app.regTimeout();
    var current_window = chrome.app.window.current();

    document.getElementById('minimize').onclick = function () {
        current_window.minimize();
    }
    chrome.app.window.onFullscreened.addListener(function () {
        //do something when the window is set to fullscreen.
        console.log('fullscreen');
    });
    document.getElementById('close').onclick = function () {
        if (app.state == 'open') {
            //如果端口是打开的，要先关闭端口，然后再关闭窗口
            app.closeserialport();
        }     
        //post scan_complete msg to tab
        app.returnvalue(); 
        current_window.close();

       
      
    };
    document.getElementById('maximize').onclick = function () {
        current_window.isMaximized() ?
            current_window.restore() :
            current_window.maximize();
    };
  
    chrome.app.window.current().onClosed.addListener(function (e) {
        console.log(e);
    });

});