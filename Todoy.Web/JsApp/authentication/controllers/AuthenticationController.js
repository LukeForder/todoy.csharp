// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.controllers = todoy.authentication.controllers || {};

(function addAuthenticationControllerToNamespace(ns) {

    function AuthenticationController(authenticationService, locationService) {
        this.authenticationService = authenticationService;
        this.locationService = locationService;
        this.emailAddress = null;
        this.password = null;
        this.flash = null;
    };

    AuthenticationController.prototype.attemptSignIn = function attemptSignIn() {
        var self = this;

        console.log('attempt sign in');

        function onSignedIn() {
            self.locationService.path("/");
        }

        function onFailedToSignIn(reasons) {
            self.flash = reasons;
            self.password = null;
        }

        self.
            authenticationService.
            validateCredentialsAsync(self.emailAddress, self.password).
            then(onSignedIn, onFailedToSignIn);
    };

    ns.AuthenticationController = AuthenticationController;

})(todoy.authentication.controllers);