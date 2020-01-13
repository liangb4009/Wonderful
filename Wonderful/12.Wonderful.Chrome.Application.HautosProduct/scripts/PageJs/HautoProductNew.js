var app = app || {
    url: "https://localhost:44344/api/ScaleService/PostScaleSaveValue",//QA环境
    savedeviceurl: "https://localhost:44358/api/HautosProductService/SaveHautoDevice",
    getdeviceurl:"https://localhost:44358/api/HautosProductService/GetDeviceDetail",
    result: false,
    downloadEnd: false,
    downloadIndex: 0,
    downloadCount: 0,
    resultstr: "请选择设备",
    state: "close",
    hautodeviceinfo: {},
    hygrotherTDataList: [],
    hygrotherHDataList:[],
    xdata: [],
    startValue: "",
    endValue: "",
    scrolltop:0,
    TimerCommandByte: "01 16 7b 28 {0} 67 5d 29 7d 7e 04",
    DownloadCommandByte:"01 16 7b 28 {0} 5b 01 {1} 00 29 7d 7e 04",
    dataList: [],
    mychart: {},
    socketOptions: {
        persistent: false,
        name: 'tcpSocket',
        bufferSize: 4096
    },
    newSocketOption: {

    },
    popboxW: 500,
    popboxH: 350,
    tcpSocket: {},
    init: function () {
        chrome.storage.local.get('hautodeviceinfo', function (result) {
            app.hautodeviceinfo = result;
            $("#eqid").html(app.hautodeviceinfo.hautodeviceinfo.EQUIPMENT_ID);
            $("#eqname").html(app.hautodeviceinfo.hautodeviceinfo.HygrothermographName);
            $("#eqserianumber").html(app.hautodeviceinfo.hautodeviceinfo.DeviceSerialNumber);
            $("#eqtype").html(app.hautodeviceinfo.hautodeviceinfo.HygrothermographType);
            $("#eqaddress").html(app.hautodeviceinfo.hautodeviceinfo.HygrothermographAddress);
            $("#eqport").html(app.hautodeviceinfo.hautodeviceinfo.HygrothermographPort);
            $("#eqremark").html(app.hautodeviceinfo.hautodeviceinfo.Status);            
            $("#eqgetrate").val(app.hautodeviceinfo.hautodeviceinfo.GetRate);
            $("#eqtupdown").val(app.hautodeviceinfo.hautodeviceinfo.TUpdown);
            $("#eqhupdown").val(app.hautodeviceinfo.hautodeviceinfo.HUpdown);
            $("#eqarea").val(app.hautodeviceinfo.hautodeviceinfo.DeviceArea);            
        });
         $('#btndisconnectserver').attr('disabled', 'disabled');
        app.bindEvent();
        app.mychart = echarts.init(document.getElementById('HygrometerEchart'));
    },
    bindEvent: function () {

        $('#btnconnectserver').on('click', function () { app.openHautoProductPort(); });
        $("#btndisconnectserver").on('click', function () { app.closeHautoProductPort(); });
        $("#opendownload").on('click', function () {
            app.popBox();
        });
        $("#closedownload").on('click', function () {
            app.closeBox();
        });
        $("#btnconfirmdownload").on('click', function () {
            app.downloadHautoProductDatas();
        });

    },
 
    hautoInterverMethod: {},
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
    convert16ArrayBufferToString: function (buf) {
        var bufView = new Uint16Array(buf);
        var encodedString = String.fromCharCode.apply(null, bufView);
        return encodedString;
    },
    openHautoProductPort: function () {
        app.tcpSocket = new tcpServices();
        app.tcpSocket.option = {
            persistent: false,
            name: 'tcpSocket',
            bufferSize: 4096
        };
        if (app.hautodeviceinfo.hautodeviceinfo) {
            app.tcpSocket.init(function () {
                if (app.state == 'close') {
                    try {
                        //查看设备是否以被其他客户端连接
                        $.ajax({
                            url: app.getdeviceurl,
                            data: JSON.stringify({ eqid: app.hautodeviceinfo.hautodeviceinfo.EQUIPMENT_ID }),
                            type: "POST",
                            datatype: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.data.connectStatusValue != 'Disconnected') {
                                        $("#errormsg").html("设备已经在其他客户端连接，要先关闭之前的连接才能重新连接!");
                                        $("#errormsg").show();
                                        return;
                                    } else {

                                        app.tcpSocket.connect(app.hautodeviceinfo.hautodeviceinfo.HygrothermographAddress, parseInt(app.hautodeviceinfo.hautodeviceinfo.HygrothermographPort), function (info) {
                                            console.log("打开连接成功！");
                                            var socketInfo = app.tcpSocket.getInfo(function (socketInfo) {
                                                chrome.storage.local.set({ "sockettcpid": socketInfo.socketId });

                                                if (socketInfo.connected) {
                                                    //更新设备为连接状态
                                                    $.ajax({
                                                        url: app.savedeviceurl,
                                                        data: JSON.stringify({ EQUIPMENT_ID: app.hautodeviceinfo.hautodeviceinfo.EQUIPMENT_ID, ConnectStatusValue: "Connected" }),
                                                        type: "POST",
                                                        datatype: "json",
                                                        contentType: "application/json; charset=utf-8",
                                                        success: function (response) {
                                                            if (!response.success) {
                                                                $("#errormsg").html("打开连接失败!");
                                                                $("#errormsg").show();
                                                                return;
                                                            } else {
                                                                $("#errormsg").hide();
                                                            }
                                                        }
                                                    });


                                                    app.state = "open";
                                                    //如果打开tcp连接成功，那么做一个发送获取温湿度数据指令的定时器

                                                    app.str2ab(app.hautodeviceinfo.hautodeviceinfo.DeviceSerialNumber, function (buffer) {

                                                        var model = bufferToHex(buffer).join(' ');
                                                       
                                                        var TimerCommandBufferArr = [];
                                                        $.each(app.TimerCommandByte.signMix(model).split(' '), function (p1, p2) {
                                                            TimerCommandBufferArr.push(parseInt(p2, 16));
                                                        });

                                                      

                                                        var TimerCommandBuffer = new Int8Array(TimerCommandBufferArr);
                                                        //app.tcpSocket.send(TimerCommandBuffer, function (data) { });
                                                        //app.hautoInterverMethod = setInterval(function () {
                                                        //    app.tcpSocket.send(TimerCommandBuffer, function (data) { });

                                                        //}, 6000);
                                                    });
                                                    $('#btnconnectserver').attr("disabled", 'disabled');
                                                    $('#btndisconnectserver').removeAttr('disabled');


                                                }
                                            });
                                        });
                                    }
                                } else {
                                    $("#errormsg").html("获取设备信息失败!");
                                    $("#errormsg").show();     
                                    return;
                                }
                                $("#errormsg").hide();
                            }
                        });
                       
                    }
                    catch (err) {
                        console.log(err);
                    }
                }
            });
        } else {
            console.log("没有设备信息！");
        }
      
       
    },
    closeHautoProductPort: function () {
        window.clearInterval(app.hautoInterverMethod);
        app.tcpSocket.disconnect();
        app.tcpSocket.close();
        app.state = "close";
        $('#btnconnectserver').removeAttr('disabled');    
        $('#btndisconnectserver').attr('disabled', 'disabled');
        //更新设备为断开状态
        $.ajax({
            url: app.savedeviceurl,
            data: JSON.stringify({ EQUIPMENT_ID: app.hautodeviceinfo.hautodeviceinfo.EQUIPMENT_ID, ConnectStatusValue: "Disconnected" }),
            type: "POST",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (!response.success) {
                    $("#errormsg").html("断开设备失败!");
                    $("#errormsg").show();
                    return;
                } else {
                    $("#errormsg").hide();
                }
            }
        });
    },
    downloadHautoProductDatas: function () {
        if (!app.tcpSocket.send) {
            $("#errormsg").html("请先打开设备端口!");
            $("#errormsg").show();     
            return;
        }

        app.str2ab(app.hautodeviceinfo.hautodeviceinfo.DeviceSerialNumber, function (buffer) {

            var model = bufferToHex(buffer).join(' ');
            if (app.downloadIndex > 65536) {
                var a = "";
            }
            var DownLoadCommandBufferArr = [];
            var indexStr = (app.downloadIndex += 256).toString(16).PadLeft(4, '0');
            indexStr = indexStr.substr(0, 2) + ' ' + indexStr.substr(2, 2);
            $.each(app.DownloadCommandByte.signMix(model, indexStr).split(' '), function (p1, p2) {
                DownLoadCommandBufferArr.push(parseInt(p2, 16));
            });
            var DownLoadCommandBuffer = new Int8Array(DownLoadCommandBufferArr);
            app.tcpSocket.send(DownLoadCommandBuffer, function (data) { });
        });
        $("#errormsg").hide();   
    },
    decodeTimerBuffer: function (bufferArr) {
        var dateNow = getTime();
        var hex = bufferToHex(bufferArr).join('');
        var T = parseFloat(parseInt(hex.substr(34, 2) + hex.substr(32, 2), 16) / 10);//温度
        var H = parseFloat(parseInt(hex.substr(42, 2) + hex.substr(40, 2), 16) / 10);//湿度

        $("#hatudatath").append('<tr><td>' + dateNow + '</td><td>' + T + '</td><td>' + H + '</td></tr>');
        app.hygrotherTDataList.push(T);
        app.hygrotherHDataList.push(H);
        app.xdata.push(dateNow);
        if (app.startValue == '') {
            app.startValue = dateNow;
        }
        app.endValue = dateNow;
        app.initialChart();
            //app.xdata.push(dateNow);
            //    var data = {
            //        EQUIPMENT_ID: $("#eqid").val(),
            //        DATE_TIME: dateNow,
            //        TEMPERATURE: T,
            //        HUMIDITY: H
            //    };
            //    $.ajax({
            //        url: "https://localhost:44358/HautosProduct/LogTemperatureAndHumidityList",
            //        data: { modelList:[data]},
            //        type: "POST",
            //        success: function (response) {
            //            console.log(response);
            //        }
            //    });
    },
    decodeDownLoadBuffer: function (bufferArr) {
        var hex = bufferToHex(bufferArr).join('').toUpperCase();
        if (hex.substr(32, 4) == hex.substr(36, 4)) {
            app.downloadEnd = true; //下载完成
            return;
        }
        hex = hex.substr(42, hex.length);
        var hexdatalist = [];
        for (var i = 0; i < hex.length / 24; i++)
        {
            hexdatalist.push(hex.substr(24 * i, 24));
        }
        $.each(hexdatalist, function (p1, p2) {
            if (p2.length == 24) {
                app.HexToDateTime(p2);

                var TR = parseFloat(parseInt(hex.substr(10, 2) + hex.substr(8, 2), 16)) / 10;
                var HR = parseFloat(parseInt(hex.substr(18, 2) + hex.substr(16, 2), 16)) / 10;               
                app.downloadCount++;
            }
        });
        app.downloadHautoProductDatas();
        console.log(app.downloadCount);
    },
    HexToDateTime: function (item) {
        var dateStr = item.substr(6, 2) + item.substr(4, 2) + item.substr(2, 2) + item.substr(0, 2);
        var dateArr = [];

        for (var i = 0; i < dateStr.length; i++) {
            var ds = dateStr.substr(i, 1);
            var r = parseInt(ds + "", 16).toString(2).PadLeft(4, '0');
            dateArr.push(r);
        }
        var date = dateArr.join('');

        var year ='20'+parseInt(date.substr(0, 6), 2);
        var month = parseInt(date.substr(6, 4), 2);
        var day = parseInt(date.substr(10, 5), 2);
        var hour = parseInt(date.substr(15, 5), 2);
        var minute = parseInt(date.substr(20, 6), 2);
        var second = parseInt(date.substr(26, 6), 2);
        return new Date(year, month, day, hour, minute, second);
    },
    tcpServices: function () {
       

    },
    str2ab: function (str, callback) {
        var b = new Blob([str], { type: 'utf-8' });
        var r = new FileReader();
        r.readAsArrayBuffer(b);
        r.onload = function () {
            if (callback) {
                callback.call(null,r.result);
            }
        };
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
            _time = year + "-" + month + "-" + date + " " + hour + ":" + minu + ":" + sec
        }
        return _time;
    },
    popBox: function () {
       
        var popBox = document.getElementById("popBox");
        var popLayer = document.getElementById("popLayer");
        popBox.style.display = "block";
        popLayer.style.display = "block";
        var iWidth = document.documentElement.clientWidth;
        var iHeight = document.documentElement.clientHeight; 
        popBox.style.left = (iWidth - app.popboxW) / 2 + "px";
        popBox.style.top = (iHeight - app.popboxH) / 2 + "px";       
        app.scrolltop = $(window).scrollTop(); // 获取top值
        $("body").addClass("bodyFixed").css({ "top": -top }); // 定位body
        $(window).scrollTop(app.scrolltop);
      
    },
    closeBox: function () {
        var popBox = document.getElementById("popBox");
        var popLayer = document.getElementById("popLayer");
        popBox.style.display = "none";
        popLayer.style.display = "none";
        $("body").removeClass("bodyFixed"); // 移除body定位
        $(window).scrollTop(app.scrolltop);
    },
    initialChart: function () {

        var option = {
            title: {
                text: '温湿度'
            },
            tooltip: {
                trigger: 'axis',
                formatter: function (p) {
                    var str = '●&nbsp;' + p[0].name;
                    $.each(p, function (idx, item) {
                        str += '<br/>&nbsp;&nbsp;&nbsp;&nbsp;' + item.seriesName + ': ' + item.value + (item.seriesName == '温度' ? '℃' : '%');
                    });
                    return str;
                }
            },
            legend: {
                data: ['温度','湿度']
            },
            xAxis: {
                data: app.xdata 
            },
            dataZoom: [{ startValue: app.startValue }, { endValue: app.endValue }, { type: 'slider' }],
            yAxis: {},
            series: [{
                name: '温度',
                type: 'line',
                data: app.hygrotherTDataList
            }, {
                    name: '湿度',
                    type: 'line',
                    data: app.hygrotherHDataList
                } ]
        };
        app.mychart.setOption(option);
        //app.mychart.setOption(option = {
        //    title: {
        //        text: '温湿度'
        //    },
        //    tooltip: {
        //        trigger: 'axis',
        //        formatter: function (p) {
        //            var str = '●&nbsp;' + p[0].name;
        //            $.each(p, function (idx, item) {
        //                str += '<br/>&nbsp;&nbsp;&nbsp;&nbsp;' + item.seriesName + ': ' + item.value;
        //            });
        //            return str;
        //        }
        //    },
        //    legend: {
        //        data: ['温度', '湿度']
        //    },
        //    xAxis: { data: app.xdata },
           
        //    dataZoom: [{ startValue: app.startValue }, { endValue: app.endValue }, { type: 'slider' }],
        //    series: [{
        //        name: '温度',
        //        type: 'line',
        //        yAxisIndex: 0,
        //        data: app.hygrotherTDataList
        //    },
        //    {
        //        name: '湿度',
        //        type: 'line',
        //        yAxisIndex: 0,
        //        data: app.hygrotherHDataList
        //    }
        //    ]
        //});

    }
};

