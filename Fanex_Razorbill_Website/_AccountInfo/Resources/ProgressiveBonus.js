function ShowFrmUpdProgressiveBonus(obj) {

    ajax.Request('../CustomerSetting/ProgressiveBonus/CasinoBonus/Index?custId=0', {
		asynchronous: true,
		method: 'get',
		onComplete: function (data) {
			$('disabledProgressiveBonusStatusPop').innerHTML =
				'<iframe src="../CustomerSetting/ProgressiveBonus/CasinoBonus/Index?custId=0" style="width:180px;height:88px;border:0;box-shadow:none;">' + data.responseText + '</iframe>';

			var curTop = 0;
			if (obj.offsetParent) {
				do {
					curTop += obj.offsetTop;
				} while (obj = obj.offsetParent);
			}
			var popupOffsetHeight = 100; // height of popup + margin
			curTop -= popupOffsetHeight;

			$('disabledProgressiveBonusStatusPop').style.top = curTop + 'px';
			$('disabledProgressiveBonusStatusPop').style.display = 'block'; //show popup
			$('overlayBg').style.display = 'block'; //show overlay
			window.closeProgressive = closeDivProgressiveBonusPopup;
		}
	});
}
function closeDivProgressiveBonusPopup() {
	if ($('disabledProgressiveBonusStatusPop')) $('disabledProgressiveBonusStatusPop').style.display = 'none';
	$('overlayBg').style.display = 'none';
}