//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.RegularExpressions;

//namespace Aegis.AgentNumba2
//{

//    public class MarkdownChunking
//    {
//        public static void Main(string[] args)
//        {



//            //TODO: THIS NEEDS TO BE CHANGED SINCE THE FILE STRUCTURE IS CALLED FROM THE BIN FOLDER WHEN TESTING NOT THE SAME FILE STRUCTURE AS THE REPO
//            string relativePath = Path.Combine( "..", "..", "..", "DAGInstructions.md");
//            string filePath = Path.GetFullPath(relativePath);
//            int chunkSize = 100; // chunk size in characters

//            ReadMarkdownFile(filePath, chunkSize);
//        }

//        private static void ReadMarkdownFile(string filePath, int chunkSize)
//        {
//            string text = File.ReadAllText(filePath);

//            List<string> chunks = new List<string>();
//            StringBuilder currentChunk = new StringBuilder();
//            bool inCodeBlock = false;

//            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

//            foreach (var line in lines)
//            {
//                if (line.StartsWith("```"))
//                {
//                    if (inCodeBlock)
//                    {
//                        // Closing a code block
//                        currentChunk.Append(line).Append(Environment.NewLine);
//                        chunks.Add(currentChunk.ToString().Trim());
//                        currentChunk.Clear();
//                        inCodeBlock = false;
//                    }
//                    else
//                    {
//                        // Opening a code block
//                        if (currentChunk.Length > 0)
//                        {
//                            chunks.Add(currentChunk.ToString().Trim());
//                            currentChunk.Clear();
//                        }
//                        currentChunk.Append(line).Append(Environment.NewLine);
//                        inCodeBlock = true;
//                    }
//                    continue;
//                }

//                if (inCodeBlock)
//                {
//                    currentChunk.Append(line).Append(Environment.NewLine);
//                }
//                else
//                {
//                    string[] sentences = Regex.Split(line, @"(?<=[.!?])\s+");

//                    foreach (var sentence in sentences)
//                    {
//                        if (currentChunk.Length + sentence.Length > chunkSize && currentChunk.Length > 0)
//                        {
//                            chunks.Add(currentChunk.ToString().Trim());
//                            currentChunk.Clear();
//                        }

//                        currentChunk.Append(sentence).Append(" ");
//                    }
//                }
//            }

//            if (currentChunk.Length > 0)
//            {
//                chunks.Add(currentChunk.ToString().Trim());
//            }

//            // Print each chunk
//            for (int i = 0; i < chunks.Count; i++)
//            {
//                Console.WriteLine($"Chunk {i + 1}:\n{chunks[i]}\n");
//            }
//        }
//    }

//}
