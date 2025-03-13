using System;
using System.Diagnostics;
using airplaneWar.Panels.Core;
using airplaneWar.Panels.Game;




namespace airplaneWar.Core.Manager.PanelManager
{
    //public class panel_manager
    //{
    //    public enum PanelType
    //    {
    //        Start,
    //        Game,
    //        Result
    //    }

    //    private Panel current_panel = new();//当前面板
    //    private startPanel startPanel = new startPanel();
    //    private gamePanel gamePanel = new gamePanel();
    //    private resultPanel resultPanel = new resultPanel();

    //    //单例模式
    //    private static panel_manager instance;
    //    public static panel_manager Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                instance = new panel_manager();
    //            }
    //            return instance;
    //        }
    //    }

    //    public void set_current_panel(PanelType panel_type)
    //    {
    //        switch (panel_type)
    //        {
    //            case PanelType.Start:
    //                current_panel = startPanel;
    //                break;
    //            case PanelType.Game:
    //                current_panel = gamePanel;
    //                break;
    //            case PanelType.Result:
    //                current_panel = resultPanel;
    //                break;
    //        }
    //        current_panel.on_enter();
    //    }

    //    public void switch_panel(PanelType panel_type)
    //    {
    //        current_panel.ExitPanel();
    //        switch (panel_type)
    //        {
    //            case PanelType.Start:
    //                current_panel = startPanel;
    //                break;
    //            case PanelType.Game:
    //                current_panel = gamePanel;
    //                break;
    //            case PanelType.Result:
    //                current_panel = resultPanel;
    //                break;
    //        }
    //        current_panel.ShowPanel();
    //    }

    //    public void update_panel()
    //    {
    //        current_panel.UpdatePanel();
    //    }

    //    public void paint_panel(PaintEventArgs e)
    //    {
    //        current_panel.PaintPanel(e);
    //    }

    //    public void exit_panel()
    //    {
    //        current_panel.ExitPanel();
    //    }

    //    public void MainForm_msg(object sender, EventArgs e)
    //    {
    //        current_panel.MainForm_Msg(sender, e);
    //    }
    //}

    public class PanelManager
    {
        private readonly Control _container;
        private readonly Stack<IPanel> _panelStack = new();

        private Stopwatch gameStopwatch;
        private double accumulatedTime = 0;
        private const int FPS = 120;
        private double targetFramTime = 1000 / FPS;


        private bool isRunning = true;

        public static PanelManager Instance { get; private set; }
        public PanelManager(Control container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            Instance = this;
            gameStopwatch = Stopwatch.StartNew();
            Task.Run(() => GameLoop());
        }
        enum PanelType
        {
            Start,
            Game,
            Result
        }
        // 导航到新Panel
        public void NavigateTo<T>(IPanelArgs args = null) where T : UserControl, IPanel
        {
            if (_container.InvokeRequired)
            {
                _container.Invoke(new Action(() => NavigateTo<T>(args)));
                return;
            }

            var current = _panelStack.Count > 0 ? _panelStack.Peek() : null;
            //current?.on_exit();

            var newPanel = PanelFactory.Create<T>();
            newPanel.Dock = DockStyle.Fill;
            _container.Controls.Clear();
            _container.Controls.Add(newPanel);
            _panelStack.Push(newPanel);
            newPanel.on_enter(args);
        }

        // 返回上一级
        public bool GoBack(object returnData = null)
        {
            if (_panelStack.Count <= 1) return false;

            var current = _panelStack.Pop();
            current.on_exit();

            var prevPanel = _panelStack.Peek();
            _container.Controls.Clear();
            _container.Controls.Add(prevPanel as Control);
            prevPanel.on_return(returnData);

            return true;
        }

        public void Clear()
        {
            _panelStack.Clear();
            _container.Controls.Clear();
        }

        public void GameLoop()
        {
            double previousTime = gameStopwatch.Elapsed.TotalMilliseconds;
            while (isRunning)
            {
                double currentTime = gameStopwatch.Elapsed.TotalMilliseconds;
                double delTime = currentTime - previousTime;
                previousTime = currentTime;
                accumulatedTime += delTime;

                //执行固定步长的逻辑更新
                while (accumulatedTime >= targetFramTime)
                {
                    on_update(targetFramTime);
                    accumulatedTime -= targetFramTime;
                }

                int sleepTime = (int)Math.Max(0, targetFramTime - accumulatedTime);
                Thread.Sleep(sleepTime);

            }
        }

        private void on_update(double deltaTime)
        {
            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().on_update(deltaTime);
            }
        }

    }

    // Panel 工厂
    public static class PanelFactory
    {
        public static T Create<T>() where T : UserControl, IPanel
        {
            return Activator.CreateInstance<T>();
        }
    }

}
