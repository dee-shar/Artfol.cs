using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace ArfolApi
{
    public class Artfol
    {
        private string authToken;
        private string refreshToken;
        private readonly HttpClient httpClient;
        private readonly string apiUrl = "https://www.artfol.club/api";
        public Artfol()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("okhttp/4.9.2");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> Register(
            string username,
            string email,
            string password, 
            string birthDate = "2001-09-11",
            bool isArtist = false)
        {
            var data = JsonContent.Create(new
            {
                username = username,
                email = email,
                password = password,
                enabled_nsfw = false,
                intro = "",
                about = "",
                is_artist = isArtist,
                location = "",
                birthdate = birthDate,
                cake = 0,
                links = new[]
                {
                    new {
                        index = "0",
                        url = ""
                    }
                },
                check = true,
            });
            var response = await httpClient.PostAsync($"{apiUrl}/user/register", data);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Login(string email, string password)
        {
            var data = JsonContent.Create(new
            {
                email = email,
                loginwithemail = true,
                password = password
            });
            var response = await httpClient.PostAsync($"{apiUrl}/user/login", data);
            var content = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(content);
            if (document.RootElement.TryGetProperty("token", out var authTokenElement)) {
                authToken = authTokenElement.GetString();
                httpClient.DefaultRequestHeaders.Add("auth-token", authToken);
            }
            if (document.RootElement.TryGetProperty("refresh_token", out var refreshTokenElement)) {
                refreshToken = refreshTokenElement.GetString();
                httpClient.DefaultRequestHeaders.Add("refresh-token", authToken);
            }
            return content;
        }

        public async Task<string> GetCommunities()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/get/v2/community");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAnnouncements()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/get/announcement");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNotifications()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/user/get/notifications");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetGroups()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/group/public");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPosts()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/v2/app/public/paginated_posts");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountInfo()
        {
            var response = await httpClient.GetAsync($"{apiUrl}/user/settings");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
