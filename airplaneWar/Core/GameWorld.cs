// GameWorld.cs (单例类)
//using airplaneWar.Core.Manager.CollisionManager;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.GameLogic.Entities.Bullet.Core;
using airplaneWar.GameLogic.Entities.Bullet;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.GameLogic.Entities.Enemies;
using airplaneWar.GameLogic.Entities.Player;
using airplaneWar.Panels.Game;
using System.Numerics;
using DocumentFormat.OpenXml.Drawing;
using System.Drawing.Drawing2D;
using airplaneWar.Core.Manager;

namespace airplaneWar.GameLogic.Core
{
    public sealed class GameWorld
    {
        private static GameWorld _Instance;
        public static GameWorld Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameWorld();
                }
                return _Instance;
            }
        }
        public Player Player { get; set; } = new Player();
        public List<IEnemy> ActiveEnemies { get; } = new List<IEnemy>();//敌人
        public List<IBullet> ActiveBullets { get; } = new List<IBullet>();//子弹
        public RectangleF ClientRectangle { get; private set; }
        public ProjectTimer BulletSpawnTimer = new();
        public BulletPool bulletPool = new();
        public ProjectTimer EnemySpawnTimer = new();
        public EnemyPool enemyPool = new();

        public void InitializeGameWorld()
        {
            _Instance = this;
            // 初始化子弹池
            // 初始化敌人池
            ClientRectangle = new RectangleF(0, 0, 2230, 1400);//切换为MainForm的屏幕大小
            //ClientRectangle = new RectangleF(0, 0, 1130, 1100);//切换为MainForm的屏幕大小

            // 创建玩家实例（应在游戏世界初始化时创建）

            GameWorld.Instance.Player = new Player();
            var playerStartPos = new Vector2(ClientRectangle.Width / 2 - Player.Size.Width / 2, ClientRectangle.Height / 2 - Player.Size.Height / 2);
            GameWorld.Instance.Player.Position = playerStartPos;

            // 初始化子弹生成器
            BulletSpawnTimer = new ProjectTimer();
            BulletSpawnTimer.set_wait_time(100);
            BulletSpawnTimer.set_on_timeout(() =>
            {
                GameWorld.Instance.Shooting(InputManager.Instance.MousePosition, BulletType.Normal);
            });
            //初始化敌人生成器
            EnemySpawnTimer = new ProjectTimer();
            EnemySpawnTimer.set_wait_time(1000);
            EnemySpawnTimer.set_on_timeout(() =>
            {
                GameWorld.Instance.SpawnEnemy();
            });
        }

        public void on_update(double deltaTime)
        {
            // 更新敌人位置
            for (int i = GameWorld.Instance.ActiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = GameWorld.Instance.ActiveEnemies[i];
                enemy.on_update(Player.Position, deltaTime);
                if (!enemy.IsAlive)
                {
                    GameWorld.Instance.ActiveEnemies.RemoveAt(i);
                }
            }
            // 更新子弹位置
            for (int i = GameWorld.Instance.ActiveBullets.Count - 1; i >= 0; i--)
            {
                var bullet = GameWorld.Instance.ActiveBullets[i];

                bullet.on_update(deltaTime);
                if (!bullet.IsActive)
                {
                    GameWorld.Instance.ActiveBullets.RemoveAt(i);
                }
            }
            GameWorld.Instance.Player.on_move(deltaTime);
            // 更新时间
            BulletSpawnTimer.on_update(deltaTime);
            EnemySpawnTimer.on_update(deltaTime);
        }

        // Add a Font field to the GameWorld class
        private static readonly Font DefaultFont = new Font("Arial", 12);

        public void RenderGameWorld(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制背景
            using (var brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                g.FillRectangle(brush, ClientRectangle);
            }

            // 绘制敌人
            List<IEnemy> enemiesCopy = new List<IEnemy>(GameWorld.Instance.ActiveEnemies);
            foreach (var enemy in enemiesCopy)
            {
                enemy.on_render(g);
            }

            // 绘制子弹
            List<IBullet> bulletsCopy = new List<IBullet>(GameWorld.Instance.ActiveBullets);
            foreach (var bullet in bulletsCopy)
            {
                bullet.on_render(g);
            }

            // 绘制玩家
            Instance.Player.on_render(g);

            //绘制UI
            //g.DrawString($" {ActiveBullets.Count:F1}", DefaultFont, Brushes.Black, 10, 20);
        }

        public void Shooting(Vector2 mouse_vector2, BulletType type)
        {

            if (Player.IsShooting == false)
            {
                return;
            }

            Vector2 position = new Vector2(Player.Position.X + Player.Size.Width / 2, Player.Position.Y + Player.Size.Height / 2);
            double angle = Math.Atan2(mouse_vector2.Y - Player.Position.Y, mouse_vector2.X - Player.Position.X);

            IBullet bullet = bulletPool.GetBullet(type);
            bullet.Initialize(position, angle);
            GameWorld.Instance.ActiveBullets.Add(bullet);//添加到活动子弹列表
        }

        public void SpawnEnemy()
        {
            //if (enemySpawner.EnemyCount <= 0)
            //{
            //    return;
            //}
            //Enemy1 enemy = enemySpawner.TrySpawnEnemy();
            var type = EnemyType.normal;
            IEnemy enemy = enemyPool.GetEnemyType(type);
            //随机在屏幕外生成敌人
            Random random = new Random();
            int x = random.Next(0, (int)ClientRectangle.Width);
            int y = random.Next(0, (int)ClientRectangle.Height);
            enemy.Position = new Vector2(x, y);


            GameWorld.Instance.ActiveEnemies.Add(enemy);

        }
    }
}