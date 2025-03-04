﻿using System;
using System.Threading.Tasks;
using IdentityServer4.Admin.BusinessLogic.Dtos.Log;
using IdentityServer4.Admin.BusinessLogic.Mappers;
using IdentityServer4.Admin.BusinessLogic.Services.Interfaces;
using IdentityServer4.Admin.EntityFramework.Repositories.Interfaces;

namespace IdentityServer4.Admin.BusinessLogic.Services
{
    public class LogService : ILogService
    {
        protected readonly ILogRepository Repository;

        public LogService(ILogRepository repository)
        {
            Repository = repository;
        }

        public virtual async Task<LogsDto> GetLogsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await Repository.GetLogsAsync(search, page, pageSize);
            var logs = pagedList.ToModel();

            return logs;
        }

        public virtual async Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan)
        {
            await Repository.DeleteLogsOlderThanAsync(deleteOlderThan);
        }
    }
}
