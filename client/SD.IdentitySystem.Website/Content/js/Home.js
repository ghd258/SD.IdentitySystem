﻿//消息框
var messageBox = null;

//提供给iframe里子页面操作当前页面的一些便捷方法
var topHelper = {};

//DOM初始化事件
$(function () {
    //初始化消息框
    messageBox = new MessageBox({ imghref: "/Content/images/" });

    //初始化用户菜单
    $("#menuTree").tree({
        url: "/Menu/GetMenuTree",
        animate: true,
        lines: true,
        onClick: function (node) {
            addTab(node.text, node.attributes.href, node.attributes.isLink);
        }
    });

    //初始化修改密码窗口
    $("#dvUpdatePwd").window({
        width: 350,
        height: 200,
        maximizable: false,
        resizable: false,
        draggable: false,
        modal: true,
        minimizable: false,
        collapsible: false,
        closed: true
    });

    //初始化公共窗体
    topHelper.comWin = $("#commonWindow").window({
        width: 800,
        height: 500,
        collapsible: false,
        minimizable: false,
        maximizable: true
    }).window("close");

    //添加一个打开公共窗体的方法
    topHelper.showComWindow = function (title, url, width, height) {
        var trueTitle = "公共窗体";
        var trueWidth = 1200;
        var trueHeight = 500;
        if (title) trueTitle = title;
        if (width && parseInt(width) > 10) {
            trueWidth = width;
        }
        if (height && parseInt(height) > 10) {
            trueHeight = height;
        }
        //判断是否置顶url，如果有，则设置公共窗体里的iframe的src
        if (url && url.length && url.length > 10) {
            $("#commonWindow iframe").attr("src", url);
        }
        //重新设置窗体的大小，并自动居中，然后才显示
        topHelper.comWin.window({
            title: trueTitle,
            width: trueWidth,
            height: trueHeight
        }).window("center").window("open");
    };

    //添加一个关闭公共窗体方法
    topHelper.closeComWindow = function () {
        topHelper.comWin.window("close");
    };
});

//#region

//注销
function logout() {
    $.messager.confirm("警告", "确定注销当前用户吗？", function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: "/User/Logout",
                dataType: "json",
                data: null,
                contentType: "application/json; charset=utf-8",
                success: function () {
                    window.location.href = "/User/Login";
                },
                error: function (error) {
                    if (error.status === 200) {
                        window.location.href = "/User/Login";
                    }
                    else {
                        $.messager.alert("Error", error.responseText);
                    }
                }
            });
        }
    });
}

//#endregion

//修改密码窗口
function openChangePassword() {
    $("#dvUpdatePwd").window("open");
    $("#dvUpdatePwd").window("setTitle", "修改密码");
    $("#fmUpdatePwd").form("clear");
}

//修改密码
function changePassword() {
    var oldPassword = $("#oldPassword").val();
    var newPassword = $("#newPassword").val();
    var confirmPassword = $("#confirmPassword").val();
    if (oldPassword.length === 0 ||
        newPassword.length === 0 ||
        confirmPassword.length === 0) {
        messageBox.showMsgErr("密码不可为空，请重新输入！");
    }
    else if (newPassword !== confirmPassword) {
        messageBox.showMsgErr("两次密码不一致，请重试！");
    }
    else {
        var loginId = $("#spLoginId").text();
        $("#hdLoginId").val(loginId);
        $("#fmUpdatePwd").submit();
    }
}

//修改密码中事件
function updatingPassword() {
    messageBox.showMsgWait("修改中，请稍后...");
}

//修改密码成功
function updateSucceed() {
    messageBox.showMsgOk("修改成功");
    $("#dvUpdatePwd").window("close");
}

//修改密码失败
function updateFailed(result) {
    messageBox.showMsgErr(result.responseText);
    $("#dvUpdatePwd").window("close");
}

//添加tab方法
function addTab(title, url, isLink) {
    //判断是否是链接
    if (isLink == "1") {          //是链接
        if ($("#tabs").tabs("exists", title)) {
            var currTab = $("#tabs").tabs("getSelected");
            $("#tabs").tabs("select", title);
            var url = $(currTab.panel("options").content).attr("src");
            if (url != undefined) {
                $("#tabs").tabs("update", {
                    tab: currTab,
                    options: {
                        content: createFrame(url)
                    }
                });
            }
        } else {
            var content = createFrame(url);
            $("#tabs").tabs("add", {
                title: title,
                content: content,
                closable: true
            });
        }
    }
}

//创建iframe方法
function createFrame(url) {
    var tabHeight = $("#tabs").height() - 35;
    var frame = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:' + tabHeight + 'px;"></iframe>';
    return frame;
}

//新增或修改成功后，可通过此方法更新tab里的DataGrid组件
function updateGridInTab() {
    //1.获取后台首页的tab容器
    var $tabBox = $("#tabs");

    //2.获取选中的tab
    var $curTab = $tabBox.tabs("getSelected");

    //3.从选中的tab中获取iframe，并以jq对象返回
    var $ifram = $("iframe", $curTab);

    //4.从jq对象中获取iframe，并通过contentWindow对象操作iframe里的window的全局变量$tbGrid

    //清除选中
    $ifram[0].contentWindow.$tbGrid.datagrid("clearSelections");

    //刷新表格
    $ifram[0].contentWindow.$tbGrid.datagrid("reload");                      
}