app.controller('AdminUserPasswordCtrl', function ($scope, $routeParams, AdminUserFactory, DBSrv, GroupsFactory, $location, rootUrl) {

    $scope.user = {};

    $scope.updateUser = function () {
        AdminUserFactory.update($scope.user)
            .$promise
            .then(function () {
                $location.path('/adminuser-list');
            })
    };

    $scope.GetFrontGroups = function () {
        GroupsFactory.get({ type: 1 })
            .then(function (resp) {
                $scope.groups = resp.data;
            });
    }

    $scope.GetFrontGroups();

    $scope.cancel = function () {
        $location.path('/dashboard');
    };

    DBSrv.Resource(rootUrl + '/api/AdminUser/:id')
    .show({ id: $routeParams.id })
    .$promise.then(function (resp) {
        $scope.user = resp.data;
    });

    var model = this;

    model.message = "";

    model.user = {
        oldPassword: "",
        password: "",
        confirmPassword: ""
    };

    model.submit = function (isValid) {
        if (isValid) {
            model.message = "Submitted " + model.user.username;
        } else {
            model.message = "There are still invalid fields below";
        }
    };



});


