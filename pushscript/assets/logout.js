function logout() {
    first();
    function first() {
        TimerSweet('Signout!', 'You are going to logout', 'info',1000);
        setTimeout(function () {
            second();
        }, 1000);
    }
    function second() {
        firebase
            .auth()
            .signOut()
            .then(function () {
                window.location = 'Auth/Login';
            })
            .catch(function (error) {
            });
    }
    
}