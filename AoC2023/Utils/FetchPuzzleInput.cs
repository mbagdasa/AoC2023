using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023.Utils
{
    internal static class FetchPuzzleInput
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static string GetPuzzleInputFromWebsite(int day)
        {
            try
            {
                
                var url = $@"https://adventofcode.com/2023/day/{day}/input";
                var uri = new Uri(url);
                var cookies = new CookieContainer();
                cookies.Add(uri, new System.Net.Cookie("session", Environment.GetEnvironmentVariable("AOC_SESSION", EnvironmentVariableTarget.User)));
                
                using var handler = new HttpClientHandler() { CookieContainer = cookies };
                using var client = new HttpClient(handler) { BaseAddress = uri };

                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public static List<string> GetPuzzleAsList(int day)
        {
            string websiteText = GetPuzzleInputFromWebsite(day);
            //websiteText = "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)";
            if (websiteText != null)
            {
                // Split the text into a list using LINQ
                List<string> textList = websiteText.Split('\n')
                                   .Select(line => line.Trim())
                                   .Where(line => !string.IsNullOrEmpty(line))
                                   .ToList();
                return textList;
            }
            else
            {
                // Handle case where websiteText is null (request failed)
                // Return an empty list or handle error as appropriate
                return new List<string>();
            }
        }
    }
}
