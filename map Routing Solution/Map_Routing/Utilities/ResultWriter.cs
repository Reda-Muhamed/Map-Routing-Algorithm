using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathFinder.MapRouting.Utilities
{
    public  class ResultWriter
    {
        public static void appendQueryResultInTheFile(string pathNodes,double shortestTime, double totalDistance,
                                                        double walkingDistance, double vehicleDistance)
        {
            try
            {
                Console.WriteLine($"Writing : \n"+ $"{pathNodes}\n" +
                                $"{Math.Round(shortestTime , 2)} mins\n" +
                                $"{Math.Round(totalDistance, 2)} km\n" +
                                $"{Math.Round(walkingDistance, 2)} km\n" +
                                $"{Math.Round(vehicleDistance, 2)} km\n" + "\n");


                File.AppendAllText(@".\myOutput\results.txt",
                                $"{pathNodes}\n" +
                                $"{Math.Round(shortestTime , 2)} mins\n" +
                                $"{Math.Round(totalDistance, 2)} km\n" +
                                $"{Math.Round(walkingDistance, 2)} km\n" +
                                $"{Math.Round(vehicleDistance, 2)} km\n" + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
            return;
        }public static void appendTimeInTheFile(long logicTime, long totalTime)
        {
            try
            {
                Console.WriteLine($"Writing : \n" +
                                    $"{logicTime} ms\n\n" +
                                    $"{totalTime} ms\n\n");
                File.AppendAllText(@".\myOutput\results.txt",
                                    $"{logicTime} ms\n\n" +
                                    $"{totalTime} ms\n\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
            return;
        }

    
    }

}