$(function () {
    app.init();
    var current_window = chrome.app.window.current();
    
    document.getElementById('minimize').onclick = function () {
        current_window.minimize();
    };
    document.getElementById('close').onclick = function () {
        if (app.state == 'open') {
            $("#errormsg").html("请先关闭设备连接，然后再关闭窗口!");
            $("#errormsg").show();          
        } else {
            current_window.close();          
        }
      
    };
    document.getElementById('maximize').onclick = function () {
        current_window.isMaximized() ?
            current_window.restore() :
            current_window.maximize();
    };
    var msgObj = document.getElementById("popBox");
    var titleBar = document.getElementById("titleBar");
    var moveX = 0;
    var moveY = 0;
    var moveTop = 0;
    var moveLeft = 0;
    var moveable = false;
    var w = app.popboxW;
    var h = app.popboxH;
    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight; 
    var docMouseMoveEvent = document.onmousemove; //www.w3cschool.cn w3cschool
    var docMouseUpEvent = document.onmouseup;
    titleBar.onmousedown = function () {
        var evt = getEvent();
        moveable = true;
        moveX = evt.clientX;
        moveY = evt.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);

        document.onmousemove = function () {
            if (moveable) {
                var evt = getEvent();
                var x = moveLeft + evt.clientX - moveX; //www.w3cschool.cn w3cschool
                var y = moveTop + evt.clientY - moveY;
                if (x > 0 && (x + w < iWidth) && y > 0 && (y + h < iHeight)) {
                    msgObj.style.left = x + "px";
                    msgObj.style.top = y + "px";
                }
            }
        };
        document.onmouseup = function () {
            if (moveable) {
                document.onmousemove = docMouseMoveEvent; //www.w3cschool.cn w3cschool
                document.onmouseup = docMouseUpEvent;
                moveable = false;
                moveX = 0;
                moveY = 0;
                moveTop = 0;
                moveLeft = 0;
            }
        };
    }; 


});
// 获得事件Event对象，用于兼容IE和FireFox 
function getEvent() {
    return window.event || arguments.callee.caller.arguments[0];
} 

