using System;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Web.ViewModels;
using System.Collections.Generic;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.SQLDB;
using log4net;

namespace GVUZ.Web.Controllers
{
    public partial class ApplicationController : BaseController
    {
        // Save btnUnauthenticated - Не прошедшее проверку
        // При нажатии кнопки «Оставить непрошедшим проверку» процесс работы Мастера завершается, 
        // заявлению присваивается статус «Не прошедшее проверку» (полю Application. StatusID присваивается «3»). 

        [Authorize]
        [HttpPost]
        public ActionResult Wz5SaveUnauthenticated(AppResultsModel model)
        {
            try
            {
                ApplicationSQL.UpdateApplicationCompetitiveGroupItems(model.ApplicationID, model.ApplicationPriorities.ApplicationPriorities);
                ApplicationSQL.Wz5SaveUnauthenticated(model);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        #region ForcedAdmission

        [Authorize]
        [HttpPost]
        public ActionResult ForceAdmission(ForceAdmissionModel model)
        {
            //try
            //{
            //    ApplicationSQL.ForceAdmission(model);
            //    return new AjaxResultModel();
            //}
            try
            {
                //обновить условия приема
                ApplicationSQL.UpdateApplicationCompetitiveGroupItems(model.ApplicationID, model.ApplicationPriorities.ApplicationPriorities);
                //обновить само заявление
                ApplicationSQL.ForceAdmission(model);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }


        #endregion

        /// <summary>
        /// Сохранение без проверок
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SaveWz5(AppResultsModel model)
        {
            a_logger.DebugFormat("SaveWz5 -> {0}: {1} ({2}) {3}", model.userLogin, model.ApplicationID, model.ApplicationNumber, model.method);
            try
            {
                List<string> UidList = ApplicationSQL.GetUidListWz5(InstitutionID);

                for (int i = 0; i < UidList.Count; i++)
                {
                    if (model.Uid == UidList[i])
                    {
                        i++;
                        if (model.ApplicationID != Convert.ToInt32(UidList[i]))
                        {
                            return new AjaxResultModel { Data = null, Extra = "UidList" };
                        }

                    }
                }
                a_logger.DebugFormat("Проверка {0} > {1} ?", model.RegistrationDate.Year, DateTime.Now.Year);
                if (model.RegistrationDate.Year > DateTime.Now.Year) { return new AjaxResultModel { Extra = "DateError" }; }
                if (model.RegistrationDate.Year == DateTime.Now.Year)
                {
                    if (model.RegistrationDate.Month > DateTime.Now.Month)
                    {
                        return new AjaxResultModel { Extra = "DateError" };
                    }
                    if (model.RegistrationDate.Month == DateTime.Now.Month)
                    {
                        if (model.RegistrationDate.Day > DateTime.Now.Day)
                        {
                            return new AjaxResultModel { Extra = "DateError" };
                        }
                    }
                }
                if (model.RegistrationDate.Year <= DateTime.Now.Year - 100) { return new AjaxResultModel { Extra = "DateError" }; }
                a_logger.Debug("Обновление UpdateApplicationCompetitiveGroupItems...");
                ApplicationSQL.UpdateApplicationCompetitiveGroupItems(model.ApplicationID, model.ApplicationPriorities.ApplicationPriorities);
                ApplicationSQL.SaveWz5(model);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                a_logger.ErrorFormat("Ошибка при сохранении визарда 5: {0}", ex.Message);
                return new AjaxResultModel(ex.ToString());
            }
        }

        /// <summary>
        /// Сохранение с проверками
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Wz5Save(AppResultsModel model)
        {
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw3 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw4 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw5 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw6 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swTotal = new System.Diagnostics.Stopwatch();

            swTotal.Start();
            sw5.Start();
            List<string> UidList = ApplicationSQL.GetUidListWz5(InstitutionID);

            for (int i = 0; i < UidList.Count; i++)
            {
                if (model.Uid == UidList[i])
                {
                    i++;
                    if (model.ApplicationID != Convert.ToInt32(UidList[i]))
                    {
                        return new AjaxResultModel { Data = null, Extra = "UidList" };
                    }

                }
            }

            List<string> violationErrors = new List<string>();
            List<string> otherViolationErrors = new List<string>();

            if (model.RegistrationDate.Year > DateTime.Now.Year) { return new AjaxResultModel { Extra = "DateError" }; }
            if (model.RegistrationDate.Year == DateTime.Now.Year)
            {
                if (model.RegistrationDate.Month > DateTime.Now.Month)
                {
                    return new AjaxResultModel { Extra = "DateError" };
                }
                if (model.RegistrationDate.Month == DateTime.Now.Month)
                {
                    if (model.RegistrationDate.Day > DateTime.Now.Day)
                    {
                        return new AjaxResultModel { Extra = "DateError" };
                    }
                }
            }

            if (model.RegistrationDate.Year <= DateTime.Now.Year - 100)
            {
                return new AjaxResultModel { Extra = "DateError" };
            }
            sw5.Stop();
            //=======================================================================
            // проверки заявления
            //=======================================================================

            bool isCritical = false;
            bool egeError = false;
            // проверка наличия необходимых документов
            sw6.Start();
            //#DocumentsCheck - добавлена проверка (FIS-1711)
            IEnumerable<string> requiredDocuments;
            var checkDocuments = ApplicationSQL.CheckApplicationDocuments(model.ApplicationID, out requiredDocuments);

            sw6.Stop();
            if (!checkDocuments)//Если документов нет - никаких доп. проверок не делаем
            {
                otherViolationErrors.Add(String.Format("Заявление должно содержать один из следующих документов об образовании: {0}", String.Join("; ", requiredDocuments)));
                isCritical = true;
            }
            else
            {
                // проверка Проверка ЕГЭ (предупредительного характера)
                sw1.Start();
                var check1 = EntrantApplicationSQL.Check_EGE(model);
                sw1.Stop();
                if (check1 != null && check1.violationId != 0)
                {
                    violationErrors.Add(check1.violationMessage);
                    isCritical = false;
                    egeError = true;
                }

                // проверка Проверка Олимпиады
                sw2.Start();
                var check2 = EntrantApplicationSQL.Check_Olympic(model);
                sw2.Stop();
                if (check2.violationId != 0)
                {
                    violationErrors.Add(check2.violationMessage);
                    isCritical = true;
                }

                // проверка льгот
                sw3.Start();
                var check3 = EntrantApplicationSQL.Check_Benefit(model.ApplicationID);
                sw3.Stop();
                if (check3.violationId != 0)
                {
                    violationErrors.Add(check3.violationMessage);
                    isCritical = true;
                }

                // проверка 5 вузов (предупредительного характера)
                sw4.Start();
                var check4 = EntrantApplicationSQL.Check_5VUZ(model.ApplicationID);
                sw4.Stop();
                if (check4.violationId != 0)
                {
                    isCritical = false;
                    violationErrors.Add(check4.violationMessage);
                }
            }
             
            swTotal.Stop();

            if (violationErrors.Count > 0)
            {
                string result = "";

                for (int i = 0; i < violationErrors.Count; i++)
                {
                    if (i == violationErrors.Count - 1) { result = result + string.Format("{0} ", violationErrors[i]); }
                    else { result = result + string.Format("{0}; ", violationErrors[i]); }
                }
                ApplicationSQL.UpdateViolation(model.ApplicationID, result);

                var res = new AjaxResultModel(result);
                res.IsError = true;
                res.Extra = !isCritical;

                res.Data = new { EgeError = egeError };

                return res;
            }
            if (otherViolationErrors.Count > 0)
            {
                string result = String.Join("; ", otherViolationErrors);
                var res = new AjaxResultModel(result);
                res.IsError = true;
                res.Extra = false;
                return res;
            }

            try
            {
                ApplicationSQL.UpdateApplicationCompetitiveGroupItems(model.ApplicationID, model.ApplicationPriorities.ApplicationPriorities);
                ApplicationSQL.Wz5Save(model);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult ErrorAdditionalInfo(AppResultsModel model)
        {

            //для принудительного принятия(2017 FIS-1712) мы хотим загрузить доп. инфу по заявлению                 
            var vmf = ApplicationSQL.getViolationMoreInfo(model.ApplicationID);
            var res = new AjaxResultModel { Data = vmf };
            return res;
        }
    }
}