using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Aegis.Snort_logs
{
    public class ReadSnortLogs
    {
       

        // Method to read and process the snort log file
        public string ProcessSnortLogs()
        {
            // TODO: this is hard coded needs to be dynamic
            string relativePath = Path.Combine("..", "..", "..", ".\\Snort_logs\\20240723_pcap_signatures.txt");
            string filePath = Path.GetFullPath(relativePath);

            if (!File.Exists(filePath))
            {
                return "File not found.";
            }

            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);

            // Process each line to extract the required part
            var processedLines = lines
                .Select(line => line.Split(']')[2])
                .Select(part => part.Split('[')[0])
                .Select(part => part.Split('"')[1])
                .ToList();

            // Count occurrences and sort
            var lineCounts = processedLines
                .GroupBy(line => line)
                .Select(group => new { Line = group.Key, Count = group.Count() })
                .OrderByDescending(x => x.Count);

            // Build the result string
            StringBuilder result = new StringBuilder();
            foreach (var entry in lineCounts)
            {
                result.AppendLine($"{entry.Count} {entry.Line}");
            }

            return result.ToString();
        }
    }
}
