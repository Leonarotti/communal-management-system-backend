using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunalManagementSystem.BusinessWorkflow.UseCases
{
    public class DashboardBW : IDashboardBW
    {
        private readonly IManagePersonBW _managePersonBW;
        private readonly IManageIncomeBW _manageIncomeBW;
        private readonly IManageExpenseBW _manageExpenseBW;
        private readonly IManageQuotaBW _manageQuotaBW;

        public DashboardBW(
                       IManagePersonBW managePersonBW,
                                  IManageIncomeBW manageIncomeBW,
                                             IManageExpenseBW manageExpenseBW,
                                                        IManageQuotaBW manageQuotaBW)
        {
            _managePersonBW = managePersonBW;
            _manageIncomeBW = manageIncomeBW;
            _manageExpenseBW = manageExpenseBW;
            _manageQuotaBW = manageQuotaBW;
        }

        public async Task<Dashboard> GetDashboardMetricsAsync()
        {
            var totalPersons = await _managePersonBW.GetTotalPersonsAsync();
            var totalIncome = await _manageIncomeBW.GetTotalIncomesAsync();
            var totalExpenses = await _manageExpenseBW.GetTotalExpensesAsync();
            var totalQuotasPaid = await _manageQuotaBW.GetTotalQuotasPaidAsync();

            return new Dashboard
            {
                TotalPersons = totalPersons,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                CurrentBalance = (totalIncome + totalQuotasPaid) - totalExpenses,
                TotalQuotasPaid = totalQuotasPaid
            };
        }

        public async Task<Dashboard> GetQuarterSummary()
        {
            var totalIncomes = await _manageIncomeBW.GetTotalForLast3MonthsAsync();
            var totalExpenses = await _manageExpenseBW.GetTotalForLast3MonthsAsync();
            var totalQuotas = await _manageQuotaBW.GetTotalPaidForLast3MonthsAsync();

            return new Dashboard
            {
                TotalIncome = totalIncomes,
                TotalExpenses = totalExpenses,
                CurrentBalance = (totalIncomes + totalQuotas) - totalExpenses,
                TotalQuotasPaid = totalQuotas
            };

        }
    }
}
