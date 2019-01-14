/** 
@Js-name:regist.js
@Zh-name:用户注册
@Author:tyron
@Date:2015-07-13
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
    };
    $.ajax({
        type: "POST",
        data: { "Phone": memberPhone },
        url: "/Account/IsSamePhone",
        dataType: "json",
        success: function (data) {
            if (data==0) {//如果没有注册，发送验证码
                $.ajax({
                    type: "GET",
                    url: SMSURl,
                    data: { "mobile": memberPhone },
                    //url: "/Account/GetValidatorGraphics",
                    //dataType: "json",
                    success: function (data) {
                        if (data == '提交成功') {
                            alert("验证码已经发送！请注意查看手机！");
                            RemainTime();
                        } else { alert(data); }
                    },
                    error: function () {
                        UsTips("network error.");
                    }
                });
            } else {
                UsTips("该手机号码已经注册，无需重复注册.");
            }
        },
        error: function () {
            UsTips("network error.");
        }
    });
}
var register_false = false;
function register() {
    // tmn  ="";
    //$(function(){
    //var queryURL = parsURL(window.location.href).queryURL;
    var tmn = parsURL(window.location.href).params.tmn;
    //});

    var memberYCID = $('#memberYCID').val();
    var RealName = $('#RealName').val();
    var memberPhone = $('#memberPhone').val();
    var yzm = $('#yzm').val();
    var password1 = $('#password1').val();
    var password2 = $('#password2').val();
    //var Email = $("#Email").val();
    var RequestNumber = $("#RequestNumber").val();
    if (memberYCID == null || memberYCID == '') {
        UsTips("请您填写昵称.");
        return;
    } else if ($("#memberYCIDfald").html() == 1) {
        UsTips("您的昵称已被注册，请更换！");
        return;
    }
    if (memberPhone == null || memberPhone == '') {
        UsTips("请您填写手机号码.");
        return;
    }
    if (!moblieReg.test(memberPhone)) {
        UsTips("请输入正确的手机号码.");
        return;
    }
    if (yzm == null || yzm == "" || typeof (yzm) == undefined) {
        UsTips("请输入验证码");
        return;
    }
    if (password1 == null || password1 == "" || typeof (password1) == undefined) {
        UsTips("请输入密码");
        return;
    }
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
        return;
    }
    if (register_false) {
        UsTips("无需重复提交。");
        return;
    }
    register_false = true;
    $("#submitbefor").hide();
    $("#submitafter").show();//"Email": Email,"RealName": RealName,?v_=" + new Date().getTime()
    $.ajax({
        type: 'POST',
        data: { "Telphone": memberPhone, "CheckCode": yzm, "Password": password1, "RealName": RealName, "Name": memberYCID, "RequestNumber": RequestNumber },
        url: "/Account/HZhuCe",
        //dataType: 'json',
        success: function (d) {
            if (d == "True") {
                dialogMsg("注册成功！");
                goUrl('/Member/PersonalInfo');//change to real 
            } else {
                // UsTips(data.message);
                register_false = false;
                dialogMsg("注册失败！");
                $("#submitafter").hide();
                $("#submitbefor").show();
            }
        }
    });
}
function validateMemberYCID() {
    var memberYCID = $("#memberYCID").val();
    if (memberYCID == "" || memberYCID == null || memberYCID == undefined) {
        return "";
    };
    $.ajax({
        type: 'POST',
        data: { "Name": memberYCID },
        url: "/Account/IsSameName",
        dataType: 'json',
        success: function (data) {
            //alert(data.is_regist);
            if (data == "0") {
                //UsTips("您的昵称可用！");
                $("#memberYCIDfald").html(0);
                return true;
            } else {
                UsTips("您的昵称已被注册，请更换！");
                $("#memberYCIDfald").html(1);
                return false;
            }
        }
    });
}