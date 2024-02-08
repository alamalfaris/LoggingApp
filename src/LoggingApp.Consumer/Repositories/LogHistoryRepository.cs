using LoggingApp.Consumer.Database;
using LoggingApp.Consumer.Interfaces;
using LoggingApp.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingApp.Consumer.Repositories
{
    public class LogHistoryRepository : ILogHistory
    {
        private readonly DatabaseContext _context;

        public LogHistoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task InsertLogHistory(LogEntity logEntity)
        {
            _context.LogHistories.Add(logEntity);
            await _context.SaveChangesAsync();
        }
    }
}
