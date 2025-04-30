using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ShortestPathFinder.MapRouting.Utilities
{
   

    public static class TimerHandler
    {
        private static Stopwatch timerForTheAllProgram = new Stopwatch();
        private static Stopwatch timerForLogicCode = new Stopwatch();
        
        public static void StartTotalProgramTimer()
        {
            timerForTheAllProgram.Start();
        }

        public static void EndTotalProgramTimer()
        {
            timerForTheAllProgram.Stop();
        }

        public static void StartLogicTimer()
        {
            timerForLogicCode.Start();
        }

        public static void EndLogicTimer()
        {
            timerForLogicCode.Stop();
        }
       public static long GetTotalTimeInMilliseconds() => timerForTheAllProgram.ElapsedMilliseconds;
       public static long GetTotalLogicTimeInMilliseconds() => timerForLogicCode.ElapsedMilliseconds;



    }

}
