// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.directives = todoy.authentication.directives || {};

(function addRegistrationFormDirectiveToNamespace(ns) {


    function RegistrationFormController(scope) {
        this.scope = scope;
    }

    RegistrationFormController.prototype.onRegisterTrampoline = function () {
        this.scope.register();
        this.scope.registrationForm.$setPristine();
        this.scope.registrationForm.$setUntouched();
    }

    function RegistrationForm(siteUrl) {
        var self = this;
        this.restrict = 'A';
        this.templateUrl = siteUrl + '/JsApp/authentication/partials/registration.html';
        this.scope = {
            register: '&onRegister',
            password: '=',
            passwordConfirmation: '=',
            emailAddress: '='
        };
        this.controller = 
            [
                '$scope', 
                RegistrationFormController
            ];
        this.controllerAs = 'regCtrl';
    };

   

    function RegistrationFormFactory(siteUrl) {
        return new ns.RegistrationForm(siteUrl);
    }

    ns.RegistrationForm = RegistrationForm;
    ns.RegistrationFormFactory = RegistrationFormFactory;


})(todoy.authentication.directives);