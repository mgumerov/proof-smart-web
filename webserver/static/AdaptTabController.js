AdaptTabController = ['$scope', '$http', '$location', function ($scope,  $http, $location) {
    $scope.ReservoirPressureData = []; //Вектор Pпл
    $scope.loadPressuresFromBase = function () {
        $http.get('http://hostapp/F6D2F50B-9D0B-424F-BCE0-2011715E7B5F/getReservoirPressuresFromTM', {withCredentials: false}).then(function (response) {
          //не будем заменять референс на массив, будем прямо сам массив менять. Вдруг захотим на него слушателей вешать.
          $scope.ReservoirPressureData.length = 0;
          Array.prototype.push.apply($scope.ReservoirPressureData, response.data);
        }, function (response) {
          $scope.ReservoirPressureData.push(response);
        });
    }
}];
