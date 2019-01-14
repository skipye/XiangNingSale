
// 订单限额 2016-01-14
var sodTariff = 1000;
var doubleProduceArr;
$(function () {
    //$('#dutyTotal').text(sodTariff);
    //购物车商品列表
    var pageNum = 1;
    var totalPage = 1;
    var loadFlg = true;
    //doubleProduceArr = getDoubleProduce();//限购两瓶
    //购物车商品列表数据加载
    //$(window).dropload({ afterDatafun: cartListData });
    //获取购物车商品列表的数据
    /*    if($('#cartlist li').length == 0) {
            $("#cart_nopro").css({
                display: "block"
            });
        }*/
    function cartListData() {
        if (pageNum > totalPage) {
            return;
        }
        loadFlg = false;
        var data = { 'pageNo': pageNum, 't': new Date().getTime() };
        $.ajax({
            type: "get",
            url: msonionUrl + "cart/list",
            data: data,
            dataType: "json",
            //jsonp: "callback",
            success: function (data) {
                // 如果未登录，则跳至登录页面
                if (data.login_flag == "-1") {
                    goUrl("login.html?" + window.location.href);
                } else {
                    if (data.total == 0) {
                        $("#cart_nopro").css({
                            display: "block"
                        });
                    } else {
                        var gettpl = $('#cartData').html();
                        laytpl(gettpl).render(data, function (html) {
                            $('#cartlist').append(html);
                        });
                        //seaoffTips();
                        totalPage = data.totalPage;
                        pageNum++;
                        allCartCount();
                        //图片延迟加载插件引用
                        $('#cartlist').lazyload();
                    }
                    loadFlg = true;
                }

            }
        });
    }
    function seaoffTips() {
        m.open({
            title: ['洋葱通知', 'background:#8016AD; color:#fff;font-size:1.6rem;'],
            width: "90%",
            height: "55%",
            setType: "id",
            content: "#seaoff",
            closeBtn: [false, 1],
            btnName: ['确定'],
            btnStyle: ["color: #0e90d2;"],
            maskClose: false
        })
    }
    function dialogMsg(msg) {
        m.open({
            width: "70%",
            height: 100,
            content: "<p class='tc listinfo f16' style='width:100%'>" + msg + "</p>",
            closeBtn: [false, 1],
            btnName: ['确定'],
            btnStyle: ["color: #0e90d2;"],
            maskClose: false
        })
    }

    /*限购规则不符时的弹框提示*/
    function limitMsgBox(title, msg) {
        m.open({
            title: [title, 'background:#8016AD; color:#fff;font-size:1.6rem;'],
            width: "80%",
            height: "50%",
            content: "<p class='tc listinfo f16' style='width:100%;text-align:left;-webkit-box-pack: inherit;overflow:auto'>" + msg + "</p>",
            closeBtn: [false, 1],
            btnName: ['确定'],
            btnStyle: ["color: #0e90d2;"],
            maskClose: false
        })
    }
    function allCartCount() {
        var chkNameCell = $("#cartlist li input[name='cart']");
        var cartNumCell = $("#cartlist li .cartnum");
        var chooseAll = $('#checkAll');
        //单个选择
        chkNameCell.on("change", function (e) {
            e.preventDefault();
            var moSize = chkNameCell.size();
            var checkSize = 0;
            for (var i = 0; i < chkNameCell.length; i++) {
                if (chkNameCell[i].checked) {
                    checkSize++;
                }
            }
            if (checkSize == moSize) {
                chooseAll.prop('checked', true);
            } else {
                chooseAll.prop('checked', false);
            }
            getCartCount();
        });
        //全选与取消
        chooseAll.on('change', function (e) {
            e.preventDefault();
            if (chooseAll.is(":checked")) {
                chkNameCell.prop('checked', true);
            } else {
                chkNameCell.prop('checked', false);
            }
            var chkNameS = $("#cartlist li input[name='cart']");
            var cartNumS = $("#cartlist li .cartnum");
            var offSolfMsg = "";
            for (var i = 0; i < chkNameS.length; i++) {
                if (chkNameS[i].checked) {
                    var num = cartNumS.eq(i).val();
                    var isOutOff = chkNameS.eq(i).attr("data-value");
                    var qty = chkNameS.eq(i).attr("data-num");
                    var product_name = chkNameS.eq(i).attr("data-name");
                    //取ERP库存
                    /* var data_leid = chkNameS.eq(i).attr("data_leid");
                     var count = 0 ;
                     $.ajax({
                         type: "post",
                         data:{"leId":data_leid},
                         async: false,	
                         url: msonionUrl + "sodrest/getLecount",
                         success: function(data) {
                             count = data;
                         }
                     });*/
                    if (isOutOff == 2 || qty <= 0 || parseInt(num) > parseInt(qty)) {
                        // if (isOutOff == 2 || parseInt(num) > parseInt(count)) {
                        $("#checkAll").prop('checked', false);
                        offSolfMsg += product_name + "</br>"
                        chkNameS[i].checked = false;
                    }

                }
            }
            if (offSolfMsg != "") {
                limitMsg("库存不足温馨提示:", offSolfMsg + "----库存不足");
                return;
            }
            getCartCount();
        });
        //改变购买数量
        $.each($("#cartlist li"), function () {
            var that = $(this), numCell = ".cartnum", RealPay = ".realpay", DutyFree = ".dutyfree";
            //计算免税额
            function RealDuty(num) {
                that.find(RealPay).html("&yen;" + formatNum(parseFloat(that.find(numCell).attr("price")) * num), 2);
                that.find(DutyFree).html("&yen;" + formatNum((parseFloat(that.find(numCell).attr("duty")) * num).toFixed(1)), 2);
            }
            //增加数量
            that.find(".cartadd").on(isTap(), function () {
                // 获取商品id和分类id,供商品限购使用 2015-11-30
                var goodsId = that.find('input[name="cart"]').data("leid");
                var menuId = that.find('input[name="cart"]').data("mid");
                var parNum = parseInt(that.find(numCell).val());
                if (!limitrule(goodsId, 1, menuId)) {	// 添加限购规则 2015-11-30

                    var letqty = that.find('input[name="cart"]').attr("data-num");
                    var letstate = that.find('input[name="cart"]').attr("data-value");
                    var data_oid = parseInt(that.find(numCell).attr("data-oid"));

                    //if (parNum = 1) {
                    if (letqty > parNum && letstate == 1) {//库存大于 购物车数量才可以加
                        var addNum = parseInt(that.find(numCell).val()) + 1;
                        that.find(numCell).val(addNum);
                        RealDuty(addNum);
                    } else {
                        $("#qtyHtml" + data_oid).show();
                    }
                    // 记录修改状态
                    recordState(that.find(numCell));
                    getCartCount();
                    saveCartNum();
                }
            });
            //减少数量
            that.find(".cartmin").on(isTap(), function () {
                // 获取商品id和分类id,供商品限购使用 2015-11-30
                /*var goodsId = that.find('input[name="cart"]').data("leid");
            	var menuId = that.find('input[name="cart"]').data("mid");*/
                var parNum = parseInt(that.find(numCell).val());
                //if(!limitrule(goodsId, parNum,menuId)){	// 添加限购规则 2015-11-30

                var data_oid = parseInt(that.find(numCell).attr("data-oid"));
                $("#qtyHtml" + data_oid).hide();
                if (parNum > 1) {
                    var minNum = parseInt(that.find(numCell).val()) - 1;
                    that.find(numCell).val(minNum);
                    RealDuty(minNum);
                } else {
                    that.find(numCell).val(1);
                    RealDuty(1);
                }
                // 记录修改状态
                recordState(that.find(numCell));
                getCartCount();
                saveCartNum();
                //}
            });
        });

        //删除单个商品
        $(".delCart").on(isTap(), function () {
            var cartIds = $(this).data("delid");
            $.ajax({
                type: "get",
                url: msonionUrl + "cart/delete?cartIds=" + cartIds,
                dataType: "json",
                //jsonp: "callback",
                success: function (data) {
                    //loading_alert("正在帮您清空您选择的商品，请耐心等待","1");
                    var msg = "";
                    if (data.state == 0) {
                        msg = "删除失败！";
                    } else if (data.state == 1) {
                        msg = "删除成功！";
                        $('#cart-' + cartIds).remove();
                        getCartCount();
                    }
                    dialogMsg(msg);
                }
            });
        })
        //统计价格与数量
        function getCartCount() {
            var priceTotal = 0, dutyTotal = 0, dutyCount = 0;
            var chkNameS = $("#cartlist li input[name='cart']");
            var cartNumS = $("#cartlist li .cartnum");
            for (var i = 0; i < chkNameS.length; i++) {
                if (chkNameS[i].checked) {
                    var num = cartNumS.eq(i).val();
                    var price = cartNumS.eq(i).attr("price"), duty = cartNumS.eq(i).attr("duty");
                    priceTotal = (parseFloat(priceTotal) + parseFloat(num) * parseFloat(price));//+ 25;
                    //dutyTotal = (dutyTotal + cartNumS.eq(i).val() * duty);
                }
            }
            //var dutyCount = (dutyTotal==0 ? 0:50) - dutyTotal
            var dutyCount = sodTariff - priceTotal
            $('#countPrice').text(priceTotal.toFixed(priceTotal == 0 ? 0 : 1));
            //$("#dutyTotal").text(dutyCount.toFixed(dutyTotal==0 ? 0:1));
            $("#dutyTotal").text(dutyCount.toFixed(1));
            //判断总额是否为0，为0则冻结结算按钮
            if ($("#countPrice").text() == 0 || $("#dutyTotal").text() < 0) {
                $("#settlement,#coudan").removeClass("settl-btn").addClass("settl-btn-gray").attr('disabled', true);
            } else {
                $("#settlement,#coudan").removeClass("settl-btn-gray").addClass("settl-btn").removeAttr('disabled');
            }
        };
    }
    /**
     *结算
     */

    $("#settlement").on(isTap(), function () {
        var countPrice = $("#countPrice").text();
        if (countPrice == 0 || $("#dutyTotal").text() < 0) { return false; }
        var lim = m.open({
            title: ['跨境电商购物须知', 'background:#8016AD; color:#fff;font-size:1.6rem;text-align: center;'],
            width: "90%",
            height: "90%",
            setType: "id",
            content: "#orderinstr",
            closeBtn: [true, 2],
            btnName: ['已阅读/AGREE'],
            btnStyle: ["color: #8016AD;"],
            maskClose: false,
            yesfun: function () {
                m.close(lim);
                settlement()
            }
        })
        // settlement();
    })
    function settlement() {
        var countPrice = $("#countPrice").text();
        //if (countPrice == 0 || $("#dutyTotal").text()<0) {return false;}
        var chkNameS = $("#cartlist li input[name='cart']");
        var cartNumS = $("#cartlist li .cartnum");
        var cartIds = "";
        var cartIdsArr = new Array();
        var numsArr = new Array();
        var index = 0;
        var offSolfMsg = "";
        for (var i = 0; i < chkNameS.length; i++) {
            if (chkNameS[i].checked) {
                var num = cartNumS.eq(i).val();
                var carid = chkNameS.eq(i).attr("value");
                cartIdsArr[index] = carid;
                numsArr[index] = num;
                index++;
                var isOutOff = chkNameS.eq(i).attr("data-value");
                var qty = chkNameS.eq(i).attr("data-num");
                var product_name = chkNameS.eq(i).attr("data-name");
                //取ERP库存
                /*  var data_leid = chkNameS.eq(i).attr("data_leid");
                var count = 0 ;
                $.ajax({
                    type: "post",
                    data:{"leId":data_leid},
                    async: false,	
                    url: msonionUrl + "sodrest/getLecount",
                    success: function(data) {
                        count = data;
                    }
                });*/
                //两件起售
                /*  var data_leid = chkNameS.eq(i).attr("data_leid");
                alert(data_leid);
                alert(doubleProduceArr.indexOf(data_leid) >= 0);
                if (doubleProduceArr.indexOf(data_leid) >= 0) {
                    alert(data_leid);
                    dialogMsg(product_name+"----两件起售");
                    return "";
                }*/
                if (isOutOff == 2 || qty <= 0 || parseInt(num) > parseInt(qty)) {
                    //if (isOutOff == 2 || parseInt(num) > parseInt(count)) {
                    offSolfMsg += product_name + "</br>"
                }
            }
        }
        if (offSolfMsg != "") {
            limitMsgBox("库存不足温馨提示:", offSolfMsg + "----库存不足");
            return;
        }
        for (var i = 0; i < cartIdsArr.length; i++) {
            if (i == cartIdsArr.length - 1) {
                cartIds += cartIdsArr[i] + "_" + numsArr[i];
            } else {
                cartIds += cartIdsArr[i] + "_" + numsArr[i] + ",";
            }
        }
        if (cartIds == null || cartIds == "" || typeof (cartIds) == undefined) {
            UsTips("请选择结算商品");
            return;
        } else {
            // 接收用cartIds数据对比限购商品列表
            /*var d = checkLimit();
 			if(d && d.data.length>0){
 				var msg = "";
 				for(var i in d.data){
 					var name = $("#"+d.data[i].id).val();
 					msg += ''+name+'<br/>';
 				}
// 				msg = '商品:<br/><b>'+decodeURI(msg)+'</b>在限购日期<span style="color:red">'+d.limitDate+
// 					'</span>起，每个用户购买总量不可超过<span style="color:red">'+d.limitNum+'</span>件,请先修改被限购商品的数量。'
 				//9月4号展会促销期间只限花王系列产品，暂时使用以下提示信息	！
 	 			msg = '商品:<br/><b>'+decodeURI(msg)+'</b>每包补贴47元，原价115元，限时采购只售68元。为保证供应稳定及维护采购商公平性一个账号一个身份证每个单品限购2包不便之处敬请包涵'
 				limitMsgBox("存在限购商品:",msg);
 			}else{*/
            goUrl("cart-order-sumbit.html?tmn=" + returnTmnNo() + "&cartIds=" + cartIds + "&countPrice=" + countPrice);
            //}
            return;
        }

    };

    /*凑单 add by cjw*/
    $("#coudan").on(isTap(), function () {
        // 获取剩余免税额
        //var countPrice = $("#countPrice").text();
        var countPrice = $("#dutyTotal").text();
        // 如果剩余税额没有了，则不给点击
        if (parseFloat(countPrice) && parseFloat(countPrice) < 0 || parseFloat(countPrice) == sodTariff)
            return false;
        var url = "search-goods-list.html?taxamt=" + countPrice;
        //goUrlByTmn(url);
        goNextPage(url);
    });
    //返回顶部插件引用
    //$(window).goTops({
    //    toBtnCell: "#gotop",
    //    posBottom: 55
    //});
    $("#instructions").on(isTap(), function () {
        $m.open({
            width: "90%",
            height: "50%",
            closeBtn: [false, 1],
            setType: 'html',
            content: "<img src='images/tips.jpg' style='width:100%'>",
            btnName: ['关闭'],
            btnStyle: ["color: #0e90d2;"],
            maskClose: false
        });
    })
});

