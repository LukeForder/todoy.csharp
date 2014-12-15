// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.services = todoy.authentication.services || {};

(function addAuthenticationServiceToNamespace(ns) {

    var currentUser = null;

    function AuthenticationService(httpService, qService) {
        this.httpService = httpService;
        this.qService = qService;
    };

    AuthenticationService.prototype.getUser = function getUser() {
        return currentUser;
    };

    AuthenticationService.prototype.validateCredentialsAsync = function validateCredentialsAsync(userName, password) {
        var self = this;
        function onLoggedIn(authenticationToken) {
            currentUser = {};
            // no mutablity for this property
            Object.defineProperty(
                currentUser,
                'token',
                {
                    value: authenticationToken
                });

            task.resolve();
        }

        function onErrorLoggingIn(reason, status) {
            if (status == 401) {
                task.reject(['Invalid user name and password combination.']);
            }
            else {
                task.reject(reason);
            }
        }

        var task = self.qService.defer();

        var userCredentials = {
            EmailAddress: userName,
            Password: password
        }

        self.httpService.post('api/login', userCredentials).
        success(onLoggedIn).
        error(onErrorLoggingIn);

        return task.promise;
    };

    ns.AuthenticationService = AuthenticationService;

})(todoy.authentication.services);