
var flag = false;
$(function () {
    findAreaById("");
});
function findAreaById(i) {
    
    if (i == "p") {
        areaParentId = 0;
    } else if (i == "c") {
        var addressProvince = $("#ProvinceId").val();
        if (addressProvince == null || addressProvince == "" || typeof (addressProvince) == undefined) {
            UsTips("先选择省区");
            return;
        } else {
            var SubUrl = '/Home/CDropdownlist';
            AjaxSub(SubUrl, addressProvince, i);
        }
    } else {
        var addresscity = $("#CityId").val();
        var addressProvince = $("#ProvinceId").val();
        if (addressProvince == null || addressProvince == "" || typeof (addressProvince) == undefined) {
            UsTips("先选择省区");
            return;
        } else if (addresscity == null || addresscity == "" || typeof (addresscity) == undefined) {
            UsTips("先选择城市");
            return;
        } else {
            var SubUrl = '/Home/ADropdownlist';
            AjaxSub(SubUrl, addresscity, i);
        }
    }
}
function AjaxSub(SubUrl, PId, i) {
    //alert(1);
    $.get(SubUrl, { areaParentId: PId }, function (date) {
        if (i == "c") {
            $("#CityId").empty();
            $("#CityId").append("<option value=''>-请选择 城市-</option>");
            $("#CityId").append(date);
        } else if (i == "r") {
            $("#RegionId").empty();
            $("#RegionId").append("<option value=''>-请选择  区县-</option>");
            $("#RegionId").append(date);
        } else {
            //$("#ProvinceId").append(date);
        }
    });

}
function checkName(name) {
    var reg = /^[\u4e00-\u9fa5]{0,}$/;
    if (!reg.test(name)) {
        UsTips("暂只支持中文姓名.");
        $("#addressUser").focus();
        return false;
    }
    return true;
}
function checkMobile(addressMobile) {
    //	var reg =/^((\(\d{3}\))|(\d{3}\-))?1(3|5|7|8)\d{9}$/;
    if (!moblieReg.test(addressMobile)) {
        UsTips("请输入正确的手机号码.");
        return false;
    }
    return true;
}

function checkCID(cid) {
    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    var isDigit = /[\u4e00-\u9fa5]/;
    if (isDigit.test(cid)) {
        UsTips("别逗...此处应输入身份证号码!");
        $("#addressUserCid").focus();
        return false;
    }
    if (!reg.test(cid)) {
        UsTips("请输入正确身份证号码.");
        $("#addressUserCid").focus();
        return false;
    }
    var cidFront = $("#upPositive").val();
    var cidBack = $("#upNegative").val();
    var cidFrontUrl = $("#cidFront").val();
    var cidBackUrl = $("#cidBack").val();
    if (cidFront != "" && cidFrontUrl == "") {
        uploadImage('cidFrontForm', cid);
    }
    if (cidBack != "" && cidBackUrl == "") {
        uploadImage('cidBackForm', cid);
    }
    /*
	$.ajax({
		type : "post",
		data : {"memberCid":cid},
		url : msonionUrl+"address/validateIdcard",
		dataType : "json",
		asyn:false,
		success:function(data){	
			if (data.errNum != 0){
				UsTips("无效的身份证号码.");
				$("#addressUserCid").focus();
			}
		}
	});
	*/
    return true;
}
var once_sumbit_flag = false;
function saveAddress() {
    var Id = $("#Id").val();
    var Name = $("#Name").val();
    var Telphone = $("#Telphone").val();
    var ProvinceId = $("#ProvinceId").val();
    var Province = $("#ProvinceId").find("option:selected").text();
    var CityId = $("#CityId").val();
    var City = $("#CityId").find("option:selected").text();
    var RegionId = $("#RegionId").val();
    var Region = $("#RegionId").find("option:selected").text();
    var addressNo = $("#addressNo").val();
    var ReturnUrl = $("#ReturnUrl").val();
    //var reg =/^((\(\d{3}\))|(\d{3}\-))?1(3|5|7|8)\d{9}$/;
    var CIDreg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    var nameReg = /^[\u4e00-\u9fa5]{0,}$/;

    //if (addressUserCid == null || addressUserCid == "" || typeof (addressUserCid) == undefined) {
    //    UsTips("身份证号码不能为空.");
    //    return;
    //} else if (!CIDreg.test(addressUserCid)) {
    //    UsTips("请输入正确的身份证号码.");
    //    return;
    //}
    if (Name == null || Name == "" || typeof (Name) == undefined) {
        UsTips("姓名不能为空.");
        return;
    } else if (!nameReg.test(Name)) {
        UsTips("只支持中文姓名.");
        return;
    }
    if (Name.length > 20) {
        UsTips("名字太长了.");
        return;
    }
    if (Telphone == null || Telphone == "" || typeof (Telphone) == undefined) {
        UsTips("电话不能为空");
        return;
    } else if (!moblieReg.test(Telphone)) {
        UsTips("电话不能为空.");
        return;
    }
    if (ProvinceId == null || ProvinceId == "" || typeof (ProvinceId) == undefined) {
        UsTips("选择省区.");
        return;
    }
    if (CityId == null || CityId == "" || typeof (CityId) == undefined) {
        UsTips("选择城市.");
        return;
    }
    if (RegionId == null || RegionId == "" || typeof (RegionId) == undefined) {
        UsTips("选择区县.");
        return;
    }
    if (addressNo == null || addressNo == "" || typeof (addressNo) == undefined) {
        UsTips("填写详细地址.");
        return;
    }
    $.post('/Member/saveAddress', {
        "Name": Name, "Telphone": Telphone, "ProvinceId": ProvinceId, "CityId": CityId,
        "RegionId": RegionId, "addressNo": addressNo, "Province": Province, "City": City, "Region": Region, "Id": Id
    }, function (date) {
        var d = date.indexOf("&");
        if (d >0)
        {
            var arrRult = date.split("&");
            var arrCout = arrRult[0];
            var arrMeg = arrRult[1];
            if (ReturnUrl != null && ReturnUrl != "")
            {
                if (ReturnUrl.indexOf("?") > 0) {
                    ReturnUrl = ReturnUrl + "&AddId=" + arrMeg;
                } else { ReturnUrl = ReturnUrl + "?AddId=" + arrMeg; }
              
                goUrl(ReturnUrl);
            }
            else {
                goUrl("/Member/Address/");
            }
        }
        else { UsTips("保存失败。"); }
    })
    
}

