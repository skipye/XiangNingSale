var dataobj = {};
/**
 * 载入评论列表
 */
function loadMessageList() {
    var PId = $('#PId').val();
    var pageNum = parseInt($("#messagelist").attr("ref"));
    var totalPage = parseInt($("#messagelist").attr("rel"));
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    if (totalPage == null || totalPage == "") {
        totalPage = 1;
    }
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        if (!loadFlg) return false;
        if (pageNum > totalPage) {
            $("#loadingbox").hide();
            $("#myorde_nocart").css({ display: "block" });
            return;
        }
        loadFlg = false;
        $.ajax({
            type: "post",
            data: { "PageSize": 2, "PageIndex": pageNum, "PId": PId },
            url: "/Message/MessageList",
            success: function (data) {
                pageNum > 1 ? $("#loadingbox").show() : $("#loadingbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                    $("#loadingbox").hide();
                } else {
                    $("#myorde_nocart").hide();
                    $('#messagelist').append(data);
                    pageNum = pageNum + 1;
                    $("#messagelist").attr("ref", pageNum);
                    loadFlg = true;
                }
            }
        });
    }
}

function saveMessage() {
    var PId = $("#PId").val();
    var strContent = $("#strContent").val();
    if (strContent == null || strContent == "" || typeof (strContent) == undefined) {
        UsTips("内容不能为空.");
        return;
    } 
    if (strContent.length > 200) {
        UsTips("内容太长了.");
        return;
    }
    $.post('/Message/AddMessage', {
        "PId": PId, "StrContent": strContent
    }, function (date) {
        if (date == "1")
        { UsTips("提交成功。审核成功后才能查看！"); loadMessageList(); }
        else { UsTips("提交失败。"); }
    })

}