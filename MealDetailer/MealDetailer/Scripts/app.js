angular.module('MealDetailer', ['ui.bootstrap']);

angular.module('MealDetailer').controller('SearchController', function($scope, $http, $rootScope) {

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
    
    $scope.addProduct = function(ndbno) {
        $rootScope.$emit('addProduct', ndbno);
    }
});

angular.module('MealDetailer').controller('MainController', function ($scope, $http, $rootScope) {
    $scope.products = [];

    $rootScope.$on('addProduct', onAddProduct, handleError);
    
    $scope.save = function() {
        $http.post('/Home/SaveReport', $scope.report).then(function(response) {
            alert('ok');
        }, handleError);
    }

    onAddProduct({}, '42291'); // add peanut butter to begin with

    function handleError (error) {
        console.log(error);
    }

    function onAddProduct (event, ndbno) {
        $http.get('/Home/GetFoodReport?id=' + ndbno).then(function(response) {
            var product = transformReportResponse(response);
            $scope.products.push(product);
        });
    };

    function transformReportResponse(response) {
        var parsedResponse = response.data;
        if (parsedResponse.IsValid) {
            var parsedProduct = JSON.parse(parsedResponse.Contents);
            var food = parsedProduct.report.food;
            return {
                name: food['@name'],
                ndbno: food['@ndbno'],
                nutrients: food.nutrients.nutrient,
                weight: 0
            };
        } else {
            return null;
        }
    }
});