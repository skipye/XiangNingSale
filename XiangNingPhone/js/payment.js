/** 
@Js-name:payment.js
@Zh-name:支付方式
@Author:tyron
@Date:2015-07-31
 */
var tmn = "";
var sodId = "";
var data_params = "";
var url = "";
var pageLogin;
$(function () {
    tmn = parsURL(window.location.href).params.tmn;
    sodId = parsURL(window.location.href).params.sodId;
    data_params = parsURL(window.location.href).params;
    url = window.location.href;
    var payFlg = false;
    if (data_params.timeStamp != undefined && !payFlg) {
        payFlg = true;
        pageLogin = m.open({
            width: "70%", height: 100, closeBtn: [false, 1],
            content: "<p class='tc listinfo f16' style='width:100%'>拼命跳转微信支付...</p>",
            maskClose: false
        })
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                wx.config({
                    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    timestamp: data_params.timeStamp,
                    nonceStr: data_params.nonceStr,
                    appId: data_params.appid,
                    signature: data_params.sign,
                    jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });

                document.addEventListener('WeixinJSBridgeReady', callpay, false);
            } else if (document.attachEvent) {
                //alert("else");
                document.attachEvent('WeixinJSBridgeReady', callpay);
                document.attachEvent('onWeixinJSBridgeReady', callpay);
            }
        } else {
            // alert("==");
            wx.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                timestamp: data_params.timeStamp,
                nonceStr: data_params.nonceStr,
                appId: data_params.appid,
                signature: sign,// 必填，签名，见附录1
                jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });
            callpay();
        }
    } else {
        loadingData();
    }
});
function loadingData() {
    $.ajax({
        type: "post",
        data: data_params,
        url: msonionUrl + "sodrest/findSodById",
        dataType: "json",
        asyn: false,
        success: function (data) {
            data.tmn = tmn;
            var gettpl = $('#myOrderData').html();
            laytpl(gettpl).render(data, function (html) {
                $('#myOrderlist').append(html);
            });
        },
        error: function (data) {
            UsTips("network error!");
        }
    });
}
//支付宝支付
function payment() {
    $.ajax({
        type: "post",
        data: $("#orderForm").serialize(),
        url: msonionUrl + "alipay/judgeBrowser",
        dataType: "text",
        asyn: false,
        success: function (data) {
            var ua = navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == "micromessenger") {
                //如果是微信浏览器，跳转换URL。根据gopay确定是否已经更换
                goUrl("alipay.html?" + data);
            } else {//不是微信浏览器，调转支付
                window.location.href = data;
            }

        },
        error: function (data) {
            UsTips("network error!");
        }
    });
}

