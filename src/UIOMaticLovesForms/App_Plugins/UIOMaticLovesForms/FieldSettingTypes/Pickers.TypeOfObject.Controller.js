angular.module("umbraco").controller("UIOMaticLovesForms.SettingTypes.Pickers.TypeOfObjectController",
	function ($scope, $http) {
	    $http.get(Umbraco.Sys.ServerVariables.uioMatic.pecBaseUrl + "GetAllTypes").then(function (response) {
	        $scope.types = response.data;
	    });

	});