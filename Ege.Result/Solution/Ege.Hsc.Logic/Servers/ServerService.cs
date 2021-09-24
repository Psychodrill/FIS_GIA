namespace Ege.Hsc.Logic.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Common;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;
    using TsSoft.Excel.Generators;

    class ServerService : IServerService
    {
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IRegionServerRepository _repository;
        [NotNull] private readonly IServerChecker _serverChecker;
        [NotNull] private readonly IMapper<BlankServerError, BlankServerErrorExcelModel> _errorMapper;
        [NotNull] private readonly IExcelGenerator<BlankServerErrorExcelModel> _excelGenerator;
            
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<ServerService>();

        public ServerService(
            [NotNull]IDbConnectionFactory connectionFactory, 
            [NotNull]IRegionServerRepository repository, 
            [NotNull]IServerChecker serverChecker, 
            [NotNull]IMapper<BlankServerError, BlankServerErrorExcelModel> errorMapper, 
            [NotNull]IExcelGenerator<BlankServerErrorExcelModel> excelGenerator)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;
            _serverChecker = serverChecker;
            _errorMapper = errorMapper;
            _excelGenerator = excelGenerator;
        }

        public async Task CheckServersAvailability()
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var servers = await _repository.GetServersHavingBlanks(connection) ?? Enumerable.Empty<BlankServerAvailabilityModel>();
                var tasks = servers.Select(_serverChecker.CheckAvailability).ToList();
                await Task.WhenAll(tasks);
                var result = tasks.Where(t => t != null).Select(t => t.Result).ToDictionary(r => r.Key, r => r.Value);
                await _repository.UpdateServerAvailability(connection, result);
            }
        }

        public async Task LoadServersFromCheckEgeDb()
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                await _repository.LoadFromCheckEgeDb(connection);
            }
            Logger.Trace("Loaded servers");
        }

        private class RegionCheckResult
        {
            [NotNull]
            public BlankServerStatus Status { get; set; }
            [NotNull]
            public IEnumerable<ServerErrors> Errors { get; set; }
        }

        private async Task<RegionCheckResult> CheckStatus(int regionId, [NotNull]ServerBlanks regionBlanks)
        {
            Logger.InfoFormat("Checking region {0}", regionId);
            var dbCount = regionBlanks.Blanks.Select(b => b.Value != null ? b.Value.Count : 0).Sum();
            var tasks = regionBlanks.Blanks.Select(b => _serverChecker.CheckFile(regionBlanks.Url, b)).ToList();
            await Task.WhenAll(tasks);
            Logger.InfoFormat("Checked region {0}", regionId);
            var serverCount = tasks.Sum(t => t != null && t.Result != null ? t.Result.Count : 0);
            var isAvailable = tasks.Any(t => t != null && t.Result != null && t.Result.FileRead);
            return new RegionCheckResult
            {
                Status = new BlankServerStatus
                {
                    Id = regionId,
                    LastFileCheck = DateTime.Now,
                    Server = regionBlanks.Url,
                    IsAvailable = isAvailable,
                    DbCount = dbCount,
                    HasErrors = tasks.Any(t => t != null && t.Result != null && (t.Result.Extra.Any() || t.Result.Missing.Any())),
                    ServerCount = serverCount,
                },
                Errors = tasks.Where(t => t != null).Select(t => t.Result),
            };
        }

        public async Task<BlankServerStatus> CheckStatus(int regionId)
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var serverBlanks = (await _repository.GetServersWithBlanks(connection, regionId)).FirstOrDefault();
                if (serverBlanks == null)
                {
                    return null;
                }
                var result = await CheckStatus(regionId, serverBlanks);
                if (result == null)
                {
                    throw new InvalidOperationException("CheckStatus returned null");
                }
                await _repository.UpdateServerData(
                        connection, regionId, result.Status.ServerCount, result.Errors, result.Status.IsAvailable);
                return result.Status;
            }
        }

        public async Task<ICollection<BlankServerStatus>> CheckAllStatuses()
        {
            ICollection<ServerBlanks> serverBlanks;
            Logger.InfoFormat("Retrieving servers with blanks");
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                serverBlanks = (await _repository.GetServersWithBlanks(connection, null));
            }
            Logger.InfoFormat("Retrieved servers with blanks");
            if (serverBlanks == null)
            {
                return new BlankServerStatus[0];
            }
            var tasks = serverBlanks.Where(b => b != null).Select(b => CheckStatus(b.RegionId, b)).ToList();
            await Task.WhenAll(tasks);
            var result = new List<BlankServerStatus>();
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null (2)");
                }
                foreach (var task in tasks)
                {
                    if (task == null)
                    {
                        continue;
                    }
                    var taskResult = task.Result;
                    Logger.InfoFormat("Updating database for region {0}", taskResult.Status.Id);
                    await _repository.UpdateServerData(
                            connection, taskResult.Status.Id, taskResult.Status.ServerCount, taskResult.Errors,
                            taskResult.Status.IsAvailable);
                    Logger.InfoFormat("Updated database for region {0}", taskResult.Status.Id);
                    result.Add(task.Result.Status);
                }
            }
            return result;
        }

        public async Task<ICollection<BlankServerStatus>> GetStatuses(int? regionId = null)
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var result = await _repository.GetStatuses(connection, regionId);
                if (result == null)
                {
                    throw new InvalidOperationException("IRegionServerRepository::GetStatuses returned null");
                }
                foreach (var status in result)
                {
                    if (status == null)
                    {
                        continue;
                    }
                    if (status.LastAvailabilityCheck != null)
                    {
                        status.LastAvailabilityCheck = new DateTime(
                            status.LastAvailabilityCheck.Value.Ticks, DateTimeKind.Local);
                    }
                    if (status.LastFileCheck != null)
                    {
                        status.LastFileCheck = new DateTime(
                            status.LastFileCheck.Value.Ticks, DateTimeKind.Local);
                    }
                }
                return result;
            }
        }

        public async Task<Stream> GenerateErrorsFile(int regionId)
        {
            ICollection<BlankServerError> errors;
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                errors = await _repository.GetErrors(connection, regionId) ?? new BlankServerError[0];
            }
            var errorModels = errors.Select(_errorMapper.Map);
            var stream = _excelGenerator.Generate(errorModels);
            return stream;
        }
    }
}
