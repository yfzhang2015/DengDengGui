        (function (doc, win) {
  			var docEl = doc.documentElement;
  			var resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize';
  			var recalc = function () {
    		var clientWidth = docEl.clientWidth;
    		if (!clientWidth) return;
    			docEl.style.fontSize = (clientWidth / 320 * 10).toFixed(1) + 'px';
  			};
  				recalc();
  			if (!doc.addEventListener) return;
  				win.addEventListener(resizeEvt, recalc, false);
				})(document, window);