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