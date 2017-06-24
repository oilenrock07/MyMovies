var app = angular.module('MovieApp', ['ngCookies']);
app.controller('HeaderController', function ($scope, $cookies, $window) {
    $scope.setMovieOrder = function (order) {
        $cookies.put('MovieOrder', order);
        $window.location.reload();
    }

    $scope.setMovieSorting = function (sort) {
        $cookies.put('MovieSorting', sort);
        $window.location.reload();
    }
});

app.controller('BannerController', function ($scope) {
    $scope.banner = '';
    $scope.showBannerImage = function(category) {
        $scope.banner = category;
    }
});

app.directive('selectOnClick', ['$window', function ($window) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.on('click', function () {
                if (!$window.getSelection().toString()) {
                    // Required for mobile Safari
                    this.setSelectionRange(0, this.value.length)
                }
            });
        }
    };
}]);