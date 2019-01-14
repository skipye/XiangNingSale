/** 
@Js-name:goods-liast.js
@Zh-name:产品详情页JS函数
*/
var menuId = 0;
var messageFlag = 0;
var gid = "";
$(function () {
    var ParHref = parsURL(window.location.href).params;
    gid = ParHref.id
    var qrCodeid = $('#erwmbox');
    $("#saoerwm").on(isTap(), function () {
        qrCodeid.css({ display: '-webkit-box' });
    })
    qrCodeid.on(isTap(), function () {
        $(this).hide();
    })
    var qrcode = new QRCode(qrCodeid.find(".erwmcon")[0], {
        text: window.location.href,
        width: 160,
        height: 160
    });
    
})

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


//添加收藏
function addAtten(tmn, goodsId) {
    var ele = event.target;
    $.ajax({
        type: "get",
        url: msonionUrl + "myatten/add?tmn=" + tmn + "&goodsId=" + goodsId,
        dataType: "json",
        //jsonp:"callback",
        success: function (data) {
            var msg = "";
            if (data.state == -1) {  //帐户未登录或无权限
                goUrl("login.html?" + window.location.href);
            } else {
                if (data.state == 0) {
                    msg = "此商品收藏失败！";
                } else if (data.state == 1) {
                    msg = "此商品已在收藏夹中！";
                } else if (data.state == 2) {
                    $(ele).removeClass("graysc").addClass("redsc").text("已收藏");
                    msg = "商品收藏成功！";
                } else if (data.state == 3) {
                    msg = "洋葱商家不能使用此功能！";
                }
                dialogMsg(msg);
            }
        }
    });
}



function shareFunction(store_name) {
    /***************微信分享************************/
    if (store_name == "" || store_name == "undefined") {
        store_name = "洋葱海外仓";
    }
    //alert(store_name);
    $.ajax({
        type: "get",
        url: msonionUrl + "getWeChatSign",
        data: { "url": window.location.href },
        dataType: "json",
        success: function (data) {
            wx.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: data.appid, // 必填，公众号的唯一标识
                timestamp: data.timestamp, // 必填，生成签名的时间戳
                nonceStr: data.noncestr, // 必填，生成签名的随机串
                signature: data.finalsign,// 必填，签名，见附录1
                jsApiList: [
				            'checkJsApi',
				            'onMenuShareTimeline',
				            'onMenuShareAppMessage',
				            'onMenuShareQQ',
				            'onMenuShareWeibo',
				            'onMenuShareQZone'
                ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });
            wx.ready(function () {
                wx.onMenuShareTimeline({
                    title: store_name, // 分享标题
                    link: window.location.href, // 分享链接
                    imgUrl: msPicPath + photo_url, // 分享图标
                    success: function () {
                        // 用户确认分享后执行的回调函数
                        //alert("3q");
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                        //alert(" no 3q");
                    }
                });
                wx.onMenuShareAppMessage({
                    title: store_name, // 分享标题
                    desc: '全球研选 日用之美', // 分享描述
                    link: window.location.href,//'http://m.msyc.cc/wx/index.html?tmn='+tmn, // 分享链接
                    imgUrl: msPicPath + photo_url, // 分享图标
                    type: '', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                    success: function () {
                        // 用户确认分享后执行的回调函数
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                    }
                });
            });
            wx.onMenuShareQZone({
                title: store_name, // 分享标题
                desc: '全球研选 日用之美', // 分享描述
                link: window.location.href, // 分享链接
                imgUrl: msPicPath + photo_url, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareQQ({
                title: store_name, // 分享标题
                desc: '全球研选 日用之美', // 分享描述
                link: window.location.href, // 分享链接
                imgUrl: msPicPath + photo_url, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
        }
    });
    /***************微信分享 end************************/
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
