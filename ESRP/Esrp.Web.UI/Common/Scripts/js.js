// JavaScript Document
function sel(){
	if ($('.actions input[type=checkbox]').attr('checked')) 
	{
		$('.main_table td input[type=checkbox]').attr('checked', true);
		return false;
	} else {
		$('.main_table td input[type=checkbox]').attr('checked', false);
		return false;
	}
	
}

jQuery(document).ready(function($){
	$('.selected li').click(function(){
		$('.selected li').removeClass('active');
		$(this).addClass('active');
	});
	/*
	$('.closed').click(function(){
		$('.pop').hide();
	});
	
	$('.pop_bg').click(function(){
		$('.pop').hide();
	});
    */
});