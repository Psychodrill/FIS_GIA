using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.Repository.Interfaces.AllowedDirections;
using GVUZ.DAL.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Configuration;

namespace GVUZ.DAL.Dapper.Repository.Model.AllowedDirections
{
    /// <summary>
    /// Репозиторий для работы с заявками на управление списком разрешенных направлений в т.ч. и с профильными специальностями
    /// </summary>
    public class AllowedDirectionsRepository : GvuzRepository, IAllowedDirectionsRepository
    {
        public static readonly int DefaultYear = 0; 

        static AllowedDirectionsRepository()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeStart"], out DefaultYear) || DefaultYear < 2000)
            {
                DefaultYear = 2017;
            }
        }

        /// <summary>
        /// Получение списка заявок для управления списком по типам заявок
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestTypes">Список требуемых типов</param>
        /// <returns>Список заявок требуемого типа</returns>
        public List<InstitutionDirectionRequestDto> GetDirectionRequestsByTypes(int institutionId, IEnumerable<InstitutionDirectionRequestType> requestTypes)
        {
            const string inTypesPlaceholder = "@@REQUESTTYPES@@";
            string inTypes = string.Join(", ", new HashSet<InstitutionDirectionRequestType>(requestTypes ?? Enumerable.Empty<InstitutionDirectionRequestType>()).Select(t => t.ToString("d")));
            string query = SQLQuery.GetInstitutionDirectionRequestByTypes.Replace(inTypesPlaceholder, inTypes);

            return WithTransaction(tx => 
            {
                return tx.Query<InstitutionDirectionRequestDto>(query, new { institutionId }).ToList();
            });
        }

        /// <summary>
        /// Подтверждение заявки на добавление или удаление разрешенных направлений, а также на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        public void ApproveDirectionRequest(int institutionId, int requestId)
        {
            WithTransaction(tx =>
            {
                var req = GetDirectionRequestById(tx, institutionId, requestId, false);

                if (req == null)
                {
                    return;
                }

                if (req.RequestType == InstitutionDirectionRequestType.AddAllowedDirection)
                {
                    AddAllowedDirection(tx, req);
                }
                else if (req.RequestType == InstitutionDirectionRequestType.RemoveAllowedDirection)
                {
                    RemoveAllowedDirection(tx, req);
                }
                else if (req.RequestType == InstitutionDirectionRequestType.AddProfDirection)
                {
                    AddProfDirection(tx, req);
                }
                else
                {
                    return;
                }

                DeleteDirectionRequestById(tx, institutionId, req.RequestId);
            });
        }

        /// <summary>
        /// Добавление направления в список разрешенных для ОО
        /// </summary>
        /// <param name="tx">Unit of work</param>
        /// <param name="dto">Данные заявки на добавление</param>
        private void AddAllowedDirection(IDbTransaction tx, InstitutionDirectionRequestDto dto)
        {
            tx.Execute(SQLQuery.AddAllowedDirection, new { dto.InstitutionId, dto.RequestId, year = DefaultYear });
        }

        /// <summary>
        /// Удаление направления из списка разрешенных для ОО
        /// </summary>
        /// <param name="tx">Unit of work</param>
        /// <param name="dto">Данные заявки на удаление</param>
        private void RemoveAllowedDirection(IDbTransaction tx, InstitutionDirectionRequestDto dto)
        {
            tx.Execute(SQLQuery.DeleteAllowedDirection, new { dto.InstitutionId, dto.RequestId });
        }


        /// <summary>
        /// Добавление разрешенного направления в список с направлений с профильными ВИ для ОО
        /// </summary>
        /// <param name="tx">Unit of work</param>
        /// <param name="dto">Данные заявки на добавление</param>
        private void AddProfDirection(IDbTransaction tx, InstitutionDirectionRequestDto dto)
        {
            tx.Execute(SQLQuery.AddAllowedProfDirection, new { dto.InstitutionId, dto.RequestId });
        }


        /// <summary>
        /// Отклонение заявки на добавление или удаление разрешенных направлений, а также на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        /// <param name="denialComment">Комментарий к отклоняемой заявке</param>
        public void DenyDirectionRequest(int institutionId, int requestId, string denialComment)
        {
            var args = new { institutionId, requestId, denialComment };

            WithTransaction(tx => 
            {
                tx.Execute(SQLQuery.DenyInstitutionDirectionRequest, args);
            });
        }

