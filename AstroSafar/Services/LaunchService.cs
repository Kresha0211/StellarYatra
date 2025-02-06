//namespace AstroSafar.Services
//{
//    public class LaunchService
//    {

//        private readonly HttpClient _httpClient;

//        public LaunchService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        // Replace with your Spoonacular API key
//       // private const string ApiKey = "YOUR_SPOONACULAR_API_KEY";
//        private const string BaseUrl = "https://lldev.thespacedevs.com/2.3.0/launches?mode=detailed&limit=2";

//        // Method to get recipes based on ingredients
//        public async Task<string> GetRecipesByIngredientsAsync(string ingredients)
//        {
//            var url = $"{BaseUrl}recipes/findByIngredients?ingredients={ingredients}&apiKey={ApiKey}";
//            var response = await _httpClient.GetStringAsync(url);
//            return response;
//        }

//        // You can add more methods to fetch specific recipe details, nutritional info, etc.
//    }
//}
//}
