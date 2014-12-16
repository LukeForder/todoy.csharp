// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.services = todoy.authentication.services || {};

(function addAuthenticationServiceToNamespace(ns) {

    var currentUser = null;

    function AuthenticationService(httpService, qService, siteUrl) {
        this.httpService = httpService;
        this.qService = qService;
        this.siteUrl = siteUrl;
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

        self.httpService.post(self.siteUrl + '/api/login', userCredentials).
        success(onLoggedIn).
        error(onErrorLoggingIn);

        return task.promise;
    };

    AuthenticationService.prototype.registerAsync = function registerAsync(registrationDetails) {
        var self = this;

        function onRegistered() {
            task.resolve();
        }

        function onErrorRegistering(reasons, status) {
            task.reject(reasons);
        }

        var task = self.qService.defer();

        var dto = {
            EmailAddress: registrationDetails.emailAddress,
            Password: registrationDetails.password,
            PasswordConfirmation: registrationDetails.PasswordConfirmation
        };

        self.httpService.post(self.siteUrl + '/api/register', dto).
        success(onRegistered).
        error(onErrorRegistering);

        return task.promise;
    };

    ns.AuthenticationService = AuthenticationService;

})(todoy.authentication.services);