Here is a simple implementation of a .NET console application that reads a text file, breaks it down into chunks, and identifies the end of a sentence:

**ChunkingTextReader.cs**
```csharp
using System;
using System.IO;
using System.Text.RegularExpressions;

public class ChunkingTextReader
{
    public static void Main(string[] args)
    {
        string filePath = "example.txt";
        string chunkSize = "50"; // chunk size in characters

        ReadTextFile(filePath, int.Parse(chunkSize));
    }

    private static void ReadTextFile(string filePath, int chunkSize)
    {
        string text = File.ReadAllText(filePath);

        // Identify end-of-sentence characters
        string eolChars = "?.!";

        // Create a regex pattern to match sentence boundaries
        string sentenceBoundaryPattern = @"\s([" + eolChars + "])\s";

        // Find all sentence boundaries in the text
        MatchCollection sentenceBoundaries = Regex.Matches(text, sentenceBoundaryPattern);

        // Iterate over the sentence boundaries and print the corresponding chunks
        int chunkIndex = 0;
        for (int i = 0; i < sentenceBoundaries.Count; i++)
        {
            int start = sentenceBoundaries[i].Index;
            int end = start + chunkSize > text.Length ? text.Length : start + chunkSize;

            Console.WriteLine($"Chunk {chunkIndex}: {text.Substring(start, end - start)}");
            chunkIndex++;

            // Check if the current chunk is the last one before the end of the sentence
            if (i == sentenceBoundaries.Count - 1 || sentenceBoundaries[i + 1].Index > end)
            {
                Console.WriteLine($"End of sentence reached.");
                break;
            }
        }
    }
}
```
The DAG of tasks for this AI agent is:

**DAG**

* **Read text file** (`File.ReadAllText(filePath)`)
	+ **Identify end-of-sentence characters** (`string eolChars = "?.!"`)
	+ **Create regex pattern** (`string sentenceBoundaryPattern = @"\s([" + eolChars + "])\s"`)
* **Find sentence boundaries** (`Regex.Matches(text, sentenceBoundaryPattern)`)
	+ **Iterate over sentence boundaries** (loop over `MatchCollection sentenceBoundaries`)
		- **Get chunk** (`text.Substring(start, end - start)`), where `start` and `end` are determined by the current sentence boundary and chunk size
		- **Check if current chunk is last one before sentence end** (`if (i == sentenceBoundaries.Count - 1 || sentenceBoundaries[i + 1].Index > end)`)

**How a single AI agent should best approach the problem:**

1. **Read the text file** and store its content in memory.
2. **Identify end-of-sentence characters** (e.g., `?.!`) and create a regex pattern to match sentence boundaries.
3. **Find sentence boundaries** in the text using the regex pattern.
4. **Iterate over sentence boundaries** and, for each boundary, determine the corresponding chunk of text by substringning the text with the current sentence boundary as the start index and the chunk size as the end index.
5. **Check if the current chunk is the last one before the end of the sentence** by checking if the next sentence boundary is beyond the end of the current chunk or if it's the last boundary in the collection.
6. **Output the chunk(s)**, including the end of sentence marker when reached.

Note that this AI agent assumes a simple text structure, where sentences are separated by spaces and end with `?.!`. More complex text structures (e.g., paragraphs, sections, or more complex punctuation) may require additional processing and handling.