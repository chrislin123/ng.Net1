app.controller('FrontendLogCtrl', function ($scope, $resource, DBSrv, $location, rootUrl) {

    $scope.apiUrl = rootUrl + '/api/FrontendLog';
    $scope.currentPage = 1;
    $scope.recordsPerPage = 15;
    $scope.maxSize = 10;
    $scope.NumberOfPageButtons = 8;
    $scope.keyWord = "";

    $scope.QueryData = function () {
        $scope.queryParam = { keyWord: $scope.keyWord };
        DBSrv.Get($scope).$promise.then(function (resp) {
            $scope.data = resp.data;
            $scope.totalItems = resp.data.count;
        });
    }

    $scope.btnSearch = function () {
        $scope.QueryData();
    };

    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
        $scope.QueryData();
    };

    $scope.datetimeRange = {
        name: "近7日",
        id: -7
    };

    $scope.datetimeRangeChanged = function () {
        $scope.QueryData();
    };

    $scope.datetimeRangeItem =
    [
        { id: 0, name: "今日" },
        { id: -3, name: "近3日" },
        { id: -7, name: "近7日" },
        { id: -30, name: "近30日" },
        { id: -90, name: "近90日" },
        { id: -180, name: "近180日" },
        { id: 1, name: "不限時間" }
    ];

    $scope.QueryData();

});
