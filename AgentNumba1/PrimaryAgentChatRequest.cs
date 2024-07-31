using System.Text;



class PrimaryAgent
{



    public static async Task<string> GetChatResponse(string userInput, string agentName)
    {
        try
        {



            //Testing how to best give context to the LLM by reading and writing small tasks from this file.
            string filepath = @""; //for test

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



            string Numba2instructionlocation = @"..\TextFile2.txt";

            string Numba2instructions = ChatGetRequest.ParseResponse(responseContent);

            File.WriteAllText(Numba2instructionlocation, Numba2instructions);

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
                    content = $"You are {agentName}, an AI assistant. Create a DAG of the task the user has asked you to fufill and at the end of the DAG. Break down how a single AI agent should best approach the problem to complete the task following the DAG."
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