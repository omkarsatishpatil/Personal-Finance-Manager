document.getElementById("email").addEventListener("input", function() {
    var email = this.value;
    var username = email.substring(0, email.indexOf("@"));
    document.getElementById("username").value = username;
});

//$(document).ready(function () {
//    $("#proBtn").click(function () {
//        $.ajax({
//            url: '@Url.Action("_userProfileView", "Dashboard")',
//            type: 'GET',
//            success: function (data) {
//                $("#user_profile_container").html(data);
//            },
//            error: function () {
//            }
//        });
//    });
//});
