App.controller('AddController',
    ['$scope', '$uibModalInstance', '$http', 'searchInfo', function ($scope, $uibModalInstance, $http, searchInfo) {
        var self = this;

        self.scope = $scope;
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
                var base64Cleaned = base64.substring(base64.indexOf(",") + 1);//get everything after the metadata

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
                }).then(function (data, status) {
                    console.log("Person successfully added: " + data.data);
                    searchInfo.function(searchInfo.term, false); //not slow search
                }, function (data, status) {
                    console.log("Failure: " + data.data);


                },
                    function (data, status) {
                        console.log("Failure: " + data.data);
                    });
            }

            reader.readAsDataURL(self.scope.files[0]);

            $uibModalInstance.close(true);
        }

        self.close = function () {
            $uibModalInstance.close(false);
        }

        self.openDatePicker = function () {
            console.log("Opened Date Picker");
            self.datePickerOpened = true;
        }

        self.fileChosen = function () {
            return typeof self.scope.files != 'undefined';
        }

        self.convertImageToByteArray = function (file) {
            console.log(file);
            var reader = new FileReader();

            reader.onload = function () {
                var arrayBuffer = this.result;
                array = new Uint8Array(arrayBuffer);
                binaryString = String.fromCharCode.apply(null, array);

                console.log(binaryString);

                self.imageAsByteArray = binaryString;
            }

            reader.readAsArrayBuffer(file);
        }
    }]);