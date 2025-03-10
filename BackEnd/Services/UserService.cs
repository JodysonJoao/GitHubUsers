using BackEnd.Models;
using System.Text.Json;

namespace BackEnd.Services
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
            try
            {
                var userUrl = $"https://api.github.com/users/{username}";
                var userResponse = await GetApiResponseAsync(userUrl);

                if (userResponse == null)
                {
                    return null;
                }

                var user = JsonSerializer.Deserialize<User>(userResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var reposUrl = $"https://api.github.com/users/{username}/repos";
                var reposResponse = await GetApiResponseAsync(reposUrl);

                if (reposResponse == null)
                {
                    return null;
                }

                var repos = JsonSerializer.Deserialize<List<UserRepository>>(reposResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var totalLanguages = new Dictionary<string, int>();

                foreach (var repo in repos)
                {
                    var languagesUrl = $"https://api.github.com/repos/{username}/{repo.Name}/languages";
                    var languagesResponse = await GetApiResponseAsync(languagesUrl);

                    if (languagesResponse != null)
                    {
                        var languages = JsonSerializer.Deserialize<Dictionary<string, int>>(languagesResponse);
                        repo.Languages = languages;

                        foreach (var lang in languages)
                        {
                            if (totalLanguages.ContainsKey(lang.Key))
                            {
                                totalLanguages[lang.Key] += lang.Value;
                            }
                            else
                            {
                                totalLanguages[lang.Key] = lang.Value;
                            }
                        }
                    }
                }

                string mostUsedLanguage = totalLanguages
                    .OrderByDescending(l => l.Value)
                    .Select(l => l.Key)
                    .FirstOrDefault();

                user.Repositories = repos;
                user.LanguagesSom = totalLanguages;
                user.MostUsedLanguage = mostUsedLanguage;

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter dados do GitHub: {ex.Message}");
                return null;
            }
        }




        private async Task<string> GetApiResponseAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine($"Erro ao acessar a API: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
                return null;
            }
        }
    }
}
