using KpiSchedule.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiSchedule.Common.Clients.Interfaces
{
    public interface IKpiGroupsClient
    {
        Task<KpiApiGroupsList> GetGroups(string groupPrefix);
    }
}
