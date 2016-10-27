app.controller('AdminUserCreationCtrl', ['$scope', 'AdminUserFactory', 'DBSrv', 'GroupsFactory', '$location', 'rootUrl', 'Flash',
    function ($scope, AdminUserFactory, DBSrv, GroupsFactory, $location, rootUrl, Flash) {

        $scope.user = {};

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
                $scope.user.role = resp.data[0].name;
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

        $scope.createNewUser = function () {
            if ($scope.user.role == undefined) {
                Flash.create('danger', "請選擇角色", 'custom-class');
            }
            else {
                DBSrv.Resource(rootUrl + '/api/AdminUser')
                    .create($scope.user)
                    .$promise.then(function (resp) {
                        $location.path('/adminuser-list');
                    }).catch(function (error) {
                        Flash.create('danger', error.data.errorMessage, 'custom-class');
                    });
            }
        }

        $scope.cancel = function () {
            $location.path('/adminuser-list');
        };

    }]);