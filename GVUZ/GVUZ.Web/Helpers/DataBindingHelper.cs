using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.Helpers
{
    /// <summary>
    /// Вспомогательнй класс для контролов использующих knockout
    /// </summary>
    public static class DataBindingHelper
    {
        public static IHtmlString BoundedSelectFor<TModel, TProperty>(this HtmlHelper<TModel> html,
                                                                      Expression<Func<TModel, TProperty>> propertyAccessor,
                                                                      Expression<Func<TModel, SelectListViewModelBase>> collectionAccessor,
                                                                      object htmlAttributes = null)
        {
            ModelMetadata propertyMetadata = ModelMetadata.FromLambdaExpression(propertyAccessor, html.ViewData);
            ModelMetadata collectionMetadata = ModelMetadata.FromLambdaExpression(collectionAccessor, html.ViewData);
            SelectListViewModelBase collection = collectionAccessor.Compile().Invoke(html.ViewData.Model);

            return BoundedSelect(html, propertyMetadata.PropertyName, collectionMetadata.PropertyName, collection.ShowUnselectedText ? collection.UnselectedText : null, "Id", "DisplayName", htmlAttributes);
        }

        public static IHtmlString BoundedSelectFor<TModel, TProperty>(this HtmlHelper<TModel> html,
                                                                      Expression<Func<TModel, TProperty>> propertyAccessor,
                                                                      string collectionPropertyName,
                                                                      SelectListViewModelBase collection,
                                                                      object htmlAttributes = null)
        {
            ModelMetadata propertyMetadata = ModelMetadata.FromLambdaExpression(propertyAccessor, html.ViewData);
            
            return BoundedSelect(html, propertyMetadata.PropertyName, collectionPropertyName, collection.ShowUnselectedText ? collection.UnselectedText : null, "Id", "DisplayName", htmlAttributes);
        }

        public static IHtmlString BoundedSelect(this HtmlHelper html, string targetProperty, string collectionName, string unselectedText, string idValueProperty, string displayTextProperty, object htmlAttributes = null)
        {
            Dictionary<string, string> bindings = new Dictionary<string, string>();

            bindings.Add("value", targetProperty);
            bindings.Add("options", string.Format("{0}.Items", collectionName));
            if (!string.IsNullOrEmpty(unselectedText))
            {
                bindings.Add("optionsCaption", string.Format("{0}.UnselectedText", collectionName));    
            }
            if (!string.IsNullOrEmpty(idValueProperty))
            {
                bindings.Add("optionsValue", string.Format("'{0}'", idValueProperty));
            }
            if (!string.IsNullOrEmpty(idValueProperty))
            {
                bindings.Add("optionsText", string.Format("'{0}'", displayTextProperty));
            }

            IDictionary<string, object> attr = htmlAttributes == null ? null : new RouteValueDictionary(htmlAttributes);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter writer = new Html32TextWriter(sw))
                {
                    writer.WriteBeginTag("select");
                    
                    if (attr != null)
                    {
                        foreach (var a in attr)
                        {
                            writer.WriteAttribute(a.Key, a.Value == null ? string.Empty : a.Value.ToString(), true);
                        }
                    }

                    writer.WriteAttribute("data-bind", string.Join(", ", bindings.Select(x => string.Format("{0}: {1}", x.Key, x.Value))));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("select");
                }    
            }
            /*IDictionary<string, object> attr = new RouteValueDictionary(htmlAttributes);*/
            return html.Raw(sb.ToString());
        }

        public static IHtmlString BoundedTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html,
                                                                       Expression<Func<TModel, TProperty>>
                                                                           propertyAccessor, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(propertyAccessor, html.ViewData);
            return BoundedTextBox(html, metadata.PropertyName, htmlAttributes);
        }

        public static IHtmlString BoundedTextBox(this HtmlHelper html, string targetProperty, object htmlAttributes = null)
        {
            Dictionary<string, string> bindings = new Dictionary<string, string>();

            bindings.Add("value", targetProperty);
            
            IDictionary<string, object> attr = htmlAttributes == null ? null : new RouteValueDictionary(htmlAttributes);

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter writer = new Html32TextWriter(sw))
                {
                    writer.WriteBeginTag("input");
                    writer.WriteAttribute("type", "text");

                    if (attr != null)
                    {
                        foreach (var a in attr)
                        {
                            writer.WriteAttribute(a.Key, a.Value == null ? string.Empty : a.Value.ToString(), true);
                        }
                    }

                    writer.WriteAttribute("data-bind", string.Join(", ", bindings.Select(x => string.Format("{0}: {1}", x.Key, x.Value))));
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                }
            }
            /*IDictionary<string, object> attr = new RouteValueDictionary(htmlAttributes);*/
            return html.Raw(sb.ToString());
        }
    }
}