// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.directives = todoy.authentication.directives || {};

(function addErrorsDirectiveToNamespace(ns) {

    function Errors(siteUrl) {
        this.restrict = 'A';
        this.templateUrl = siteUrl + '/JsApp/authentication/partials/errorMessages.html';
        this.scope = {
            isValid: '=',
            errorMessages: '=',
            emailMessage: '@?',
            patternMessage: '@?',
            requiredMessage: '@?',
            matchMessage: '@?'
        };
    };

    function ErrorsFactory(siteUrl) {
        return new ns.Errors(siteUrl);
    }

    ns.Errors = Errors;
    ns.ErrorsFactory = ErrorsFactory;


})(todoy.authentication.directives);