//财务通 支付
function tenpayment() {
    $.ajax({
        type: "post",
        data: $("#tenpayForm").serialize(),
        url: msonionUrl + "wx/tenpay/toTenpay",
        dataType: "text",
        asyn: false,
        success: function (data) {
            //alert(data);
            window.location.href = data;
            /*var ua = navigator.userAgent.toLowerCase();
			if(ua.match(/MicroMessenger/i) == "micromessenger"){
				//如果是微信浏览器，跳转换URL。根据gopay确定是否已经更换
				goUrl("alipay.html?"+data);
			}else{//不是微信浏览器，调转支付
				window.location.href = data;
			}  */

        },
        error: function (data) {
            UsTips("network error!");
        }
    });
}
//店主支付
function storeBuy() {
    var sodId = parsURL(window.location.href).params.sodId;
    $.ajax({
        type: "post",
        data: { "sodId": sodId },
        url: msonionUrl + "sodrest/updateSodByStoreBuy",
        dataType: "json",
        asyn: false,
        success: function (data) {
            if (data) {
                goUrl('my-order-payment.html?sodStat=1&tmn=' + tmn);
            } else {
                UsTips("失败了，刷新页面重新点击");
            }
        },
        error: function (data) {
            UsTips("network error!");
        }
    });
}
//钱袋宝支付
function qiandaibao() {
    var sodId = parsURL(window.location.href).params.sodId;
    var sodRemake = $("#WIDbody").val();
    var sodNo = $("#WIDout_trade_no").val();
    //alert(sodRemake)
    //alert(sodRemake.length);
    if (sodId == "" || sodId == null || sodId == "undefined") {
        UsTips("系统出错，返回刷新!");
        return "";
    }
    $.ajax({
        type: "post",
        data: { "sodId": sodId, "sodRemake": sodRemake, "tmn": tmn, "platform": "weixin", "urlOrder": url },//weixin,mobile
        url: msonionUrl + "qiandaibao/toPay",
        dataType: "text",
        asyn: false,
        success: function (data) {
            $("#payment").html(data);
        },
        error: function (data) {
            UsTips("network error!");
        }
    });
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
function wxpay() {
    $("#wechatForm").submit();
}
function callpay() {
    m.close(pageLogin);
    wx.chooseWXPay({
        timestamp: data_params.timestamp2,
        nonceStr: data_params.nonceStr2,
        package: "prepay_id=" + data_params.packageValue,
        signType: "MD5",
        paySign: data_params.paySign,
        success: function (res) {
            goUrl("my-order.html?sodStat=2&tmn=" + tmn);
        },
        cancel: function (res) {
            // var text = JSON.stringify(res); 
            // alert("text="+text);
            goUrl("payment.html?tmn=" + tmn + "&sodId=" + sodId);
        },
        fail: function (res) {
            //loadingData();
            //			var text = JSON.stringify(res); 
            //			alert("text="+text);
            goUrl("payment.html?tmn=" + tmn + "&sodId=" + sodId);
            //alert("支付失败--->"+fail);
            getScanUri();
        }
    });


}

/**************************************************************/
/*********************   scan     begin************************/
/**************************************************************/
function getScanUri() {
    $.ajax({
        type: 'POST',
        url: msonionUrl + "/wx/wxpay/getWxScanPayUri",
        data: $("#wechatForm").serialize(),
        dataType: 'json',
        success: function (data) {
            // dialogMsg("正在生成微信支付二维码，请耐心等待");
            $("#payScanImgSrc").show();
            var uri = data.result;
            //alert("uri="+uri);
            if (uri == "" || uri == null || uri == undefined) {
                uri = msPicPath + "/wx/index.html?tmn=1";
            }
            $("#payScanImgSrc").attr("src", "http://api.kuaipai.cn/qr?chl=" + uri + "&chs=400x400");
            //$("#payBUt").hide();
            seaoffTips();
        },
        error: function (data) {
            dialogMsg("生成微信支付二维码失败，请刷新重试");
            $("#payScanImgSrc").hide();
            $("#payBUt").show();
        }
    });
}
function seaoffTips() {
    m.open({
        title: ['长按二维码支付', 'background:#8016AD; color:#fff;font-size:1.6rem;'],
        width: "90%",
        height: "66%",
        setType: "id",
        content: "#payScanImgSrc",
        closeBtn: [true, 2],
        maskClose: false
    })
}
/**************************************************************/
/*********************   scan  end   **************************/
/**************************************************************/
function checkTimeout(key) {
    $.ajax({
        type: "post",
        data: { "sodId": sodId },
        url: msonionUrl + "sodrest/findSodById",
        dataType: "json",
        asyn: false,
        success: function (data) {
            if (data.sodStat == 1) {
                switch (key) {
                    case 0: //微信支付
                        wxpay();
                        break;
                    case 1: //支付宝支付
                        payment();
                        break;
                    case 2: //店主代付
                        storeBuy();
                        break;
                    case 5: //钱袋宝
                        qiandaibao();
                        break;
                    default:
                        break;
                }
            } else if (data.sodStat == 0) {//交易超时关闭
                tip("支付超时，订单已关闭!");
            } else {
                tip("订单已支付");
            }

        },
        error: function () {
            UsTips("network error!");
        }
    });
}
function tip(msg) {
    m.open({
        width: "70%",
        height: 100,
        content: "<p class='tc listinfo f16' style='width:100%'>" + msg + "</p>",
        closeBtn: [false, 1],
        btnName: ['访问首页', '查看订单'],
        btnStyle: ["color: #0e90d2;", "color: #0e90d2;"],
        maskClose: false,
        yesfun: function () {
            goUrl('index.html?tmn=' + tmn);
        },
        nofun: function () {
            goUrl('my-order.html?tmn=' + tmn);
        }
    });
}