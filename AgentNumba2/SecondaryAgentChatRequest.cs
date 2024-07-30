using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;




class SecondaryAgent
{



    public static async Task<string> GetChatResponse(string userInput, string agentName)
    {
        try
        {



            //Testing how to best give context to the LLM by reading and writing small tasks from this file.
            string filepath = @"C:\Users\Afro\Projects\LocalChatBot\LocalChatBot\TextFile2.txt";

            string filecontent = File.ReadAllText(filepath);

            var SystemContent = filecontent;

            //Create the POST that will be sent to the LLM server
            var payload = CreatePayload(userInput, agentName, SystemContent);
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, ChatGetRequest.url);
            request.Headers.Add("Authorization", ChatGetRequest.apiKey);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await ChatGetRequest.client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return ChatGetRequest.ParseResponse(responseContent);
        }
        catch (HttpRequestException httpEx)
        {
            return $"Request error: {httpEx.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }

    private static object CreatePayload(string userInput, string agentName, string Textfile1contents)
    {
        return new
        {
            model = "Meta Llama 3.1 8B Instruct",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"You are {agentName}, an AI assistant. Your top priority is following the instructions laid out for you. Here is a file containing context about your task {Textfile1contents}"
                },
                new
                {
                    role = "user",
                    content = userInput
                }
            }
        };
    }


}