const uriforimage = "api/ImageService/";
const uriforimagepage = "ImagePageService/";
let todos = null;
function getCount(data) {
    const el = $("#counter");
    let name = "to-do";
    if (data) {
        if (data > 1) {
            name = "to-dos";
        }
        el.text(data + " " + name);
    } else {
        el.text("No " + name);
    }
};
function getData(taskid) {
    var imageCapturePageResult = false;
    var todoItemResult = false;
    const TodoItemRequest = {
        "TaskId": taskid
    };
    const imageCapturePageRequest = {
        "TaskId": taskid,
        "RunMode": $("input[name='runmode']").val(),
        "CameraType": $("input[name='cameratype']").val(),
        "MachineName": $("input[name='machinename']").val(),
        "CaptureCount": $("input[name='capturecount']").val(),
        "CaptureFrequency": $("input[name='capturefrequency']").val(),
        "ImageValues": null
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforimage + "TodoItem",
        data: JSON.stringify(TodoItemRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("TodoItem Fail!");
        },
        success: function (result) {
            console.log(result);
            todoItemResult = true;
            imageCapturePageRequest.ImageValues = result.imageValues;
        }
    });
    $.ajax({
        type: "POST",
        accepts: "text/plain",
        contentType: "application/json",
        url: uriforimagepage + 'ImageCapturePage',
        data: JSON.stringify(imageCapturePageRequest),
        success: function (data) {
            showPage(data);
            imageCapturePageResult = true;
        }
    });
    $.ajax({
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforimage + "TodoItems",
        cache: false,
        success: function (data) {
            const tBody = $("#todos");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append(
                        $("<td></td>").append(
                            $("<input/>", {
                                type: "checkbox",
                                disabled: true,
                                checked: item.isComplete
                            })
                        )
                    )
                    .append($("<td></td>").html("<a onclick=ImageValuesPrev('" + item.taskId + "')>" + item.taskId + "</a>"))
                    .append($("<td></td>").text(item.runMode))
                    .append($("<td></td>").text(item.cameraType))
                    .append($("<td></td>").text(item.machineName))
                    .append($("<td></td>").text(item.captureCount))
                    .append($("<td></td>").text(item.captureFrequency));
                tr.appendTo(tBody);
            });
        }
    });
};
function ImageValuesPrev(taskid) {
    var todoItemRequestResult = false;
    var imageCapturePageResult = false;
    const TodoItemRequest = {
        "TaskId": taskid
    };
    const imageCapturePageRequest = {
        "TaskId": taskid,
        "RunMode": $('input[name="runmode"]:checked').val(),
        "CameraType": $('input[name="cameratype"]:checked').val(),
        "MachineName": $('input[name="machinename"]').val(),
        "CaptureCount": $('input[name="capturecount"]').val(),
        "CaptureFrequency": $('input[name="capturefrequency"]').val(),
        "ImageValues": null
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforimage + "TodoItem",
        data: JSON.stringify(TodoItemRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("TodoItem Fail!");
        },
        success: function (result) {
            imageCapturePageRequest.RunMode = result.runMode;
            imageCapturePageRequest.CameraType = result.cameraType;
            imageCapturePageRequest.MachineName = result.machineName;
            imageCapturePageRequest.CaptureCount = result.captureCount;
            imageCapturePageRequest.CaptureFrequency = result.captureFrequency;
            imageCapturePageRequest.ImageValues = result.imageValues;
            todoItemRequestResult = true;
        }
    });
    if (todoItemRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "text/plain",
            contentType: "application/json",
            url: uriforimagepage + "ImageCapturePage",
            data: JSON.stringify(imageCapturePageRequest),
            error: function (jqXHR, textStatus, errorThrown) {
            },
            success: function (result) {
                showPage(result);
                imageCapturePageResult = true;
            }
        });
    }
};
function autodivheight() {
    var winHeight = 0;
    if (window.innerHeight) {
        winHeight = window.innerHeight;
    } else if ((document.body) && (document.body.clientHeight)) {
        winHeight = document.body.clientHeight;
    }
    //通过深入Document内部对body进行检测，获取浏览器窗口高度
    if (document.documentElement && document.documentElement.clientHeight) {
        winHeight = document.documentElement.clientHeight;
    }
    height = winHeight * 0.68
    document.getElementById("iframeResult").style.height = height + "px";
};
function showPage(text) {
    var text = text;
    var patternHtml = /<html[^>]*>((.|[\n\r])*)<\/html>/im
    var patternHead = /<head[^>]*>((.|[\n\r])*)<\/head>/im
    var array_matches_head = patternHead.exec(text);
    var patternBody = /<body[^>]*>((.|[\n\r])*)<\/body>/im;

    var array_matches_body = patternBody.exec(text);
    var basepath_flag = 1;
    var basepath = '';
    if (basepath_flag) {
        basepath = '<base href="//www.runoob.com/try/demo_source/" target="_blank">';
    }
    if (array_matches_head) {
        text = text.replace('<head>', '<head>' + basepath);
    } else if (patternHtml) {
        text = text.replace('<html>', '<head>' + basepath + '</head>');
    } else if (array_matches_body) {
        text = text.replace('<body>', '<body>' + basepath);
    } else {
        text = basepath + text;
    }
    var ifr = document.createElement("iframe");
    ifr.setAttribute("frameborder", "0");
    ifr.setAttribute("id", "iframeResult");
    document.getElementById("iframewrapper").innerHTML = "";
    document.getElementById("iframewrapper").appendChild(ifr);

    var ifrw = (ifr.contentWindow) ? ifr.contentWindow : (ifr.contentDocument.document) ? ifr.contentDocument.document : ifr.contentDocument;
    ifrw.document.open();
    ifrw.document.write(text);
    ifrw.document.close();
    autodivheight();
};
function capture() {
    var captureResult = false;
    var saveCaptureTaskResult = false;
    var imageCapturePageResult = false;
    var todoItemResult = false;
    const captureRequest = {
        "TokenId": "E2FEEFD1-F172-4F94-A818-9276200DF642"
    };
    const saveCaptureTaskRequest = {
        "TaskId": "",
        "TokenId": "E2FEEFD1-F172-4F94-A818-9276200DF642",
        "RunMode": $('input[name="runmode"]:checked').val(),
        "CameraType": $('input[name="cameratype"]:checked').val(),
        "MachineName": $('input[name="machinename"]').val(),
        "CaptureCount": $('input[name="capturecount"]').val(),
        "CaptureFrequency": $('input[name="capturefrequency"]').val()
    };
    const imageCapturePageRequest = {
        "TaskId": "",
        "RunMode": $('input[name="runmode"]:checked').val(),
        "CameraType": $('input[name="cameratype"]:checked').val(),
        "MachineName": $('input[name="machinename"]').val(),
        "CaptureCount": $('input[name="capturecount"]').val(),
        "CaptureFrequency": $('input[name="capturefrequency"]').val(),
        "ImageValues": null
    };
    const TodoItemRequest = {
        "TaskId": ""
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforimage + "Capture",
        data: JSON.stringify(captureRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Can't get TaskId!");
        },
        success: function (result) {
            saveCaptureTaskRequest.TaskId = result.taskId;
            imageCapturePageRequest.TaskId = result.taskId;
            TodoItemRequest.TaskId = result.taskId;
            captureResult = true;
        }
    });
    if (captureResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uriforimage + "SaveCaptureTask",
            data: JSON.stringify(saveCaptureTaskRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("SaveTask Fail!");
            },
            success: function (result) {
                saveCaptureTaskResult = true;
            }
        });
    }
    if (saveCaptureTaskResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uriforimage + "TodoItem",
            data: JSON.stringify(TodoItemRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("TodoItem Fail!");
            },
            success: function (result) {
                todoItemResult = true;
                getData(TodoItemRequest.TaskId);
               // imageCapturePageRequest.ImageValues = result.imageValues;
            }
        });
    }
    //if (todoItemResult == true) {
    //    $.ajax({
    //        async: false,
    //        type: "POST",
    //        accepts: "text/plain",
    //        contentType: "application/json",
    //        url: uriforimagepage + "ImageCapturePage",
    //        data: JSON.stringify(imageCapturePageRequest),
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            alert("Request ImageCapturePage Fail!");
    //        },
    //        success: function (result) {
    //            getData(imageCapturePageRequest.TaskId);
    //            imageCapturePageResult = true;
    //        }
    //    });
    //}
};
$(document).ready(function () {
    getData("00000000-0000-0000-0000-000000000001");
});