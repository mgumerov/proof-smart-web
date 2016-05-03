angular.module('TabsApp', [])
.controller('TabsCtrl', ['$scope', '$http', function ($scope,  $http) {
    $scope.tabs = [{
            title: 'Шаг 1. Параметры объекта',
            url: 'file://./params.html'
        }, {
            title: 'Шаг 2. Адаптация',
            url: 'adaptation.html'
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

    //Пока все в один контроллер валим - все tab-ы (пока есть только одна)

    $scope.model = {}; //Параметры, вводимые юзером
    //Надо, к сожалению, создать заранее вложенные в model объекты, т.к. при binding их автоматически не создадут
    //А искать ссылающиеся html-элементы каким-то образом мне влом, поэтому просто напишу тут, какие именно вложенные надо создать
    $scope.model.oilBalances = {};
    //Первоначальные значения. По-хорошему они придут с сервера, а до того могут быть пустыми, но мы для теста пока что-то напишем
    $scope.model.oilBalances.square = 56738;
    $scope.model.oilBalances.initialSaturatedThickness = 0.0;
    $scope.model.waterCompressibility = 0.000084;

    //See JS-fiddle for this: http://dygraphs.com/gallery/#g/highlighted-region to learn
    //how we can paint over the chart to, say, draw a custom vertical marker - in cases when built-in annotations are not enough
    var g3 = new Dygraph(
      document.getElementById("graphdiv"),
      [[0,0]]
    );

    $scope.updateChart = function () {
        var config = {
            headers : {
                'Content-Type': 'application/json'
            }
        }

        $http.post('/chart', $scope.model, config).then(function (response) {
          g3.updateOptions({file: response.data});
        });
    }
   
    $scope.updateChart();

    //Правда это годится только для тех значений, которые именно в таблице. Если я захочу кастомные редакторы сделать, в них надо отдельно позаботиться о вызове onChange()
    // - также и о том, как значения из них должны попадать в генерируемый там JSON.
    var pvalues = document.getElementsByClassName("pvalue");
    var i;
    for (i = 0; i < pvalues.length; i++) {
        pvalues[i].contentEditable = true;
        pvalues[i].oninput = $scope.updateChart;
    }

}]);
