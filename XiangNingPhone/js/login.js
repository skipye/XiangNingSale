
var tmn = "", wxmember;
//$(function () {
//    var queryURL = parsURL(window.location.href).queryURL;
//    tmn = parsURL(queryURL).params.tmn;
//    if (tmn == null | tmn == "" || typeof (tmn) == undefined) {
//        tmn = parsURL(window.location.href).params.tmn;
//    }
//    var ua = navigator.userAgent.toLowerCase();
//    if (ua.match(/MicroMessenger/i) != "micromessenger") {
//        $("#wechat_login").hide();
//    } else {
//        $("#wechat_forget").hide();
//    }
//    $.ajax({
//        type: "post",
//        url: msonionUrl + "menbercenter/memberInfo",
//        dataType: "json",
//        success: function (data) {
//            wxmember = data.memberrec;
//            if (data.login_flag) {
//                if (data.memberrec.memberType == 2) {//代理商
//                    goLoginUrl("shop-agent.html?tmn=tmn");
//                } else if (data.memberrec.memberType == 3) {
//                    goLoginUrl("shop-store.html?tmn=tmn");
//                } else {
//                    goLoginUrl("home.html?tmn=tmn");
//                }
//            }
//        }
//    });

//});
function login() {
    var params = parsURL(window.location.href).queryURL;
    var ReturnUrl = $("#ReturnUrl").val();
    var userCode = $("#userCode").val();
    var passWord = $("#passWord").val();
    if (userCode == null | userCode == "" || typeof (userCode) == undefined) {
        UsTips("请您填写昵称.");
        return false;
    }

    if (passWord == null | passWord == "" || typeof (passWord) == undefined) {
        UsTips("请输入密码.");
        return false;
    }
    var pageLogin = m.open({
        width: "70%", height: 100, closeBtn: [false, 1],
        content: "<p class='tc listinfo f16' style='width:100%'>正在登陆，请耐心等待！</p>",
        maskClose: true
    });
    var LoginUrl = '/Account/LogOn';
    //alert(ReturnUrl);
    $.ajax({
        type: "post",
        url: LoginUrl,
        data: { "userCode": userCode, "passWord": passWord },
        //dataType: "json",
        success: function (d) {
            m.close(pageLogin);
            if (d == "1") {
                UsTips("登录成功！");
                if (ReturnUrl != null && ReturnUrl != "") {
                    window.location.href = ReturnUrl;
                } else {
                    window.location.href = "/Member/Index";
                }
            } else { UsTips("登录错误！"); }

        }
    });

    //$.post(LoginUrl, { "userCode": userCode, "passWord": passWord }, function (d) {
    //    m.close(pageLogin);
    //    var data = d.indexOf("&");
    //    //alert(data);
    //    if (data > 0) {
    //        UsTips("登录成功！");

    //        var arrRult = d.split("&");
    //        //alert(arrRult[0]);
    //        if (arrRult[1] == "True") {
    //            if (confirm('是否去完善信息？第一次完善个人信息可获得10000积分。'))
    //                window.location.href = "/Member/Index";
    //            else { window.location.href = "/Cart/Index"; }
    //        }
    //        else { window.location.href = "/Cart/Index"; }

    //    } else { UsTips("登录错误！"); }
    //});

}
function goLoginUrl(url) {
    if (tmn == null || tmn == "" || typeof (tmn) == undefined) {
        tmn = parsURL(window.location.href).params.tmn;
    }
    if (url.indexOf("tmn=tmn") > 0) {
        url = url.replace("tmn=tmn", "tmn=" + tmn);
    }
    window.location.href = url + "&p=" + ramdom(6);
}
/* 
* url 目标url 
* arg 需要替换的参数名称 
* arg_val 替换后的参数的值 
* return url 参数替换后的url 
*/
function changeURLArg(url, arg, arg_val) {
    var pattern = arg + '=([^&]*)';
    var replaceText = arg + '=' + arg_val;
    if (url.match(pattern)) {
        var tmp = '/(' + arg + '=)([^&]*)/gi';
        tmp = url.replace(eval(tmp), replaceText);
        return tmp;
    } else {
        if (url.match('[\?]')) {
            return url + '&' + replaceText;
        } else {
            return url + '?' + replaceText;
        }
    }
    return url + '\n' + arg + '\n' + arg_val;
}

function wxlogin() {
    var ReturnUrl = $("#ReturnUrl").val();
    //alert(ReturnUrl);
    var RUrl = "/WXPayApi/WXLogin.aspx?ReturnUrl=" + ReturnUrl;

    var wxurl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx277d07db1a908b4f&redirect_uri=http://ycwap.yiju360.com/WXPayApi/WXLogin.aspx&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
    //var tmn= parsURL(window.location.href).params.tmn;
    /*	if (tmn == 1 || tmn == 107) {
            tipAndToIndex("官方商城暂时不支持微信登录，请联系你认识的店主/经济人/客服！",150);
            return ;
        }*/
    //var url = parsURL(window.location.href).queryURL;
    window.location.href = wxurl;
}