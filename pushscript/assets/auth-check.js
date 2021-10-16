firebase.auth().onAuthStateChanged(function (user) {
    if (user) {
        var usersRef = db.collection("admin").doc(user.email);
        usersRef.onSnapshot((doc) => {
            if (doc.exists) {
                var data = doc.data();
                $('#uid').val(doc.id);
                $(".imageUrl").attr('src', data.image_url);
                $('#username').val(data.name);
                $('#userName').val(data.name);
                $('#email').val(data.user_email);
                if (data.name.length > 16) {
                    var substr = data.name.substr(0, 14)
                    $('#nav-user-name').html("Hello " + substr + "..");
                }
                else {
                    $('#nav-user-name').html("Hello " + data.name);
                }
                $('.userName').html(data.name);
                $('.user-name').html(data.name);
            }
        })
        setTimeout(function () {
            $(".loader").fadeOut("slow");
        }, 2000);
    }
    else {
        window.location.href = "/Auth/Login";
    }
});