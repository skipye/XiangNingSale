//设置ajax全局默认参数
//$.ajaxSetup({
//    contentType : "application/x-www-form-urlencoded",
//    timeout : 60000,
//    dataType : "json",
//    async: false,
//    xhrFields: {
//        withCredentials: true    // 要在这里设置 跨域设置cookie
//    },
//    error : function (xmlRequest, errorInfo, exception) {
//        console.log("XmlHttpRequest:" + xmlRequest + ", errorInfo:" + errorInfo + ", exception:" + exception);
//    }
//});
//操作日志，MSGSTatus状态：登录：1，增加：2，删：3，改：4，审核：5，驳回：6，客户：7,付款：8
function AddWorkLogs(MSG, MSGSTatus)
{
    var PostUrl = '/Users/AddWorkLogs';
    $.post(PostUrl, { MSG: MSG, MSGSTatus: MSGSTatus }, function (d) { });
}

//删除
function del(obj,id) {
    var ListId = "";
    var PostUrl = $(obj).attr("data-url");
    var ListId = "";
    if (id > 0) {
        ListId = id + "$";
    } else {
        var NewObj = $(obj).parent().parent().siblings("div.checkmodel").find("table.table");
        NewObj.find("input[type='checkbox']:checked").each(function () {
            ListId += $(this).val() + "$";
        });
    }
    if (ListId != "" && ListId != undefined) {
        layer.confirm('您确定要这么操作吗？', function (index) {
            $.post(PostUrl, { ListId: ListId }, function (d) {
                if (d == "True") {
                    var MSG = "删除操作，删除ID：" + ListId + "，删除网址：" + PostUrl;
                    AddWorkLogs(MSG, 3);
                    layer.msg('操作成功!', { icon: 1, time: 1000 });
                    ResetWindow();
                }
                else { layer.msg('服务器错误!', { icon: 2, time: 1000 }); }
            });
        });
        }
    else { layer.alert("请先去选中！"); }
}
/*审核*/
function checked(obj, id) {
    var PostUrl = $(obj).attr("data-url");
    var ListId = "";
    if (id > 0) {
        ListId = id + "$";
    } else {
        
        var NewObj = $(obj).parent().parent().siblings("div.checkmodel").find("table.table");
        NewObj.find("input[type='checkbox']:checked").each(function () {
            ListId += $(this).val() + "$";
        });
    }
    if (ListId == "" || ListId == undefined) {
        layer.alert("请先去选中！");
        return false;
    }
    layer.confirm('您正在进行审核操作，请认真核对信息！', {
        btn: ['通过', '不通过', '取消'],
        shade: false,
        closeBtn: 0
    },
	function () {
	    $.post(PostUrl, { ListId: ListId, CheckedId: 1 }, function (d) {
	        if (d == "True") {
	            var MSG = "审核操作，审核ID：" + ListId + "，审核网址：" + PostUrl;
	            AddWorkLogs(MSG, 5);
	            layer.msg('已审核', { icon: 6, time: 1000 });
	            if (id > 0) {
	                $(obj).parents("tr").find(".checkedstatus").html('<span class="label label-success radius">通过</span>');
	                $(obj).remove();
	            } else { ResetWindow(); }
	        } else { layer.msg('操作失败！', { icon: 2, time: 1000 }); ResetWindow(); }
	    });
	},
	function () {
	    $.post(PostUrl, { ListId: ListId, CheckedId: 2 }, function (d) {
	        if (d == "True") {
	            var MSG = "驳回操作，驳回ID：" + ListId + "，驳回网址：" + PostUrl;
	            AddWorkLogs(MSG, 6);
	            layer.msg('未通过', { icon: 5, time: 1000 });
	            if (id > 0) {
	                $(obj).parents("tr").find(".checkedstatus").html('<span class="label label-danger radius">被驳回</span>');
	                $(obj).remove();
	            } else { ResetWindow(); }
	        } else { layer.msg('操作失败！', { icon: 2, time: 1000 }); ResetWindow(); }
	    });
	});
}
function belong(title, url, id, w, h) {
    var MSG = title + "，客户所属操作，操作ID：" + id + "，编辑网址：" + url;
    AddWorkLogs(MSG, 7);
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?Id=" + id
    });
    layer.full(index);
}
function belongwindow(title, url, id, w, h) {
    var MSG = title + "，客户所属操作，操作ID：" + id + "，编辑网址：" + url;
    AddWorkLogs(MSG, 7);
    layer_show(title, url + "?Id=" + id, w, h);
}
function edit(title, url, id, w, h) {
    var MSG = title + "编辑操作，编辑ID：" + id + "，编辑网址：" + url;
    AddWorkLogs(MSG, 4);
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?Id=" + id
    });
    layer.full(index);
}
function FR(title, url, id, w, h) {
    var MSG = title + "操作，付款ID：" + id + "，付款网址：" + url;
    AddWorkLogs(MSG, 8);
    layer_show(title, url + "?Id=" + id, w, h);
}
function add(title, url, w, h) {
    var MSG = title + "增加操作，增加网址：" + url;
    AddWorkLogs(MSG, 2);
    var index = layer.open({
        type: 2,
        title: title,
        content: url 
    });
    layer.full(index);
}
function editwindow(title, url, id, w, h) {
    var MSG = title + "编辑操作，编辑ID：" + id + "，编辑网址：" + url;
    AddWorkLogs(MSG, 4);
    layer_show(title, url + "?Id=" + id, w, h);
}
function addwindow(title, url, w, h) {
    var MSG = title + "增加操作，增加网址：" + url;
    AddWorkLogs(MSG, 2);
    layer_show(title, url, w, h);
}
//查看
function show(title, url, id, w, h) {
    layer_show(title, url + "?Id=" + id, w, h);
}
//打开窗口
function opneview(obj, id)
{
    alert(1);
    var ListId = "";
    var PostUrl = $(obj).attr("data-url");
    var title = $(obj).attr("title");;
    var ListId = "";
    if (id > 0) {
        ListId = id + "$";
    } else {
        var NewObj = $(obj).parent().parent().siblings("div.checkmodel").find("table.table");
        NewObj.find("input[type='checkbox']:checked").each(function () {
            ListId += $(this).val() + "$";
        });
    }
    if (ListId != "" && ListId != undefined) {
        var MSG = title+"审核操作，审核ID：" + ListId + "，审核网址：" + PostUrl;
        AddWorkLogs(MSG, 5);
        layer_show(title, PostUrl + "?ListId=" + ListId, 600, 300);
    }
    else { layer.alert("请先去选中！"); }
    
}
function ajaxRequest(requestType, url, params, backFuc) {
    $.ajax({
        method : requestType,
        url : url,
        data: params,
        dataType: "json",
        success : function (data) {
            backFuc(data);
        }
    });
}

