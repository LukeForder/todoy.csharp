// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.services = todoy.authentication.services || {};

(function addAuthenticationServiceToNamespace(ns) {

    function AuthenticationService(httpService, qService, siteUrl, identityService) {
        this.httpService = httpService;
        this.qService = qService;
        this.siteUrl = siteUrl;
        this.identityService = identityService;
    };

    
    AuthenticationService.prototype.validateCredentialsAsync = function validateCredentialsAsync(userName, password) {
        var self = this;
        function onLoggedIn(authenticationToken) {
            self.identityService.authorizationToken = authenticationToken;
            task.resolve();
        }

        function onErrorLoggingIn(reason, status) {
            if (status == 401) {
                task.reject(['Invalid user name and password combination.']);
            }
            else {
                console.log(reason.Errors);
                task.reject(reason.Errors);
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
            PasswordConfirmation: registrationDetails.passwordConfirmation
        };

        self.httpService.post(self.siteUrl + '/api/register', dto).
        success(onRegistered).
        error(onErrorRegistering);

        return task.promise;
    };

    ns.AuthenticationService = AuthenticationService;

})(todoy.authentication.services);