app.controller('falcotaAdminCtrl', function ($scope, $rootScope,DBSrv,$location, rootUrl) {
	/* Variable */

	//appService.checkVersion().success(function (response) {
	//	appUtility.checkAppVersion(response.data.version);
	//})
    $scope.date = new Date();
    
    /*
    var m = [
    { name: "關於長安醫院", icon: "fa fa-qrcode", path: "#/about", isVisible: true },
    ];

    var m_login = [
    { name: "儀表版", icon:"fa fa-dashboard", path:"#/dashboard", isVisible: true },
    { name: "院方人員帳號管理", icon: "fa fa-file", path: "#/adminuser-list", isVisible: true },
    { name: "APP使用者帳號管理", icon: "fa fa-file", path: "#/user-list", isVisible: true },
    { name: "最新消息維護", icon: "fa fa-file", path: "#/news-list", isVisible: true },
    { name: "APP傳輸記錄查詢", icon: "fa fa-table", path: "#/frontend_log", isVisible: true },
    { name: "院方傳輸記錄查詢", icon: "fa fa-table", path: "#/hospital_log", isVisible: true },
    { name: "群組資料維護", icon: "fa fa-sitemap", path: "#/groups", isVisible: true },
    { name: "社群群組資料維護", icon: "fa fa-sitemap", path: "#/chatgroups", isVisible: true },
    { name: "版本修定記錄", icon: "fa fa-edit", path: "#/release", isVisible: true },
    { name: "關於長安醫院", icon: "fa fa-qrcode", path: "#/about", isVisible: true },
    ];
    */

    $scope.activedMenu = null;
    $scope.onMenuClick = function (menu) {
        //console.log($scope.activedMenu);
        $scope.activedMenu = menu;
    }
   

    function ChangeMenu(loggedIn) {
        $scope.GetMenu(loggedIn);
    }

    $scope.GetMenu = function (loggedIn) {
        //$scope.queryParam = { keyWord: $scope.keyWord };
        DBSrv.Resource(rootUrl + "/api/Menu").get({loggedIn:loggedIn}).$promise
            .then(function (resp) {
                $scope.menus = resp.data;
        });
    }


    //$scope.GetMenu();

    $scope.$watch('loggedIn',function(newValue, oldValue){
        ChangeMenu(newValue);
    });

    $scope.UserProfile = function () {
        $location.path('/adminuser-detail/' + $rootScope.userid);
    }


});

