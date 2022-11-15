namespace StackoverflowAPI.Web.Constants.Errors
{
    public static class QuestionsSummaryComponentErrors
    {
        public static string FromDateShouldBeLesser { get; } = "{From date} should be less than {To date}";
        public static string FromDateShouldBeInPast { get; } = "{From date} should be in past";
    }
}
