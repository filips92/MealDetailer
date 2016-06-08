angular.module('MealDetailer', ['ui.bootstrap']);

angular.module('MealDetailer').controller('SearchController', function ($scope, $http, $rootScope) {

    $scope.doSearch = function () {
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

    $scope.addProduct = function (ndbno) {
        $rootScope.$emit('addProduct', ndbno);
    }
});

angular.module('MealDetailer').controller('MainController', function ($scope, $http, $rootScope) {
    $scope.products = [];
    $scope.name = 'My dish';
    $rootScope.$on('addProduct', onAddProduct, handleError);

    $scope.recalculateSummary = function () {
        var result = [];
        var nutrientsAmount = $scope.products.length ? $scope.products[0].nutrients.length : 0;
        var totalWeight = 0;

        for (var i = 0; i < $scope.products.length; i++) {
            var singleProduct = $scope.products[i];
            totalWeight += parseFloat(singleProduct.weight);

            for (var j = 0; j < nutrientsAmount; j++) {
                var singleNutrient = singleProduct.nutrients[j];
                if (i === 0) {
                    result.push({
                        name: singleNutrient['@name'],
                        nutrient_id: singleNutrient['@nutrient_id'],
                        unit: singleNutrient['@unit'],
                        group: singleNutrient['@group'],
                        amountInRecipe: 0,
                        amountInUnit: 0
                    });
                }
                var amountInRecipe = parseFloat(singleNutrient['@value']) * parseFloat(singleProduct.weight) / 100;
                result[j].amountInRecipe += amountInRecipe;
                result[j].amountInUnit += amountInRecipe;
            }
        }

        if (totalWeight) {
            for (var i = 0; i < nutrientsAmount; i++) {
                result[i].amountInUnit = result[i].amountInUnit * 100 / totalWeight;
            }
        }
        
        $scope.summary = result;
    }

    $scope.save = function () {
        var data = {
            report: {
                food: {
                    nutrients: {
                        nutrient: []
                    }
                }
            }
        };

        data.report.food['@ndbno'] = 0;
        data.report.food['@name'] = $scope.name;

        $scope.summary.forEach(function(singleNutrient) {
            var newNutrient = {};
            newNutrient['@nutrient_id'] = singleNutrient.nutrient_id;
            newNutrient['@name'] = singleNutrient.name;
            newNutrient['@unit'] = singleNutrient.unit;
            newNutrient['@value'] = singleNutrient.amountInUnit;
            newNutrient['@group'] = singleNutrient.group;

            data.report.food.nutrients.nutrient.push(newNutrient);
        });

        $http.post('/Home/SaveReport', {
            contents: JSON.stringify(data),
            name: $scope.name
        }).then(function (response) {
            if (response.data.IsValid) {
                location.href = response.data.Contents;
            } else {
                $scope.errors = response.data.Errors;
            }
        }, handleError);
    }

    onAddProduct({}, '42291'); // add peanut butter to begin with

    function handleError(error) {
        console.log(error);
    }

    function onAddProduct(event, ndbno) {
        $http.get('/Home/GetFoodReport?id=' + ndbno).then(function (response) {
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