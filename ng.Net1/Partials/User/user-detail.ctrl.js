app.controller('UserDetailCtrl', function ($scope, $routeParams, UserFactory, GroupsFactory, $location, rootUrl, DBSrv, $resource, $cookieStore, ngToast, $http) {

    $scope.updateUser = function () {
        UserFactory.update($scope.user).$promise
            .then(function () {
                $location.path('/user-list');
            });
    };

    $scope.GetFrontGroups = function () {
        GroupsFactory.get({ type: 1 })
            .then(function (resp) {
                $scope.groups = resp.data;
            });
    }

    $scope.GetFrontGroups();

    $scope.apiUrl = rootUrl + '/api/ChatGroups';
    $scope.GetChatGroups = function () {
        $scope.queryParam = {};
        DBSrv.Get($scope).$promise.then(function (resp) {
            //console.log(JSON.stringify(resp));
            $scope.chatgroups = resp.data;
        });
    }
    $scope.GetChatGroups();


    // callback for ng-click 'cancel':
    $scope.cancel = function () {
        $location.path('/user-list');
    };

    $scope.checkAll = function () {
        $scope.user.groups = $scope.groups.map(function (item) { return item.id; });
    };
    $scope.uncheckAll = function () {
        $scope.user.groups = [];
    };
    $scope.checkFirst = function () {
        $scope.user.groups.splice(0, $scope.user.groups.length);
        $scope.user.groups.push(1);
    };

    UserFactory.show({ id: $routeParams.id })
    .$promise.then(function (resp) {
        $scope.user = resp.data;
    });

    $scope.resetPassword = function () {
        var User = $resource(rootUrl + '/api/ResetUserPassword/:id', {}, {
            update: {
                method: 'PUT', params: { id: '@id' }
            }
        });

        User.update({
            id: $routeParams.id,
            password: $scope.user.resetPassword,
            token: 'Bearer ' + $cookieStore.get('_Token'),
            url: 'http://api.falcota.com/everanhospital/v1/user/password/reset'
        })
            .$promise.then(function (resp) {
                var resp = angular.fromJson(resp.data);
                if (resp.error == "0")
                    //ngToast.pop('success', "更新密碼成功", "更新成功");
                    ngToast.success("更新成功")
                else
                    //ngToast.pop('error', "更新密碼失敗", resp.error_message);
                    ngToast.danger("更新失敗:" + resp.error_message);
            });
    };

});
