// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.directives = todoy.authentication.directives || {};

(function addRegistrationFormDirectiveToNamespace(ns) {

    function RegistrationForm(siteUrl) {
        this.restrict = 'A';
        this.templateUrl = siteUrl + '/JsApp/authentication/partials/registration.html';
        this.scope = {
            register: '&onRegister',
            password: '=',
            passwordConfirmation: '=',
            emailAddress: '='
        };
    };

    function RegistrationFormFactory(siteUrl) {
        return new ns.RegistrationForm(siteUrl);
    }

    ns.RegistrationForm = RegistrationForm;
    ns.RegistrationFormFactory = RegistrationFormFactory;


})(todoy.authentication.directives);