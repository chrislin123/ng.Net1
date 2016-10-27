app.controller('AdminUserDetailCtrl', function ($scope, $routeParams, AdminUserFactory, DBSrv, GroupsFactory, $location, rootUrl) {

    // callback for ng-click 'updateUser':
    $scope.updateUser = function () {
        AdminUserFactory.update($scope.user)
            .$promise
            .then(function () {
                $location.path('/adminuser-list');
            })
    };

    // callback for ng-click 'cancel':
    $scope.cancel = function () {
        $location.path('/adminuser-list');
    };

    $scope.GetGroups = function () {
        GroupsFactory.get({ type: 2 })
            .then(function (resp) {
                $scope.groups = resp.data;
            });
    }
    $scope.GetGroups();

    $scope.apiUrl = rootUrl + '/api/AdminRoles';
    $scope.GetAdminRoles = function () {
        $scope.queryParam = {};
        DBSrv.Get($scope).$promise.then(function (resp) {
            //$scope.user.role = resp.data[0].name;
            $scope.roles = resp.data;
        });
    }
    $scope.GetAdminRoles();

    $scope.apiUrl = rootUrl + '/api/ChatGroups';
    $scope.GetChatGroups = function () {
        $scope.queryParam = {};
        DBSrv.Get($scope).$promise.then(function (resp) {
            //console.log(JSON.stringify(resp));
            $scope.chatgroups = resp.data;
        });
    }
    $scope.GetChatGroups();

    DBSrv.Resource(rootUrl + '/api/AdminUser/:id')
    .show({ id: $routeParams.id })
    .$promise.then(function (resp) {
        $scope.user = resp.data;
    });

});

