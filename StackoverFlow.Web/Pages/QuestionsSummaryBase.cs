using Microsoft.AspNetCore.Components;
using StackoverflowAPI.Models.Dtos;
using StackoverflowAPI.Web.Constants.Errors;
using StackoverflowAPI.Web.Services.Contracts;
using System.Text;

namespace StackoverflowAPI.Web.Pages
{
    public class QuestionsSummaryBase : ComponentBase
    {
        [Inject]
        public IStackOverflowService StackOverflowService { get; set; }


        //data props
        public DateTime FromDate { get; set; } = DateTime.Now.Date.AddDays(-1);
        public DateTime ToDate { get; set; } = DateTime.Now.Date;

        public StackoverFlowQuestionsAPIResponseDTO StackoverFlowQuestions { get; set; }
        public int TotalNumberOfQuestions { get; set; }
        public long TotalUniqueViews { get; set; } = 0;
        public HashSet<string> TotalUniqueTags { get; set; } = new HashSet<string>();
        public StringBuilder TotalUniqueTagsList { get; set; } = new StringBuilder("");

        

        //ui props
        public string ErrorMessage { get; set; }
        public bool isLoading { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                await FetchQuestions();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

            }

        }


        protected async Task RefreshQuestions()
        {
            try
            {

                if (FromDate == ToDate)
                {
                    ErrorMessage = QuestionsSummaryComponentErrors.FromDateShouldBeLesser;
                    return;
                }
                if (FromDate > DateTime.Now.Date)
                {
                    ErrorMessage = QuestionsSummaryComponentErrors.FromDateShouldBeInPast;
                    return;
                }

                await FetchQuestions();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

            }
        }

        private async Task FetchQuestions()
        {
            ErrorMessage = null;
            isLoading = true;
            TotalUniqueTags = new HashSet<string>();
            TotalUniqueViews = 0;
            TotalUniqueTagsList = new StringBuilder("");

            StackoverFlowQuestions = await StackOverflowService.GetQuestionsAsync(FromDate, ToDate);


            //total questions
            TotalNumberOfQuestions = StackoverFlowQuestions.items.Count;

            //total views
            Parallel.ForEach(StackoverFlowQuestions.items, x => TotalUniqueViews += x.view_count);

            #region TODO: performance improvement
            //Parallel.ForEachAsync(StackoverFlowQuestions.items, CancellationToken.None,
            //    item => {

            //        foreach (var tag in item.tags)
            //        {
            //            TotalUniqueTags.Add(tag);
            //        }
            //    });
            #endregion

            //total unique tags
            foreach (var question in StackoverFlowQuestions.items)
            {
                foreach (var tag in question.tags)
                {
                    TotalUniqueTags.Add(tag);
                }
            }

            Parallel.ForEach(TotalUniqueTags, x => TotalUniqueTagsList.Append(x + ", "));

            isLoading = false;
        }

    }
}
