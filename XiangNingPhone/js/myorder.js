
var dataobj = {};
$(function () {
    
    //返回顶部插件引用
    $(window).goTops({ toBtnCell: "#gotop", posBottom: 70 });
});

/**
 * 载入订单列表
 */
function loadSodList() {
    var url = window.location.href;
    var searchKey = $('#formalSearchTxt').val();
    var params = parsURL(url).params;
    var sodStat = params.sodStat;
    //if (sodStat == 1) {
    //    $("#orderName").text("待付款订单");
    //} else if (sodStat == 2) {
    //    $("#orderName").text("国外出仓订单");
    //} else if (sodStat == 3) {
    //    $("#orderName").text("国内配送订单");
    //} else {
    //    $("#orderName").text("我的订单");
    //}
    ////var pageNum = 1;
    //lowadData();
    var pageNum = 0;
    var totalPage = 1;
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    //lowadData();
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        //if(pageNum > totalPage){ return; }
        
        //if (!loadFlg) return false;
        if (pageNum > totalPage) {
            $("#loadaimbox i").css({ display: 'none' });
            $("#loadaimbox em").text('到底了,没有更多订单了');
            return;
        }
        loadFlg = false;
        var pageNo = pageNum;
        params.iDisplayLength = 10;
        $.extend(params, dataobj);
        $.ajax({
            type: "post",
            data: { "PageSize": 10, "PageIndex": pageNum, "KeyWord": searchKey },
            url: "/Order/OrderList",
            //dataType: "json",
            //asyn: false,
            success: function (data) {
                var gettpl = $('#myOrderData').html(data);
                
                pageNum > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data)== "") {
                    $("#myorde_nocart").css({ display: "block" });
                } else {
                    $("#myorde_nocart").hide();
                    
                    $('#myOrderlist').append(data);
                    //laytpl(gettpl).render(data, function (html) {
                        
                    //});
                    totalPage = pageNum+1;
                    //alert(totalPage);
                    pageNum = 1;
                    pageNum++;
                    loadFlg = true;
                }
            }
        });
    }
}
function loadPaySodList() {
    var url = window.location.href;
    var searchKey = $('#formalSearchTxt').val();
    var params = parsURL(url).params;
    var sodStat = params.sodStat;
    //if (sodStat == 1) {
    //    $("#orderName").text("待付款订单");
    //} else if (sodStat == 2) {
    //    $("#orderName").text("国外出仓订单");
    //} else if (sodStat == 3) {
    //    $("#orderName").text("国内配送订单");
    //} else {
    //    $("#orderName").text("我的订单");
    //}
    ////var pageNum = 1;
    //lowadData();
    var pageNum = 0;
    var totalPage = 1;
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    //lowadData();
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        //if(pageNum > totalPage){ return; }

        //if (!loadFlg) return false;
        if (pageNum > totalPage) {
            $("#loadaimbox i").css({ display: 'none' });
            $("#loadaimbox em").text('到底了,没有更多订单了');
            return;
        }
        loadFlg = false;
        var pageNo = pageNum;
        params.iDisplayLength = 10;
        $.extend(params, dataobj);
        $.ajax({
            type: "post",
            data: { "PageSize": 10, "PageIndex": pageNum, "KeyWord": searchKey, "TimeOut": true ,"PayState":false},
            url: "/Order/OrderList",
            //dataType: "json",
            //asyn: false,
            success: function (data) {
                var gettpl = $('#myOrderData').html(data);

                pageNum > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                } else {
                    $("#myorde_nocart").hide();

                    $('#myOrderlist').append(data);
                    //laytpl(gettpl).render(data, function (html) {

                    //});
                    totalPage = pageNum + 1;
                    //alert(totalPage);
                    pageNum = 1;
                    pageNum++;
                    loadFlg = true;
                }
            }
        });
    }
}
/**
 * 删除订单
 * @param id
 */
