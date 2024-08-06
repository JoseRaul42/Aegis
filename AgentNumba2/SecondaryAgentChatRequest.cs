using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MilvusDatabase;




class SecondaryAgent
{



    public static async Task<string> GetChatResponse(string userInput, string agentName)
    {
        try
        {

            var SystemContent = await PrimaryAgent.GetChatResponse(userInput, "Agent1"); ;

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

        string Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        return new
        {
           
            messages = new[]
            {
                new
                {
                    role = "system",   //TODO: Edit prompt to include todays date to give context to an agent. and more details on how to respond to questions about logs
                    content = $"You are {agentName}. Today's date is {Datetime} Use these instructions to generate a threat report. {Textfile1contents}." 
                }
            
            }
        };
    }


}