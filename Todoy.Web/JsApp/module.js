angular.module(
    'todoy',
    ['ngRoute', 'validation.match', 'ngCookies', 'ngProgressLite']).
constant('siteUrl', "https://localhost/Todoy").
//constant('siteUrl', "http://todoy.azurewebsites.net").
constant('user', null).
config([
    '$routeProvider',
    'siteUrl',
    function (routeProviderService, siteUrl) {
        routeProviderService.
            when(
                '/',
                {
                    controller: 'toDoListController',
                    controllerAs: 'ctrl',
                    templateUrl: siteUrl + '/JsApp/todo/partials/todoList.html'
                }
            ).
            when(
                '/login',
                {
                    controller: 'authenticationController',
                    controllerAs: 'ctrl',
                    templateUrl: siteUrl + '/JsApp/authentication/partials/login.html'
                }).
            otherwise(
                {
                    redirectTo: '/login'
                });
    }
]).
run([
    '$rootScope',
    '$location',
    'identityService',
    function (rootScopeService, locationService, identityService) {

        // register listener to watch route changes
        rootScopeService.$on("$routeChangeStart", function (event, next, current) {
            if (identityService.authorizationToken == null) {
                // no logged user, we should be going to #login
                if (next.templateUrl == "partials/login.html") {
                    // already going to #login, no redirect needed
                } else {
                    // not going to #login, we should redirect now
                    locationService.path("/login");
                }
            }
        });
    }
]).
service(
    'toDoService',
    [
        '$http',
        '$q',
        'siteUrl',
        'authenticationService',
        todoy.toDo.services.ToDoService
    ]).
service(
    'authenticationService',
    [
        '$http',
        '$q',
        'siteUrl',
        'identityService',
         todoy.authentication.services.AuthenticationService
    ]).
service(
    'identityService',
    [
        '$cookieStore',
        todoy.authentication.services.IdentityService
    ]).
controller(
    'authenticationController',
    [
        '$scope',
        'authenticationService',
        '$location',
        todoy.authentication.controllers.AuthenticationController
    ]).
controller(
    'toDoListController',
    [
        'toDoService',
        todoy.toDo.controllers.ToDoListController
    ]).
controller(
    'registrationController',
    [
        '$scope',
        'authenticationService',
        todoy.authentication.controllers.RegistrationController
    ]).
directive(
    'errors',
    [
        'siteUrl',
        todoy.authentication.directives.ErrorsFactory
    ]).
directive(
    'registrationForm',
    [
        'siteUrl',
        todoy.authentication.directives.RegistrationFormFactory
    ]).
filter(
    'readableDate',
    function readableDateProvider() {
        return todoy.toDo.filters.HumanDateFilter;
    }).
factory(
    'authorizationHeaderInterceptor',
    [
        'identityService',
        function (identityService) {
            return new todoy.authentication.interceptors.AuthorizationHeaderInterceptor(identityService);
        }
    ]).
service(
    'httpRequestProgressInterceptor',
    [
        '$q',
        'ngProgressLite',
        todoy.common.interceptors.HttpRequestProgressInterceptor
    ]).
config([
    '$httpProvider',
    function ($httpProvider) {
        $httpProvider.interceptors.push('authorizationHeaderInterceptor');
        $httpProvider.interceptors.push('httpRequestProgressInterceptor');
    }
]);

