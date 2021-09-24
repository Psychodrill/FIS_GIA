<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>

<style type="text/css">
    
<%------------ tree control images -----------%>
	.Tree .ExpandOpen .Expand 
	{ 
		background-image:	url('<%= Url.Images("minus.gif") %>'); 
	} 
	.Tree .ExpandClosed .Expand 
	{ 
		background-image: url('<%= Url.Images("plus.gif") %>');
		background-position: left bottom !important;
	}
	.Tree .ExpandLeaf .Expand 
	{ 
		background-image:	url('<%= Url.Images("leaf.gif") %>');
	} 
	.Tree .ExpandLeaf .Expand 
	{ 
		background-image:	url('<%= Url.Images("leaf.gif") %>');
	} 
<%------------ end of tree control images -----------%>

<%------------ ui-lightness images -----------%>
	.ui-icon { width: 16px; height: 16px; background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_222222_256x240.png") %>'); }
	.ui-widget-content .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_222222_256x240.png") %>'); }
	.ui-widget-header .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_ffffff_256x240.png") %>'); }
	.ui-state-default .ui-icon { background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_ef8c08_256x240.png") %>'); }
	.ui-state-hover .ui-icon, .ui-state-focus .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_ef8c08_256x240.png") %>'); }
	.ui-state-active .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_ef8c08_256x240.png") %>'); }
	.ui-state-highlight .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_228ef1_256x240.png") %>'); }
	.ui-state-error .ui-icon, .ui-state-error-text .ui-icon {background-image: url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-icons_ffd27a_256x240.png") %>'); }
	
	.ui-widget-overlay { background: #666666 url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_diagonals-thick_20_666666_40x40.png") %>') 50% 50% repeat; opacity: .50;filter:Alpha(Opacity=50); }
	.ui-widget-shadow { margin: -5px 0 0 -5px; padding: 5px; background: #000000 url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_flat_10_000000_40x100.png") %>') 50% 50% repeat-x; opacity: .20;filter:Alpha(Opacity=20); -moz-border-radius: 5px; -webkit-border-radius: 5px; border-radius: 5px; }
	
	.ui-widget-content { border: 1px solid #dddddd; background: #eeeeee url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_highlight-soft_100_eeeeee_1x100.png") %>') 50% top repeat-x; color: #333333; }
	.ui-widget-header { border: 1px solid #e78f08; background: #f6a828 url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_gloss-wave_35_f6a828_500x100.png") %>') 50% 50% repeat-x; color: #ffffff; font-weight: bold; }
	
	.ui-state-hover, .ui-widget-content .ui-state-hover, .ui-widget-header .ui-state-hover, .ui-state-focus, .ui-widget-content .ui-state-focus, .ui-widget-header .ui-state-focus { border: 1px solid #fbcb09; background: #fdf5ce url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_glass_100_fdf5ce_1x400.png") %>') 50% 50% repeat-x; font-weight: bold; color: #c77405; }
	.ui-state-active, .ui-widget-content .ui-state-active, .ui-widget-header .ui-state-active { border: 1px solid #fbd850; background: #ffffff url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_glass_65_ffffff_1x400.png") %>') 50% 50% repeat-x; font-weight: bold; color: #eb8f00; }
	
	.ui-state-highlight, .ui-widget-content .ui-state-highlight, .ui-widget-header .ui-state-highlight  {border: 1px solid #fed22f; background: #ffe45c url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_highlight-soft_75_ffe45c_1x100.png") %>') 50% top repeat-x; color: #363636; }
	.ui-state-error, .ui-widget-content .ui-state-error, .ui-widget-header .ui-state-error {border: 1px solid #cd0a0a; background: #b81900 url('<%= Url.Resource("Resources/Themes/ui-lightness/images/ui-bg_diagonals-thick_18_b81900_40x40.png") %>') 50% 50% repeat; color: #ffffff; }


<%------------ end of ui-lightness images -----------%>	
	a.busy
	{
		background-image: url('<%= Url.Images("busy.gif") %>');
	}	
	
	a.delete
	{
		background-image: url('<%= Url.Images("delete_16.gif") %>');
	}
	
	a.add
	{
		background-image: url(<%= Url.Images("plus.jpg") %>);
		background-position: left top;
		background-repeat: no-repeat;
	}
	
	a.btnDelete, a.btnDeleteUS, a.btnDeleteS, a.btnDeleteU
	{
		background-image: url('<%= Url.Images("delete_16.gif") %>');
		background-position: left top;
		background-repeat: no-repeat;
		width: 16px; display: inline-block; height: 16px;
	}

	a.btnSave, a.btnSaveS
	{
		background-image: url('<%= Url.Images("save_16.gif") %>');
		background-position: left top;
		background-repeat: no-repeat;
		width: 16px; display: inline-block; height: 16px;
	}
	
	a.btnEdit, a.btnEditS
	{
		background-image: url('<%= Url.Images("edit_16.gif") %>');
		background-position: left top;
		background-repeat: no-repeat;
		width: 16px; display: inline-block; height: 16px;
	}

	a.btnUp
{
	background-image: url('<%= Url.Images("up_16.gif") %>');
	background-position: left top;
	background-repeat: no-repeat;
	width: 16px; display: inline-block; height: 16px;
	text-decoration:none;
}

a.btnDown
{
	background-image: url('<%= Url.Images("down_16.gif") %>');
	background-position: left top;
	background-repeat: no-repeat;
	width: 16px; display: inline-block; height: 16px;
	text-decoration:none;
}

span.btnDeleteGray
{
	background-image: url('<%= Url.Images("delete_16_gray.gif") %>');
	background-position: left top;
	background-repeat: no-repeat;
	width: 16px; display: inline-block; height: 16px;
	text-decoration:none;
}

span.btnEditGray
{
	background-image: url('<%= Url.Images("edit_16_gray.gif") %>');
	background-position: left top;
	background-repeat: no-repeat;
	width: 16px; display: inline-block; height: 16px;
	text-decoration:none;
}

span.btnDownGray
{
	background-image: url('<%= Url.Images("down_16_gray.gif") %>');
	background-position: left top;
	background-repeat: no-repeat;
	width: 16px; display: inline-block; height: 16px;
	text-decoration:none;
}

</style>
