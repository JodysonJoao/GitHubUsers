using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GitHubUsers.Server.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHubUsersApp");
        }

        public async Task<User> GetGitHubUser(string username)
        {
            var userUrl = $"https://api.github.com/users/{username}";
            var userResponse = await _httpClient.GetStringAsync(userUrl);
            var user = JsonSerializer.Deserialize<User>(userResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var reposUrl = $"https://api.github.com/users/{username}/repos";
            var reposResponse = await _httpClient.GetStringAsync(reposUrl);
            var repos = JsonSerializer.Deserialize<List<UserRepository>>(reposResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            user.Repositories = repos;

            return user;

        }

        public class User
        {
            public string Name { get; set; }

            [JsonPropertyName("avatar_url")]
            public string AvatarUrl { get; set; }
            public List<UserRepository> Repositories { get; set; }
        }

        public class UserRepository
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
