var ProgressiveBonus = angular.module("ProgressiveBonus", []);

ProgressiveBonus.controller(
    "ProgressiveBonusController",
    function ($scope, $window) {
    	$scope.$window = $window;
    	$scope.module = null;
    	$scope.products = [];
    	$scope.roleId = 0; // request roleid
    	$scope.directDownline = false;
    	$scope.error = null;

    	$scope.DisableUpdateButton = function () {
    		var isDisabled = true;
    		if (!$scope.directDownline) {
    			return isDisabled;
    		}
    		var keepGoing = true;
    		angular.forEach($scope.products, function (value, key) {
    			if (keepGoing) {
    				if (value.preVal != value.Enable) {
    					isDisabled = false;
    					keepGoing = false;
    				}
    			}
    		})
    		return isDisabled;
    	}

    	$(document).ready(function () {
    		$scope.load();
    	});

    	$scope.load = function () {
    		var parentPage = $scope.$window.parent._page;
    		$scope.roleId = parentPage.CustRoleId;
    		$scope.directDownline = parentPage.RoleId - 1 == parentPage.CustRoleId;

    		var custId = parentPage.UserId;
    		if (custId && custId > 0) {
    			$.ajax({
    				type: 'POST',
    				url: rootUrl + "CustomerSetting/ProgressiveBonus/CasinoBonus/GetProducts?CustId=" + custId,
    				contentType: "application/json; charset=utf-8",
    				dataType: "json",
    				beforeSend: function () {
    				},
    				success: function (rs) {
    					$scope.$apply(function () {
    						$scope.module = rs;
    						$scope.products = rs.Products;
    					});
    				},
    				error: function (e) { },
    				complete: function (e) {
    				}
    			});
    		}
    	},

		$scope.hideEnableDownlineLink = function () {
			return this.roleId == 1; // Member
		};

    	$scope.getClass = function () {
    		if ($scope.error) {
    			return $scope.error.ErrorCode == 0 ? 'alertboxscc' : 'alertboxerr';
    		}
    	};

    	$scope.parentChange = function (product) {
    		if (product.preVal) {
    			product.EnableDownlines = false;
    			product.EnableDownlinesButton = false;
    		}
    		else {
    			if (product.Enable) {
    				product.EnableDownlines = false;
    				product.EnableDownlinesButton = true;
    			}
    			else {
    				product.EnableDownlines = false;
    				product.EnableDownlinesButton = false;
    			}
    		}
    	};

    	$scope.update = function () {
    		var isDisabled = false;
    		var keepGoing = true;
    		angular.forEach($scope.products, function (value, key) {
    			if (keepGoing) {
    				if (!value.Enable) {
    					isDisabled = true;
    					keepGoing = false;
    				}
    			}
    		})

    		if (isDisabled) { // if Progressive Bonus is disabled => show confirm dialog
    			var confirmMsg = this.roleId == 1 // Member
					? viewResources.memberConfirm
					: viewResources.confirm
    			if (!confirm(confirmMsg)) {
    				return;
    			}
    		}
    		$.ajax({
    			type: "POST",
    			url: "Casino/UpdateProgressive",
    			contentType: "application/json; charset=utf-8",
    			dataType: "json",
    			data: JSON.stringify($scope.module),
    			beforeSend: function () {

    			},
    			success: function (rs) {
    				if (rs && rs.length > 0) {
    					if (rs[0].ErrorCode == 0) {
    						window.setTimeout(function () {
    							try { $scope.cancel(); } catch (e) { }
    						}, 1000);
    					}
    					else {
    						alert(rs[0].ErrorMessage);
    					}
    				}
    			},
    			error: function (e) { },
    			complete: function (e) {

    			}
    		});
    	};

    	$scope.cancel = function () {
    		$scope.$window.parent.closeProgressive();
    		$scope.$window.parent.closeProgressive = null;
    	};
    }
);