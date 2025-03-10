using airplaneWar.Core.Manager.PanelManager;
using airplaneWar.Panels.Start;
using Untrie;

namespace airplaneWar
{
    public partial class MainForm : Form
    {
        // �����������
        private const int DesignWidth = 2560;  // ��Ʒֱ��ʿ��
        private const int DesignHeight = 1440; // ��Ʒֱ��ʸ߶�

        // �����ؼ������ڶ�̬����Panel��
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

        #region ��ʼ���׶�
        // ��ʼ���Զ���ؼ������Designer���ɵĴ��룩
        private void InitializeCustomComponents()
        {
            // �����������
            this.Text = " v1.0";
            //this.ClientSize = new System.Drawing.Size(DesignWidth, DesignHeight);
            //this.FormBorderStyle = FormBorderStyle.None;//�ޱ߿�
            this.WindowState = FormWindowState.Maximized;//���
            this.FormBorderStyle = FormBorderStyle.Sizable;//�б߿�
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;//�б߿�
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            Form1 form1 = new Form1();

            // ����������
            this.Controls.Add(_mainContainer);
            _mainContainer.BackColor = Color.Black;

            gameTimer = new System.Windows.Forms.Timer();

            // ���ڳߴ�仯�¼�
            //this.Resize += (s, e) => ScreenManager.HandleResolutionChange();
        }

        // ��ʼ������ϵͳ
        private void InitializeCoreSystems()
        {
            // ����ϵͳ
            //InputManager.Instance.Initialize(this);

            // ��Դ����
            //ResourceLoader.LoadTextures();
            //ResourceLoader.LoadSounds();

            // ��Ϸ����
            //GameConfig.Load("config.xml");

            // ��ʼ��������
            panelManager = new PanelManager(_mainContainer);




            //AudioManager.Initialize();
        }

        // ע��ȫ���¼�
        private void RegisterGlobalEvents()
        {
            // ��ʼ��Ϸ�¼�
            //EventSystem.Register(EventType.StartGame, () =>
            //{
            //    PanelManager.SwitchTo<GamePanel>();
            //    GameManager.StartNewGame();
            //});
            panelManager.NavigateTo<startPanel>();

            // �������˵��¼�
            //EventSystem.Register(EventType.ReturnToMenu, () =>
            //{
            //    PanelManager.SwitchTo<StartMenuPanel>();
            //    GameManager.StopGame();
            //});

            // �˳���Ϸ�¼�
            //EventSystem.Register(EventType.QuitGame, () =>
            //{
            //    this.Close();
            //});
        }

        #endregion

        #region �������
        // ��ʾ��ʼ�˵�
        private void ShowStartMenu()
        {
            // ���ű�������
            //AudioManager.PlayBGM("bgm_menu", loop: true);
        }
        #endregion

        //#region ������������
        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    // ������Ϸ����
        //    GameConfig.Save();

        //    // ������Դ
        //    ResourceLoader.ReleaseTextures();
        //    AudioManager.Release();

        //    base.OnFormClosing(e);
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    // ȫ�ֿ�ݼ�����
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
