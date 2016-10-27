app.controller('chatGroupsCtrl', ['$scope', '$q', 'ChatGroupsFactory','ngToast',
    function ($scope, $q, ChatGroupsFactory, ngToast) {

        //以下公家會用
        //控制
        var factory = ChatGroupsFactory;
        //查
        $scope.query = function () {
            var param = {};
            factory.get(param)
                .then(function (resp) { $scope.data = resp.data; })
                .catch(function (error) {
                    console.error(error);
                    ngToast.danger(error.data.errorMessage);
                });
        };
        //存
        $scope.save = function (data, id) {
            if (id == undefined) {
                //新
                factory.create(data)
                    .then(function (resp) { ngToast.success("新增成功") })
                    .catch(function (error) {
                        console.error(error);
                        ngToast.danger(error.data.errorMessage);
                    })
                    .finally(function () { $scope.query(); });
            }
            else {
                //修
                factory.update(id, data)
                    .then(function (resp) { ngToast.success("更新成功") })
                    .catch(function (error) {
                        console.error(error);
                        ngToast.danger(error.data.errorMessage);
                    })
                    .finally(function () { $scope.query(); });


            }
        };
        //刪
        $scope.delete = function (id) {
            factory.delete(id)
                .then(function (resp) { ngToast.success("刪除成功") })
                .catch(function (error) {
                    console.error(error);
                    ngToast.danger(error.data.errorMessage);
                })
                .finally(function () { $scope.query(); });

        };
        //以上公家會用
        //獨有
        $scope.add = function () {
            $scope.inserted = {
                name: ''
            };
            $scope.data.push($scope.inserted);
        };

        $scope.checkName = function (data, id) {
            var deferred = $q.defer();
            var param = { name: data, id: id }
            factory.get(param)
                .then(function (resp) {
                    if (resp.data.length > 0) {
                        deferred.reject('名稱重覆');
                    } else {
                        deferred.resolve()
                    }
                })
                .catch(function (error) {
                    console.error("error:" + JSON.stringify(error));
                });
            return deferred.promise;
        }

        $scope.query();
    }]);