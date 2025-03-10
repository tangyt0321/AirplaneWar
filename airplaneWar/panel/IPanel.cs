using System;


namespace airplaneWar.Panels.Core
{
    public interface IPanel
    {
        void on_enter(object args);
        void on_exit();
        void on_render(Graphics e);
        void on_return(object data); // 返回时数据传递
        void on_update(double deltaTime);
    }
    public interface IPanelArgs { }

    public class GameArgs : IPanelArgs
    {
        public int Level { get; set; }
    }

    //// 启动游戏时
    //_panelManager.NavigateTo<GamePanel>(new GameArgs { Level = 5 });

    //// 返回时传递数据
    //_panelManager.GoBack(new { Score = 1000 });

    //public interface IPanelTransition
    //{
    //    Task EnterTransition(Control panel);
    //    Task LeaveTransition(Control panel);
    //}

    //// 滑动动画实现
    //public class SlideTransition : IPanelTransition
    //{
    //    public async Task EnterTransition(Control panel)
    //    {
    //        panel.Left = panel.Width;
    //        for (int i = 0; i < 20; i++)
    //        {
    //            panel.Left -= panel.Width / 20;
    //            await Task.Delay(10);
    //        }
    //    }
    //}
}