        /// <summary>
        /// Удаление отклоненных заявок на добавление или удаление разрешенных направлений или на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="forProfTests">Признак для удаления отклоненных заявок на включение в список с профильными ВИ</param>
        public void RemoveDeniedRequests(int institutionId, bool forProfTests)
        {
            const string inTypesPlaceholder = "@@REQUESTTYPES@@";
            const string delTypes = "0, 1"; // при удалении отклоненных заявок на добавление или удаление разрешенных направлений (forProfTests = false)
            const string delTypesProf = "2"; // при удалении отклоненных заявок на добавление в список с профильными ВИ (forProfTests = true)

            string query = SQLQuery.DeleteDeniedInstitutionRequests.Replace(inTypesPlaceholder, forProfTests ? delTypesProf : delTypes);
            var args = new { institutionId };

            WithTransaction(tx => 
            {
                tx.Execute(query, args);
            });
        }

        /// <summary>
        /// Получение сведений о заявке на добавление или удаление разрешенных направлений или на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        /// <returns>Сведения о заявке</returns>
        public InstitutionDirectionRequestDto GetDirectionRequestById(int institutionId, int requestId, bool loadComments = false)
        {
            return WithTransaction(tx => GetDirectionRequestById(tx, institutionId, requestId, loadComments));
        }

        /// <summary>
        /// Получение сведений о заявке на добавление или удаление разрешенных направлений или на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        /// <param name="loadComments">Загружать текст комментария</param>
        /// <returns>Сведения о заявке</returns>
        private InstitutionDirectionRequestDto GetDirectionRequestById(IDbTransaction tx, int institutionId, int requestId, bool loadComments)
        {
            return tx.Query<InstitutionDirectionRequestDto>(SQLQuery.GetInstitutionDirectionRequestById, new { institutionId, requestId, loadComments }).SingleOrDefault();
        }

        /// <summary>
        /// Удаление нерассмотренной заявки на добавление или удаление разрешенных направлений или на включение направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id нерассмотренной заявки</param>
        public void DeleteDirectionRequest(int institutionId, int requestId)
        {
            WithTransaction(tx => { DeleteDirectionRequestById(tx, institutionId, requestId); });
        }

        private void DeleteDirectionRequestById(IDbTransaction tx, int institutionId, int requestId)
        {
            tx.Execute(SQLQuery.DeleteInstitutionRequestById, new { institutionId, requestId });
        }


        private static readonly Dictionary<InstitutionDirectionSearchType, string> FindDirectionsQueryFactory = new Dictionary<InstitutionDirectionSearchType, string>
        {
            { InstitutionDirectionSearchType.IncludeAllowedDirection, SQLQuery.FindDirectionsToInclude },
            { InstitutionDirectionSearchType.ExcludeAllowedDirection, SQLQuery.FindDirectionsToExclude },
            { InstitutionDirectionSearchType.IncludeProfDirection, SQLQuery.FindDirectionsToIncludeProf },
            { InstitutionDirectionSearchType.IncludeAllowedDirectionAdmin, SQLQuery.FindDirectionsToIncludeAdmin }
        };

        /// <summary>
        /// Поиск направлений для включения в список
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="dto">Критерии поиска</param>
        /// <returns>Список найденных направлений</returns>
        public List<DirectionDto> FindDirections(int institutionId, InstitutionDirectionSearchCommand dto)
        {
            var args = new { institutionId, dto.UgsId, dto.EducationLevelId, dto.Year };

            string query = FindDirectionsQueryFactory[dto.SearchType];
            
            return WithTransaction(tx =>
            {
                if (dto.SearchType != InstitutionDirectionSearchType.IncludeAllowedDirectionAdmin) // временные id используются только в заявках
                {                    
                    tx.WriteToTempTable(dto.TempDirectionsId, "tmpId", "DirectionId");
                }
                
                return tx.Query<DirectionDto>(query, args).ToList();
            });
        }

