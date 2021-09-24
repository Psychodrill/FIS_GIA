using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Main.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ImportService2016.Core.Dto.Partial;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ImportService2016.Core.Main.Import;

namespace GVUZ.ImportService2016.Core.Main.Check
{
    public class CheckManager
    {
        private PackageData _packageData;
        private IEnumerable<int> _applicationIDs;
        private string _userLogin;
        //private CampaignVoc _campaigns;
        private string deadlockWork = "deadlock";

        public CheckManager(PackageData packageData, IEnumerable<int> applicationIDs)
            : this(packageData, applicationIDs, null)
        {
        }

        public CheckManager(PackageData packageData, IEnumerable<int> applicationIDs, string userLogin)
        {
            _packageData = packageData;
            _applicationIDs = applicationIDs;
            //_campaigns = campaigns;
            _userLogin = userLogin;

            deadlockWork = System.Configuration.ConfigurationManager.AppSettings["DeadlockWord"];
            if (string.IsNullOrWhiteSpace(deadlockWork))
                deadlockWork = "deadlock";
        }


        public void DoCheck()
        {
            ApplicationChecker applicationChecker = null;
            string commentError = "";
            try
            {
                //bool isSingleCheck = (_packageData.Application != null);

                ////var applicationUIDS = |

                //List<CheckableApplication> applications = new List<CheckableApplication>(_applicationIDs.Count());
                //foreach (int applicationId in _applicationIDs)
                //{
                //    var application = _packageData.GetApplications.FirstOrDefault(x => x.ID == applicationId);

                //    string comment = null;
                //    bool isFromCrimea = false;
                //    bool needsEgeCheck = true;
                //    int statusId = 0;
                //    if (application != null)
                //    {
                //        // Если заявление в статусе 2 - не надо проверки!
                //        if (application.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted)
                //        {
                //            continue;
                //        }
                //        comment = application.StatusComment;
                //        statusId = application.StatusID.To(0);
                        
                        
                //    }
                //    applications.Add(new CheckableApplication(applicationId, comment, isFromCrimea, needsEgeCheck, statusId));
                //}

                applicationChecker = new ApplicationChecker(_packageData, _applicationIDs.ToList(), _userLogin);
                applicationChecker.DoCheck();
            }
            catch (Exception ex)
            {
                commentError = ex.Message + "  " + ex.StackTrace;
                Log.LogHelper.Log.ErrorFormat("№ {0} Ошибка: {1}", _packageData.ImportPackageId, ex.Message);
            }

            //var isDeadlock = commentError.ToLower().Contains(deadlockWork.ToLower());
            //if (isDeadlock)
            //{
            //    if (!ImportManager.DeadlockErrors.ContainsKey(_packageData.ImportPackageId))
            //        ImportManager.DeadlockErrors.Add(_packageData.ImportPackageId, 1);
            //    else
            //        ImportManager.DeadlockErrors[_packageData.ImportPackageId]++;

            //    if (ImportManager.DeadlockErrors[_packageData.ImportPackageId] <= ImportManager.MaxDeadlockErrors)
            //    {
            //        GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.ErrorFormat("№ {0} упал с DEADLOCK (CHECK) в {1} раз и ушел на повторный импорт", _packageData.ImportPackageId, ImportManager.DeadlockErrors[_packageData.ImportPackageId]);
            //        System.Threading.Thread.Sleep(30000); // Делаем паузу, чтобы дедлок разсосался за это время
            //        ADOPackageRepository.ResetDeadlockCheckPackage(_packageData.ImportPackageId);
            //    }
            //    else
            //    {
            //        var checkResultString = applicationChecker != null ? applicationChecker.CheckResultString : "";
            //        ADOPackageRepository.UpdateImportPackageCheckResult(_packageData.ImportPackageId, checkResultString, commentError);
            //        GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Проверка превысила максимальное число DEADLOCK-ов", _packageData.ImportPackageId);
            //        return;
            //    }
            //}
            //else
            //{
                var checkResultString = applicationChecker != null ? applicationChecker.CheckResultString : "";
                ADOPackageRepository.UpdateImportPackageCheckResult(_packageData.ImportPackageId, checkResultString, commentError);
                GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Проверка завершена", _packageData.ImportPackageId);
                return;
            //}

        }


    

        //public void DoWork()
        //{
        //    while (true)
        //    {

        //        ApplicationChecker applicationChecker = null;
        //        string commentError = "";
        //        try
        //        {
        //            bool isSingleCheck = (_packageData.Application != null);

