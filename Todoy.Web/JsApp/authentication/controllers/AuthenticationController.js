// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.controllers = todoy.authentication.controllers || {};

(function addAuthenticationControllerToNamespace(ns) {

    function AuthenticationController(scope, authenticationService, locationService) {
        this.authenticationService = authenticationService;
        this.locationService = locationService;
        this.emailAddress = null;
        this.password = null;
        this.flash = null;
        this.flashStyle = 'error';

        this._scope = scope;
        this._scope.$on('user-registered', this.onUserRegistered.bind(this));
        this._scope.$on('user-registeration-failed', this.onUserRegistrationFailed.bind(this))

        console.log(this._scope);
    };

    AuthenticationController.prototype.onUserRegistrationFailed = function onUserRegistrationFailed(event, errors) {
        this.flash = errors;
        this.flashStyle = 'error';
    };

    AuthenticationController.prototype.onUserRegistered = function onUserRegistered() {
        var self = this;

        this.flashStyle = 'success';

        this.flash = ["You have sucessfully registered, an email has been sent to your account with instructions on how to login."];
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