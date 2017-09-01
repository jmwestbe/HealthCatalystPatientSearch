App.controller('SearchController', ['$scope', '$uibModal', '$http', '$interval', function ($scope, $uibModal, $http, $interval) {

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
            params: { searchString: self.searchTerm }
        }).then(function (data, status) {
            console.log("Search returned successfully...");
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
    };

    self.setItemsPerPage = function (num) {
        self.viewby = num;
        self.itemsPerPage = num;
        self.currentPage = 1; //set to first page on itemsPerPage update
    }

    self.addPerson = function () {
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

        modalInstance.result.then(function (success) {
            self.alertShouldShow = success;

            if (success) {
                $interval(function () {
                    self.alertShouldShow = false;
                },
                    5000,
                    1); //close alert after 5 seconds, if not already closed
            }
        },
            function () {
                console.log('Modal dismissed at ' + new Date());
            });
    }

    self.getBase64 = function (buffer) {
        var base64 = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            base64 += String.fromCharCode(bytes[i]);
        }

        return btoa(base64);
    }

    self.closeAlert = function () {
        console.log("closing alert");
        self.alertShouldShow = false;
    }

    self.beginAppendLoadingDots = function () {
        self.loadingPromise = $interval(function () {
            self.loadingMessage += " .";
        }, 600);
    }

    //init search on load
    self.init = function () {
        if (self.persons.length == 0) {
            console.log(self.persons.length);
            self.loading = true;
            self.loadingMessage = "SEEDING DATABASE  . . .";
            self.performSearch();
        }
    }

    self.init();

}]);