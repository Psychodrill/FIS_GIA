using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper.MVC;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.Helpers {
	/// <summary>
	/// Хелперные методы для генерации HTML
	/// </summary>
	public static class HtmlHelperExtensions {
		public static bool InMenuSections(this HtmlHelper htmlHelper,params string[] sections) {
			if(htmlHelper.ViewContext.Controller==null||sections==null||sections.Length==0)
				return false;

			var attributes=htmlHelper.ViewContext.Controller.GetType()
				.GetAttributes<MenuSectionAttribute>(false,false,false);
			if(attributes==null||attributes.Length==0)
				return false;
			var attribute=attributes[0];

			foreach(string section in sections) {
				if(attribute.Section.Equals(section,StringComparison.CurrentCultureIgnoreCase))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Возвращает ссылку на меню и может сделать ее активной (в зависимости от параметров).</summary>
		/// <seealso cref="MenuSectionAttribute"/>
		public static MvcHtmlString MenuLink<TController>(
			this HtmlHelper htmlHelper,string linkText,string actionName,
			string activeSection=null,string activeClass="active",string elementID=null,
			bool hideForReadonly=false,string showForRoles=null,string extraClasses="")
			where TController:Controller {
			if(!String.IsNullOrEmpty(showForRoles)) {
				if(!UserRole.CurrentUserInRole(showForRoles))
					return new MvcHtmlString("");
			}

			if(hideForReadonly&&UserRole.IsCurrentUserReadonly()) return new MvcHtmlString("");

			string name=GetControllerName<TController>();
			bool isCurrent=(htmlHelper.ViewContext.Controller!=null
									&&htmlHelper.ViewContext.Controller.GetType()==typeof(TController));
			if(!isCurrent&&!string.IsNullOrEmpty(activeSection))
				isCurrent=htmlHelper.InMenuSections(activeSection);

			object htmlAttributes=new {
				@class=String.Format("{0} {1} {2}","menu ",
					isCurrent?activeClass:"",extraClasses).TrimEnd(),
				@id=elementID
			};

			return htmlHelper.ActionLink(linkText,actionName,name,null,htmlAttributes);
		}

		private static string GetControllerName<TController>() {
			string name=typeof(TController).Name;
			if(name.EndsWith("Controller"))
				name=name.Substring(0,name.Length-"Controller".Length);
			return name;
		}

		public static MvcHtmlString TextBoxExFor<TModel,TProperty>(
			this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel,TProperty>> expression) {
			return htmlHelper.TextBoxExFor(expression,null);
		}

		public static MvcHtmlString TextBoxExFor<TModel,TProperty>(
			this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel,TProperty>> expression,object htmlAttributes) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(expression,htmlHelper.ViewData);
			StringLengthAttribute slAttr=(StringLengthAttribute)Attribute
																						.GetCustomAttribute(
																							metadata.ContainerType.GetProperty(
																								metadata.PropertyName),
																							typeof(StringLengthAttribute));
			RequiredAttribute reqAttr=(RequiredAttribute)Attribute
																				.GetCustomAttribute(
																					metadata.ContainerType.GetProperty(
																						metadata.PropertyName),typeof(RequiredAttribute));
			RangeAttribute rangeAttr=(RangeAttribute)Attribute.GetCustomAttribute(
																			metadata.ContainerType.GetProperty(metadata.PropertyName),
																			typeof(RangeAttribute));
			IDictionary<string,object> attr=new RouteValueDictionary(htmlAttributes);
			if(slAttr!=null)
				attr.Add("MaxLength",slAttr.MaximumLength);
			if(rangeAttr!=null&&rangeAttr.OperandType==typeof(int))
				attr.Add("MaxLength",((int)Math.Log10((int)rangeAttr.Maximum))+1);
			if(reqAttr!=null)
				attr.Add("val-required",
							HttpUtility.HtmlAttributeEncode(reqAttr.FormatErrorMessage(metadata.DisplayName)));
			if(attr.ContainsKey("disabled")&&!attr["disabled"].To(false))
				attr.Remove("disabled");
			return htmlHelper.TextBoxFor(expression,attr);
		}

		#region File
		public static string FileForm<TModel>(this HtmlHelper<TModel> htmlHelper,string prefix) {
			return FileForm(htmlHelper,prefix,null);
		}

		public static string FileForm<TModel>(this HtmlHelper<TModel> htmlHelper,string prefix,object htmlAttributes) {
			RouteValueDictionary routeValueDictionary=new RouteValueDictionary(htmlAttributes);
			StringBuilder builder=new StringBuilder();
			foreach(KeyValuePair<string,object> pair in routeValueDictionary) {
				builder.Append(pair.Key).Append("=\"").Append(pair.Value).Append("\" ");
			}
			return String.Format(
					@"<form id=""file{0}Form"" name=""file{0}Form"" action="""" enctype=""multipart/form-data"">
						<input type=""file"" name=""post{0}File"" id=""post{0}File"" {1} />
					</form>",
					prefix,builder);
		}

		public static MvcHtmlString GenerateFileLink(this HtmlHelper html,string href,string fileName) {
			return GenerateFileLink(html,href,fileName,null);
		}
        
        public static string GetExtensionFromFileName(this HtmlHelper html, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            string ext = string.IsNullOrWhiteSpace(fileName) ? null : Path.GetExtension(fileName);

            if (ext == null)
            {
                return string.Empty;
            }

            return ext.ToLowerInvariant() == "pdf" ? "pdf" : "doc";
        }

		public static MvcHtmlString GenerateFileLink(this HtmlHelper html,string href,string fileName,string deleteButtonName) {
			//<div class="pdf"><a href="#">ВолгГТУ.pdf</a>			
			if(String.IsNullOrEmpty(fileName)) return MvcHtmlString.Empty;

			string ext=Path.GetExtension(fileName);
			ext=String.IsNullOrEmpty(ext)?"":ext.ToLower();
			string className;
			switch(ext) {
				case "doc":
				className="doc";
				break;
				case "pdf":
				className="pdf";
				break;
				default:
				className="doc";
				break;
			}

			TagBuilder divTag=new TagBuilder("div");
			divTag.Attributes.Add("class",className);
            divTag.Attributes.Add("style", "height: auto");
			TagBuilder aTag=new TagBuilder("a");
			aTag.Attributes.Add("href",href);
			aTag.SetInnerText(fileName);
			divTag.InnerHtml=aTag.ToString(TagRenderMode.Normal);
			if(!String.IsNullOrEmpty(deleteButtonName)) {
				divTag.InnerHtml+="<button class=\"fileDelete\" title=\"Удалить файл\" id=\"{0}\"></button>"
					.FormatWith(deleteButtonName);
			}

			return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));
		}

		#endregion
		public static string PortletCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
																		Expression<Func<TModel,bool>> expression) {
			string name=ExpressionHelper.GetExpressionText(expression);
			object eval=htmlHelper.ViewData.Eval(name);

			return htmlHelper.Hidden(name,"false")
					 +
					 String.Format(@"<input type=""checkbox"" name=""{0}"" id=""{0}"" value=""true""{1}/>",name,
										((bool)eval?" checked=\"checked\"":""));
		}

		public static string PortletTriStateCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper,
																				  Expression<Func<TModel,string>> expression,
																				  string onClick) {
			string name=ExpressionHelper.GetExpressionText(expression);
			if(!"Value".Equals(name.Substring(name.Length-5,5)))
				throw new Exception(
					"TriStateCheckBox name should be ended by \"Value\".");
			string checkboxName=name.Substring(0,name.Length-5);

			return htmlHelper.HiddenFor(expression)
					 +
					 String.Format(
						@"<input type=""checkbox"" name=""{0}"" id=""{0}"" value=""true"" onClick=""{1}""/>",
						checkboxName,onClick);
		}

		public static string Serialize(this HtmlHelper htmlHelper,object obj) {
			StringBuilder sb=new StringBuilder();
            //new JavaScriptSerializer().Serialize(obj,sb);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            serializer.Serialize(obj, sb);

            sb.Replace("\\","\\\\");
			return sb.ToString();
		}

		public static MvcHtmlString DatePickerFor<TModel,TProperty>(
			this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel,TProperty>> expression) {
			return DatePickerFor(htmlHelper,expression,null);
		}

		public static MvcHtmlString DatePickerFor<TModel,TProperty>(
			this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel,TProperty>> expression,object htmlAttributes) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(expression,htmlHelper.ViewData);
			RequiredAttribute reqAttr=(RequiredAttribute)Attribute
													.GetCustomAttribute(
														metadata.ContainerType.GetProperty(
															metadata.PropertyName),typeof(RequiredAttribute));

			IDictionary<string,object> attr=new RouteValueDictionary(htmlAttributes);
			attr["class"]="shortInput datePicker";
			attr["maxlength"]=10;
			if(reqAttr!=null)
				attr.Add("val-required",HttpUtility.HtmlAttributeEncode(reqAttr.FormatErrorMessage(metadata.DisplayName)));

			string name=ExpressionHelper.GetExpressionText(expression);
			object eval=htmlHelper.ViewData.Eval(name);
			if(eval!=null) {
				DateTime dateTime=((DateTime)eval);
				//всё работает с локальными датами, так что если пришло в UTC, то надо в локальную, иначе потеряем день
				eval=dateTime==DateTime.MinValue?"":dateTime.ToLocalTime().ToString("dd.MM.yyyy",CultureInfo.CreateSpecificCulture("ru-RU"));
			}

			if(attr.ContainsKey("disabled")&&!attr["disabled"].To(false))
				attr.Remove("disabled");
			return htmlHelper.TextBox(name,eval,attr);
		}

		public static MvcHtmlString DropDownListExFor<TModel,TProperty>(this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel,TProperty>> expression,
				IEnumerable listDataIDName,object htmlAttributes) {
			IDictionary<string,object> attr=new RouteValueDictionary(htmlAttributes);
			if(attr.ContainsKey("disabled")&&!attr["disabled"].To(false))
				attr.Remove("disabled");
			return htmlHelper.DropDownListFor(expression,new SelectList(listDataIDName,"ID","Name"),attr);
		}

		#region Labels

		/// <summary>
		/// Проставляет звёздочки к обязательным документам, если данное поле Required
		/// </summary>
		public static MvcHtmlString TableLabelReqFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression, bool showColon=true) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(expression,html.ViewData);
			RequiredAttribute reqAttr=(RequiredAttribute)Attribute.GetCustomAttribute(	metadata.ContainerType.GetProperty(	metadata.PropertyName),typeof(RequiredAttribute));

			return TableLabelFor(html,expression,null, required:reqAttr!=null, showColon :showColon);
		}

		public static MvcHtmlString TableLabelFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression,bool required=false,bool showColon=true) {
			return TableLabelFor(html,expression,null, required :required,showColon :showColon);
		}

		public static MvcHtmlString TableLabelFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression,object htmlAttributes,bool required=false,
			bool showColon=true) {
			return TableLabelFor(html,expression,new RouteValueDictionary(htmlAttributes),required :required,
				showColon :showColon);
		}

		public static MvcHtmlString TableLabelFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression,IDictionary<string,object> htmlAttributes,
			bool required=false,bool showColon=true) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(expression,html.ViewData);
			string htmlFieldName=ExpressionHelper.GetExpressionText(expression);
			string labelText=metadata.DisplayName??metadata.PropertyName??htmlFieldName.Split('.').Last();
			if(String.IsNullOrEmpty(labelText)) {
				return MvcHtmlString.Empty;
			}

			TagBuilder tag=new TagBuilder("label");
			tag.MergeAttributes(htmlAttributes);
			tag.Attributes.Add("for",html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

			string resultString=labelText;
			if(required)
				resultString+=" <span class=\"required\">(*)</span>";
			if(showColon) resultString+=":";

			tag.InnerHtml=resultString;
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString StrongLabelFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression,bool required=false) {
			return TableLabelFor(html,expression,new { @class="big" },required :required);
		}

		/*/// <summary>
		/// Label for table view/edit pages, adds ':' after text
		/// In future (*) for required fiels can be added
		/// </summary>
		public static string TableLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression, bool required = false, bool showColon = true)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(htmlHelper.LabelFor(expression).ToHtmlString());
			if (required)
				sb.Append(" <span class=\"required\">(*)</span>");*/
		/*
		 * GVUZ-394 - displayed on view pages need more complex solution http://qa.fogsoft.ru/browse/GVUZ-394#action_22990
		 * else
		{
			// find required attribute for property
			MemberExpression body = expression.Body as MemberExpression;
			PropertyInfo pi;
			if (body != null)
			{
				pi = typeof (TModel).GetProperty(body.Member.Name);
				if (pi != null)
				{
					if (pi.GetCustomAttributes(typeof (RequiredAttribute), true).Length > 0)
						sb.Append(" <span class=\"required\">(*)</span>");
				}
			}
		}*/
		/*			if (showColon) sb.Append(": ");
					return sb.ToString();
				}*/

		public static MvcHtmlString LabelTextFor<TModel,TValue>(this HtmlHelper<TModel> html,
			Expression<Func<TModel,TValue>> expression) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(expression,html.ViewData);
			string htmlFieldName=ExpressionHelper.GetExpressionText(expression);
			string labelText=metadata.DisplayName??metadata.PropertyName??htmlFieldName.Split('.').Last();
			if(String.IsNullOrEmpty(labelText)) {
				return MvcHtmlString.Empty;
			}

			return MvcHtmlString.Create(labelText);
		}

        #endregion

        public static MvcHtmlString CommonDateReadOnly(this HtmlHelper html,
            DateTime? value, object htmlAttributes)
        {
            return CommonInputReadOnly(html, value.HasValue ? value.Value.ToString("dd.MM.yyyy") : string.Empty, htmlAttributes);
        }

        public static MvcHtmlString CommonDateReadOnly(this HtmlHelper html,
            DateTime? value)
        {
            return CommonDateReadOnly(html, value, null);
        }

        public static MvcHtmlString CommonInputReadOnly(this HtmlHelper html,
			string value) {
            return CommonInputReadOnly(html, value, null);
		}

        public static MvcHtmlString CommonInputReadOnly(this HtmlHelper html, string value, object htmlAttributes)
        {
            IDictionary<string, object> htmlAttributesDict = htmlAttributes != null ? new RouteValueDictionary(htmlAttributes) : new RouteValueDictionary();
            if (htmlAttributesDict.ContainsKey("class"))
            {
                string classAttr = htmlAttributesDict["class"] as string ?? string.Empty;
                classAttr += " inputboxAd view";
                htmlAttributesDict["class"] = classAttr.Trim();
            }
            else
            {
                htmlAttributesDict.Add("class", "inputboxAd view");
            }
            
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttributes(htmlAttributesDict);
            tag.Attributes.Add("value", value);
            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("readonly", "readonly");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

		public static MvcHtmlString CommonTextAreaReadOnly(this HtmlHelper html,
			string value) {
			IDictionary<string,object> htmlAttributesDict=new RouteValueDictionary(
				new { @class="inputboxAd view" });
			TagBuilder tag=new TagBuilder("textarea");
			tag.MergeAttributes(htmlAttributesDict);
			tag.Attributes.Add("readonly","readonly");
			tag.SetInnerText(value);

			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static IHtmlString ValueOrMdash(this HtmlHelper html,string value) {
			if(string.IsNullOrEmpty(value)) {
				return new HtmlString("&mdash;");
			}

			return new HtmlString(html.Encode(value));
		}

		public static IHtmlString ValueOrSpace(this HtmlHelper html,string value) {
			if(string.IsNullOrEmpty(value)) {
				return new HtmlString("&nbsp;");
			}

			return new HtmlString(html.Encode(value));
		}

		public static IHtmlString ValueOrMdash<T>(this HtmlHelper html,T? value) where T:struct {
			return new HtmlString(value.HasValue?html.Encode(value.ToString()):"&mdash;");
		}

		public static IHtmlString ValueOrSpace<T>(this HtmlHelper html,T? value) where T:struct {
			return new HtmlString(value.HasValue?html.Encode(value.ToString()):"&nbsp;");
		}

		public static IHtmlString DisplayNameFor<TModel,TProperty>(this HtmlHelper<TModel> html,
																						Expression<Func<TModel,TProperty>> propertyAccessor) {
			ModelMetadata metadata=ModelMetadata.FromLambdaExpression(propertyAccessor,html.ViewData);
			string htmlFieldName=ExpressionHelper.GetExpressionText(propertyAccessor);
			string displayName=metadata.DisplayName??metadata.PropertyName??htmlFieldName.Split('.').Last();

			if(string.IsNullOrEmpty(displayName)) {
				return MvcHtmlString.Empty;
			}

			return new MvcHtmlString(displayName);
		}

	    public static IHtmlString CustomJson(this HtmlHelper html, object value)
	    {
	        var isoDateFormat = new IsoDateTimeConverter();
	        isoDateFormat.DateTimeFormat = "dd.MM.yyyy";
	        return html.Raw(JsonConvert.SerializeObject(value, isoDateFormat));
	    }

		 public static bool ShowCompositionSearch(this HtmlHelper html) {
			 const string key="ShowCompositionSearch";

			 bool show;
			 return bool.TryParse(ConfigurationManager.AppSettings[key],out show)&&show;
		 }

         public static IHtmlString ValidatorTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html,
                                                              Expression<Func<TModel, TProperty>> propertyAccessor, 
                                                              object customValue = null,
                                                              object htmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(propertyAccessor, html.ViewData);
            object currentValue;

             if (!html.ViewData.TryGetValue(modelMetadata.PropertyName, out currentValue))
             {
                 currentValue = null;
             }

             return ValidatorTextBox(html, modelMetadata.PropertyName, customValue ?? currentValue, htmlAttributes);

        }

	    public static IHtmlString ValidatorTextBox(this HtmlHelper helper, string propertyName, object propertyValue, object htmlAttributes = null)
	    {
	        IDictionary<string, object> attr = htmlAttributes == null
	                                               ? new RouteValueDictionary()
	                                               : new RouteValueDictionary(htmlAttributes);

            if (!helper.ViewData.ModelState.IsValidField(propertyName))
            {   
                object @class;

                if (!attr.TryGetValue("class", out @class))
                {
                    @class = null;
                }

                StringBuilder sb = new StringBuilder(@class == null ? string.Empty : @class.ToString());
                sb.Append(" input-validation-error");

                attr["class"] = sb.ToString();
                attr["title"] = string.Join("\n", helper.ViewData.ModelState[propertyName].Errors.Select(x => x.ErrorMessage));
            }

	        return helper.TextBox(propertyName, propertyValue, attr);
	    }

        public static IEnumerable<SelectListItem> ToSelectList<TId>(this SelectListViewModel<TId> selectListViewModel)
        {
            var items = new List<SelectListItem>();
            if (selectListViewModel.ShowUnselectedText)
            {
                items.Add(new SelectListItem{Value = string.Empty, Text = selectListViewModel.UnselectedText});
            }
            items.AddRange(selectListViewModel.Items.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.DisplayName}));
            return items;
        }

        public static IHtmlString CustomDropDownFor<TModel, TProperty, TId>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> targetProperty, SelectListViewModel<TId> listModel, object htmlAttributes = null)
        {
            if (listModel == null)
            {
                return MvcHtmlString.Empty;
            }

            IDictionary<string, object> attr = htmlAttributes == null
                                                   ? new RouteValueDictionary()
                                                   : new RouteValueDictionary(htmlAttributes);


            List<SelectListItem> items = new List<SelectListItem>();

            if (listModel.ShowUnselectedText)
            {
                items.Add(new SelectListItem{Text = listModel.UnselectedText, Value = string.Empty });
            }

            var modelMetadata = ModelMetadata.FromLambdaExpression(targetProperty, helper.ViewData);

            object currVal;
            if (!helper.ViewData.TryGetValue(modelMetadata.PropertyName, out currVal))
            {
                currVal = null;
            }

            items.AddRange(listModel.Items.Select(x => new SelectListItem{Value = x.Id.ToString(), Text = x.DisplayName, Selected = Equals(currVal, x.Id)}));

            if (currVal == null && listModel.ShowUnselectedText)
            {
                items[0].Selected = true;
            }

            if (!helper.ViewData.ModelState.IsValidField(modelMetadata.PropertyName))
            {
                object @class;

                if (!attr.TryGetValue("class", out @class))
                {
                    @class = null;
                }

                StringBuilder sb = new StringBuilder(@class == null ? string.Empty : @class.ToString());
                sb.Append(" input-validation-error");

                attr["class"] = sb.ToString();
                attr["title"] = string.Join("\n", helper.ViewData.ModelState[modelMetadata.PropertyName].Errors.Select(x => x.ErrorMessage));
            }

            return helper.DropDownList(modelMetadata.PropertyName, items, attr);
        }

        public static string BreakStringByLength(this HtmlHelper html, string s, int maxStringLen)
        {
            if (string.IsNullOrWhiteSpace(s) || maxStringLen <= 0)
            {
                return s;
            }

            s = s.Trim();
            int order = 1;
            int lim = maxStringLen * order;
            
            while (s.Length > lim)
            {
                s = s.Insert(lim, " ");
                lim = maxStringLen * (++order);
            }

            return s;
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<SimpleDto> dto, string defaultOption)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            if (defaultOption != null)
            {
                items.Add(new SelectListItem { Value = string.Empty, Text = defaultOption });
            }

            if (dto != null)
            {
                items.AddRange(dto.Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Name ?? string.Empty }));
            }

            return items;
        }
	}
}