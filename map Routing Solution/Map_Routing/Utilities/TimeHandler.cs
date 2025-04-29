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

        public static void StartIO()
        {
            timerForTheAllProgram.Start();
        }

        public static void EndIO()
        {
            timerForTheAllProgram.Stop();
        }

        public static void StartCore()
        {
            timerForLogicCode.Start();
        }

        public static void EndCore()
        {
            timerForLogicCode.Stop();
        }

       
    }

}
