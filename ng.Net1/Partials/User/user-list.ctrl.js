app.controller('UserListCtrl', function ($scope, $resource, UserFactory, DBSrv, $location, rootUrl) {
    // callback for ng-click 'editUser':
    $scope.editUser = function (userId) {
        $location.path('/user-detail/' + userId);
    };

    // callback for ng-click 'deleteUser':
    $scope.deleteUser = function (userId) {
        var userFactory = UserFactory.delete({ id: userId })
        userFactory.$promise.then(function () {
            $scope.QueryData();
        });
    };

    // callback for ng-click 'createUser':
    $scope.createNewUser = function () {
        $location.path('/user-detail');
    };

    $scope.apiUrl = rootUrl + '/api/User';
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

    $scope.QueryData();

    $scope.btnSearch = function () {
        $scope.QueryData();
    };

    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
        $scope.QueryData();
    };

});