/*保存修改后的购物车数量*/
function saveCartNum() {
    var cartnumdata = '';
    // 获取已改过的数量
    $("#cartlist").find("p > input").each(function (index) {
        var isedit = $(this).data("isedit");
        if (isedit) {
            index != 0 && (cartnumdata += "&");
            cartnumdata += "numdata=" + ($(this).data("oid") + ":" + $(this).val());
        }
    });
    // 发送数据到后台
    if (cartnumdata != '') {
        $.ajax({
            type: 'post',
            async: false,
            url: msonionUrl + "cart/editnum",
            data: cartnumdata
        });
    }
}


/**
 * 记录修改购物车数量的状态
 * 增加或减少时判断是否真正修改了数量
 * @param that
 */
function recordState(inputele) {
    var val = inputele.val();
    var dvalue = inputele.data("value");
    // 标识修改过数量的记录,保证只有数量发生真正修改时才会与数据库交互
    val != dvalue ? inputele.data('isedit', true) : inputele.data('isedit', false);
}

/**
 * 根据地址跳转/返回至下个页面
 * @param url
 */
function goNextPage(url) {
    // 保存购物车的修改数量
    saveCartNum();
    // 如果有传地址，则跳至地址处
    if (url) {
        goUrlByTmn(url);
    } else {
        // 没传地址则返回上一页
        goBack();
    }
}

