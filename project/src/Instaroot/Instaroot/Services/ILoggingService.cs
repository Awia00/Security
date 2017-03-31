using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;

namespace Instaroot.Services
{
    public interface ILoggingService
    {
        Task<IEnumerable<LogEntry>> GetLog();
        Task LogTrace(string trace);
        Task LogInfo(string info);
        Task LogWarning(string warning);
        Task LogError(string error);
    }
}