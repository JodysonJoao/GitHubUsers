﻿using System.Text.Json.Serialization;

namespace BackEnd.Models
{
    public class GitHubUserRequest
    {
        public string Username { get; set; }
    }

    public class User
    {
        public string Name { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }

        public List<UserRepository> Repositories { get; set; }
    }

    public class UserRepository
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