function ab2str(u, f) {
    var b = new Blob([u], {type:'text/plain'});
    var r = new FileReader();
    r.readAsText(b, 'utf-8');
    r.onload = function () { if (f) f.call(null, r.result); };
}
function bufferToHex(buffer) {
    return Array.prototype.map.call(new Uint8Array(buffer), x => ('00' + x.toString(16)).slice(-2));
}

function tcpServices() {
    var _tcp = chrome.sockets.tcp;
    this.option = {},
        this.socketId = 0,
        this.create = function (callback) {
            _tcp.create(this.option, function (socketInfo) {
                this.socketId = socketInfo.socketId;
                callback();
            }.bind(this));
        }.bind(this),
        this.update = function () {
            _tcp.update(this.socketId, app.newSocketOption, callback);
        }.bind(this),
        this.pause = function (isPaused, callback) {
            _tcp.setPaused(this.socketId, isPaused, callback);
        }.bind(this),
        this.keepAlive = function (enable, delay, callback) {
            _tcp.setKeepAlive(this.socketId, enable, delay, function (code) {
                if (code < 0) {
                    this.error(code);
                }
                else {
                    callback();
                }
            }.bind(this));
        }.bind(this),
        this.connect = function (address, port, callback) {
            _tcp.connect(this.socketId, address, port, function () {
                _tcp.onReceive.addListener(function (info) {
                    if (info.socketId == this.socketId) {

                        this.receive(info);
                    }
                }.bind(this));
                _tcp.onReceiveError.addListener(function (info) {
                    if (info.socketId == this.socketId) {
                        this.error(info.resultCode);
                    }
                }.bind(this));
                callback();
            }.bind(this));
        }.bind(this),
        this.getInfo = function (callback) {
            _tcp.getInfo(this.socketId, callback);
        }.bind(this),
        this.getSockets = function (callback) {
            _tcp.getSockets(callback);
        }.bind(this),
        this.send = function (data, callback) {
            _tcp.send(this.socketId, data, callback);
        }.bind(this),
        this.receive = function (info) {
        if (info.data) {
            var hex = bufferToHex(info.data).join('');
            if (hex.length % 2 == 0 && hex.indexOf("297d7e04") > -1 && hex.length<100) {
                app.decodeTimerBuffer(info.data);
            } else {
                app.decodeDownLoadBuffer(info.data);
            }
            }
        }.bind(this),
        this.noDelay = function (noDelay, callback) {
            _tcp.setNoDelay(this.socketId, noDelay, function (code) {
                if (code < 0) {
                    this.error(code);
                }
                else {
                    callback();
                }
            }.bind(this));
        }.bind(this),
        this.disconnect = function (callback) {
            _tcp.disconnect(this.socketId, callback);
        }.bind(this),
        this.disconnectSocketById =
        function (socketid,callback) {
            _tcp.disconnect(socketid, callback);
        }.bind(this),
        this.close = function (callback) {
            _tcp.close(this.socketId, callback);
        }.bind(this),
        this.closeBySocketId = function (socketid, callback) {
            _tcp.close(socketid, callback);
        }.bind(this),
        this.error = function (code) {
            console.log('An error occurred with code ' + code);
        },
        this.init = function (callback) {
            this.create(callback);
        }.bind(this);

}
String.prototype.signMix = function () {
    if (arguments.length === 0) return this;
    var param = arguments[0], str = this;
    if (typeof (param) === 'object') {
        for (var key in param)
            str = str.replace(new RegExp("\\{" + key + "\\}", "g"), param[key]);
        return str;
    } else {
        for (var i = 0; i < arguments.length; i++)
            str = str.replace(new RegExp("\\{" + i + "\\}", "g"), arguments[i]);
        return str;
    }
};
String.prototype.PadLeft = function (len, charStr) {
    var s = this + '';
    return new Array(len - s.length + 1).join(charStr, '') + s;
};
function getTime() {
    var date = new Date();// 创建日期对象的实例
    // 年月日
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    // 时分秒
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();
    // 补零
    if (hour < 10) {
        hour = '0' + hour;
    }
    if (minute < 10) {
        minute = '0' + minute;
    }
    if (second < 10) {
        second = '0' + second;
    }

    // 星期
    var w = date.getDay();

    var time = year + '-' + month + '-' + day +' ' + hour + ':' + minute + ':' + second;
    return time;
}