/*检查限购商品*/
function checkLimit() {
    var products = [];
    // 取所有复选框
    var chkNameS = $("#cartlist").find("input[name='cart']");
    chkNameS.each(function () {
        var that = $(this);
        if (that.attr("checked")) {
            // 取商品信息
            var pinfo = that.siblings('input').val();
            // 取购买数量
            var num = that.parents('li').find('input[name="cartname"]').val();
            var pinfos = pinfo.split(':');
            //var jsonparam = '{\"id\":\"'+pinfos[0]+'\",\"name\":\"'+pinfos[1]+'\",\"qty\":\"'+num+'\"}';
            var jsonparam = '{\"id\":\"' + pinfos[0] + '\",\"qty\":\"' + num + '\"}';
            products.push(jsonparam);
        }
    });
    var result = {};
    if (products.length > 0) {
        $.ajax({
            type: 'post',
            url: msonionUrl + "sodrest/sodlimit",
            data: 'products=' + (products.join('-')),
            dataType: 'json',
            async: false,
            success: function (msg) {
                result = msg;
            }
        });
    }
    return result;
}

function checkState(obj) {
    var data_name = $(obj).attr("data-name");
    var data_stat = $(obj).attr("data-value");//上架状态 2--下架
    var data_num = $(obj).attr("data-num");//库存
    var data_cartnum = $(obj).attr("data-cartnum");//购物车数量
    //取ERP库存
    /*	var data_leid = $(obj).attr("data_leid");
         var count = 0 ;
         $.ajax({
             type: "post",
             data:{"leId":data_leid},
             async: false,	
             url: msonionUrl + "sodrest/getLecount",
             success: function(data) {
              count = data;
             }
         });*/
    if (data_stat == 2 || data_num <= 0 || parseInt(data_cartnum) > parseInt(data_num)) {
        limitMsg("库存不足温馨提示:", data_name + "----库存不足", obj);
    }
}
/*限购规则不符时的弹框提示*/
function limitMsg(title, msg, obj) {
    var lim = m.open({
        title: [title, 'background:#8016AD; color:#fff;font-size:1.6rem;'],
        width: "70%",
        height: "25%",
        content: "<p class='tc listinfo f16' style='width:100%'>" + msg + "</p>",
        //content: "<p class='tc listinfo f16' style='width:100%;text-align:left;-webkit-box-pack: inherit;overflow:auto'>" + msg + "</p>",
        closeBtn: [false, 1],
        btnName: ['确定'],
        btnStyle: ["color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            $(obj).prop('checked', false);
            getCartCount()
            m.close(lim);
        }
    })
}
function getCartCount() {
    var priceTotal = 0, dutyTotal = 0, dutyCount = 0;
    var chkNameS = $("#cartlist li input[name='cart']");
    var cartNumS = $("#cartlist li .cartnum");
    for (var i = 0; i < chkNameS.length; i++) {
        if (chkNameS[i].checked) {
            var num = cartNumS.eq(i).val();
            var price = cartNumS.eq(i).attr("price"), duty = cartNumS.eq(i).attr("duty");
            priceTotal = (parseFloat(priceTotal) + parseFloat(num) * parseFloat(price));//+ 25;
            //dutyTotal = (dutyTotal + cartNumS.eq(i).val() * duty);
        }
    }
    //var dutyCount = (dutyTotal==0 ? 0:50) - dutyTotal
    var dutyCount = sodTariff - priceTotal
    $('#countPrice').text(priceTotal.toFixed(priceTotal == 0 ? 0 : 1));
    //$("#dutyTotal").text(dutyCount.toFixed(dutyTotal==0 ? 0:1));
    $("#dutyTotal").text(dutyCount.toFixed(1));
    //判断总额是否为0，为0则冻结结算按钮
    if ($("#countPrice").text() == 0 || $("#dutyTotal").text() < 0) {
        $("#checkAll").prop('checked', false);
        $("#settlement,#coudan").removeClass("settl-btn").addClass("settl-btn-gray").attr('disabled', true);

    } else {
        $("#settlement,#coudan").removeClass("settl-btn-gray").addClass("settl-btn").removeAttr('disabled');
    }
};
function getDoubleProduce() {
    var numsArr = new Array();
    $.ajax({
        url: "js/common/carNum.txt",
        dataType: "json",
        async: false,
        success: function (data) {
            var ids = data.double
            var objArr = ids.split(",");
            for (var i = 0; i < objArr.length; i++) {
                //alert(objArr[i]);
                numsArr[i] = objArr[i].split(":")[1];
            }
        }
    });
    /*for (var int = 0; int < numsArr.length; int++) {
		alert(numsArr[int]);
	}*/
    return numsArr;
}

