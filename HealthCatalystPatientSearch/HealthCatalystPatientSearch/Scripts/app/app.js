var App = angular.module('App', ['ngRoute', 'ui.bootstrap', 'angularSpinner']);

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

App.config(['$compileProvider', function($compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|file|ftp|blob):|data:image\//);
}]);

App.directive("myFiles", function ($parse) {
    return function linkFn(scope, elem, attrs) {
        elem.on("change", function (e) {
            scope.$eval(attrs.myFiles + "=$files", { $files: e.target.files });
            scope.$apply();
        });
    };
});

App.controller('MainController', ['$scope', function ($scope) { }]);
App.controller('AboutController', ['$scope', function ($scope) { }]);
App.controller('ContactController', ['$scope', function ($scope) { }]);

App.controller('AddController',
    ['$scope', '$uibModalInstance', '$http','searchInfo', function ($scope, $uibModalInstance, $http, searchInfo) {
        var self = this;

        self.scope = $scope;
        self.datePickerOpened = false;
        self.altInputFormats = ['MMMM dd, yyyy', 'MM/dd/yyyy', 'MMM/dd/yyyy', 'MMM dd, yyyy'];
        self.format = self.altInputFormats[0];
        self.dateOptions = {
            maxDate: new Date(),
            formatYear: 'yyyy'
        };

        //person attributes
        self.firstName = "";
        self.lastName = "";
        self.interests = "";
        self.dateOfBirth = null;
        self.imageBase64 = null;

        //address attributes
        self.streetLine1 = "";
        self.streetLine2 = "";
        self.city = "";
        self.state = "";
        self.postalCode = "";
        self.country = "";

        self.addPerson = function () {

            //Create reader to convert uploaded image to base64
            var reader = new FileReader();

            //once image is base64 encoded, execute post to store person
            reader.onload = function () {

                var base64 = this.result;
                var base64Cleaned = base64.substring(base64.indexOf(",")+1);//get everything after the metadata

                self.imageBase64 = base64Cleaned;

                var person = {
                    firstName: self.firstName,
                    lastName: self.lastName,
                    interests: self.interests,
                    dateOfBirth: self.dateOfBirth,
                    image: self.imageBase64,
                    address: {
                        streetLine1: self.streetLine1,
                        streetLine2: self.streetLine2,
                        city: self.city,
                        state: self.state,
                        postalCode: self.postalCode,
                        country: self.country
                    }
                };

                console.log("Adding person: " + person.firstName + " " + person.lastName);

                //Make actual call to C# controller

                var post = $http({
                    method: "POST",
                    url: "/Home/Add",
                    dataType: 'json',
                    data: {
                        person: person
                    },
                    headers: { "Content-Type": "application/json" }
                }).then( function(data, status) {
                    console.log("Success: " + data.data);
                    searchInfo.function(searchInfo.term, false); //not slow search
                }, function (data, status) {
                        console.log("Failure: " + data.data);
                    

                    },
                    function(data, status) {
                        console.log("Failure: " + data.data);
                    });
            }

            reader.readAsDataURL(self.scope.files[0]);

            $uibModalInstance.close();
        }

        self.close = function () {
            $uibModalInstance.close();
        }

        self.openDatePicker = function() {
            console.log("Opened Date Picker");
            self.datePickerOpened = true;
        }

        self.fileChosen = function() {
            return typeof self.scope.files != 'undefined';
        }

        self.convertImageToByteArray = function(file) {
            console.log(file);
            var reader = new FileReader();

            reader.onload = function() {
                var arrayBuffer = this.result;
                array = new Uint8Array(arrayBuffer);
                binaryString = String.fromCharCode.apply(null, array);

                console.log(binaryString);

                self.imageAsByteArray = binaryString;
            }

            reader.readAsArrayBuffer(file);
        }
    }]);

App.controller('SearchController', ['$scope', '$uibModal','$http', '$interval', function ($scope, $uibModal, $http, $interval) {

    var self = this;

    self.persons = [];

    self.alertShouldShow = false;
    self.firstSearch = true;
    self.loading = false;
    self.loadingMessage = "Nothing to see here";
    self.loadingPromise = null;
    self.slowSearch = false;
    self.searchTerm = "";
    self.viewby = 5;
    self.totalItems = self.persons.length;
    self.currentPage = 1;
    self.itemsPerPage = self.viewby;
    self.maxSize = 5;
    self.pageSizeOptions = [3, 5, 10];

    self.performSearch = function (searchTerm, slowSearch) {
        console.log("Performing search for: " + searchTerm);
        self.loading = true;

        self.beginAppendLoadingDots();

        var post = $http({
            method: "GET",
            url: slowSearch ? "/Home/SlowGetPersons" : "/Home/GetPersons",
            params: { searchString : self.searchTerm }
        }).then(function (data, status) {
            console.log("Success: " + data.data);
            self.persons = data.data;
            self.totalItems = data.data.length;
            self.loading = false;

            $interval.cancel(self.loadingPromise);//stop appending '.' to message

            //reset message
            self.loadingMessage = "LOADING SEARCH RESULTS  . . .";

        }, function (data, status) {
            console.log("Failure: " + data.data);
            self.loading = false;
        });
    }

    //Pagination methods
    self.setPage = function (pageNo) {
        self.currentPage = pageNo;
    };

    self.pageChanged = function () {
        console.log('Page changed to: ' + self.currentPage);
    };

    self.setItemsPerPage = function (num) {
        self.viewby = num;
        self.itemsPerPage = num;
        self.currentPage = 1; //set to first page on itemsPerPage update
    }

    self.addPerson = function() {
        console.log("in addPerson");
        var modalInstance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: '/AngularViews/add.html',
            controller: 'AddController as add',
            size: "",
            resolve: {
                searchInfo: {
                    term: self.searchTerm,
                    function: self.performSearch
                }
            }
        });

        modalInstance.result.then(function(success) {
                self.addSuccess = success;
                self.alertShouldShow = true;

                $interval(function() {
                    self.alertShouldShow = false;
                }, 5000, 1); //close alert after 5 seconds, if not already closed
            },
            function() {
                console.log('Modal dismissed at ' + new Date());
            });
    }

    self.getBase64 = function(buffer) {
        console.log("In getBase64");
        var base64 = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            base64 += String.fromCharCode(bytes[i]);
        }

        return btoa(base64);
    }

    self.closeAlert = function() {
        console.log("closing alert");
        self.alertShouldShow = false;
    }

    self.beginAppendLoadingDots = function()
    {
        self.loadingPromise = $interval(function () {
            self.loadingMessage += " .";
        }, 600);
    }

    //init search on load
    self.init = function () {
        self.loading = true;
        self.loadingMessage = "SEEDING DATABASE  . . .";
        self.performSearch();
    }

    self.init();
    
}]);