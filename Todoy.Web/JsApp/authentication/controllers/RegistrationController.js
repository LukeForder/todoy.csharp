// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.controllers = todoy.authentication.controllers || {};

(function addRegistrationControllerToNamespace(ns) {

    function RegistrationController(
        authenticationService) {
        console.log('RegistrationController.ctor');

        this.authenticationService = authenticationService;
        this.password = null;
        this.passwordConfirmation = null;
        this.userName = null;
    }

    RegistrationController.prototype.register = function register() {
        var self = this;

        console.log('RegistrationController.register');

        function onRegistered() {
            // TODO: notify user to validate email address
        }

        function onFailedToRegister(reasons) {
            self.messages = reasons;
        }

        self.authenticationService.registerAsync(
            {
                userName: self.userName,
                password: self.password,
                passwordConfirmation: self.passwordConfirmation
            }).
        then(onRegistered, onFailedToRegister);
            
    };

    RegistrationController.prototype.canRegister = function canRegister() {
        console.log('RegistrationController.canRegister');
        return !!(this.password &&
            this.passwordConfirmation &&
            this.userName &&
            this.password === this.passwordConfirmation);
    };

    ns.RegistrationController = RegistrationController;

})(todoy.authentication.controllers);