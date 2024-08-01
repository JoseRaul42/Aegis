using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatBot.EmbeddingsAgent
{
    internal class EmbeddingsAgent
    {



        public static string EmbeddingsParseResponse(string responseContent)
        {
            try
            {
                var jsonResponse = JObject.Parse(responseContent);

                // Check for errors in the response
                if (jsonResponse["error"] != null)
                {
                    return $"Error from server: {jsonResponse["error"]["message"]?.ToString()}";
                }

                // Assuming the embeddings response contains 'embedding' key
                var embeddings = jsonResponse["data"]?.ToString();

                return embeddings ?? "No embeddings found in the response.";
            }
            catch (Exception ex)
            {
                // Log the raw response for debugging
                Console.WriteLine("Raw response content:");
                Console.WriteLine(responseContent);
                return $"Error parsing response: {ex.Message}";
            }
        }



        public static async Task<string> GetChatResponse(string userInput, string agentName)
        {
            try
            {



                //Testing how to best give context to the LLM by reading and writing small tasks from this file.
                string filepath = @"C:\Users\Afro\Projects\LocalChatBot\LocalChatBot\DAGInstructions.txt";

                string filecontent = File.ReadAllText(filepath);

                var SystemContent = filecontent;

                //Create the POST that will be sent to the LLM server
                var payload = CreatePayload(userInput, agentName, SystemContent);
                string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                var request = new HttpRequestMessage(HttpMethod.Post, ChatGetRequest.embeddingsurl);
                request.Headers.Add("Authorization", ChatGetRequest.apiKey);
                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ChatGetRequest.client.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                //Console.WriteLine(filepath);
                //Console.WriteLine(SystemContent);
                Console.WriteLine(EmbeddingsParseResponse(responseContent));



                return EmbeddingsParseResponse(responseContent);
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




        private static object CreatePayload(string userInput, string agentName, string textfileContents)
        {
            return new
            {
                model = "Meta Llama 3.1 8B Instruct",
                prompt = textfileContents
            };
        }





    }
}
