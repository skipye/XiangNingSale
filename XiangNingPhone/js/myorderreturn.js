/** 
@Js-name:myorderreturn.js
@Zh-name:退货订单
@Date:2016-07-14
*/
$(function () {
     //返回顶部插件引用
    $(window).goTops({ toBtnCell: "#gotop", posBottom: 70 });
    loadReturnOrderList();
});

function loadReturnOrderList() {
    var pageNum = parseInt($('#myOrderlist').attr("rel"));
    var totalPage = parseInt($('#myOrderlist').attr("ref"));
    $(window).off("scroll");
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        //alert(pageNum+";"+totalPage);
        //if(pageNum > totalPage){ return; }
        if (pageNum > totalPage) {
            $("#loadaimbox").hide();
            $("#myorde_nocart").css({ display: "block" });
            return;
        }
        $.ajax({
            type: "post",
            data: { "PageSize": 4, "PageIndex": pageNum },
            url: "/Order/ReturnOrderList",
            success: function (data) {
                totalPage > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                } else {
                    $("#myorde_nocart").hide();
                    $('#myOrderlist').append(data);
                    pageNum = pageNum + 1;
                    $("#myOrderlist").attr("rel", pageNum);
                }
            }
        });
    }
}
function TuiKuan(d, SodPrice)
{
    var OrderNum = d;
    var SodPrice = SodPrice;
    m.open({
        width: "70%",
        height: 100,
        content: "<p class='tc listinfo f16' style='width:100%'>确定要退款吗？</p>",
        closeBtn: [false, 1],
        btnName: ['确定', '取消'],
        btnStyle: ["color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            m.open({
                width: "90%",
                height: 150,
                content: "<p class='tc listinfo f16' style='width:100%'>退款理由：<textarea id='sodremark' style='width:70%;height:50px;'></textarea></p>",
                closeBtn: false,
                btnName: ['确定'],
                btnStyle: ["color: #0e90d2;"],
                maskClose: false,
                yesfun: function () {
                    var Remarks = $("#sodremark").val();
                    if (Remarks == "" || Remarks == null) { alert("请填写退款理由！"); return; }

                    $.ajax({
                        type: "post",
                        data: { "OrderNum": OrderNum, "SodPrice": SodPrice, "Remarks": Remarks },
                        url: "/Member/TuiKuan",
                        dataType: "json",
                        asyn: false,
                        success: function (data) {
                            if (data == "1") {
                                UsTips("申请成功！");
                                window.location.reload();
                            } else {
                                UsTips("删除失败，");
                            }
                        },
                        error: function (data) {
                            UsTips("network error!");
                        }
                    });
                }
            });
        },
        nofun: null
    });
}

/*function cancelSod(id){
	$.ajax({
        type : "post",
        data : {"sodId":id},
        url : msonionUrl+"sodrest/cancelSod",
        dataType : "json",
        asyn:false,
		success:function(data){
			if(data.flg == 1){
				window.location.reload();
			}else{
				UsTips("删除失败，");
			}

		}
	});
	if (id == null || id == "" || typeof(id) == undefined){
			UsTips("获取订单失败，请刷新");
			return ;
	}
	 m.open({
		   width:"70%",
		   height:100,
		   content:"<p class='tc listinfo f16' style='width:100%'>确定要删除吗？</p>",
		   closeBtn: [false,1],
		   btnName:['确定', '取消'],
		   btnStyle:["color: #0e90d2;"],
		   maskClose:false,
		   yesfun : function(){
			   $.ajax({
			        type : "post",
			        data : {"sodId":id},
			        url : msonionUrl+"sodrest/cancelSod",
			        dataType : "json",
			        asyn:false,
					success:function(data){
						if(data.flg == 1){
							window.location.reload();
						}else{
							UsTips("删除失败，");
						}

					},
					error:function(data){
						UsTips("network error!");
					}
				});
		   } ,     
		   nofun :  null
	 });
}*/