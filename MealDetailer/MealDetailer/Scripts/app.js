angular.module('MealDetailer', []);

angular.module('MealDetailer').controller('SearchController', function($scope, $http) {

    $scope.doSearch = function() {
        if ($scope.query && $scope.query.length > 2) {
            $http.get('/Home/QueryDatabase?query=' + $scope.query).then(function (response) {
                var parsedResponse = response.data;
                $scope.searchResults = parsedResponse.Contents === null ? null : JSON.parse(parsedResponse.Contents).list.item;
                $scope.errors = parsedResponse.Errors;
                $scope.isValid = parsedResponse.IsValid;
            }, function (error) {
                console.log(error);
            });
        }
    }
    
});

angular.module('MealDetailer').controller('MainController', function($scope, $http) {
    $http.get('/Home/GetFoodReport').then(function (response) {
        var parsedResponse = response.data;
        $scope.report = parsedResponse.Contents === null ? null : JSON.parse(parsedResponse.Contents).report;
        $scope.errors = parsedResponse.Errors;
        $scope.isValid = parsedResponse.IsValid;
    }, function (error) {
        console.log(error);
    });

    $scope.save = function() {
        $http.post('/Home/SaveReport', $scope.report).then(function(response) {
            alert('ok');
        }, function(error) {
            console.log(error);
        });
    }
});