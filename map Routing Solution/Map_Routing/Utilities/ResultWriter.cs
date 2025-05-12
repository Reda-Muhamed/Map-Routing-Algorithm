using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Utilities
{
    public  class ResultWriter
    {


        // use index to make the order of the output file same as the order of the queries in the input file
        public static void WriteResultsAndTiming(
            List<(int index, string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength)> results,
            double logicTime,
            double totalTime,
            string outputFilePath)

        {
            try
            {
                string directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                int count = 0;
                StringBuilder sb = new StringBuilder();
                foreach (var result in results.OrderBy(r => r.index))
                {
                    sb.AppendLine($"{result.path}");
                    sb.AppendLine($"{Math.Round(result.shortestTime, 2).ToString("F2")} mins");
                    sb.AppendLine($"{Math.Round(result.pathLength, 2).ToString("F2")} km");
                    sb.AppendLine($"{Math.Round(result.walkingDistance, 2).ToString("F2")} km");
                    sb.AppendLine($"{Math.Round(result.roadsLength, 2).ToString("F2")} km");
                    sb.AppendLine();
                    Console.WriteLine($"{result.path}\n" + $"{Math.Round(result.shortestTime, 2).ToString("F2")} mins\n" +
                        $"{Math.Round(result.pathLength, 2).ToString("F2")} km\n" +
                        $"{Math.Round(result.walkingDistance, 2).ToString("F2")} km\n" +
                        $"{Math.Round(result.roadsLength, 2).ToString("F2")} km\n\n");
                    count++;

                }

                sb.AppendLine($"{Math.Round(logicTime, 2)} ms");
                sb.AppendLine();
                sb.AppendLine($"{Math.Round(totalTime, 2)} ms");
                sb.AppendLine();
                Console.WriteLine($"{Math.Round(logicTime, 2)} ms\n\n" + $"{Math.Round(totalTime, 2)} ms\n\n" );

                Console.WriteLine($"######### Total number of queries: {count} #######################");
                File.WriteAllText(outputFilePath, sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        public static void WriteResultsAndTimingTesting(
            string path, double shortestTime, double pathLength, double walkingDistance, double roadsLength
           )

        {
            
                  
                    Console.WriteLine($"{path}\n" + $"{Math.Round(shortestTime, 2).ToString("F2")} mins\n" +
                        $"{Math.Round(pathLength, 2).ToString("F2")} km\n" +
                        $"{Math.Round(walkingDistance, 2).ToString("F2")} km\n" +
                        $"{Math.Round(roadsLength, 2).ToString("F2")} km\n\n");
               
            
        }



    }

}



