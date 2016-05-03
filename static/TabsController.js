TabsController = ['$scope', function ($scope) {
    $scope.tabs = [{
            title: 'Шаг 1. Параметры объекта',
            url: 'params.html'
        }, {
            title: 'Шаг 2. Адаптация',
            url: 'adapt.html'
        }, {
            title: 'Шаг 3. Прогноз',
            url: 'prognosis.html'
    }];


    $scope.currentTab = $scope.tabs[0];

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab;
    }
    
    $scope.isActiveTab = function(tabUrl) {
        return tabUrl == $scope.currentTab;
    }
}];
