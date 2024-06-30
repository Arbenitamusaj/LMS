using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.Interfaces
{
    public interface ILeaveBackgroundService
    {
        void AddDaysToAnnualLeaveMonthly();
        void ResetAnnualLeaveOnJulyFirst();
        Task AddDaysToAnnualLeaveJob();
        Task ResetAnnualLeaveJob();
    }
}
