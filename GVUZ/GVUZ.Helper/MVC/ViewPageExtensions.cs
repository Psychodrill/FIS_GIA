using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GVUZ.Helper.MVC
{
    /// <summary>
    ///     Хелперные методы для работы с View
    /// </summary>
    public static class ViewPageExtensions
    {
        /// <summary>
        ///     Подготовка классов для текстбокса
        /// </summary>
        private static void PrepareClassAttributesForTextBox(string[] classNames,
                                                             out Dictionary<string, object> classAttributes,
                                                             out Dictionary<string, object> classAttributesWithDisable)
        {
            classAttributes = new Dictionary<string, object>();
            if (classNames != null && classNames.Length > 0)
                classAttributes.Add("class", String.Join(" ", classNames));

            classAttributesWithDisable = new Dictionary<string, object>(classAttributes) {{"readonly", "readonly"}};
            object classStyle;
            if (classAttributesWithDisable.TryGetValue("class", out classStyle))
                classAttributesWithDisable["class"] += " readOnlyControl";
            else
                classAttributesWithDisable.Add("class", "readOnlyControl");
        }

        /// <summary>
        ///     Подготовка классов для контролов
        /// </summary>
        private static void PrepareClassAttributes(string[] classNames,
                                                   out Dictionary<string, object> classAttributes,
                                                   out Dictionary<string, object> classAttributesWithDisable)
        {
            classAttributes = new Dictionary<string, object>();
            if (classNames != null && classNames.Length > 0)
                classAttributes.Add("class", String.Join(" ", classNames));

            classAttributesWithDisable = new Dictionary<string, object>(classAttributes) {{"disabled", "disabled"}};
            object classStyle;
            if (classAttributesWithDisable.TryGetValue("class", out classStyle))
                classAttributesWithDisable["class"] += " readOnlyControl";
            else
                classAttributesWithDisable.Add("class", "readOnlyControl");
        }

        /// <summary>
        ///     Враппер для валидационных сообщений
        /// </summary>
        private static string ValidationWrapper<TModel, T>(this ViewPage<TModel> viewPage,
                                                           Expression<Func<TModel, T>> modelValue, string htmlCode)
        {
            return String.Format("{0} {1}", htmlCode, viewPage.Html.ValidationMessageFor(modelValue));
        }

        /// <summary>
        ///     Автоматический выбор страницы просмотра/редактирования
        /// </summary>
        public static string ViewOrEdit<TModel>(this ViewPage<TModel> viewPage, string viewInfo,
                                                MvcHtmlString editInfo)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");
            return baseModel.IsEdit ? editInfo.ToHtmlString() : viewInfo;
        }

        /// <summary>
        ///     Автоматический выбор страницы просмотра/редактирования
        /// </summary>
        public static string ViewOrEdit<TModel>(this ViewPage<TModel> viewPage,
                                                MvcHtmlString viewInfo, MvcHtmlString editInfo)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");
            return baseModel.IsEdit ? editInfo.ToHtmlString() : viewInfo.ToHtmlString();
        }

        /// <summary>
        ///     Автоматический выбор текстбокса просмотра/редактирования
        /// </summary>
        public static string ViewOrEditTextBox<TModel, T>(this ViewPage<TModel> viewPage,
                                                          Expression<Func<TModel, T>> modelValue,
                                                          params string[] classNames)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");

            Dictionary<string, object> classAttributes, classAttributesWithDisable;
            PrepareClassAttributesForTextBox(classNames, out classAttributes, out classAttributesWithDisable);

            if (baseModel.IsEdit)
                return ValidationWrapper(viewPage, modelValue,
                                         viewPage.Html.TextBoxFor(modelValue, classAttributes).ToHtmlString());

            return viewPage.Html.TextBoxFor(modelValue, classAttributesWithDisable).ToHtmlString();
        }

        /// <summary>
        ///     Автоматический выбор даты, для просмотра/редактирования
        /// </summary>
        public static string ViewOrEditDate<TModel>(this ViewPage<TModel> viewPage,
                                                    Expression<Func<TModel, DateTime>> modelValue,
                                                    params string[] classNames)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");

            Dictionary<string, object> classAttributes, classAttributesWithDisable;
            PrepareClassAttributesForTextBox(classNames, out classAttributes, out classAttributesWithDisable);

            if (baseModel.IsEdit)
                return ValidationWrapper(viewPage, modelValue,
                                         viewPage.Html.EditorFor(modelValue, new {baseModel.IsEdit})
                                                 .ToHtmlString());

            return viewPage.Html.EditorFor(modelValue, new {baseModel.IsEdit}).ToHtmlString();
        }

        /// <summary>
        ///     Автоматический выбор чекбокса просмотра/редактирования
        /// </summary>
        public static string ViewOrEditCheckBox<TModel>(this ViewPage<TModel> viewPage,
                                                        Expression<Func<TModel, bool>> modelValue,
                                                        params string[] classNames)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");

            Dictionary<string, object> classAttributes, classAttributesWithDisable;
            PrepareClassAttributes(classNames, out classAttributes, out classAttributesWithDisable);

            return ValidationWrapper(viewPage, modelValue, baseModel.IsEdit
                                                               ? viewPage.Html.CheckBoxFor(modelValue, classAttributes)
                                                                         .ToHtmlString()
                                                               : viewPage.Html.CheckBoxFor(modelValue,
                                                                                           classAttributesWithDisable)
                                                                         .ToHtmlString());
        }

        /// <summary>
        ///     Автоматический выбор дропдауна просмотра/редактирования
        /// </summary>
        public static string ViewOrEditDropDown<TModel, T>(this ViewPage<TModel> viewPage,
                                                           Expression<Func<TModel, T>> modelValue,
                                                           IEnumerable<SelectListItem> selectList,
                                                           params string[] classNames)
        {
            var baseModel = viewPage.Model as BaseEditViewModel;
            if (baseModel == null)
                throw new Exception(
                    "Model object in ViewPage should be inherited from BaseEditViewModel.");

            Dictionary<string, object> classAttributes, classAttributesWithDisable;
            PrepareClassAttributes(classNames, out classAttributes, out classAttributesWithDisable);

            return baseModel.IsEdit
                       ? viewPage.Html.DropDownListFor(modelValue, selectList, classAttributes).ToHtmlString()
                       : viewPage.Html.DropDownListFor(modelValue, selectList, classAttributesWithDisable)
                                 .ToHtmlString();
        }
    }
}