/**
 * 商品限购
 * @param gid
 * @param num
 * @param mid
 * @returns {Boolean}
 */
function limitrule(gid, num, mid) {
    var limit = true;
    var params = { "gid": gid, "buynum": num, "menuid": mid, "t": new Date().getTime() };
    var url = msonionUrl + "sodrest/sodlimit1";
    $.ajax({
        type: 'get',
        url: url,
        data: params,
        dataType: 'json',
        async: false,
        success: function (msg) {
            var info = "该商品是限购商品";
            //info += "<br />限购日期："+msg.sdate+"~"+msg.edate;
            info += "<br />每人限购" + msg.limitNum + "件";
            msg.islimit && m.open({
                width: "70%",
                height: 150,
                content: "<p class='listinfo f16' style='width:100%'>" + info + "</p>",
                closeBtn: [false],
                btnName: ['确定'],
                btnStyle: ["color: #0e90d2;"],
                maskClose: false,
                yesfun: null,
                nofun: null
            });
            //dialogMsg("该商品是限购商品，从"+msg.sdate+"~"+msg.edate+"期间，每人限购"+msg.limitNum+"件");
            limit = msg.islimit;
        }
    });
    return limit;
}



var chkNameCell = $("#cartlist li input[name='cart']");
var cartNumCell = $("#cartlist li .cartnum");
var chooseAll = $('#checkAll');
//单个选择
chkNameCell.on("change", function (e) {
    e.preventDefault();
    var moSize = chkNameCell.size();
    var checkSize = 0;
    for (var i = 0; i < chkNameCell.length; i++) {
        if (chkNameCell[i].checked) {
            checkSize++;
        }
    }
    if (checkSize == moSize) {
        chooseAll.prop('checked', true);
    } else {
        chooseAll.prop('checked', false);
    }
    getCartCount();
});
//全选与取消
chooseAll.on('change', function (e) {
    e.preventDefault();
    if (chooseAll.is(":checked")) {
        chkNameCell.prop('checked', true);
    } else {
        chkNameCell.prop('checked', false);
    }
    var chkNameS = $("#cartlist li input[name='cart']");
    var cartNumS = $("#cartlist li .cartnum");
    var offSolfMsg = "";
    for (var i = 0; i < chkNameS.length; i++) {
        if (chkNameS[i].checked) {
            var num = cartNumS.eq(i).val();
            var isOutOff = chkNameS.eq(i).attr("data-value");
            var qty = chkNameS.eq(i).attr("data-num");
            var product_name = chkNameS.eq(i).attr("data-name");
            //取ERP库存
            /* var data_leid = chkNameS.eq(i).attr("data_leid");
             var count = 0 ;
             $.ajax({
                 type: "post",
                 data:{"leId":data_leid},
                 async: false,	
                 url: msonionUrl + "sodrest/getLecount",
                 success: function(data) {
                     count = data;
                 }
             });*/
            if (isOutOff == 2 || qty <= 0 || parseInt(num) > parseInt(qty)) {
                // if (isOutOff == 2 || parseInt(num) > parseInt(count)) {
                $("#checkAll").prop('checked', false);
                offSolfMsg += product_name + "</br>"
                chkNameS[i].checked = false;
            }

        }
    }
    if (offSolfMsg != "") {
        limitMsg("库存不足温馨提示:", offSolfMsg + "----库存不足");
        return;
    }
    getCartCount();
});
//改变购买数量
$.each($("#cartlist li"), function () {
    var that = $(this), numCell = ".cartnum", RealPay = ".realpay", DutyFree = ".dutyfree";
    //计算免税额
    function RealDuty(num) {
        that.find(RealPay).html("&yen;" + formatNum(parseFloat(that.find(numCell).attr("price")) * num), 2);
        that.find(DutyFree).html("&yen;" + formatNum((parseFloat(that.find(numCell).attr("duty")) * num).toFixed(1)), 2);
    }
    //增加数量
    that.find(".cartadd").on(isTap(), function () {
        // 获取商品id和分类id,供商品限购使用 2015-11-30
        var goodsId = that.find('input[name="cart"]').data("leid");
        var menuId = that.find('input[name="cart"]').data("mid");
        var parNum = parseInt(that.find(numCell).val());
        if (!limitrule(goodsId, 1, menuId)) {	// 添加限购规则 2015-11-30

            var letqty = that.find('input[name="cart"]').attr("data-num");
            var letstate = that.find('input[name="cart"]').attr("data-value");
            var data_oid = parseInt(that.find(numCell).attr("data-oid"));

            //if (parNum = 1) {
            if (letqty > parNum && letstate == 1) {//库存大于 购物车数量才可以加
                var addNum = parseInt(that.find(numCell).val()) + 1;
                that.find(numCell).val(addNum);
                RealDuty(addNum);
            } else {
                $("#qtyHtml" + data_oid).show();
            }
            // 记录修改状态
            recordState(that.find(numCell));
            getCartCount();
            saveCartNum();
        }
    });
    //减少数量
    that.find(".cartmin").on(isTap(), function () {
        // 获取商品id和分类id,供商品限购使用 2015-11-30
        /*var goodsId = that.find('input[name="cart"]').data("leid");
        var menuId = that.find('input[name="cart"]').data("mid");*/
        var parNum = parseInt(that.find(numCell).val());
        //if(!limitrule(goodsId, parNum,menuId)){	// 添加限购规则 2015-11-30

        var data_oid = parseInt(that.find(numCell).attr("data-oid"));
        $("#qtyHtml" + data_oid).hide();
        if (parNum > 1) {
            var minNum = parseInt(that.find(numCell).val()) - 1;
            that.find(numCell).val(minNum);
            RealDuty(minNum);
        } else {
            that.find(numCell).val(1);
            RealDuty(1);
        }
        // 记录修改状态
        recordState(that.find(numCell));
        getCartCount();
        saveCartNum();
        //}
    });
});

