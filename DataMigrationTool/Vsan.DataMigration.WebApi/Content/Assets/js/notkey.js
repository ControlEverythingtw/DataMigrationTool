//屏蔽鼠标右键、Ctrl+N、Shift+F10、F11、F5刷新、退格键
document.oncontextmenu = function () { event.returnValue = false; }//屏蔽鼠标右键
window.onhelp = function () { return false }   //屏蔽F1帮助
document.onkeydown = function () {
    if ((window.event.altKey) &&
        ((window.event.keyCode == 37) ||       //屏蔽   Alt+   方向键   ←
            (window.event.keyCode == 39))) {     //屏蔽   Alt+   方向键   →
        event.returnValue = false;
        return false;
    }
    /* 注：这还不是真正地屏蔽Alt+方向键，
     因为Alt+方向键弹出警告框时，按住Alt键不放，
    用鼠标点掉警告框，这种屏蔽方法就失效了。*/
    if ((event.keyCode == 8) ||                                 //屏蔽退格删除键
        (event.keyCode == 116) ||                             //屏蔽   F5   刷新键
        (event.ctrlKe && event.keyCode == 82)) {              //Ctrl   +   R
        event.keyCode = 0;
        event.returnValue = false;
    }
    if (event.keyCode == 122) { event.keyCode = 0; event.returnValue = false; }         //屏蔽F11
    if (event.ctrlKey && event.keyCode == 78) event.returnValue = false;      //屏蔽Ctrl+n
    if (event.shiftKey && event.keyCode == 121) event.returnValue = false;     //屏蔽shift+F10
    if (window.event.srcElement.tagName == "A" && window.event.shiftKey)
        window.event.returnValue = false;                                   //屏蔽shift加鼠标左键新开一网页
    if ((window.event.altKey) && (window.event.keyCode == 115)) {              //屏蔽Alt+F4
        window.showModelessDialog("about:blank", "", "dialogWidth:1px;dialogheight:1px");
        return false;
    }
}