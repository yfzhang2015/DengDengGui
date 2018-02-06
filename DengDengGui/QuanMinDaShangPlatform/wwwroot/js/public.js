$(document).ready(function(){
	var ckHeight =window.innerHeight;
	$('.contentMiddle').css("height",ckHeight-115+"px");
	$(window).resize(function() {
		var bhHeight =window.innerHeight;
		$('.contentMiddle').css("height",bhHeight-115+"px");
	});
	$("input").focus(function(){
  		this.select();
 	});
});	