        /// <summary>
        /// Формирование заявок на добавление или исключение из списка разрешенных направлений или из списка с профильными ВИ
        /// </summary>
        /// <param name="institutionID">Id ОО</param>
        /// <param name="submits">Сведения о формируемых заявках</param>
        public void SubmitDirectionRequests(int institutionID, IEnumerable<SubmitDirectionRequestDto> submits)
        {
            if (!submits.Any())
            {
                return;
            }

            WithTransaction(tx => {
                using (var cmd = tx.Connection.CreateCommand())
                {
                    cmd.CommandText = SQLQuery.InsertInstitutionDirectionRequest;
                    cmd.Transaction = tx;
                    cmd.CommandType = CommandType.Text;

                    var pInsId = new SqlParameter("@institutionId", SqlDbType.Int, 4) { Value = institutionID };
                    var pDirId = new SqlParameter("@directionId", SqlDbType.Int, 4) { Value = DBNull.Value };
                    var pType = new SqlParameter("@requestType", SqlDbType.Int, 4) { Value = DBNull.Value };
                    var pComment = new SqlParameter("@comment", SqlDbType.VarChar, 255) { Value = DBNull.Value };
                    cmd.Parameters.Add(pInsId);
                    cmd.Parameters.Add(pDirId);
                    cmd.Parameters.Add(pType);
                    cmd.Parameters.Add(pComment);

                    cmd.Prepare();

                    foreach (var dto in submits)
                    {
                        pDirId.Value = dto.DirectionId;
                        pType.Value = (int)dto.RequestType;
                        pComment.Value = !string.IsNullOrWhiteSpace(dto.Comment) ? dto.Comment.Trim() : (object)DBNull.Value;
                        cmd.ExecuteNonQuery();
                    }
                }
                
            });
        }

        /// <summary>
        /// Добавление выбранных администратором направлений в список разрешенных
        /// </summary>
        /// <param name="institutionID">ID ОО</param>
        /// <param name="items">Сведения о добавляемых направлениях</param>
        public void AddAllowedDirections(int institutionID, IEnumerable<AllowedDirectionCreateDto> items)
        {
            if (!items.Any())
            {
                return;
            };

            WithTransaction(tx => {

                using (var cmd = tx.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = tx;
                    cmd.CommandText = SQLQuery.InsertAllowedDirection;

                    var pInsId = new SqlParameter("@institutionId", SqlDbType.Int, 4) { Value = institutionID };
                    var pDirId = new SqlParameter("@directionId", SqlDbType.Int, 4) { Value = DBNull.Value };
                    var pYear = new SqlParameter("@year", SqlDbType.Int, 4) { Value = DBNull.Value };

                    cmd.Parameters.Add(pInsId);
                    cmd.Parameters.Add(pDirId);
                    cmd.Parameters.Add(pYear);

                    cmd.Prepare();

                    foreach (var dto in items)
                    {
                        pDirId.Value = dto.DirectionId;
                        pYear.Value = dto.Year.HasValue ? dto.Year.Value : DateTime.Now.Year;

                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        /// <summary>
        /// Получение сведений о нерассмотренных заявках для всех ОО
        /// </summary>
        /// <param name="pageable">Правила постраничной разбивки</param>
        /// <param name="sortable">Правила сортировки</param>
        public List<InstitutionDirectionRequestSummaryDto> GetDirectionsRequestsPaged(IPageable pageable, ISortable sortable)
        {
            return WithTransaction(tx =>
            {
                pageable.TotalRecords = tx.Query<int>(SQLQuery.InstitutionDirectionRequestListCount).Single();

                return pageable.TotalRecords > 0 ?
                      tx.Query<InstitutionDirectionRequestSummaryDto>(SQLQuery.InstitutionDirectionRequestListPaged.OrderBy(sortable), new { page = pageable.CurrentPage, pageSize = pageable.PageSize }).ToList()
                    : new List<InstitutionDirectionRequestSummaryDto>(0);
            });
        }

        /// <summary>
        /// Получение списка запросов на добавление или удаление направлений из списка разрешенных или списка с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>Список запросов на добавление направлений подготовки в качестве разрешенных для указанной ОО или разрешенных направлений с профильными ВИ</returns>
        public List<InstitutionDirectionRequestDto> GetDirectionsRequestDetails(int institutionId)
        {
            return WithTransaction(tx =>
            {
                return tx.Query<InstitutionDirectionRequestDto>(SQLQuery.InstitutionDirectionRequestDetails, new { institutionId }).ToList();
            });
        }
    }
}