//时间方法,时间对象转换成"2010-09-12 22:12:23"格式
function formatDateTime(inputTime) {
    var date = new Date(inputTime);
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? ('0' + m) : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    var h = date.getHours();
    h = h < 10 ? ('0' + h) : h;
    var minute = date.getMinutes();
    var second = date.getSeconds();
    minute = minute < 10 ? ('0' + minute) : minute;
    second = second < 10 ? ('0' + second) : second;
    return y + '-' + m + '-' + d+' '+h+':'+minute+':'+second;
}

//时间方法，时间对象转换成"2010-09-12"格式
function formatDate(inputTime) {
    var date = new Date(inputTime);
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? ('0' + m) : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;

    return y + '-' + m + '-' + d;
}
//时间方法，将毫秒转换成标准时间格式
function transDate(date){
    date = new Date(date);
    var y=date.getFullYear();
    var m=date.getMonth()+1;
    var d=date.getDate();
    var h=date.getHours();
    var m1=date.getMinutes();
    var s=date.getSeconds();
    m = m<10?("0"+m):m;
    d = d<10?("0"+d):d;
    return y+"-"+m+"-"+d+" "+h+":"+m1+":"+s;
}
//时间方法，形如"2010-09-12"格式转换成时间对象
function str2Date(dateStr) {
    if (!dateStr) {
        return null;
    }
    return new Date(dateStr.replace(/-/g,"/"));
}


//数组转换成后台可识别模式
function transArray(data) {
    var dataType = typeof data;
    if (dataType == "string" || dataType == "number") {
        return data;
    }
    var afterTransData = {};
    if (data) {
        for (var key in data) {
            var value = data[key];
            if (Array.isArray(value)) {
                //是数组
                for (var i = 0; i < value.length; ++i) {
                    var o = value[i];
                    var result = transArray(o);
                    var resultType = typeof result;
                    if (resultType == "string" || resultType == "number") {
                        var newKey = key + "[" + i + "]";
                        afterTransData[newKey] = result;
                        continue;
                    }
                    for (var key1 in result) {
                        var newKey = key + "[" + i + "]" + "." + key1;
                        afterTransData[newKey] = result[key1];
                    }
                }
            } else {
                //非数组
                afterTransData[key] = value;
            }
        }
    }

    return afterTransData;
}

//判断是否编辑模式，并获取编辑id
function editModel(url) {
    var qIndex = url.lastIndexOf("?");
    if (qIndex != -1) {
        var requestParam = url.substring(qIndex + 1);
        if (requestParam) {
            var id = requestParam.substring(requestParam.lastIndexOf("=") + 1);
            return id;
        }
    }

    return null;
}

//判断是否编辑模式，获取url参数
function editModelParam(url) {
    var qIndex = url.lastIndexOf("?");
    if (qIndex != -1) {
        var requestParam = url.substring(qIndex + 1);
        if (requestParam) {
            return decodeURIComponent(requestParam);
        }
    }

    return null;
}

//判断是否编辑模式，并获取编辑id
function editModel2(url) {
    var qIndex = url.lastIndexOf("?");
    if (qIndex != -1) {
        var requestParam = url.substring(qIndex + 1);
        if (requestParam) {
            var codeNunber = requestParam.substring(requestParam.lastIndexOf("=") + 1);
            return encodeURIComponent(codeNunber);
        }
    }

    return null;
}

//对象转换url参数
function object2UrlParamStr(object) {
    var urlParam = "";
    for (var key in object) {
        if (Array.isArray(object[key])) {
            continue;
        }
        urlParam += (key + "=" + object[key]);
        urlParam += ",";
    }
    urlParam = urlParam.substring(0, urlParam.lastIndexOf(","));

    return urlParam;
}

//百度地图API功能
function loadJScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "http://api.map.baidu.com/api?v=2.0&ak=iEWlGKfvfIdgtRo0GYimqcGzhmLvi3cb&callback=init";
    document.body.appendChild(script);
}

//获取bitNumber上第bit位的数值为0还是1
function getNumberBitValue(bitNumber, bit) {
    return ((1 << bit) & bitNumber) >> bit;
}

//并刷新父页面
function ResetWindow() {
    setTimeout(function () { window.location.reload(); }, 1000);//0.5秒后强制刷新
}
function removeIframe() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}
//限制只能输入数字
$(".number").keyup(function () {  //keyup事件处理
    $(this).val($(this).val().replace(/\D|^0/g, ''));
}).bind("paste", function () {  //CTR+V事件处理
    $(this).val($(this).val().replace(/\D|^0/g, ''));
}).css("number", "disabled");  //CSS设置输入法不可用