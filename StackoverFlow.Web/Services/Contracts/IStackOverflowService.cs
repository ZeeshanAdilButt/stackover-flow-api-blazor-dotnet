using StackoverflowAPI.Models.Dtos;

namespace StackoverflowAPI.Web.Services.Contracts
{
    public interface IStackOverflowService
    {
        Task<StackoverFlowQuestionsAPIResponseDTO> GetQuestionsAsync(DateTime? fromDate, DateTime? toDate);


    }
}
