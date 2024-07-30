using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class ChatGetRequest
{
    public static readonly HttpClient client = new HttpClient();
    public const string url = "http://localhost:1234/v1/chat/completions";
    public const string apiKey = "Bearer no-key";

    private static async Task Main(string[] args)
    {
        int consoleWidth = Console.WindowWidth;

        // Centered Title
        string title = @$"                  .,,uod8B8bou,,.
              ..,uod8BBBBBBBBBBBBBBBBRPFT?l!i:.
         ,=m8BBBBBBBBBBBBBBBRPFT?!||||||||||||||
         !...:!TVBBBRPFT||||||||||!!^^""'   ||||
         !.......:!?|||||!!^^""'            ||||
         !.........||||                     ||||
         !.........||||  ## by Jose Valois  ||||
         !.........||||                     ||||
         !.........||||                     ||||
         !.........||||                     ||||
         !.........||||                     ||||
         `.........||||                    ,||||
          .;.......||||               _.-!!|||||
   .,uodWBBBBb.....||||       _.-!!|||||||||!:'
!YBBBBBBBBBBBBBBb..!|||:..-!!|||||||!iof68BBBBBb....
!..YBBBBBBBBBBBBBBb!!||||||||!iof68BBBBBBRPFT?!::   `.
!....YBBBBBBBBBBBBBBbaaitf68BBBBBBRPFT?!:::::::::     `.
!......YBBBBBBBBBBBBBBBBBBBRPFT?!::::::;:!^`;:::       `.
!........YBBBBBBBBBBRPFT?!::::::::::^''...::::::;         iBBbo.
`..........YBRPFT?!::::::::::::::::::::::::;iof68bo.      WBBBBbo.
  `..........:::::::::::::::::::::::;iof688888888888b.     `YBBBP^'
    `........::::::::::::::::;iof688888888888888888888b.     `
      `......:::::::::;iof688888888888888888888888888888b.
        `....:::;iof688888888888888888888888888888888899fT!
          `..::!8888888888888888888888888888888899fT|!^'
            `' !!988888888888888888888888899fT|!^'
                `!!8888888888888888899fT|!^'
                  `!988888888899fT|!^'
                    `!9899fT|!^'
                      `!^' ";
        string centeredTitle = title.PadLeft((consoleWidth + title.Length) / 2).PadRight(consoleWidth);
        Console.WriteLine(centeredTitle);




        Console.WriteLine("Welcome to your Team of Agents. Type 'exit' Mash CTRL + C to quit.");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("You: ");
            string userInput = Console.ReadLine();

            if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            // Send requests to both agents asynchronously
            var agent1Task = PrimaryAgent.GetChatResponse(userInput, "Agent1");
            var agent2Task = SecondaryAgent.GetChatResponse(userInput, "Agent2");

            // Wait for both agents to respond
            await Task.WhenAll(agent1Task, agent2Task);

            // Get responses
            string response_Agent_01 = await agent1Task;
            string response_Agent_02 = await agent2Task;

            // Display responses
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Agent1:");
            PrintWrappedText(response_Agent_01);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Agent2:");
            PrintWrappedText(response_Agent_02);

          

        }

        // Reset color before exiting
        Console.ResetColor();
    }





    public static string ParseResponse(string responseContent)
    {
        try
        {
            var jsonResponse = JObject.Parse(responseContent);
            return jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString() ?? "No response from server.";
        }
        catch (Exception ex)
        {
            return $"Error parsing response: {ex.Message}";
        }
    }

    private static void PrintWrappedText(string text)
    {
        int windowWidth = Console.WindowWidth;
        int currentIndex = 0;

        while (currentIndex < text.Length)
        {
            int lineLength = Math.Min(windowWidth, text.Length - currentIndex);
            string line = text.Substring(currentIndex, lineLength);

            // Find the last space in the line to wrap properly
            int lastSpaceIndex = line.LastIndexOf(' ');
            if (lastSpaceIndex > 0 && currentIndex + lineLength < text.Length)
            {
                lineLength = lastSpaceIndex + 1;
                line = text.Substring(currentIndex, lineLength).Trim();
            }

            Console.WriteLine(line);
            currentIndex += lineLength;
        }
    }
}
