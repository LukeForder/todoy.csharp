// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.controllers = todoy.authentication.controllers || {};

(function addRegistrationControllerToNamespace(ns) {

    function RegistrationController(
        scope,
        authenticationService) {
        
        this.scope = scope;

        this.authenticationService = authenticationService;
        this.password = null;
        this.passwordConfirmation = null;
        this.userName = null;
        this.messages = [];
    }

    RegistrationController.prototype.register = function register() {
        var self = this;

        function onRegistered() {
            self.userName = null;
            self.password = null;
            self.passwordConfirmation = null;

            self.scope.$emit('user-registered');
        }

        function onFailedToRegister(data) {
            self.messages = data.Errors;

            self.scope.$emit(
                'user-registeration-failed',
                self.messages);
        }

        var registrationDetails = new todoy.authentication.models.RegistrationDetails(self.userName, self.password, self.passwordConfirmation);

        console.log(registrationDetails);

        self.authenticationService.registerAsync(registrationDetails).
        then(onRegistered, onFailedToRegister);
            
    };

    ns.RegistrationController = RegistrationController;

})(todoy.authentication.controllers);