$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    var location = document.location.href;
    $.each($(".side-menu li"), function () {
        var thisHref = $(this).children("a").attr("href");
        var pos = location.indexOf(thisHref);
        if (pos !== -1) {
            $(this).addClass("active");
        }
    });

    $(".dialog").click(function (e) {
        e.preventDefault();
        $("#contentDialog .modal-title").html($(this).attr("title"));
        $("#contentDialog .modal-body").html("Загрузка...");
        $("#contentDialog .modal-body").load($(this).attr("href"));
        $("#contentDialog").modal("show");
    });

    $(".logout").click(function (e) {
        e.preventDefault();
        Logout();
    });

    function Logout() {
        var tokenKey = "tokenInfo";
        var initialInner = $("#login").html();
        $(".logout").html("<img src='/Areas/Admin/Img/loader.gif' />");

        $.ajax({
            type: 'GET',
            url: '/api/Account/Logout',
            beforeSend: function (xhr) {
                var token = localStorage.getItem(tokenKey);
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            },
            complete: function (data) {
                if (data.responseText === "") {
                    localStorage.removeItem(tokenKey);
                    $(".logout").html(initialInner);
                    location.reload();
                }
                else {
                    alert(data.responseText);
                }
            }
        });
    }
});