 $(function() {
 	var ckHeight =window.innerHeight;
	$('.contentLeftBox').css("height",ckHeight-51+"px");
	$('.contentMiddle_rightbox').css("max-height",ckHeight-148+"px");
	$(window).resize(function() {
		var bhHeight =window.innerHeight;
		$('.contentLeftBox').css("height",bhHeight-51+"px");
		$('.contentMiddle_rightbox').css("max-height",bhHeight-148+"px");
	});
	$(".select p").each(function(index){
		$(this).click(function(){
            if($(this).parent().hasClass('open')){
                $('.select').eq(index).removeClass('open');
            }else{
                $('.select').removeClass('open');
                $('.select').eq(index).addClass('open');
            }
		})
	});
	$(".select>ul>li>a").click(function(){
        $('.select>ul>li>a').removeClass('selected');
        $(this).addClass('selected');
	})
		 
});