//删除单个商品
$(".delCart").on(isTap(), function () {
    var cartIds = $(this).data("delid");
    $.ajax({
        type: "get",
        url: msonionUrl + "cart/delete?cartIds=" + cartIds,
        dataType: "json",
        //jsonp: "callback",
        success: function (data) {
            //loading_alert("正在帮您清空您选择的商品，请耐心等待","1");
            var msg = "";
            if (data.state == 0) {
                msg = "删除失败！";
            } else if (data.state == 1) {
                msg = "删除成功！";
                $('#cart-' + cartIds).remove();
                getCartCount();
            }
            dialogMsg(msg);
        }
    });
})
//统计价格与数量
function getCartCount() {
    var priceTotal = 0, dutyTotal = 0, dutyCount = 0;
    var chkNameS = $("#cartlist li input[name='cart']");
    var cartNumS = $("#cartlist li .cartnum");
    for (var i = 0; i < chkNameS.length; i++) {
        if (chkNameS[i].checked) {
            var num = cartNumS.eq(i).val();
            var price = cartNumS.eq(i).attr("price"), duty = cartNumS.eq(i).attr("duty");
            priceTotal = (parseFloat(priceTotal) + parseFloat(num) * parseFloat(price));//+ 25;
            //dutyTotal = (dutyTotal + cartNumS.eq(i).val() * duty);
        }
    }
    //var dutyCount = (dutyTotal==0 ? 0:50) - dutyTotal
    var dutyCount = sodTariff - priceTotal
    $('#countPrice').text(priceTotal.toFixed(priceTotal == 0 ? 0 : 1));
    //$("#dutyTotal").text(dutyCount.toFixed(dutyTotal==0 ? 0:1));
    $("#dutyTotal").text(dutyCount.toFixed(1));
    //判断总额是否为0，为0则冻结结算按钮
    if ($("#countPrice").text() == 0 || $("#dutyTotal").text() < 0) {
        $("#settlement,#coudan").removeClass("settl-btn").addClass("settl-btn-gray").attr('disabled', true);
    } else {
        $("#settlement,#coudan").removeClass("settl-btn-gray").addClass("settl-btn").removeAttr('disabled');
    }
};