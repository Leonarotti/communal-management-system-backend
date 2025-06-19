using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class IncomeMapper
    {
        public static IncomeDTO IncomeToIncomeDTO(Income income)
        {
            return new IncomeDTO
            {
                _id = income.Id,
                description = income.Description,
                amount = income.Amount,
                date = income.Date,
                created_at = income.CreatedAt
            };
        }

        public static Income IncomeDTOToIncome(IncomeDTO incomeDTO)
        {
            return new Income
            {
                Id = incomeDTO._id,
                Description = incomeDTO.description,
                Amount = incomeDTO.amount,
                Date = incomeDTO.date,
                CreatedAt = incomeDTO.created_at
            };
        }

        public static IEnumerable<IncomeDTO> IncomesToIncomeDTOs(IEnumerable<Income> incomes)
        {
            return incomes.Select(IncomeToIncomeDTO);
        }

        public static Income CreateIncomeDTOToIncome(CreateIncomeDTO createIncomeDTO)
        {
            return new Income
            {
                Description = createIncomeDTO.Description,
                Amount = createIncomeDTO.Amount,
                Date = createIncomeDTO.Date,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
