angular.module('MealDetailer', []);

angular.module('MealDetailer').controller('MainController', function($scope, $http) {
    $http.get('/Home/GetFoodReport').then(function(response) {
        $scope.report = response.data;
    }, function (error) {
        alert(Error);
        console.log(error);
    });

    $scope.save = function() {
        $http.post('/Home/SaveReport', $scope.report).then(function(response) {
            alert('ok');
        }, function(error) {
            alert(Error);
            console.log(error);
        });
    }
});