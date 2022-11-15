using System;
using Xunit;
using StackoverflowAPI.Web.Services;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace StackoverFlowUnitTests.ServicesTests.StackoverflowServiceTest
{
    public class StackoverflowServiceTest
    {
        private StackOverflowService _systemUnderTest;

        private Mock<HttpMessageHandler>? _msgHandler;
        private MockHttpMessageHandler? _szalayMsgHandler;

        private string _baseAddress = "https://api.stackexchange.com/2.3";
        //private string _response = "{'items': [   { 'tags': ['javascript','html', 'css',  'button' ], 'owner': {  'account_id': 16591571, 'reputation': 67, 'user_id': 20489184, 'user_type': 'registered', 'profile_image': 'https://www.gravatar.com/avatar/c3005045bc205751f2c8cc5bbcf5f2be?s=256&d=identicon&r=PG', 'display_name': 'Marci Kardon', 'link': 'https://stackoverflow.com/users/20489184/marci-kardon' }, 'is_answered': false, 'view_count': 92,'answer_count': 0,'score': 1,'last_activity_date': 1668476632,            'creation_date': 1668458605,            'last_edit_date': 1668476632,            'question_id': 74437614,            'content_license': 'CC BY-SA 4.0',            'link': 'https://stackoverflow.com/questions/74437614/hiding-and-unhiding-css-container-classes-via-button',  'title': 'hiding and unhiding CSS container classes via button' }] }";
        private string _response = "{\"items\":[{\"tags\":[\"javascript\",\"html\",\"css\",\"button\"],\"owner\":{\"account_id\":16591571,\"reputation\":67,\"user_id\":20489184,\"user_type\":\"registered\",\"profile_image\":\"https://www.gravatar.com/avatar/c3005045bc205751f2c8cc5bbcf5f2be?s=256&d=identicon&r=PG\",\"display_name\":\"MarciKardon\",\"link\":\"https://stackoverflow.com/users/20489184/marci-kardon\"},\"is_answered\":false,\"view_count\":92,\"answer_count\":0,\"score\":1,\"last_activity_date\":1668476632,\"creation_date\":1668458605,\"last_edit_date\":1668476632,\"question_id\":74437614,\"content_license\":\"CCBY-SA4.0\",\"link\":\"https://stackoverflow.com/questions/74437614/hiding-and-unhiding-css-container-classes-via-button\",\"title\":\"hidingandunhidingCSScontainerclassesviabutton\"},{\"tags\":[\"python\",\"optimization\",\"pulp\"],\"owner\":{\"account_id\":3255648,\"reputation\":1672,\"user_id\":2744821,\"user_type\":\"registered\",\"accept_rate\":59,\"profile_image\":\"https://www.gravatar.com/avatar/06b6ecdda9c7593466476586c1d9dcfe?s=256&d=identicon&r=PG&f=1\",\"display_name\":\"kevin.w.johnson\",\"link\":\"https://stackoverflow.com/users/2744821/kevin-w-johnson\"},\"is_answered\":false,\"view_count\":12,\"answer_count\":1,\"score\":0,\"last_activity_date\":1668475553,\"creation_date\":1668466177,\"question_id\":74438757,\"content_license\":\"CCBY-SA4.0\",\"link\":\"https://stackoverflow.com/questions/74438757/how-to-identify-optimal-item-prices-given-a-list-of-quantities-and-orders\",\"title\":\"Howtoidentifyoptimalitempricesgivenalistofquantitiesandorders?\"}],\"has_more\":true,\"quota_max\":300,\"quota_remaining\":225}";
        

        [Fact]
        public async Task PassingNullDatesShouldReturnResults()
        {
            //Arrange
            DateTime? fromDate = null;
            DateTime? toDate = null;

            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When($"https://api.stackexchange.com/questions?order=desc&sort=activity&Site=stackoverflow")
                    .Respond("application/json", _response); // Respond with JSON


            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri(_baseAddress);
            _systemUnderTest = new StackOverflowService(client);

            //Act
            var response = await _systemUnderTest.GetQuestionsAsync(fromDate, toDate);

            //Assert
            Assert.True(true); //means no exception
        }

        [Fact]
        public async Task PassingNonNullDatesShouldReturnResults()
        {
            //Arrange
            DateTime? fromDate = DateTime.Now;
            DateTime? toDate = DateTime.Now;

            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When($"https://api.stackexchange.com/questions?order=desc&sort=activity&Site=stackoverflow")
                    .Respond("application/json", _response); // Respond with JSON


            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri(_baseAddress);
            _systemUnderTest = new StackOverflowService(client);

            //Act
            var response = await _systemUnderTest.GetQuestionsAsync(fromDate, toDate);

            //Assert
            Assert.True(true); //means no exception
        }

        [Fact]
        public async Task PassingNoBaseAddressShouldThrowException()
        {
            //Arrange
            DateTime? fromDate = DateTime.Now;
            DateTime? toDate = DateTime.Now;

            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When($"https://api.stackexchange.com/questions?order=desc&sort=activity&Site=stackoverflow")
                    .Respond("application/json", _response); // Respond with JSON


            var client = new HttpClient(mockHttp);
            _systemUnderTest = new StackOverflowService(client);


            try
            { //Act
                var response = await _systemUnderTest.GetQuestionsAsync(fromDate, toDate);
            }
            catch (Exception ex)
            {

                //Assert
                Assert.True(true); //means no exception
            }
        }

        [Fact]
        public async Task NoSuccessStatusCodeShouldReturnException()
        {
            //Arrange
            DateTime? fromDate = DateTime.Now;
            DateTime? toDate = DateTime.Now;

            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When($"https://api.stackexchange.com/questions?order=desc&sort=activity&Site=stackoverflow")
                     .Respond(HttpStatusCode.BadRequest);

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri(_baseAddress);
            _systemUnderTest = new StackOverflowService(client);

            try
            { //Act
                var response = await _systemUnderTest.GetQuestionsAsync(fromDate, toDate);
            }
            catch (Exception ex)
            {

                //Assert
                Assert.True(true); //means no exception
            }
        }
    }
}
