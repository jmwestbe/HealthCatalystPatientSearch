App.directive("myFiles", function ($parse) {
    return function linkFn(scope, elem, attrs) {
        elem.on("change", function (e) {
            scope.$eval(attrs.myFiles + "=$files", { $files: e.target.files });
            scope.$apply();
        });
    };
});