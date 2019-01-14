var dataobj = {};
/**
 * 载入评论列表
 */
function loadNoticesList() {
    var url = window.location.href;
   // var PId = $('#PId').val();
    var params = parsURL(url).params;
    var sodStat = params.sodStat;
   
    var pageNum =0;
    var totalPage = 1;
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    //lowadData();
    if (pageNum == null || pageNum == "")
    {
        pageNum = 0;
    }
    //alert(pageNum);
    if (totalPage == null || totalPage == "") {
        totalPage = 1;
    }
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        //if(pageNum > totalPage){ return; }
        //alert(pageNum + "," + totalPage);
        //if (!loadFlg) return false;
        if (pageNum >= totalPage) {
            $("#loadaimbox i").css({ display: 'none' });
            $("#loadaimbox em").text('到底了,没有更多消息了');
            $(".get-comments").html('到底了,没有更多消息了');
            return;
        }
        loadFlg = false;
        var pageNo = pageNum;
        params.iDisplayLength = 10;
        $.extend(params, dataobj);
        $.ajax({
            type: "post",
            data: { "PageSize": 20, "PageIndex": pageNum },
            url: "/Member/NoticesList",
            success: function (data) {
                pageNum > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#no_record").css({ display: "block" });
                } else {
                    $('#indexData').html(data);
                    pageNum++;
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