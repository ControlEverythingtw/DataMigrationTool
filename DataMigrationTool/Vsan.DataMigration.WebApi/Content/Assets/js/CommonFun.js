//重写时间转换方法
Date.prototype.toLocaleString = function () {
    var mm = (this.getMonth() + 1);
    var d = this.getDate();
    var m = this.getMinutes();
    var h = this.getHours();
    var s = this.getSeconds()
    if (mm < 10) mm = "0" + mm;
    if (d < 10) d = "0" + d;
    if (m < 10) m = "0" + m;
    if (h < 10) h = "0" + h;
    if (s < 10) s = "0" + s;
    return this.getFullYear() + "/" + mm + "/" + d + " " + h + ":" + m + ":" + s
};
function ConvertDate(str) {
    if (str === null || str === '' || str.indexOf(')')<=0) {
        return '-';
    }
    str = str.replace("/Date(", "").replace(")/", "");
    var unixTimestamp = new Date(parseInt(str));
    return unixTimestamp.toLocaleString();
}
//判断数组中是否存在莫个值
Array.prototype.in_array = function (e) {
    var r = new RegExp(',' + e + ',');
    return (r.test(',' + this.join(this.S) + ','));
};
//判断数组中是否存在莫个值
function IsInArray(arr, val) {
    var testStr = ',' + arr.join(",") + ",";
    return testStr.indexOf("," + val + ",") !== -1;
}
//弹出成功提示
function okAlert(msg, callBack) {
    var index= top.layer.alert(msg, {
        icon: 6,
        skin: 'layer-ext-moon',
        anim: Math.round(Math.random() * 5 + 1),
        closeBtn: 0
    }, function () {
        top.layer.close(index);
        if (callBack) {
            callBack();
        }
    });
}
//弹出错误提示
function errorAlert(msg, callBack) {
    var alIndex = top.layer.alert(msg, {
        icon: 2,
        skin: 'layer-ext-moon',
        anim: 6,
        closeBtn: 1
    }, function () {
        if (callBack) {
            callBack();
        } 
        top.layer.close(alIndex);
    });
}
function confirmAjax(msg, url) {
    layer.confirm(msg, function (index) {
        layer.close(index);
        $.get(url, function (res) {
            if (res.code === 0) {
                alertOK(res.message);
            } else {
                alertError(res.message);
            }
        });
    });
}
function ajaxGet(url,callback) {
    // 1.创建请求对象
    var xhr = new XMLHttpRequest();
    // 2.创建open方法确认请求方式和地址
    //   ps(记住get方法有参数的话在url后面用 ? 符号连接再加上参数如 : url ? num = 3, 多个参数用 & 符号连接);
    xhr.open('get', url);
    //3.监听事件完成
    xhr.onreadystatechange = function () {
        //readYstate: readyState：存有 XMLHttpRequest 的状态。从 0 到 4 发生变化;
        //status:响应的HTTP状态码;
        if (xhr.readyState === 4 & xhr.status === 200) {
            //打印响应体 console.log(xhr.responseText)
            if (callback) {
                var res = JSON.parse(xhr.responseText);
                callback(res);
            } else {
                console.log(xhr.responseText);
            }
        }
    };
    // 4.发送请求
    xhr.send();
}

//渲染Select
function renderSelect(elem, url, option) {

    if (!option) {
        alert('option undefined! ');
        return;
    }
    if (!elem) {
        alert('option.elem undefined! ');
        return;
    }
    if (!option.defaultValue) {
        option.defaultValue = '';
    }
    if (!option.defaultText) {
        option.defaultText = '请输入或选择';
    }
    if (!option.valueField) {
        option.valueField = 'id';
    }
    if (!option.textField) {
        option.textField = 'text';
    }
    ajaxGet(url, function (res) {

        if (res.code === 0) {
            var data = res.data;
            var html = ['<option value="', option.defaultValue, '">', option.defaultText, '</option>'];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var value = item[option.valueField] || item;
                var text = item[option.textField] || item;
                html.push(' <option value="', value, '">', text, '</option>');
            }

            var htmlStr = html.join('');
            if (elem.substr(0, 1) === ".") {
                var elemArr = document.getElementsByClassName(elem.substr(1));
                for (var j = 0; j < elemArr.length; j++) {
                    elemArr[j].innerHTML = htmlStr;
                }
            } else {
                document.getElementById(elem).innerHTML = htmlStr;
            }

            if (option.callBack) {
                option.callBack(res);
            }

        }
    });
}


// ReSharper disable once NativeTypePrototypeExtending
Array.prototype.find = function (field, value) {
    var that = this;
    var obj = {};
    for (var i = 0; i < that.length; i++) {
        var item = that[i];
        if (item[field]==value) {
            obj = item;
            break;
        }
    }
    return obj;
};


var width = window.innerWidth > 800 ? '800px' : '90%';
var height = window.innerWidth > 494 ? '494px' : '90%';