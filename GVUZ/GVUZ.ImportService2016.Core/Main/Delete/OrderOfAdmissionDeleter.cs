using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ServiceModel.Import;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Application;
using log4net;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class OrderOfAdmissionDeleter : BaseDeleter
    {
        public static readonly ILog oai_logger = LogManager.GetLogger("OrderOfAdmissionImporter");
        public OrderOfAdmissionDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Console.WriteLine("Валидация пакета #{0} на удаление приказов для ОО {1}...", dataForDelete.ImportPackageId, dataForDelete.InstitutionId);
            oai_logger.DebugFormat("Валидация пакета #{0} на удаление приказов для ОО {1}...", dataForDelete.ImportPackageId, dataForDelete.InstitutionId);
            if (dataForDelete.ApplicationsInOrders != null)
            {
                foreach (var applicationInfo in dataForDelete.ApplicationsInOrders)
                {
                    //var dbApplications = vocabularyStorage.ApplicationVoc.Items.Where(x => (x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder || x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted)
                    //                                                                    && ((string.IsNullOrWhiteSpace(applicationInfo.UID) || x.UID == applicationInfo.UID)
                    //                                                                    && (string.IsNullOrWhiteSpace(applicationInfo.ApplicationNumber) || x.ApplicationNumber == applicationInfo.ApplicationNumber)
                    //                                                                    && (x.RegistrationDate.Date.Equals(applicationInfo.RegistrationDate.Date)))
                    //    );

                    var dbApplications = vocabularyStorage.ApplicationVoc.Items.Where(x => (x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder || x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted)
                                                                                        && (string.IsNullOrWhiteSpace(applicationInfo.UID) || x.UID == applicationInfo.UID));
                    if (!dbApplications.Any() && !string.IsNullOrWhiteSpace(applicationInfo.ApplicationNumber))
                    {
                        dbApplications = vocabularyStorage.ApplicationVoc.Items.Where(x => (x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder || x.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted)
                                                                                        && (x.ApplicationNumber == applicationInfo.ApplicationNumber));

                    }
                    oai_logger.DebugFormat("Найдено заявлений в БД: {0} (должно быть одно)...", dbApplications.Count());
                    if (dbApplications.Count() != 1)
                    {
                        SetBroken(ConflictMessages.ApplicationIsNotFoundOrAbsentInOrder
                                , applicationInfo.UID
                                , applicationInfo.ApplicationNumber
                                , applicationInfo.RegistrationDate.GetDateTimeAsString() 
                                , applicationInfo.OrderUID
                                , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                {
                                    // Applications = new[] { id },
                                });
                        continue;
                    }
                    var dbApplication = dbApplications.First();


                    // OrderOfAdmission.OrderStatus д.б. != 3
                    var dbOrder = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.UID == applicationInfo.OrderUID).FirstOrDefault();
                    if (dbOrder == null)
                    {
                        SetBroken(ConflictMessages.ApplicationIsNotFoundOrAbsentInOrder
                                , applicationInfo.UID
                                , applicationInfo.ApplicationNumber
                                , applicationInfo.RegistrationDate.GetDateTimeAsString()
                                , applicationInfo.OrderUID
                                , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                {
                                });
                        continue;
                    }
                    else if (dbOrder.OrderOfAdmissionStatusID == OrderOfAdmissionVocDto.OrderStatusPublished)
                    {
                        //oai_logger.ErrorFormat("Ошибка! Приказ опубликован {0} (должно быть одно)...", dbApplications.Count());
                        SetBroken(ConflictMessages.ApplicationIsNotFoundOrAbsentInOrder
                                , applicationInfo.UID
                                , applicationInfo.ApplicationNumber
                                , applicationInfo.RegistrationDate.GetDateTimeAsString()
                                , applicationInfo.OrderUID
                                , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                {
                                });
                        continue;
                    }

                    // Проверить, что заявление включено в такой приказ
                    var acgi = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.FirstOrDefault(t => t.ApplicationId == dbApplication.ApplicationID && (t.OrderOfAdmissionID == dbOrder.OrderID || t.OrderOfExceptionID == dbOrder.OrderID));
                    if (acgi == null)
                    {
                        SetBroken(ConflictMessages.ApplicationIsNotFoundOrAbsentInOrder
                                , applicationInfo.UID
                                , applicationInfo.ApplicationNumber
                                , applicationInfo.RegistrationDate.GetDateTimeAsString()
                                , applicationInfo.OrderUID
                                , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                {
                                });
                        continue;
                    }

                    // TODO: проверки, типа: нельзя удалять из OOA, если ACGI уже имеет OOE?...
                    if (acgi.OrderOfAdmissionID == dbOrder.OrderID && acgi.OrderOfExceptionID != 0)
                    {
                        SetBroken(ConflictMessages.ApplicationOrderTryToDeleteOOAButHasOOE
                                , applicationInfo.UID
                                , applicationInfo.ApplicationNumber
                                , applicationInfo.RegistrationDate.GetDateTimeAsString()
                                , applicationInfo.OrderUID
                                , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                {
                                });
                        continue;
                    }

                    // Что-нибудь с согласиями?

                    if (acgi.OrderOfAdmissionID == dbOrder.OrderID)
                        deleteItems.Add(new ApplicationInOrderOfAdmissionDto(acgi));
                    else if (acgi.OrderOfExceptionID == dbOrder.OrderID)
                        deleteItems.Add(new ApplicationInOrderOfExceptionDto(acgi));
                    else
                        throw new Exception("Unknown condition");
                }
            }
            if (dataForDelete.Orders != null) 
            {
                Console.WriteLine("Подготовка удаления приказов (всего: {0})", dataForDelete.Orders.OrdersOfAdmission.Length);
                oai_logger.DebugFormat("Подготовка удаления приказов (всего: {0})", dataForDelete.Orders.OrdersOfAdmission.Length);
                if (dataForDelete.Orders.OrdersOfAdmission != null)
                    foreach (var orderUID in dataForDelete.Orders.OrdersOfAdmission)
                    {
                        Console.WriteLine("Добавление приказа №{0} на удаление...", orderUID);
                        oai_logger.DebugFormat("Добавление приказа №{0} на удаление...", orderUID);
                        var dbOrder = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.UID == orderUID && t.OrderOfAdmissionTypeID == OrderOfAdmissionVocDto.OrderTypeAdmission).FirstOrDefault();
                        if (dbOrder == null)
                        {
                            SetBroken(ConflictMessages.OrderIsNotFound, orderUID
                                    , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                    {

                                    });
                            continue;
                        }

                        // Чтобы удалить приказ у него должен быть статус = 1 (нет заявлений), т.е. не должно быть acgi, где ACGI.OrderOfAdmissionID = данный приказ
                        // Возможна ситуация, что у приказа есть заявления, которые мы в этом же пакете исключаем из приказа - тогда их не нужно учитывать для конфликта
                        //var applications = vocabularyStorage.ApplicationVoc.Items.Where(t => t.StatusID == 8 && t.OrderOfAdmissionID == dbOrder.OrderID && !deleteItems.Any(d => d.ID == t.ApplicationID));
                        var sw2 = new System.Diagnostics.Stopwatch();
                        sw2.Start();
                        Console.WriteLine("Ищем среди условий приема, выбранных в заявлении (всего: {0}) наличия приказа {1}...", vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Count, dbOrder.OrderID);
                        var acgis = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.OrderOfAdmissionID == dbOrder.OrderID && !deleteItems.Any(d => d.ID == t.ID)).Select(s => s.ApplicationId).ToList();
                        // WTF??? это шутка?
                        //var acgis = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.OrderOfAdmissionID == dbOrder.OrderID && !deleteItems.Any(d => d.ID == t.ID));
                        //var applications = vocabularyStorage.ApplicationVoc.Items.Where(t => acgis.Any(a => a.ApplicationId == t.ApplicationID));
                        if(acgis != null && acgis.Count > 0)
                        {
                            Console.WriteLine("Ищем среди заявлений (всего: {0}) наличия приказа {1}...", vocabularyStorage.ApplicationVoc.Items.Count, dbOrder.OrderID);
                            var applications = vocabularyStorage.ApplicationVoc.Items.Where(t => acgis.Contains(t.ApplicationID)).ToList();
                            sw2.Stop();
                            if (applications.Count > 0 )
                            {
                                Console.WriteLine("Найдены заявления (за {0} с.) в приказе #{1}, добавлем ошибку...", sw2.Elapsed.TotalSeconds, dbOrder.OrderID);
                                var appShortRefs = applications.Select(t => new ApplicationShortRef() { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.RegistrationDate, UID = t.UID }).ToArray();
                                SetBroken(ConflictMessages.OrderHasApplications, orderUID
                                     , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { Applications = appShortRefs });
                                continue;
                            }
                        }
                        deleteItems.Add(dbOrder);
                    }

                if (dataForDelete.Orders.OrdersOfException != null)
                    foreach (var orderUID in dataForDelete.Orders.OrdersOfException)
                    {
                        var dbOrder = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.UID == orderUID && t.OrderOfAdmissionTypeID == OrderOfAdmissionVocDto.OrderTypeException).FirstOrDefault();
                        if (dbOrder == null)
                        {
                            SetBroken(ConflictMessages.OrderIsNotFound, orderUID
                                    , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                                    {

                                    });
                            continue;
                        }

                        // Чтобы удалить приказ у него должен быть статус = 1 (нет заявлений), т.е. не должно быть acgi, где ACGI.OrderOfAdmissionID = данный приказ
                        // Возможна ситуация, что у приказа есть заявления, которые мы в этом же пакете исключаем из приказа - тогда их не нужно учитывать для конфликта
                        //var applications = vocabularyStorage.ApplicationVoc.Items.Where(t => t.StatusID == 8 && t.OrderOfAdmissionID == dbOrder.OrderID && !deleteItems.Any(d => d.ID == t.ApplicationID));

                        var sw2 = new System.Diagnostics.Stopwatch();
                        sw2.Start();
                        var acgis = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.OrderOfExceptionID == dbOrder.OrderID && !deleteItems.Any(d => d.ID == t.ID)).Select(s=>s.ApplicationId).ToList();
                        // Нет, это, определённо издевательство тут просто всё висит!
                        //var applications = vocabularyStorage.ApplicationVoc.Items.Where(t => acgis.Any(a => a.ApplicationId == t.ApplicationID));
                        if (acgis != null && acgis.Count > 0)
                        {
                            Console.WriteLine("Ищем среди заявлений (всего: {0}) наличия приказа об отчислении {1}...", vocabularyStorage.ApplicationVoc.Items.Count, dbOrder.OrderID);
                            var applications = vocabularyStorage.ApplicationVoc.Items.Where(t=> acgis.Contains(t.ApplicationID)).ToList();
                            sw2.Stop();
                            if (applications.Count > 0)
                            {
                                var appShortRefs = applications.Select(t => new ApplicationShortRef() { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.RegistrationDate, UID = t.UID }).ToArray();
                                SetBroken(ConflictMessages.OrderHasApplications, orderUID
                                     , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { Applications = appShortRefs });
                                continue;
                            }
                        }
                        deleteItems.Add(dbOrder);
                    }
            }
            sw.Stop();
            Console.WriteLine("Валидация пакета {0} завершена за {1} с.", dataForDelete.ImportPackageId, sw.Elapsed.TotalSeconds);
            oai_logger.DebugFormat("Валидация пакета {0} завершена за {1} с.", dataForDelete.ImportPackageId, sw.Elapsed.TotalSeconds);
            return;
        }

        private void SetBroken(int errorCode, string uid, string number, string regDate, string orderUID, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new OrderApplicationFailDetailsDto();
            if (!string.IsNullOrEmpty(uid))
                failDto.UID = uid;
            if (!string.IsNullOrEmpty(number))
                failDto.ApplicationNumber = number;
            if (!string.IsNullOrEmpty(number))
                failDto.RegistrationDate = regDate;
            failDto.OrderUID = orderUID;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notExcludedApps.Add(failDto);
        }

        private void SetBroken(int errorCode, string uid, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new OrderFailDetailsDto();
            if (!string.IsNullOrEmpty(uid))
                failDto.OrderUID = uid;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedOrders.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }

        public override List<string> DoDelete()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Validate();
            sw.Stop();
            Console.WriteLine(this.GetType().Name + ".Validate(): {0} сек", sw.Elapsed.TotalSeconds);
            oai_logger.InfoFormat(this.GetType().Name + ".Validate(): {0} сек", sw.Elapsed.TotalSeconds);
            sw.Restart();
            var res = DeleteFromDB();
            sw.Stop();
            Console.WriteLine(this.GetType().Name + ".DeleteFromDB(): {0} сек", sw.Elapsed.TotalSeconds);
            oai_logger.InfoFormat(this.GetType().Name + ".DeleteFromDB(): {0} сек", sw.Elapsed.TotalSeconds);
            return res;
        }
    }

    public abstract class ApplicationInOrderDto : VocabularyBaseDto
    {
        public ApplicationInOrderDto() : base() { }

        //public ApplicationInOrderDto(ApplicationVocDto appDto)
        //{
        //    this.ID = appDto.ID;
        //}
        public ApplicationInOrderDto(Dictionaries.Application.ApplicationCompetitiveGroupItemVocDto appDto)
        {
            this.ID = appDto.ID;
        }
    }
    public class ApplicationInOrderOfAdmissionDto : ApplicationInOrderDto
    {
        public ApplicationInOrderOfAdmissionDto(ApplicationCompetitiveGroupItemVocDto appDto) : base(appDto)
        {
        }
    }
    public class ApplicationInOrderOfExceptionDto : ApplicationInOrderDto
    {
        public ApplicationInOrderOfExceptionDto(ApplicationCompetitiveGroupItemVocDto appDto) : base(appDto)
        {
        }
    }
}
