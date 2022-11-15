using StackoverflowAPI.Models.Dtos;
using StackoverflowAPI.Web.Helpers;
using StackoverflowAPI.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace StackoverflowAPI.Web.Services
{
    public class StackOverflowService : IStackOverflowService
    {
        private readonly HttpClient httpClient;

        public StackOverflowService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<StackoverFlowQuestionsAPIResponseDTO> GetQuestionsAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                StringBuilder uri = new StringBuilder("/questions?order=desc&sort=activity&Site=stackoverflow");

                if (fromDate != null)
                    uri.Append($"&fromDate={fromDate?.dateToUnixMiliseconds()}");

                if (toDate != null)
                    uri.Append($"&toDate={toDate?.dateToUnixMiliseconds()}");

                var response = await this.httpClient.GetAsync(uri.ToString());

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<StackoverFlowQuestionsAPIResponseDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

    }
}
