app.controller('AdminUserListCtrl', function ($scope, $resource, DBSrv, $location, rootUrl) {

    // callback for ng-click 'createUser':
    $scope.createNewUser = function () {
        $location.path('/adminuser-creation');
    };

    // callback for ng-click 'editUser':
    $scope.editUser = function (userId) {
        $location.path('/adminuser-detail/' + userId);
    };

    // callback for ng-click 'deleteUser':
    $scope.deleteUser = function (userId) {
        DBSrv.Resource(rootUrl + '/api/AdminUser/:id')
            .delete({ id: userId })
            .$promise.then(function () {
                $scope.QueryData();
            });
    };

    $scope.apiUrl = rootUrl + '/api/AdminUser';
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
