angular.module('MealDetailer', []);

angular.module('MealDetailer').controller('MainController', function($scope, $http) {
    $http.get('/Home/GetFoodReport').then(function (response) {
        var parsedResponse = response.data;
        $scope.report = parsedResponse.Report;
        $scope.errors = parsedResponse.Errors;
        $scope.isValid = parsedResponse.IsValid;
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