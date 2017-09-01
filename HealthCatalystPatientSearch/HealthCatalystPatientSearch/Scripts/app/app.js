var App = angular.module('App', ['ngRoute', 'ui.bootstrap', 'angularSpinner']);

//set up routing
App.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {
    $routeProvider.when('/home',
        {
            templateUrl: '/AngularViews/search.html',
            controller: 'SearchController as sc'
        })
        .when('/about',
        {
            templateUrl: '/AngularViews/about.html',
            controller: 'AboutController'
        })
        .when('/contact',
        {
            templateUrl: '/AngularViews/contact.html',
            controller: 'ContactController'
        })
        .otherwise({
            redirectTo: function () {
                return '/home';
            }
        });
}]);

//allow images
App.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|file|ftp|blob):|data:image\//);
}]);

//These controllers currently don't have functionality.
//If they gain functionality in future, move to separate files
App.controller('MainController', ['$scope', function ($scope) { }]); 
App.controller('AboutController', ['$scope', function ($scope) { }]);
App.controller('ContactController', ['$scope', function ($scope) { }]);