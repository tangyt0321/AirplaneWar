using System;

namespace airplaneWar.Core.Manager
{
    public static class EventManager
    {

        public static long score { get; set; } = 0;
        public static bool is_debug = false;
        public static bool isGameOver = false;
        public static bool isGameStart = false;
        public static bool isGamePause = false;
    }
}
