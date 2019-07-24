/** ilayui.js By tanwei */
layui.define(['form'], function (exports) {
    "use strict";
    var $ = layui.jquery,
        layer = parent.parent.layer || parent.layer || layui.layer,
        form = layui.form,
        cacheName = 'ilayui',
        window = window
        ;
    var Ilayui = function () {
		/**
		 *  默认配置 
		 */
        this.config = {
            province: undefined, //省容器
            city: undefined, //市容器
            area: undefined, //区容器
            data: undefined,//数据源
            url: undefined, //数据源地址
            type: 'GET', //读取方式
            cached: false, //是否使用缓存
        };
        this.v = '0.0.1';
    };
    /**
    *  初始化 
    */
    Ilayui.prototype.init = function () {
        var _that = this;
        var _config = _that.config;

        if (!_config.province || !_config.city || !_config.area) _that.alertError('Ilayui error: province,city,area参数未定义或设置出错.');
        if (!_config.url) _that.alertError('Ilayui error: url参数未定义或设置出错.');

        var province = _config.province,
            city = _config.city,
            area = _config.area,
            url = _config.url;
        //初始化省
        _that.render(url, 0, province);
        //初始化市
        _that.render(url, city.val(), city);
        //初始化区
        _that.render(url, area.val(), area);
        //省下拉值改变事件
        form.on('select(Province)', function (data) {
            _that.render(url, data.value, city, function (val) {
                _that.render(url, val, area);
            });
        });
        //市下拉值改变事件
        form.on('select(City)', function (data) {
            _that.render(url, data.value, area);
        });
    }
    /**
    *  渲染省市区联动 
    */
    Ilayui.prototype.render = function (url, pid, elem, callback) {
        var _that = this;
       
        $.post(url, { pid: pid }, function (data) {
            if (elem.val() != null) {

                var op = elem.children("option[value=" + elem.val() + "]");

                elem.html("");

                $.each(data, function (i, v) {
                    if (op.text() == v.s_name) {
                        elem.append('<option value=' + v.s_id + '>' + v.s_name + '</option>');
                    }
                });
                $.each(data, function (i, v) {
                    if (op.text() != v.s_name) {
                        elem.append('<option value=' + v.s_id + '>' + v.s_name + '</option>');
                    }
                });
                form.render('select');
                if (callback) {
                    callback(data[0].s_id);
                }
            }
        }).error(function (ex) { _that.showExMsg(ex) });
        return _that;
    };
    /** 快速Post
     * param data 传到服务器的参数
     */
    Ilayui.prototype.myPost = function (url, data,callBack) {
        var _that = this;
        data = data || { id: 0 };
        $.post(url, data, function (data) {
            _that.alertMsg(data, callBack);
        }).error(function (ex) {
            layer.alert(ex.status + " 网络繁忙~请联系管理员!", {
                icon: 2,
                skin: 'layer-ext-moon',
                anim: 6,
                closeBtn: 1
            }, function () {
                location.reload();
            });
        });
    }
    /** 快速弹窗
     * param data 服务器返回的json数据
     */
    Ilayui.prototype.alertMsg = function (data, callBack) {
        var _that = this;
        var _config = _that.config;
        if (data.code == 1000) {
            var index=layer.alert(data.msg, {
                icon: 6,
                skin: 'layer-ext-moon',
                anim: Math.round(Math.random() * 5 + 1),
                closeBtn: 0
            }, function () {
                layer.close(index);
                if (callBack) {
                    callBack();
                }
            });
        } else if (data.code == 2222) {
            _that.alertError(data.msg, function () {
                location.reload();
            });
        } else {
            if (data.msg) {
                _that.alertError(data.msg);
            } else {
                _that.alertError(data);
            }
            
        }
    }
    /*
     *弹出错误提示
     */
    Ilayui.prototype.alertError = function (msg, callBack) {
        var that = this;
        if (msg) {
            var alIndex= layer.alert(msg, {
                icon: 2,
                skin: 'layer-ext-moon',
                anim: 6,
                closeBtn: 1
            }, function () {
                if (callBack) {
                    callBack();
                } else {
                    layer.close(alIndex);
                }
            });
        }
        if (!msg) {
            layer.msg("未知错误", {
                anim: 6
            });
        }
        return that;
    };
	/**
	 * 配置Ilayui
	 * @param {Object} options
	 */
    Ilayui.prototype.set = function (options) {
        var that = this;
        that.config.data = undefined;
        $.extend(true, that.config, options);
        return that;
    };
	/**
	 * 清除缓存
	 */
    Ilayui.prototype.cleanCached = function () {
        layui.data(cacheName, null);
    };
    var ilayui = new Ilayui();
    exports('ilayui', function (options) {
        return ilayui.set(options);
    });
});