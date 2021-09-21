﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class YeekitTranslator:ITranslator
    {
        private readonly string name = "Yeekit";
        public string Name { get => name; }

        public async Task<string> Translate(string sourceText)
        {
            string desLang = "nzh";
            string srcLang = "nja";

            string bodyString = $"content[]={HttpUtility.UrlEncode(sourceText)}&sourceLang={srcLang}&targetLang={desLang}";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            string url = @"https://www.yeekit.com/site/dotranslate";

            HttpClient client = CommonFunction.Client;

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = await response.Content.ReadAsStringAsync();

                responseBody = responseBody.Replace(@"\n", string.Empty).Replace(" ", string.Empty);

                Regex reg = new Regex(@",\\""text\\"":\\""(.*?)\\"",\\""");
                Match match = reg.Match(responseBody);

                string result = match.Groups[1].Value;
                //Unicode解码
                result = Regex.Unescape(result);
                return result;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }
        }

        public void TranslatorInit(string param1, string param2)
        {

        }
    }
}