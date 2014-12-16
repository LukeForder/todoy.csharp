// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.models = todoy.authentication.models || {};

(function addRegistrationDetailsToNamespace(ns) {

    function RegistrationDetails() {
        this.userName = null;
        this.password = null;
        this.passwordConfirmation = null;
    }

    ns.RegistrationDetails = RegistrationDetails;

})(todoy.authentication.models);