function cancelSod(id) {
    if (id == null || id == "" || typeof (id) == undefined) {
        UsTips("获取订单失败，请刷新");
        return;
    }
    m.open({
        width: "70%",
        height: 100,
        content: "<p class='tc listinfo f16' style='width:100%'>确定要删除吗？</p>",
        closeBtn: [false, 1],
        btnName: ['确定', '取消'],
        btnStyle: ["color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            $.ajax({
                type: "post",
                data: { "sodId": id },
                url: msonionUrl + "sodrest/cancelSod",
                dataType: "json",
                asyn: false,
                success: function (data) {
                    if (data.flg == 1) {
                        window.location.reload();
                    } else {
                        UsTips("删除失败，");
                    }
                },
                error: function (data) {
                    UsTips("network error!");
                }
            });
        },
        nofun: null
    });
}
/**
 * 确认收货
 * @param id
 */
function sureOrder(id) {
    if (id == null || id == "" || typeof (id) == undefined) {
        UsTips("获取订单失败，请刷新");
        return;
    }
    m.open({
        width: "70%",
        height: 200,
        content: "<p class='tc listinfo f16' style='width:100%;'><span class='red'>请慎点！</span>确认收货之后将不能再申请退款，请确保您购买的商品已全部收到并确认无任何破损。</p>",
        closeBtn: [false, 1],
        btnName: ['确定', '取消'],
        btnStyle: ["color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            $.ajax({
                type: "post",
                data: { "sodId": id },
                url: msonionUrl + "sodrest/comfirmReceipt",
                dataType: "json",
                asyn: false,
                success: function (data) {
                    if (data.errCode > 0) {
                        window.location.reload();
                    } else {
                        UsTips("删除失败，");
                    }
                },
                error: function (data) {
                    UsTips("network error!");
                }
            });
        },
        nofun: null
    });
}
function dialogTimeout(msg) {
    m.open({
        width: "70%",
        height: 100,
        content: "<p class='tc listinfo f16' style='width:100%'>" + msg + "</p>",
        closeBtn: [false],
        btnName: ['确定'],
        btnStyle: ["color: #0e90d2;"],
        maskClose: false,
        yesfun: function () { window.location.reload(); },
        nofun: null
    })
}
function toPay(sid) {
    $.ajax({
        type: "get",
        data: { "sodId": sid },
        url: msonionUrl + "sodrest/checkTimeout",
        dataType: "json",
        asyn: false,
        success: function (data) {
            if (data == 1) {
                dialogTimeout("交易超时");
                return;
            } else {
                /*var tip = ""
					$.each($("#paylist"+sid+" li"), function() {
						var sodItemQty =  $(this).find('input[name="sodItemQty"]').val();
						var productQty =  $(this).find('input[name="productQty"]').val();//
						var name =  $(this).find('h3[name="pName"]').html();

						if (parseInt(productQty) < parseInt(sodItemQty)){
							tip += name +"<br/>";
						}		
					});
				if (tip != ""){
					dialogMsgATip(tip+"库存不足！",sid);
					return "";
				} else */
                goUrl('payment.html?tmn=' + returnTmnNo() + '&sodId=' + sid);
            }
        },
        error: function (data) {
            UsTips("network error!");
        }
    });
}

function dialogMsgATip(msg, sid) {
    m.open({
        width: "70%",
        height: 100,
        content: "<p class='tc listinfo f16' style='width:100%'>" + msg + "</p>",
        closeBtn: [false, 1],
        btnName: ['继续购买', '删除订单'],
        btnStyle: ["color: #0e90d2;", "color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            goUrl('index.html?tmn=' + returnTmnNo());
        },
        nofun: function () {
            // goUrl('home.html?tmn='+returnTmnNo());
            cancelSod(sid);
        }
    });
}

/**
 * 订单搜所框事件
 */
function search() {
    var searchKey = $('#formalSearchTxt').val();
    /*var reg = /^\d+/;
	if(reg.test(searchKey)){
		//alert('请输入商品名称查询');
		return;
	}*/
    if (searchKey) {
        // 添加关键字参数
        dataobj.searchWords = $.trim(searchKey);
    } else {
        delete dataobj.searchWords;
    }
    $('#myOrderlist').empty();
    loadSodList();
}

function loadPMList() {
    var pageNum =  parseInt($('#myOrderlist').attr("rel"));
    var totalPage =  parseInt($('#myOrderlist').attr("ref"));
    if (totalPage == null || totalPage <= 0)
    { totalPage = 1; }
    // 取消之前绑定的滚动事件，载入数据时重新绑定
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
            data: { "PageSize": 10, "PageIndex": pageNum },
            url: "/Member/MyPMList",
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