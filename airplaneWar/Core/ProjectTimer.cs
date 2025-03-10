using System;

namespace airplaneWar.GameLogic.Core
{
    public class ProjectTimer
    {
        private double pass_time = 0;//经过的时间//
        private double wait_time = 0;
        private bool paused = false;
        private bool shotted = false;   //是否触发//
        private bool one_shot = false;  //单次触发//
        private Action on_timeout; //触发回调//
        public ProjectTimer()
        {

        }
        public void restart()
        {
            pass_time = 0;
            shotted = false;
        }

        public void set_wait_time(double val)
        {
            wait_time = val;
        }

        public void set_one_shot(bool flag)
        {
            one_shot = flag;
        }

        public void set_on_timeout(Action on_timeout)
        {
            this.on_timeout = on_timeout;
        }

        public void pause()
        {
            paused = true;
        }

        public void resume()
        {
            paused = false;
        }

        public void on_update(double delta)
        {
            if (paused)
                return;

            pass_time += delta;
            if (pass_time >= wait_time)
            {
                bool can_shot = (!one_shot || (one_shot && !shotted));//单次触发//
                shotted = true;
                if (can_shot && on_timeout != null)
                    on_timeout();
                pass_time -= wait_time;
            }
        }

    }
}