using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Utilities
{
    public  class ResultWriter
    {

        public static void WriteResultsAndTiming(
            List<( string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)> results,
            
            double logicTime,
            string outputFilePath)

        {
            try
            {
                string directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                StringBuilder sb = new StringBuilder();
                foreach (var result in results)
                {
                    sb.AppendLine($"{result.path}");
                    sb.AppendLine($"{Math.Round(result.shortestTime, 2).ToString("F2")} mins");
                    sb.AppendLine($"{Math.Round(result.pathLength, 2).ToString("F2")} km");
                    sb.AppendLine($"{Math.Round(result.walkingDistance, 2).ToString("F2")} km");
                    sb.AppendLine($"{Math.Round(result.roadsLength, 2).ToString("F2")} km");
                    sb.AppendLine();
                }

                sb.AppendLine($"{Math.Round(logicTime, 2)} ms");
                sb.AppendLine();
     
                File.WriteAllText(outputFilePath, sb.ToString());
                TimerHandler.EndTotalProgramTimer();
                double totalTime = TimerHandler.GetTotalTimeInMilliseconds();
                sb.AppendLine($"{Math.Round(totalTime, 2)} ms");
                File.WriteAllText(outputFilePath, sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }


    }

}



