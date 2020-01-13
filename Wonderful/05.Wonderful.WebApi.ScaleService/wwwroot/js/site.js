const uriforscale = "api/ScaleService/";
const uriforscalepage = "ScalePageService/";
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
}
$(document).ready(function () {
    getData("00000000-0000-0000-0000-000000000001");
});
/**
 * 获取初始化任务(TaskId:00000000-0000-0000-0000-000000000001)对应的称重任务信息
 * 获取所有称重任务列表
 * @param {any} taskid
 */
function getData(taskid) {
    const scalePageRequest = {
        "TaskId": taskid,
        "ScaleName": $('input[name="scalename"]:checked').val(),
        "ScaleType": $('input[name="scaletype"]:checked').val(),
        "ScaleParams": {
            "scaleport": $('input[name="scaleport"]').val()
        },
        "ScaleCount": $('input[name="scalecount"]').val(),
        "ScaleFrequency": $('input[name="scalefrequency"]').val()
    };
    $.ajax({
        type: "POST",
        accepts: "text/plain",
        contentType: "application/json",
        url: uriforscalepage + 'PostScalePage',
        data: JSON.stringify(scalePageRequest),
        success: function (data) {
            showScalePage(data);
        }
    });
    $.ajax({
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforscale + "TodoItems",
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
                    .append($("<td></td>").html("<a onclick=ScaleValuesPrev('" + item.taskId + "');>" + item.taskId + "</a>"))
                    .append($("<td></td>").text(item.scaleType))
                    .append($("<td></td>").text(item.scaleName))
                    .append($("<td></td>").text(item.scaleParams.scalePort))
                    .append($("<td></td>").text(item.scaleCount))
                    .append($("<td></td>").text(item.scaleFrequency));

                tr.appendTo(tBody);
            });

            todos = data;
        }
    });
}
/**
 * 获取选择任务对应的称重任务信息
 * 获取选择任务对应的称重页面
 * @param {any} taskid
 */
function ScaleValuesPrev(taskid) {
    var todoItemRequestResult = false;
    var scalePageRequestResult = false;
    const TodoItemRequest = {
        "TaskId": taskid
    };
    const ScalePageRequest = {
        "TaskId": taskid,
        "ScaleName": $('input[name="scalename"]:checked').val(),
        "ScaleType": $('input[name="scaletype"]:checked').val(),
        "ScaleParams": {
            "scaleport": $('input[name="scaleport"]').val()
        },
        "ScaleCount": $('input[name="scalecount"]').val(),
        "ScaleFrequency": $('input[name="scalefrequency"]').val(),
        "ScaleValues": null
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforscale + "TodoItem",
        data: JSON.stringify(TodoItemRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("TodoItem Fail!");
        },
        success: function (result) {
            ScalePageRequest.ScaleName = result.scaleName;
            ScalePageRequest.ScaleType = result.scaleType;
            ScalePageRequest.ScaleParams = result.scaleParams;
            ScalePageRequest.ScaleCount = result.scaleCount;
            ScalePageRequest.ScaleFrequency = result.scaleFrequency;
            ScalePageRequest.ScaleValues = result.scaleValues;
            todoItemRequestResult = true;
        }
    })
    if (todoItemRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "text/plain",
            contentType: "application/json",
            url: uriforscalepage + "PostScalePage",
            data: JSON.stringify(ScalePageRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("PostScalePage Fail!");
            },
            success: function (result) {
                showScalePage(result);
                scalePageRequestResult = true;
            }
        })
    }
}
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
}
function showScalePage(text) {
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
}
/**
 * 第一步，请求称重，根据TokenId获得TaskId
 * 第二步，请求保存称重任务，根据TaskId生成称重任务，称重值初始化为0
 * 第三步，请求称重页面
 * */
function scale() {
    var scaleRequestResult = false;
    var scaleSaveTaskRequestResult = false;
    var scalePageRequestResult = false;
    var todoItemRequestResult = false;
    const ScaleRequest = {
        "TokenId": "E2FEEFD1-F172-4F94-A818-9276200DF642"
    };
    const ScaleSaveTaskRequest = {
        "TaskId": "",
        "TokenId": "E2FEEFD1-F172-4F94-A818-9276200DF642",
        "ScaleType": $('input[name="scaletype"]:checked').val(),
        "ScaleName": $('input[name="scalename"]:checked').val(),
        "ScaleCount": $('input[name="scalecount"]').val(),
        "ScaleFrequency": $('input[name="scalefrequency"]').val(),
        "ScaleParams": {
            "ScalePort": $('input[name="scaleport"]').val()
        }
    };
    const ScalePageRequest = {
        "TaskId": "",
        "ScaleName": $('input[name="scalename"]:checked').val(),
        "ScaleType": $('input[name="scaletype"]:checked').val(),
        "ScaleParams": {
            "ScalePort": $('input[name="scaleport"]').val()
        },
        "ScaleCount": $('input[name="scalecount"]').val(),
        "ScaleFrequency": $('input[name="scalefrequency"]').val(),
        "ScaleValues": null
    };
    const TodoItemRequest = {
        "TaskId": ""
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uriforscale + "PostScale",
        data: JSON.stringify(ScaleRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Can't get TaskId!");
        },
        success: function (result) {
            ScaleSaveTaskRequest.TaskId = result.taskId;
            ScalePageRequest.TaskId = result.taskId;
            TodoItemRequest.TaskId = result.taskId;
            scaleRequestResult = true;
        }
    });
    if (scaleRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uriforscale + "PostScaleSaveTask",
            data: JSON.stringify(ScaleSaveTaskRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("SaveTask Fail!");
            },
            success: function (result) {
                scaleSaveTaskRequestResult = true;
            }
        });
    }
    if (scaleSaveTaskRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uriforscale + "TodoItem",
            data: JSON.stringify(TodoItemRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("TodoItem Fail!");
            },
            success: function (result) {
                todoItemRequestResult = true;
                ScalePageRequest.ScaleValues = result.scaleValues;
            }
        });    
    }
    if (todoItemRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "text/plain",
            contentType: "application/json",
            url: uriforscalepage + "PostScalePage",
            data: JSON.stringify(ScalePageRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Request ScalePage Fail!");
            },
            success: function (result) {
                getData(ScalePageRequest.TaskId);
                scalePageRequestResult = true;
            }
        });
    }
}