app.factory('UsersFactory', function ($resource,rootUrl) {
    return {
        all: $resource(rootUrl + '/api/User', {}, {
            'get': { method: 'GET' },
            'query': { method: 'GET', isArray: false }
        })/*,
        find: $resource('/lane/api/getWords/:word', { word: '@word' }, {
            'get': { method: 'GET', cache: $cacheFactory },
            'query': { method: 'GET', cache: $cacheFactory, isArray: true }
        })*/
    }
});

app.factory('UserFactory', function ($resource,rootUrl) {
   // console.log('UserFactory')
    return $resource(rootUrl + '/api/User/:id', {}, {
        show: {
            method: 'GET'
        },
        update: {
            method: 'PUT', params: { id: '@id' }
        },
        delete: {
            method: 'DELETE', params: { id: '@id' }
        }
    });
});

app.factory('FrontendLogFactory', function ($resource,rootUrl) {
       return {

        all: $resource(rootUrl + '/api/FrontendLog', {}, {
            'get': { method: 'GET' },
            'query': { method: 'GET', isArray: false }
        })
    }
});

app.factory('GroupsFactory', function (rootUrl, dbService, $q, $http) {

    var factory = {};
    var _url = rootUrl + '/api/groups';

    var _groupItems =
        [
            { id: 1, name: "1.APP人員群組" },
            { id: 2, name: "2.院方人員群組"}
        ];

    var _query = function (param)
    {
        console.log('newnewnew')
        var deferred = $q.defer();
        dbService.resource(_url)
            .query(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    }

    var _get = function (param) {
        var deferred = $q.defer();
        dbService.resource(_url)
            .get(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    var _update = function (id,param) {
        var deferred = $q.defer();
        dbService.resource(_url + '/:id')
            .update({ id: id }, param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    var _create = function (param) {
        var deferred = $q.defer();
        dbService.resource(_url)
            .save(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    var _delete = function (id) {
        var deferred = $q.defer();
        dbService.resource(_url + '/:id')
            .delete({ id: id }).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    factory.groupItems  = _groupItems;
    factory.get         = _get;
    factory.query       = _query;
    factory.update      = _update;
    factory.delete      = _delete;
    factory.create      = _create;

    return factory;
});

app.factory('ChatGroupsFactory', function (rootUrl, dbService, $q, $http) {

    var factory = {};
    var _url = rootUrl + '/api/chatgroups';

    var _query = function (param)
    {
        var deferred = $q.defer();
        dbService.resource(_url)
            .query(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    }

    var _get = function (param) {
        var deferred = $q.defer();
        dbService.resource(_url)
            .get(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    var _update = function (id,param) {
        var deferred = $q.defer();
        dbService.resource(_url + '/:id')
            .update({ id: id }, param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };
    var _create = function (param) {
        var deferred = $q.defer();
        dbService.resource(_url)
            .save(param).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    var _delete = function (id) {
        var deferred = $q.defer();
        dbService.resource(_url + '/:id')
            .delete({ id: id }).$promise
            .then(function (resp) { deferred.resolve(resp); })
            .catch(function (error) { deferred.reject(error); });
        return deferred.promise;
    };

    factory.get     = _get;
    factory.query   = _query;
    factory.update  = _update;
    factory.delete  = _delete;
    factory.create  = _create;
    
    return factory;

});

app.service('DBSrv', ['$resource', function ($resource) {
    return {
        Get: function ($scope) {
            var params = {
                apiUrl: $scope.apiUrl,
                currentPage: $scope.currentPage,
                recordsPerPage: $scope.recordsPerPage
            }
            //console.log("2.1" + $scope.keyWord1);
            //console.log("3.params:" + JSON.stringify(params));
            //console.log("4.$scope.queryParam:" + JSON.stringify($scope.queryParam));
            params = $.extend(params, $scope.queryParam);
            //console.log("5.params:" + JSON.stringify(params));
            
            if (!angular.isUndefined($scope.datetimeRange)) {
                params.datetimeRange = $scope.datetimeRange.id;
            }

            return $resource(
                params.apiUrl,
                params,
                { method: 'get', isArray: false }
                ).get();
        },
        Resource: function (apiUrl) {
            return $resource(
                apiUrl, {}, {
                show: { method: 'GET' },
                update: { method: 'PUT', params: { id: '@id' } },
                delete: { method: 'DELETE', params: { id: '@id' } },
                create: { method: 'POST' }
            })
        }
    }
}]);

app.service('dbService', ['$resource', function ($resource) {
    
        this.resource = function (url) {

            console.log('dbService url', url);

            return $resource(
                url,
                {id: '@id'},
                {
                    update: { method: 'PUT'
                }
                });
        }
    
}]);

app.factory('AdminUserFactory', function ($resource,rootUrl) {
    return $resource(rootUrl + '/api/AdminUser/:id', {}, {
        show: { method: 'GET' },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    })
});