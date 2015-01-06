var todoy = todoy || {};
todoy.common = todoy.common || {};
todoy.common.interceptors = todoy.common.interceptors || {};

(function registerHttpRequestProgressInterceptor(ns) {

    function HttpRequestProgressInterceptor(qService, progressBar) {

        this.request = function (config) {
            progressBar.start();
            return config;
        };

        this.requestError = function (rejection) {
            progressBar.done();
            qService.reject(rejection);
        };

        this.response = function (response) {
            progressBar.done();
            return response;
        };

        this.responseError = function (rejection) {
            progressBar.done();
            qService.reject(rejection);
        };
    }

    ns.HttpRequestProgressInterceptor = HttpRequestProgressInterceptor;

})(todoy.common.interceptors);