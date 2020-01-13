var video, canvas, context, mediaStreamTrack;

window.app = {
    //url: "https://localhost:44333/api/ImageService/SaveCaptureValue",
    url: "http://10.210.146.140:8091/api/ImageService/SaveCaptureValue",
    runmode: "takephoto", //scanqrcode
    taskid: "00000000-0000-0000-0000-000000000001",
    filename: "test.jpg",
    cameratype: "front camera",
    machinename: "CNWSGZISDL917",
    capturecount: 1,
    capturefrequency: 2,
    captureindex: 0, //一次性拍多张照片时的计数器
    videosourceid: null, //当前摄像头id
    imagevalues: null,
    smallimgwidth: 180, //缩略图宽度
    smallimgheight: 120, //缩略图高度
    imglist: [], //预加载图片数组
    language: 'zh-CN',
    initPara: function (callback) {
        chrome.storage.local.get('webcampara', function (result) {
            app.taskid = result.webcampara.taskid;
            app.runmode = result.webcampara.runmode;
            app.filename = result.webcampara.filename;
            app.cameratype = result.webcampara.cameratype;
            app.machinename = result.webcampara.machinename;
            app.capturecount = parseInt(result.webcampara.capturecount);
            app.capturefrequency = parseInt(result.webcampara.capturefrequency);
            app.imagevalues = result.webcampara.imagevalues;
            if (result.webcampara.language && result.webcampara.language == "en-US") {
                app.language = "en-US";
            }
            else {
                app.language = 'zh-CN';
            }
            if (result.webcampara.saveurl) {
                app.url = result.webcampara.saveurl;
            }
            callback();
        });
    },
    dynamicLoadJs: function (url, callback) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        if (typeof (callback) == 'function') {
            script.onload = script.onreadystatechange = function () {
                if (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") {
                    callback();
                    script.onload = script.onreadystatechange = null;
                }
            };
        }
        head.appendChild(script);
    },
    initControlLanguage: function () {
        $('#labVideoSource').text(camera_lang.videoSource);
        $('#btnTakePhoto').val(camera_lang.take);
        $('#btnSubmitData').val(camera_lang.submit);
        $('#btnCancelSubmit').val(camera_lang.cancel);
    },
    initVideoSources: function () {
        navigator.mediaDevices.enumerateDevices().then(function (devices) {
            var videoDevices = [];
            for (var i = 0; i < devices.length; i++) {
                if (devices[i].kind === 'videoinput') {
                    videoDevices.push(devices[i].deviceId);

                    var option = document.createElement("option");
                    option.text = devices[i].label || 'camera ' + (videoDevices.length);
                    option.value = devices[i].deviceId;
                    $("#videoSource").append(option);
                }
            }
            if (app.cameratype == "front camera") {
                app.videosourceid = videoDevices[0];
            }
            else {
                if (videoDevices.length > 1) {
                    app.videosourceid = videoDevices[1];
                }
                else {
                    app.videosourceid = videoDevices[0];
                }
            }
            app.initVideo();
        });
    },
    videoSourceChange: function () {
        app.videosourceid = $("#videoSource").val();
        app.initVideo();
    },
    initVideo: function () {
        video = document.getElementById("video");
        try {
            canvas = document.createElement("canvas");
            context = canvas.getContext("2d");
        } catch (e) { alert("not support canvas!"); return; }
        navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
        if (navigator.getUserMedia) {
            navigator.getUserMedia({ "video": { optional: [{ sourceId: app.videosourceid }] } }, function (stream) {
                mediaStreamTrack = stream;
                if (video.srcObject !== undefined) video.srcObject = stream;
                else if (video.mozSrcObject !== undefined) video.mozSrcObject = stream;
                else video.src = ((window.URL || window.webkitURL || window.mozURL || window.msURL) && window.webkitURL.createObjectURL(stream)) || stream;
                video.play();
                if (app.runmode == "scanqrcode") {
                    window.setTimeout(function () { app.scanQRCode(); }, 1000);
                }
            }, function (error) {
                //if(error.PERMISSION_DENIED)console.log("用户拒绝了浏览器请求媒体的权限",error.code);
                //if(error.NOT_SUPPORTED_ERROR)console.log("当前浏览器不支持拍照功能",error.code);
                //if(error.MANDATORY_UNSATISFIED_ERROR)console.log("指定的媒体类型未接收到媒体流",error.code);
                alert("Video capture error: " + error.code);
            });
        }
        else {
            alert("Native device media streaming (getUserMedia) not supported in this browser");
        }
    },
    scanQRCode: function () {
        context.drawImage(video, 0, 0, canvas.width = video.videoWidth, canvas.height = video.videoHeight);

        var img = canvas.toDataURL("image/jpeg").substr(23);
        app.imagevalues[0].value = img;
        const imageSaveCaptureValueRequest = {
            "TaskId": app.taskid,
            "ImageValue": app.imagevalues
        };

        $.ajax({
            url: app.url,
            type: 'post',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(imageSaveCaptureValueRequest),
            success: function (res) {
                //var jsondata = eval('(' + res.d + ')');
                if (res.result) {
                    mediaStreamTrack.getTracks().forEach(function (track) {
                        track.stop();
                    });
                    chrome.storage.local.get('webcamtabid', function (result) {
                        var webcamtabid = result.webcamtabid;
                        chrome.tabs.sendMessage(webcamtabid, {
                            message: 'webcamscanqrcodeok',
                            result: res.msg,
                            taskid: app.taskid
                        }, function (response) {
                            console.log('收到来自前台的回复：' + response);
                        });
                    });
                    app.closeWindow();
                }
                else {
                    if (res.msg) {
                        alert(res.msg);
                    }
                    window.setTimeout(function () { app.scanQRCode(); }, 100);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // 
            }
        });
    },
    initTakePhoto: function () {
        //获取用户勾选的需要重拍的序号
        var arr = [];
        var checkboxes = $("#divImages").find("input[type='checkbox']");
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked == true) {
                arr.push(i);
            }
        }

        //如果没有勾选则默认拍全部
        if (arr.length == 0) {
            for (var i = 0; i < app.imagevalues.length; i++) {
                arr.push(i);
            }
        }

        app.takePhoto(arr);
    },
    takePhoto: function (arr) {
        if (arr.length > 1) {
            $("#btnTakePhoto").attr("disabled", "disabled"); 

            context.drawImage(video, 0, 0, canvas.width = video.videoWidth, canvas.height = video.videoHeight);
            var img = canvas.toDataURL("image/jpeg");
            app.imagevalues[arr[app.captureindex]].value = img.substr(23);

            //生成缩略图
            var imgBig = new Image();
            imgBig.onload = function () {
                context.drawImage(this, 0, 0, canvas.width = app.smallimgwidth, canvas.height = app.smallimgheight);
                var imgSmall = canvas.toDataURL("image/jpeg");
                $("#divTakeImages").append("<li><a href='" + this.src + "' class='fancybox' data-fancybox-group='gallery' title=''><img src='" + imgSmall + "'/></a></li>");
            };
            imgBig.src = img;

            app.captureindex++;
            if (app.captureindex < arr.length) {
                setTimeout(function () { app.takePhoto(arr); }, app.capturefrequency * 1000);
            }
            else {
                //$('.fancybox').fancybox({
                //    afterLoad: function () {
                //        this.title = 'Image ' + (this.index + 1) + ' of ' + this.group.length + (this.title ? ' - ' + this.title : '');
                //    }
                //});
                $("#btnTakePhoto").removeAttr("disabled");  

                video.pause();
                $("#divTakePhoto").hide();
                $("#divSubmitData").show();
            }
        }
        else {
            context.drawImage(video, 0, 0, canvas.width = video.videoWidth, canvas.height = video.videoHeight);
            var img = canvas.toDataURL("image/jpeg").substr(23);
            app.imagevalues[0].value = img;
            video.pause();
            $("#divTakePhoto").hide();
            $("#divSubmitData").show();
        }
    },
    submitData: function () {
        const imageSaveCaptureValueRequest = {
            "TaskId": app.taskid,
            "ImageValue": app.imagevalues
        };
        $.ajax({
            url: app.url,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(imageSaveCaptureValueRequest),
            success: function (res) {
                //var jsondata = eval('(' + res.d + ')');
                if (!res.result) {
                    alert(res.msg);
                }
                else {
                    mediaStreamTrack.getTracks().forEach(function (track) {
                        track.stop();
                    });
                    chrome.storage.local.get('webcamtabid', function (result) {
                        var webcamtabid = result.webcamtabid;
                        chrome.tabs.sendMessage(webcamtabid, {
                            message: 'webcamtakephotook',
                            result: true,
                            taskid: app.taskid
                        }, function (response) {
                            console.log('收到来自前台的回复：' + response);
                        });
                    });
                    app.closeWindow();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // 
            }
        });
    },
    cancelSubmit: function () {
        app.captureindex = 0;
        $("#divTakeImages").find("li").remove();

        video.play();
        $("#divTakePhoto").show();
        $("#divSubmitData").hide();
    },
    closeWindow: function () {
        chrome.storage.local.get('webcamid', function (result) {
            var webcamid = result.webcamid;
            chrome.windows.remove(webcamid);
        });
    },
    regListener: function () {
        $(window).unload(function () {
            chrome.storage.local.remove('webcamid', function () {
                console.log('close window clear localstorage for webcamid');
            });
        });
    },
    showWebcamparam: function () {
        var paramstr = "<br>runmode:" + app.runmode + "<br>"
            + "filename:" + app.filename + "<br>"
            + "cameratype:" + app.cameratype + "<br>"
            + "machinename:" + app.machinename + "<br>"
            + "capturecount:" + app.capturecount + "<br>"
            + "capturefrequency:" + app.capturefrequency + "<br>";
        $("#webcamparam").empty().html(paramstr);
    },
    preLoadPhoto: function () {
        for (var i = 0; i < app.imagevalues.length; i++) {
            var imgSrc = app.imagevalues[i].value;
            if (imgSrc) {
                var img = new Image();
                img.src = imgSrc + "?time=" + new Date().getTime();
                app.imglist.push(img);
            }
        }
    },
    showPhoto: function () {
        //图片是否加载完成
        for (var i = 0; i < app.imglist.length; i++) {
            if (!app.imglist[i].complete) {
                setTimeout(function () { app.showPhoto(); }, 100);
                return;
            }
        }

        //处理逻辑
        if (app.imglist.length > 1) {
            var canvas1 = document.createElement("canvas");
            var context1 = canvas1.getContext("2d");
            for (var i = 0; i < app.imglist.length; i++) {
                var imgBig = this.imglist[i];
                context1.drawImage(imgBig, 0, 0, canvas1.width = app.smallimgwidth, canvas1.height = app.smallimgheight);

                //添加序号
                context1.font = "23px";
                context1.textBaseline = 'middle';
                context1.textAlign = 'center';
                var left = canvas1.width / 2;
                var top = 12;
                context1.fillStyle = "yellow";
                context1.fillText(i + 1, left, top);

                var imgSmall = canvas1.toDataURL("image/jpeg");
                $("#divImages").append("<li><a href='" + imgBig.src + "' class='fancybox' data-fancybox-group='gallery' title=''><img src='" + imgSmall + "'/></a><div style='float: right; height: " + app.smallimgheight + "px'><input type='checkbox' style='margin-top: " + app.smallimgheight / 2 + "px'/><div></li>");
            }
        }
        else {
            if (app.imagevalues[0].value) {
                var imgSrc = app.imagevalues[0].value + "?time=" + new Date().getTime();
                $("#divImages").append("<li><img src='" + imgSrc + "'/></li>");
            }
        }

        $('.fancybox').fancybox({
            afterLoad: function () {
                this.title = 'Image ' + (this.index + 1) + ' of ' + this.group.length + (this.title ? ' - ' + this.title : '');
            }
        });
    }
};

$(function () {
    $("#videoSource").change(function () { app.videoSourceChange(); });
    app.initPara(function () {
        //预加载图片，可以提高页面载入速度以及有序的生成缩略图
        if (app.runmode == "takephoto") {
            app.preLoadPhoto();
        }
        app.dynamicLoadJs("Scripts/language/" + app.language + ".js", app.initControlLanguage);
        app.initVideoSources();
        app.regListener();
        app.showWebcamparam();
        if (app.runmode == "takephoto") {
            $(".btn").show();
            $("#divTakePhoto").show();
            $("#divSubmitData").hide();
            $("#btnTakePhoto").click(function () { app.initTakePhoto(); });
            $("#btnSubmitData").click(function () { app.submitData(); });
            $("#btnCancelSubmit").click(function () { app.cancelSubmit(); });
            app.showPhoto();
        }
        if (app.runmode == "scanqrcode") {
            $(".btn").hide();
        } 
    });
})


