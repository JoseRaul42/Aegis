using Aegis.Snort_logs;
using OpenAI.Threads;
using System.Text;



class PrimaryAgent
{



    public static async Task<string> GetChatResponse(string userInput, string agentName)
    {

       
        try
        {


            ReadSnortLogs snortLogs = new ReadSnortLogs();

            string filePath = snortLogs.ProcessSnortLogs();


            string filecontent = filePath;


            var SystemContent = filecontent;

            //Create the POST that will be sent to the LLM server
            var payload = CreatePayload(userInput, agentName, SystemContent);
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, ChatGetRequest.url);
            request.Headers.Add("Authorization", ChatGetRequest.apiKey);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await ChatGetRequest.client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();



            //TODO: THIS NEEDS TO BE CHANGED SINCE THE FILE STRUCTURE IS CALLED FROM THE BIN FOLDER WHEN TESTING NOT THE SAME FILE STRUCTURE AS THE REPO
            // Define the file path for writing instructions
            string relativePath = Path.Combine("..", "..", "..", "DAGInstructions.md");
            string Numba2instructionlocation = Path.GetFullPath(relativePath);

            // Parse the response and write the instructions to a file
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

    private static object CreatePayload(string userInput, string agentName, string Systemcontent)
    {
        return new
        {
            
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"You are {agentName}, an AI assistant. Create a SOC analyst threat report with all the information necessary for a second AI assistant to generate a SOC analyst threat report that answers the question the user has asked you to fufill. This is summarized and Parsed Snort Alert Log data from the raw file.This is the Context the user is referencing. {Systemcontent} "
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