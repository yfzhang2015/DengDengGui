$(document).ready(function(){
	
	$('#tit span').click(function() {
        //var i = $(this).index();//下标第一种写法		            
        var i = $('#tit span').index(this);//下标第二种写法
        $(this).addClass('select').siblings().removeClass('select');
        $('#con li').eq(i).show().siblings().hide();
    });


	$(".cha").click(function(){
		$(".popMask").fadeOut();
		window.onscroll=function(){
			document.body.scrollTop=document.body.scrollTop;//关闭后清除保存位置的值
		}
	});

	
	$(".popMask").click(function(){
		$(".popMask").fadeOut();
		window.onscroll=function(){
			document.body.scrollTop=document.body.scrollTop;
		}
	});
	$(".pop").click(function(e){
		e = e || window.event;
		e.stopPropagation?e.stopPropagation():e.cancelBubble=true;
    });

   
});	