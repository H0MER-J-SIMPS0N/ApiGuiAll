using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;
using System.Net.Http;

namespace ApiGuiAll.Models
{
    public class GetToken
    {
        private static Token token;

        private static Dictionary<Setting, Token> Tokens = new Dictionary<Setting, Token>();
        private static object syncRoot = new object();
        public static Token Get(Setting settings)
        {
            if (settings is null)
            {
                token = null;
                return token;
            }
            if (Tokens.ContainsKey(settings))
            {
                if (Tokens[settings] is null)
                {
                    lock (syncRoot)
                    {
                        if (Tokens[settings] is null)
                        {
                            Tokens[settings] = new Token(settings);
                        }
                        else if (Tokens[settings].EndTime < DateTime.Now)
                        {
                            Tokens[settings] = new Token(settings);
                        }
                    }
                }
                else if (Tokens[settings].StatusCode == "exception")
                {
                    lock (syncRoot)
                    {
                        Tokens[settings] = new Token(settings);
                    }
                }
                else if (Tokens[settings].EndTime < DateTime.Now)
                {
                    lock (syncRoot)
                    {
                        Tokens[settings] = new Token(settings);
                    }
                }
            }
            else
            {
                lock (syncRoot)
                {
                    Tokens.Add(settings, new Token(settings));
                }
            }
            return Tokens[settings];
        }
    }

    public class Token
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string Error { get; private set; }
        public string StatusCode { get; private set; }
        public string Value { get; set; }
        public string Duration { get; private set; }
        public DateTime BeginTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public Token(Setting settings)
        {
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                FormUrlEncodedContent content = new FormUrlEncodedContent(settings.Data);
                using (var response = GetHttpClient.Get().PostAsync(settings.TokenAddress, content).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string answer = response.Content.ReadAsStringAsync().Result;
                        logger.Info("TOKEN: " + answer);
                        TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(answer);
                        Value = $"{tokenResponse.TokenType} {tokenResponse.AccessToken}";
                        BeginTime = DateTime.Now;
                        EndTime = BeginTime.AddMinutes(tokenResponse.ExpiresIn / 60 - 1);
                        Duration = BeginTime.ToString() + " - " + EndTime.ToString();
                        StatusCode = response.StatusCode.ToString();
                    }
                    else
                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                }
            }
            catch (Exception exc)
            {
                logger.Error("TOKENERROR: " + exc.ToString());
                Value = $"No {settings.Zone} token!!!";
                Error = exc.ToString();
                StatusCode = "exception";
            }
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}