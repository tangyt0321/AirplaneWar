using System;

using System.Collections.Generic;

namespace airplaneWar.Core.Manager.GameManager
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void Init()
        {
            Console.WriteLine("GameManager Init");
        }

        public void on_update(double delta)
        {
            Console.WriteLine("GameManager Update");
        }

        public void Destroy()
        {
            Console.WriteLine("GameManager Destroy");
        }
    }
}