/***  页面初始化*/
var PageInit = function () {
    /**
	*  默认配置 
	*/
    this.config = {
        projectID: 0,
        type: 'GET', //读取方式
        cached: false, //是否使用缓存
    };
    this.v = '0.0.1';
};
PageInit.prototype.getTitle = function (id) {
    var tilet = "";
    var xmlhttp;
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    } else { xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200)
        { tilet = xmlhttp.responseText; }
    }
    xmlhttp.open("GET", "/project/GetTitle?id=" + id, true);
    xmlhttp.send();
    return tilet;

}
PageInit.prototype.getUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return ''; //返回参数值
}