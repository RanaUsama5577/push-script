// login
const loginForm = document.querySelector('#login-form');
loginForm.addEventListener('submit', (e) => {
    // make auth and firestore references
    const auth = firebase.auth();
    e.preventDefault();
    // get user info
    const email = loginForm['login-email'].value;
    const password = loginForm['login-password'].value;
    if (email == "" || password == "") {

    }
    if (email == "") {
        $('#login-email').addClass('is-invalid');
        iziToast.error({
            title: 'Authentication Failed!',
            message: "Please Provide Login Credentials",
            position: 'topRight'
        });
        return false;
    }
    else if (!validateEmail(email)) {
        $('#login-email').addClass('is-invalid');
        iziToast.error({
            title: 'Authentication Failed!',
            message: "Please Provide Login Credentials",
            position: 'topRight'
        });
        return false;
    }
    else if (password == "") {
        $('#login-email').removeClass('is-invalid');
        $('#login-password').addClass('is-invalid');
        iziToast.error({
            title: 'Authentication Failed!',
            message: "Please Provide Login Credentials",
            position: 'topRight'
        });
        return false;
    }
    // log the user in
    auth.signInWithEmailAndPassword(email, password).then((cred) => {
        first();
        function first() {
            db.collection("admin").doc(email).get()
                .then(function (docSnapshot) {
                    //var data = docSnapshot.data();
                    $('#login-email').addClass('is-valid');
                    $('#login-password').addClass('is-valid');
                    $('#login-password').removeClass('is-invalid');
                    $('#login-button').addClass("btn-progress");
                    iziToast.success({
                        title: 'Authentication Successful!',
                        message: 'You are logged in',
                        position: 'topRight'
                    });
                    setTimeout(function () {
                        second();
                    }, 1000);
                })
                .catch(function (error) {
                    console.log("Error", error);
                });
        }
        function second() {
            window.location.href = "/Subscribers/Index";
        }
    }).catch(function (error) {
        if (error.code == "auth/user-not-found") {
            $('#login-email').addClass("is-invalid");
            $('#login-password').addClass("is-invalid");
        }
        else if (error.code == "auth/wrong-password") {
            $('#login-password').addClass("is-invalid");
            $('#login-email').removeClass("is-invalid");
        }
        var errorMessage = error.message;
        iziToast.error({
            title: 'Authentication Failed!',
            message: errorMessage,
            position: 'topRight'
        });
    });
});

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}