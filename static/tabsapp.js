angular.module('TabsApp', [])
.controller('TabsCtrl', ['$scope', '$http', function ($scope,  $http) {
    $scope.tabs = [{
            title: '��� 1. ��������� �������',
            url: 'file://./params.html'
        }, {
            title: '��� 2. ���������',
            url: 'adaptation.html'
        }, {
            title: '��� 3. �������',
            url: 'prognosis.html'
    }];

    $scope.currentTab = $scope.tabs[0];

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab;
    }
    
    $scope.isActiveTab = function(tabUrl) {
        return tabUrl == $scope.currentTab;
    }

    //���� ��� � ���� ���������� ����� - ��� tab-� (���� ���� ������ ����)

    $scope.model = {}; //���������, �������� ������
    //����, � ���������, ������� ������� ��������� � model �������, �.�. ��� binding �� ������������� �� ��������
    //� ������ ����������� html-�������� �����-�� ������� ��� ����, ������� ������ ������ ���, ����� ������ ��������� ���� �������
    $scope.model.oilBalances = {};
    //�������������� ��������. ��-�������� ��� ������ � �������, � �� ���� ����� ���� �������, �� �� ��� ����� ���� ���-�� �������
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

    //������ ��� ������� ������ ��� ��� ��������, ������� ������ � �������. ���� � ������ ��������� ��������� �������, � ��� ���� �������� ������������ � ������ onChange()
    // - ����� � � ���, ��� �������� �� ��� ������ �������� � ������������ ��� JSON.
    var pvalues = document.getElementsByClassName("pvalue");
    var i;
    for (i = 0; i < pvalues.length; i++) {
        pvalues[i].contentEditable = true;
        pvalues[i].oninput = $scope.updateChart;
    }

}]);
