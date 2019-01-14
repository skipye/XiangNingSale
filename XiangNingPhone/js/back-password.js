/** 
@Js-name:back-password.js
@Zh-name:忘记密码找回
@Author:tyron
@Date:2015-08-10
*/
var iTime = 59;
var Account;
function RemainTime() {
    $('#zphone').attr("disabled", true);
    var iSecond, sSecond = "", sTime = "";
    if (iTime >= 0) {
        iSecond = parseInt(iTime % 60);
        iMinute = parseInt(iTime / 60)
        if (iSecond >= 0) {
            if (iMinute > 0) {
                sSecond = iMinute + "分" + iSecond + "秒";
            } else {
                sSecond = iSecond + "秒";
            }
        }
        sTime = sSecond;
        if (iTime == 0) {
            clearTimeout(Account);
            sTime = '获取手机验证码';
            iTime = 59;
            $('#zphone').removeAttr("disabled");
        } else {
            Account = setTimeout("RemainTime()", 1000);
            iTime = iTime - 1;
        }
    } else {
        sTime = '没有倒计时';
    }
    $('#zphone').val(sTime);
}
function sendYzCode() {
    var memberPhone = $('#memberPhone').val();
    var SMSURl = '/WXPayApi/SMSpost.aspx';
    if (memberPhone == null || memberPhone == '') {
        UsTips("请您填写手机号码.");
        return false;
    }
    if (!moblieReg.test(memberPhone)) {
        UsTips("请输入正确的手机号码.");
        return false;
    }
    $.ajax({
        type: "POST",
        data: { "Phone": memberPhone },
        url: "/Account/IsSamePhone",
        dataType: "json",
        success: function (data) {
            if (data == 0) {//如果没有注册，发送验证码
                UsTips("该手机号码错误."); return false;
            } else {
                $.ajax({
                    type: "GET",
                    url: SMSURl,
                    data: { "mobile": memberPhone },
                    //url: "/Account/GetValidatorGraphics",
                    //dataType: "json",
                    success: function (data) {
                        alert(data);
                        if (data == '提交成功') {
                            RemainTime();
                        }
                    },
                    error: function () {
                        UsTips("network error.");
                    }
                });
            }
        },
        error: function () {
            UsTips("network error.");
        }
    });
}
function forgetPw() {
    var memberPhone = $('#memberPhone').val();
    //var userName = $('#userName').val();
    var yzm = $('#yzm').val();
    var password1 = $('#password1').val();
    var password2 = $('#password2').val();
    if (memberPhone == null || memberPhone == '') {
        alert("请您填写手机号码.");
        return false;
    }
    var reg = /^((\(\d{3}\))|(\d{3}\-))?1(3|5|7|8)\d{9}$/;
    if (!reg.test(memberPhone)) {
        UsTips("请输入正确的手机号码.");
        return false;
    }
    if (yzm == null || yzm == "" || typeof (yzm) == undefined) {
        UsTips("请输入验证码");
        return;
    }
    /*if (password1 == null || password1 == "" || typeof(password1) == undefined){
		UsTips("请输入验密码");
		return ;
	}*/
    var regPw = /^[0-9a-zA-Z]+$/;
    if (password1.length < 6 || password1.length > 20) {
        UsTips("请输入6-20位数字或字母");
        return;
    } else if (!regPw.test(password1)) {
        UsTips("只能输入6-20位数字或字母");
        return;
    }
    if (password1 != password2) {
        UsTips("您两次输入的密码不一致！");
        return false;
    }
    $.ajax({
        type: 'POST',
        data: { "TelPhone": memberPhone, "yzm": yzm, "newPassword": password1 },
        url: "/Account/HBackPassword",
        dataType: 'json',
        success: function (data) {
            if (data == "1") {
                UsTips("修改成功！");
                goUrl('/Home/Index');
            } else {
                UsTips("数据错误！请核实！");
            }
        }
    });
}

