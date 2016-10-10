angular.module("umbraco").controller("UIOMaticLovesForms.SettingTypes.Mappers.PocoController",
	function ($scope, $routeParams, pickerResource, uioMaticObjectResource, $http) {

	    if (!$scope.setting.value) {
	       
	    } else {
	        var value = JSON.parse($scope.setting.value);
	        $scope.typeOfObject = value.typeOfObject;
	        $scope.properties = value.properties;
	    }

	     $http.get(Umbraco.Sys.ServerVariables.uioMatic.pecBaseUrl + "GetAllTypes").then(function (response) {
	        $scope.types = response.data;

	    });

	    pickerResource.getAllFields($routeParams.id).then(function (response) {
	        $scope.fields = response.data;
	    });

	    $scope.setTypeOfObject = function () {

	        uioMaticObjectResource.getAllProperties($scope.typeOfObject).then(function (response) {
	            $scope.properties = response.data;
	            console.log($scope.properties);
	        });
	    };

	    $scope.setValue = function() {
	       
	        var val = {};
	        val.typeOfObject = $scope.typeOfObject;
	        val.properties = $scope.properties;

	        $scope.setting.value = JSON.stringify(val);
	    };
	});