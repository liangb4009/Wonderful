// <snippet_SiteJs>
const uri = "api/printservice";
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
// <snippet_GetData>
$(document).ready(function() {
  getData();
});
function getData() {
  $.ajax({
    type: "GET",
    url: uri,
    cache: false,
    success: function(data) {
      const tBody = $("#todos");

      $(tBody).empty();

      getCount(data.length);

      $.each(data, function(key, item) {
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
          .append($("<td></td>").html("<a onclick=ImagePrev('"+item.taskId+"');>"+item.taskId+"</a>"))
          .append($("<td></td>").text(item.fileName))
          .append($("<td></td>").text(item.printName)
          );

        tr.appendTo(tBody);
      });

      todos = data;
    }
  });
}
// </snippet_GetData>
// <snippet_ImagePrev>
function ImagePrev(taskid) {
    if (taskid == "00000000-0000-0000-0000-000000000001") 
        $("#imgPrev").attr("src", "PIMTemplate/blank.jpg");
    else
        $("#imgPrev").attr("src", "PIMOutput/" + taskid + "_Preview_Label_1.jpg");

    const todoItemRequest = {
        "TaskId": taskid
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uri + "/TodoItem",
        data: JSON.stringify(todoItemRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("TodoItem Fail!");
        },
        success: function (result) {
            if (result.printType == "Excel") {
                window.open("print.html?taskid=" + taskid + "&pdf=" + result.generatePdfFile);
            }
        }
    });
}
//</snippet_ImagePrev>
// </snippet_Print>
function print() {
    var printRequestResult = false;
    var executePrintResult = false;
    const PrintRequest = {
        "TokenId": "E2FEEFD1-F172-4F94-A818-9276200DF642"
    };
    const executePrintRequest = {
        "TaskId": "",
        "FileName": $('input[name="filename"]:checked').val(),
        "PrintName": $('input[name="printname"]:checked').val(),
        "PrintType": $('input[name="printtype"]:checked').val(),
        "PrintCount": $('input[name="printcount"]').val()
    };
    const printTaskRequest = {
        "TaskId": "",
        "FileName": $('input[name="filename"]:checked').val(),
        "PrintName": $('input[name="printname"]:checked').val(),
        "PrintType": $('input[name="printtype"]:checked').val(),
        "PrintCount": $('input[name="printcount"]').val(),
        "TaskContent": [
            {
                "Name": "Reason",
                "Value": $('input[name="reason"]').val()
            },
            {
                "Name": "Name",
                "Value": $('input[name="name"]').val()
            },
            {
                "Name": "ItemNo",
                "Value": $('input[name="itemno"]').val()
            },
            {
                "Name": "Qty",
                "Value": $('input[name="qty"]').val()
            },
            {
                "Name": "Wo",
                "Value": $('input[name="wo"]').val()
            },
            {
                "Name": "Uom",
                "Value": $('input[name="uom"]').val()
            },
            {
                "Name": "LotNum",
                "Value": $('input[name="lotnum"]').val()
            },
            {
                "Name": "RecordBy",
                "Value": $('input[name="recordby"]').val()
            },
            {
                "Name": "RecordDate",
                "Value": $('input[name="recorddate"]').val()
            }
        ],
        "ExcelTaskContent": [],
        "PrintNext": false
    };
    $.ajax({
        async: false,
        type: "POST",
        accepts: "application/json",
        contentType: "application/json",
        url: uri + "/PostPrint",
        data: JSON.stringify(PrintRequest),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Can't get TaskId!");
        },
        success: function (result) {
            executePrintRequest.TaskId = result.taskId;
            printTaskRequest.TaskId = result.taskId;
            printRequestResult = true;     
        }
    });
    if (printRequestResult == true) {
        $.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uri + "/ExecutePrint",
            data: JSON.stringify(executePrintRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("ExecutePrint Fail");
            },
            success: function (result) {
                if (result.result) {
                    executePrintResult = true;
                }
                else {
                    alert(result.msg);
                }
            }
        });
    }
    if (executePrintResult == true) {
        if (printTaskRequest.PrintType == "Excel") {
            printTaskRequest.TaskContent = [];
            printTaskRequest.ExcelTaskContent = [
                {
                    "SheetName": "Sheet1",
                    "CellContent": [
                        {
                            "Name": "Wo",
                            "Value": $('input[name="wo"]').val()
                        },
                        {
                            "Name": "RecordBy",
                            "Value": $('input[name="recordby"]').val()
                        },
                        {
                            "Name": "RecordDate",
                            "Value": $('input[name="recorddate"]').val()
                        }
                    ],
                    "TableContent": [
                        {
                            "TableName": "Roster",
                            "Content": [
                                { "ItemNo": $('input[name="itemno"]').val(), "Name": $('input[name="name"]').val(), "LotNum": $('input[name="lotnum"]').val(), "Reason": $('input[name="reason"]').val(), "Qty": $('input[name="qty"]').val(), "Uom": $('input[name="uom"]').val() },
                                { "ItemNo": $('input[name="itemno"]').val(), "Name": $('input[name="name"]').val(), "LotNum": $('input[name="lotnum"]').val(), "Reason": $('input[name="reason"]').val(), "Qty": $('input[name="qty"]').val(), "Uom": $('input[name="uom"]').val() },
                                { "ItemNo": $('input[name="itemno"]').val(), "Name": $('input[name="name"]').val(), "LotNum": $('input[name="lotnum"]').val(), "Reason": $('input[name="reason"]').val(), "Qty": $('input[name="qty"]').val(), "Uom": $('input[name="uom"]').val() },
                                { "ItemNo": $('input[name="itemno"]').val(), "Name": $('input[name="name"]').val(), "LotNum": $('input[name="lotnum"]').val(), "Reason": $('input[name="reason"]').val(), "Qty": $('input[name="qty"]').val(), "Uom": $('input[name="uom"]').val() },
                                { "ItemNo": $('input[name="itemno"]').val(), "Name": $('input[name="name"]').val(), "LotNum": $('input[name="lotnum"]').val(), "Reason": $('input[name="reason"]').val(), "Qty": $('input[name="qty"]').val(), "Uom": $('input[name="uom"]').val() }
                            ]
                        },
                        {
                            "TableName": "Roster1",
                            "Content": [
                                { "ItemNo1": $('input[name="itemno"]').val(), "Name1": $('input[name="name"]').val(), "LotNum1": $('input[name="lotnum"]').val(), "Reason1": $('input[name="reason"]').val(), "Qty1": $('input[name="qty"]').val(), "Uom1": $('input[name="uom"]').val() },
                                { "ItemNo1": $('input[name="itemno"]').val(), "Name1": $('input[name="name"]').val(), "LotNum1": $('input[name="lotnum"]').val(), "Reason1": $('input[name="reason"]').val(), "Qty1": $('input[name="qty"]').val(), "Uom1": $('input[name="uom"]').val() }
                            ]
                        }
                    ]
                }
            ];
        }

        setTimeout($.ajax({
            async: false,
            type: "POST",
            accepts: "application/json",
            contentType: "application/json",
            url: uri + "/PrintTask",
            data: JSON.stringify(printTaskRequest),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("PrintTask Fail");
            },
            success: function (result) {
                getData();
                ImagePrev(result.taskId);
            }
        }),3000);
    }

}
// </snippet_Print>
// </snippet_SiteJs>
