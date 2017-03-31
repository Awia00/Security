using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Storage.Database;

namespace Instaroot.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly InstarootContext _context;

        public LoggingService(InstarootContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<LogEntry>> GetLog()
        {
            return Task.FromResult(_context.Log.AsEnumerable());
        }

        public async Task LogTrace(string trace)
        {
            _context.Log.Add(new LogEntry
            {
                Log = trace,
                LogLevel = LogLevel.Trace,
                TimeStamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task LogInfo(string info)
        {
            _context.Log.Add(new LogEntry
            {
                Log = info,
                LogLevel = LogLevel.Info,
                TimeStamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task LogWarning(string warning)
        {
            _context.Log.Add(new LogEntry
            {
                Log = warning,
                LogLevel = LogLevel.Warning,
                TimeStamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task LogError(string error)
        {
            _context.Log.Add(new LogEntry
            {
                Log = error,
                LogLevel = LogLevel.Error,
                TimeStamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }
    }
}