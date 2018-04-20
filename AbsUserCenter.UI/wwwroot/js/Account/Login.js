function changeCode(id) {
    //刷新验证码
    var rd = Math.random();
    var vca = "/Account/ChkCode/" + Math.round(rd * 10) + ".jpg";
    document.getElementById(id).src = "/Account/ChkCode/" + Math.round(rd * 10) + ".jpg"; 
}

function errorClose(tar) {
    $(tar).parents(".errorBox").addClass("hidden");
}

var QueryString = function () {
    // This function is anonymous, is executed immediately and 
    // the return value is assigned to QueryString!
    var query_string = {};
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        // If first entry with this name
        if (typeof query_string[pair[0]] === "undefined") {
            query_string[pair[0]] = decodeURIComponent(pair[1]);
            // If second entry with this name
        } else if (typeof query_string[pair[0]] === "string") {
            var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
            query_string[pair[0]] = arr;
            // If third or later entry with this name
        } else {
            query_string[pair[0]].push(decodeURIComponent(pair[1]));
        }
    }
    return query_string;
}();


$(document).ready(function () {
    //lxyin 20160203
    String.prototype.trim = function () {
        return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    }

    changeCode('imgChkCode');
    $("#loginname").focusout(function () {
        if ($("#loginname").val().trim() == "")
            $("#loginerror").removeClass("hidden");
        else
            $("#loginerror").addClass("hidden");
    });
    $("#loginpwd").focusout(function () {
        if ($("#loginpwd").val().trim() == "")
            $("#pwderror").removeClass("hidden");
        else
            $("#pwderror").addClass("hidden");
    });
    $("#authcode").focusout(function () {
        if ($("#authcode").val().trim() == "")
            $("#codeerror").removeClass("hidden");
        else
            $("#codeerror").addClass("hidden");
    });

    $("#loginsubmit").click(function () {
        if ($("#loginname").val().trim() == "") {
            $("#loginname").focus();
            $("#loginerror").removeClass("hidden");
            return false;
        }

        if ($("#loginpwd").val().trim() == "") {
            $("#loginpwd").focus();
            $("#pwderror").removeClass("hidden");
            return false;
        }
        if ($("#authcode").val().trim() == "") {
            $("#authcode").focus();
            $("#codeerror").removeClass("hidden");
            return false;
        } 
        $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: { userName: $("#loginname").val().trim(), passWord: $("#loginpwd").val().trim(), txtCode: $("#authcode").val().trim(), backUrl: QueryString.backUrl, sysId: QueryString.sysId },
            success: function (data) {
                if (!data.success) {
                    $("#ccerr").removeClass("hidden");
                    $("#ccerrmsg").html("<b>" + data.msg + "</b>");
                    changeCode('imgChkCode');
                    $("#errContainer").text(data.msg);
                } else {
                    delCookie("acurrent");
                    window.location.href = data.backUrl;
                }
            } 
        }); //ajax
    });
    $("body").keydown(function () {
        if (event.keyCode == "13") {
            $('#loginsubmit').click();
        }
    });
    //delete cookie
    function delCookie(sName) {
        var date = new Date();
        date.setTime(date.getTime() - 10000);
        document.cookie = sName + "=a; expires=" + date.toGMTString();
    }
});