        //            List<CheckableApplication> applications = new List<CheckableApplication>(_applicationIDs.Count());
        //            foreach (int applicationId in _applicationIDs)
        //            {
        //                //PackageDataApplication application = null;
        //                //if ((_packageData.Application != null) && (_packageData.Application.ID == applicationId))
        //                //{
        //                //    application = _packageData.Application;
        //                //}
        //                //else if (_packageData.Applications != null)
        //                //{
        //                //    //Возможен длительный перебор _packageData.Applications, 
        //                //    //в таком случае переделать как Dictionary(ID,application)
        //                //    application = _packageData.Applications.FirstOrDefault(x => x.ID == applicationId);
        //                //}

        //                var application = _packageData.GetApplications.FirstOrDefault(x => x.ID == applicationId);

        //                string comment = null;
        //                bool isFromCrimea = false;
        //                bool needsEgeCheck = true;
        //                int statusId = 0;
        //                if (application != null)
        //                {
        //                    // Если заявление в статусе 2 - не надо проверки!
        //                    if (application.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted || application.ShortUpdate)
        //                    {
        //                        continue;
        //                    }

        //                    comment = application.StatusComment;
        //                    statusId = application.StatusID.To(0);
        //                    List<int> campaignIds = new List<int>();
        //                    if (application.SelectedCompetitiveGroupsFull != null)
        //                    {
        //                        foreach (var competitiveGroupElement in application.SelectedCompetitiveGroupsFull)
        //                        {
        //                            if (competitiveGroupElement != null)
        //                            {
        //                                campaignIds.Add(competitiveGroupElement.CampaignID);
        //                            }
        //                        }
        //                    }
        //                    //foreach (int campaignId in campaignIds.Distinct())
        //                    //{
        //                    //    if (_campaigns == null)
        //                    //        break;

        //                    //    CampaignVocDto campaign = _campaigns.Items.FirstOrDefault(x => x.CampaignID == campaignId);
        //                    //    isFromCrimea = campaign.IsFromKrym;
        //                    //    if (isFromCrimea)
        //                    //        break;
        //                    //}

        //                    //needsEgeCheck = false;
        //                    //List<int> needsEgeCheckLevelIds = new List<int>() { 2, 3, 5, 19 };
        //                    //if (application.SelectedCompetitiveGroupItemsFull != null)
        //                    //{
        //                    //    foreach (var competitiveGroupItemElement in application.SelectedCompetitiveGroupItemsFull)
        //                    //    {
        //                    //        // TODO:
        //                    //        //if ((competitiveGroupItemElement.CompetitiveGroupItem != null)
        //                    //        //    && (needsEgeCheckLevelIds.Contains(competitiveGroupItemElement.CompetitiveGroupItem.EducationLevelID)))
        //                    //        //{
        //                    //        //    needsEgeCheck = true;
        //                    //        //    break;
        //                    //        //}
        //                    //    }
        //                    //}
        //                }
        //                applications.Add(new CheckableApplication(applicationId, comment, isFromCrimea, needsEgeCheck, statusId));
        //            }

        //            applicationChecker = new ApplicationChecker(_packageData, _applicationIDs.ToList(), _userLogin);
        //            applicationChecker.DoCheck();
        //        }
        //        catch (Exception ex)
        //        {
        //            commentError = ex.Message + "  " + ex.StackTrace;
        //        }

        //        var isDeadlock = commentError.ToLower().Contains(deadlockWork.ToLower());
        //        if (isDeadlock)
        //        {
        //            if (!ImportManager.DeadlockErrors.ContainsKey(_packageData.ImportPackageId))
        //                ImportManager.DeadlockErrors.Add(_packageData.ImportPackageId, 1);
        //            else
        //                ImportManager.DeadlockErrors[_packageData.ImportPackageId]++;

        //            if (ImportManager.DeadlockErrors[_packageData.ImportPackageId] <= ImportManager.MaxDeadlockErrors)
        //            {
        //                GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.ErrorFormat("№ {0} упал с DEADLOCK (CHECK) в {1} раз и ушел на повторный импорт", _packageData.ImportPackageId, ImportManager.DeadlockErrors[_packageData.ImportPackageId]);
        //                System.Threading.Thread.Sleep(30000); // Делаем паузу, чтобы дедлок разсосался за это время
        //                ADOPackageRepository.ResetDeadlockCheckPackage(_packageData.ImportPackageId);
        //            }
        //            else
        //            {
        //                var checkResultString = applicationChecker != null ? applicationChecker.CheckResultString : "";
        //                ADOPackageRepository.UpdateImportPackageCheckResult(_packageData.ImportPackageId, checkResultString, commentError);
        //                GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Проверка превысила максимальное число DEADLOCK-ов", _packageData.ImportPackageId);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            var checkResultString = applicationChecker != null ? applicationChecker.CheckResultString : "";
        //            ADOPackageRepository.UpdateImportPackageCheckResult(_packageData.ImportPackageId, checkResultString, commentError);
        //            GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Проверка завершена", _packageData.ImportPackageId);
        //            return;
        //        }

        //    }
        //}
    }
}
