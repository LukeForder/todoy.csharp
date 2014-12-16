﻿angular.module(
    'todoy',
    ['ngRoute']).
constant('siteUrl', "http://localhost:56749/JsApp").
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
                    templateUrl: siteUrl + '/todo/partials/todoList.html'
                }
            ).
            when(
                '/login',
                {
                    controller: 'authenticationController',
                    controllerAs: 'ctrl',
                    templateUrl: siteUrl + '/authentication/partials/login.html'
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
    'authenticationService',
    function (rootScopeService, locationService, authenticationService) {

        // register listener to watch route changes
        rootScopeService.$on("$routeChangeStart", function (event, next, current) {
            if (authenticationService.getUser() == null) {
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
    'authenticationService',
    [
        '$http',
        '$q',
         todoy.authentication.services.AuthenticationService
    ]).
controller(
    'authenticationController',
    [
        'authenticationService',
        '$location',
        todoy.authentication.controllers.AuthenticationController
    ]).
controller(
    'toDoListController',
    [
        todoy.toDo.controllers.ToDoListController
    ]).
directive(
    'registrationForm',
    [
        'siteUrl',
        todoy.authentication.directives.RegistrationFormFactory
    ]);

