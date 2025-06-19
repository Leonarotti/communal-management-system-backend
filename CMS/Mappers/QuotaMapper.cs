using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.Domain.Models;

namespace CommunalManagementSystem.API.Mappers
{
    public class QuotaMapper
    {
        public static QuotaDTO QuotaToQuotaDTO(Quota quota)
        {
            return new QuotaDTO
            {
                _id = quota.Id,
                person_id = quota.PersonId,
                year = quota.Year,
                month = quota.Month,
                amount = quota.Amount,
                created_at = quota.CreatedAt
            };
        }

        public static Quota QuotaDTOToQuota(QuotaDTO quotaDTO)
        {
            return new Quota
            {
                Id = quotaDTO._id,
                PersonId = quotaDTO.person_id,
                Year = quotaDTO.year,
                Month = quotaDTO.month,
                Amount = quotaDTO.amount,
                CreatedAt = quotaDTO.created_at
            };
        }

        public static IEnumerable<QuotaDTO> QuotasToQuotaDTOs(IEnumerable<Quota> quotas)
        {
            return quotas.Select(QuotaToQuotaDTO);
        }

        public static Quota CreateQuotaDTOToQuota(CreateQuotaDTO createQuotaDTO)
        {
            return new Quota
            {
                PersonId = createQuotaDTO.person_id,
                Year = createQuotaDTO.year,
                Month = createQuotaDTO.month,
                Amount = createQuotaDTO.amount,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
