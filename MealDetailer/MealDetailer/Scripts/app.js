angular.module('MealDetailer', []);

angular.module('MealDetailer').controller('MainController', function($scope, $http) {
    $http.get('/Home/GetFoodReport').then(function(response) {
        if (response.data.IsValid) {
            $scope.report = JSON.parse(response.data.Value).report;
        } else {
            $scope.errors = JSON.parse(response.data.Value);
        }
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