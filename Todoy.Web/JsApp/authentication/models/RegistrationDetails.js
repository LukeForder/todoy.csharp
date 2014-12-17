// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.models = todoy.authentication.models || {};

(function addRegistrationDetailsToNamespace(ns) {

    function RegistrationDetails(emailAddress, password, passwordConfirmation) {
        this.emailAddress = emailAddress;
        this.password = password;
        this.passwordConfirmation = passwordConfirmation;
    }

    ns.RegistrationDetails = RegistrationDetails;

})(todoy.authentication.models);