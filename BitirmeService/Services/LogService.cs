using BitirmeEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitirmeService.Services
{
    public static class LogService
    {
        public static void CreateErrorLog(Exception ex)
        {
            var newErrorLog = new ErrorLogs()
            {
                LogSource = ex.Source,
                LogMessage = ex.Message,
                RowStateId = 1
            };

        }
    }
}
