using LoggingApp.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingApp.Consumer.Interfaces
{
    public interface ILogHistory
    {
        Task InsertLogHistory(LogEntity logEntity);
    }
}
