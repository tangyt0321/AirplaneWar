using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.Panels.Start;
using Untrie;

namespace airplaneWar
{
    public partial class MainForm : Form
    {
        // 窗体基础配置
        private const int DesignWidth = 2560;  // 设计分辨率宽度
        private const int DesignHeight = 1440; // 设计分辨率高度

        // 容器控件（用于动态加载Panel）
        private readonly Panel _mainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Black
        };
        private PanelManager panelManager;
        //private GameManager GameManager = new GameManager();
        //private InputManager inputManager;
        private System.Windows.Forms.Timer gameTimer;

        public MainForm()
        {
            InitializeCustomComponents();
            InitializeCoreSystems();
            RegisterGlobalEvents();
            //ShowStartMenu();
        }

        #region 初始化阶段
        // 初始化自定义控件（替代Designer生成的代码）
        private void InitializeCustomComponents()
        {
            // 窗体基本设置
            this.Text = " v1.0";
            //this.ClientSize = new System.Drawing.Size(DesignWidth, DesignHeight);
            //this.FormBorderStyle = FormBorderStyle.None;//无边框
            this.WindowState = FormWindowState.Maximized;//最大化
            this.FormBorderStyle = FormBorderStyle.Sizable;//有边框
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;//有边框
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            Form1 form1 = new Form1();

            // 创建主容器
            this.Controls.Add(_mainContainer);
            _mainContainer.BackColor = Color.Black;

            gameTimer = new System.Windows.Forms.Timer();

            // 窗口尺寸变化事件
            //this.Resize += (s, e) => ScreenManager.HandleResolutionChange();
        }

        // 初始化核心系统
        private void InitializeCoreSystems()
        {
            // 输入系统
            //InputManager.Instance.Initialize(this);

            // 资源加载
            //ResourceLoader.LoadTextures();
            //ResourceLoader.LoadSounds();

            // 游戏配置
            //GameConfig.Load("config.xml");

            // 初始化管理器
            panelManager = new PanelManager(_mainContainer);




            //AudioManager.Initialize();
        }

        // 注册全局事件
        private void RegisterGlobalEvents()
        {
            // 开始游戏事件
            //EventSystem.Register(EventType.StartGame, () =>
            //{
            //    PanelManager.SwitchTo<GamePanel>();
            //    GameManager.StartNewGame();
            //});
            panelManager.NavigateTo<startPanel>();

            // 返回主菜单事件
            //EventSystem.Register(EventType.ReturnToMenu, () =>
            //{
            //    PanelManager.SwitchTo<StartMenuPanel>();
            //    GameManager.StopGame();
            //});

            // 退出游戏事件
            //EventSystem.Register(EventType.QuitGame, () =>
            //{
            //    this.Close();
            //});
        }

        #endregion

        #region 界面管理
        // 显示开始菜单
        private void ShowStartMenu()
        {
            // 播放背景音乐
            //AudioManager.PlayBGM("bgm_menu", loop: true);
        }
        #endregion

        //#region 窗体生命周期
        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    // 保存游戏设置
        //    GameConfig.Save();

        //    // 清理资源
        //    ResourceLoader.ReleaseTextures();
        //    AudioManager.Release();

        //    base.OnFormClosing(e);
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    // 全局快捷键处理
        //    switch (e.KeyCode)
        //    {
        //        case Keys.F1:
        //            DebugConsole.Toggle();
        //            break;
        //        case Keys.F12:
        //            ScreenManager.ToggleFullscreen(this);
        //            break;
        //    }

        //    base.OnKeyDown(e);
        //}
        //#endregion
    }
}
