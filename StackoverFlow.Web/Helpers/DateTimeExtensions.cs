namespace StackoverflowAPI.Web.Helpers
{
    public static class DateTimeExtensions
    {
        public static long dateToUnixMiliseconds(this DateTime date) 
        {

           DateTimeOffset dto = new DateTimeOffset(date);
            // Get the unix timestamp in seconds
            long unixTime = dto.ToUnixTimeSeconds();
         
            return unixTime;
        }

        public static DateTime UnixEpochToDateTime(this double unixTimeStamp)
        {

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;

        }
    }
}
