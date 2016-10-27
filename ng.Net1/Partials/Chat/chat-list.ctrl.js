
app.controller('ChatCtrl', ['$scope', '$http', '$cookies', '$cookieStore', '$sce', '$q', 'rootUrl',
    function ($scope, $http, $cookies, $cookieStore, $sce, $q, rootUrl) {

        $scope.apiUrl = rootUrl + '/api/CheckToken?' + $scope.token;

        $scope.token = 'Bearer ' + $cookieStore.get('_Token');

        //console.log("$scope.token", $scope.token);

        //$scope.currentUrl = "http://192.168.207.13"
        //正式網址
        //http://api.falcota.com/everanhospital/news/create.php
        $scope.currentUrl = "http://192.168.205.92/news/create.php"

            + "?token=" + $scope.token;


        $scope.setCurrentUrl = function (url) {
            $scope.currentUrl = url
                + "?token=" + $scope.token;
        }

        $scope.getUrl = function () {
            //console.log($scope.currentUrl);
            return $sce.trustAsResourceUrl($scope.currentUrl);
        }

        $scope.apiUrl = rootUrl + '/api/CheckToken/:id';

        $scope.CheckToken = function () {
            $http.defaults.headers.common.Authorization = 'Bearer ' + $cookieStore.get('_Token');
            //$http.defaults.headers.common.RefreshToken = $cookieStore.get('_RefreshToken');
            //$http.defaults.headers.common.Authorization = 'Bearer ' + "8mTub1mwkTR50_38bXSjQzsykO8JJBZgB-vUB11CbM4tINzsPqgGfj37ewRygBLH0BBmqKSQWA4bQ1_8-Dfkt1hI69Y7kivL5zGGMTjJ3ervOj8bH0bYk6yg_K0qN0nDnTx0aqiVU-Zz9sg-VHHs-k6c4dH-4D2O_2NwMsunORCm454eyTZBLfWKfbJIkf6vr7jCV-XJ9p1AeaVa_zFT46E7_pszhwo6yoQLYGulr-Or9aEiz51_Qa0QGR7UJRVnSoiceGVm2DyJRgPCVC1jD-S1h3LGLHHo2KULniM7xV4DM3Yf6k124f0nr8UTOFTgdBQiKQfO4sRKG6Oqiar8fGGxBbzHtCpxyJ1HS71IM0tbj6Hq0DSj7bAbBvtmw8n87mtBDjtH3jWBvd_J58PLUatu8EIYpsz1TGGns6yaQvCNL4pGYHUl5Pg4xmDQcNSO"
            var d = $q.defer();
            $http({
                url: rootUrl + '/api/CheckToken',
                method: "post",
                data: {
                    //token: $scope.token
                }
            }).success(function (res) {
                res = res || {};
                //console.log(JSON.stringify(res))
                /*
                if (res.data.length > 0) { // {status: "ok"} 
                    d.reject('名稱重覆');
                } else { // {status: "error", msg: "Username should be `awesome`!"} 
                    d.resolve(res.msg)
                }
                */
            }).error(function (e) {
                d.reject('Server error!');
            });
            return d.promise;

        }

    }]);