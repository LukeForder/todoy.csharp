// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.interceptors = todoy.authentication.interceptors || {};

(function addAuthorizationHeaderInterceptorToNamespace(ns) {

    function AuthorizationHeaderInterceptor(identityService) {

        // HACK
        this.request = function (config) {

            var userToken = identityService.authorizationToken;

            config.headers.Authorization = "Token " + userToken;

            return config;
        };
    }

    ns.AuthorizationHeaderInterceptor = AuthorizationHeaderInterceptor;

    
})(todoy.authentication.interceptors);