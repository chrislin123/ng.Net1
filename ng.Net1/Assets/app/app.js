var app = angular.module('app', [
    'ngRoute',
    'ngCookies',
    'ngResource',
    'ngAnimate',
    'xeditable',
    'ui.bootstrap',
    'checklist-model',
    'ngMessages',
    'signIn',
    'xeditable',
    'flash',
    'ngSanitize',
    'ngToast'
]);

app.config(['$provide', '$routeProvider', '$httpProvider', function ($provide, $routeProvider, $httpProvider) {
    
    //================================================
    // Ignore Template Request errors if a page that was requested was not found or unauthorized.  The GET operation could still show up in the browser debugger, but it shouldn't show a $compile:tpload error.
    //================================================
    $provide.decorator('$templateRequest', ['$delegate', function ($delegate) {
        var mySilentProvider = function (tpl, ignoreRequestError) {
            return $delegate(tpl, true);
        }
        return mySilentProvider;
    }]);

    //================================================
    // Add an interceptor for AJAX errors
    //================================================
    $httpProvider.interceptors.push(['$q', '$location', function ($q, $location) {
        return {            
            'responseError': function (response) {
                if (response.status === 401)
                {
                    $location.url('/signin');
                    console.log('401')
                }
                return $q.reject(response);
            }
        };
    }]);

        
    //================================================
    // Routes
    //================================================
    $routeProvider
        .when('/', {
            templateUrl: 'partials/about/about.html'
        })
        /*
        .when('/signin/:message?', {
            templateUrl: 'App/SignIn',
            controller: 'signInCtrl'
        })*/
        .when('/signin', {
            templateUrl: 'partials/SignIn/SignIn.html',
            controller: 'signInCtrl'
        })
        .when('/user-list', {
            templateUrl: 'partials/User/user-list.html',
            controller: 'UserListCtrl'
        })
        .when('/user-detail/:id', {
            templateUrl: 'partials/User/user-detail.html',
            controller: 'UserDetailCtrl'
        })
        .when('/adminuser-list', {
            templateUrl: 'partials/AdminUser/user-list.html',
            controller: 'AdminUserListCtrl'
        })
        .when('/adminuser-creation', {
            templateUrl: 'partials/AdminUser/user-creation.html',
            controller: 'AdminUserCreationCtrl'
        })
        .when('/adminuser-detail/:id', {
            templateUrl: 'partials/AdminUser/user-detail.html',
            controller: 'AdminUserDetailCtrl'
        })
        .when('/adminuser-password', {
            templateUrl: 'partials/AdminUser/user-password.html'
        })
        .when('/frontend_log', {
            templateUrl: 'partials/Log/frontend_log.html',
            controller: 'FrontendLogCtrl'
        })
        .when('/hospital_log', {
            templateUrl: 'partials/Log/hospital_log.html',
            controller: 'HospitalLogCtrl'
        })
        .when('/groups', {
            templateUrl: 'partials/groups/groups.html',
            controller: 'groupsCtrl'
        })
        .when('/chatgroups', {
            templateUrl: 'partials/groups/chatgroups.html',
            controller: 'chatGroupsCtrl',
            resolve: {
                access: ["appAuth", function (appAuth) { return appAuth.checkPrivilege("/chatgroups"); }]
            }
        })
        .when('/news-list', {
            templateUrl: 'partials/news/news-list.html',
            controller: 'NewsCtrl'//,
            //resolve: {
            //   access: ["appAuth", function (appAuth) { return appAuth.checkPrivilege("/news"); }]
            //}
        })
        .when('/chat-list', {
            templateUrl: 'partials/chat/chat-list.html',
            controller: 'ChatCtrl'//,
            //resolve: {
            //   access: ["appAuth", function (appAuth) { return appAuth.checkPrivilege("/news"); }]
            //}
        })
        .when('/about', {
            templateUrl: 'partials/about/about.html'
        })
        .when('/release', {
            templateUrl: 'partials/release/release.html'
        })
        .when('/dashboard', {
            templateUrl: 'partials/dashboard/dashboard.html',
            controller: 'DashboardCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });

}])
.config(['ngToastProvider', function (ngToast) {
    ngToast.configure({
        //verticalPosition: 'bottom',
        //horizontalPosition: 'right',
        dismissButton: true,
        animation: 'fade'
    });
}]);

app.run(['$http', '$cookies', '$cookieStore', function ($http, $cookies, $cookieStore) {
    //If a token exists in the cookie, load it after the app is loaded, so that the application can maintain the authenticated state.
    $http.defaults.headers.common.Authorization = 'Bearer ' + $cookieStore.get('_Token');
    $http.defaults.headers.common.RefreshToken = $cookieStore.get('_RefreshToken');
}]);

//GLOBAL FUNCTIONS - pretty much a root/global controller.
//Get username on each page
//Get updated token on page change.
//Logout available on each page.
app.run(['$rootScope', '$http', '$cookies', '$cookieStore', 'rootUrl','$location', function ($rootScope, $http, $cookies, $cookieStore,rootUrl,$location) {

    $rootScope.logout = function () {   

        $rootScope.username = '';
        $rootScope.loggedIn = false;

        $http.post(rootUrl + '/api/Account/Logout')
            .success(function (data, status, headers, config) {
                $http.defaults.headers.common.Authorization = null;
                $http.defaults.headers.common.RefreshToken = null;
                $cookieStore.remove('_Token');
                $cookieStore.remove('_RefreshToken');
                $location.url('/signin');
            });

    }

    $rootScope.$on('$locationChangeSuccess', function (event) {
        console.log('$locationChangeSuccess')

        if ($http.defaults.headers.common.RefreshToken != null) {
            var params = "grant_type=refresh_token&refresh_token=" + $http.defaults.headers.common.RefreshToken;
            $http({
                url: 'Token',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: params
            })
            .success(function (data, status, headers, config) {
                $http.defaults.headers.common.Authorization = "Bearer " + data.access_token;
                $http.defaults.headers.common.RefreshToken = data.refresh_token;

                $cookieStore.put('_Token', data.access_token);
                $cookieStore.put('_RefreshToken', data.refresh_token);

                $http.get(rootUrl + '/api/ud/WS_Account/GetCurrentUserName')
                    .success(function (data, status, headers, config) {
                        if (data != "null") {
                            $rootScope.username = data.replace(/["']{1}/gi, "");//Remove any quotes from the username before pushing it out.
                            $rootScope.loggedIn = true;
                        }
                        else
                        {
                            $rootScope.loggedIn = false;
                            //window.location = '#/signin';
                            $location.url('/signin');
                        }
                    });

                $http.get(rootUrl + '/api/ud/WS_Account/GetCurrentUserId')
                    .success(function (data, status, headers, config) {
                        if (data != "null") {
                            console.log(rootUrl + "/api/ud/WS_Account/GetCurrentUserName resp.data:", data);
                            $rootScope.userid = data.replace(/["']{1}/gi, "");//Remove any quotes from the username before pushing it out.
                        }
                    });


            })
            .error(function (data, status, headers, config) {
                $rootScope.loggedIn = false;
                //window.location = '#/signin';
                $location.url('/signin');
            });
        }

    });
}]);

app.run(function (editableOptions) {
    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
});

var compareTo = function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
};

app.directive("compareTo", compareTo);

app.factory("appAuth", function ($rootScope, $http, $q, $location, $route, appService) {

    return {
        errorCode: {
            checkSessionLoginError: 100,
            privilegeError: 200
        },
        checkLogin: function () {
            //先wait
            var deferred = $q.defer();

            this.checkSessionLogin(deferred);

            //return defered
            return deferred.promise;
        },
        checkPrivilege: function (functionName) {
            //先wait
            var deferred = $q.defer();

            this.checkUserPrivilege(deferred, functionName);

            //return defered
            return deferred.promise;
        },
        checkSessionLogin: function (deferred) {
            var self = this;
            appService.checkSessionLogin().success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject(self.errorCode.checkSessionLoginError);
            });
        },
        checkUserPrivilege: function (deferred, functionName) {
            var self = this;
            appService.checkPrivilege({ functionName: functionName }).success(function (data) {
                deferred.resolve(data);
            }).error(function () {
                deferred.reject(self.errorCode.checkSessionLoginError);
            });
        },
    };

});


app.factory("appService", function ($rootScope, $cookieStore, $http, $location, rootUrl) {
    var appService = {};
    var services = {
        checkAppVersion: 'data/checkAppVersion.json',
        checkSessionLogin: 'data/checkSessionLogin.json',
        login: 'data/login.json',
        complexData: 'data/complexData.json',
        checkPrivilege: rootUrl + '/api/checkPrivilege'
    }

    //通用ajax
    appService.ajax = function (url, params, opt) {
        //var thisAjaxLoadingKey = null;
        var settings = $.extend({
            isShowLoading: true,
            isShowMsg: false,
            method: 'POST'
        }, opt)

        //Show loading
        //if (settings.isShowLoading) {
        //    thisAjaxLoadingKey = common.showLoading();
        //}
        return $http({
            method: settings.method,
            url: url,
            data: params,
            headers: {
                'Content-Type': "application/json;charset=UTF-8"
            }
            //XHR Success
        }).success(function (data, status, headers, config) {

            //Service success
            if (data.status == true) {
                if (settings.isShowMsg) {
                    common.showToastrSuccess({
                        title: data.title,
                        msg: data.msg
                    })
                }
                //Service error
            } else {
                //Session timeout error
                if (data.sessionTimeout) {
                    $location.path('/login');
                    //Normal error
                } else {
                    console.error("Service error : " + url);
                    //common.showToastrByMsgType({
                    //    title: data.title,
                    //    msg: data.msg,
                    //    type: data.type
                    //})
                }
            }
            //Hide loading
            //if (settings.isShowLoading) {
                //common.hideLoading(thisAjaxLoadingKey);
            //}
            //XHR Error
        }).error(function (data, status, headers, config) {
            //common.showToastrError({
            //    title: "Error",
            //    msg: "Angularjs ajax error"
            //});
        });
    }

    appService.checkSessionLogin = function () {
        return this.ajax(services.checkSessionLogin);
    }
    appService.checkVersion = function () {
        return this.ajax(services.checkAppVersion);
    }
    appService.getComplexData = function () {
        return this.ajax(services.complexData);
    }
    appService.login = function (params) {
        return this.ajax(services.login, params);
    }
    appService.checkPrivilege = function (params) {
        return this.ajax(services.checkPrivilege, params);
    }


    return appService;
});

app.config(["$provide", function ($provide) {
          var rootUrl = $("#linkRoot").attr("href");

          $provide.constant('rootUrl', rootUrl);
      }])
