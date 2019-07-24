layui.config({ base: "/Assets/js/" }).use(["form", "layer"], function () {
    var d = layui.form,
        b = parent.layer === undefined ? layui.layer : parent.layer,
        e = layui.jquery;
    if (window.innerWidth > 1000)
    { e("#videoBox").show() }
    e("#vCode").on("click", function () {
        var f = Math.random();
        e(this).attr("src", "/Login/GetVCode?random=" + f)
    });
    var c = 0;
    var flag = true;
    e(document).on("keydown", function (event)
    {
        event = event || window.event;
        if (event.keyCode === 13)
        {
            b.closeAll()
        }
    });
    d.on("submit(login)", function (f) {
        c++;
        if (c == 3) { } if (c > 5) { alert("剩余0次尝试,忘记密码请联系管理员~"); return false } b.load(); e("#login_btn").text("正在登陆..."); f.field = a(f.field); e.post("/Login/Login", f.field, function (g) { if (g.code == 1000) { e(".login_btn").html("登陆成功 正在跳转..."); location.href = "/Home/Index" } else { b.closeAll(); e("#login_btn").text("登陆"); b.msg(g.msg, { icon: 2, skin: "layer-ext-moon", anim: 6, closeBtn: 1 }); e("#vCode").click() } }).error(function (g) { b.closeAll(); e("#login_btn").text("登陆"); b.msg(g.status + "网络繁忙~请联系管理员！", { icon: 2, skin: "layer-ext-moon", anim: 6, closeBtn: 1 }); e("#vCode").click() }); return false
    });
    function a(g) {
        var f = g;
        f.uPwd = md5(f.uPwd);
        f["t"] = +new Date();
        return f
    }
});