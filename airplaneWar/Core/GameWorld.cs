// GameWorld.cs (单例类)
using airplaneWar.Core.Manager.CollisionManager;
using airplaneWar.Core.Manager.InputManager;
using airplaneWar.GameLogic.Entities.Bullet.Core;
using airplaneWar.GameLogic.Entities.Bullet;
using airplaneWar.GameLogic.Entities.Enemies.Core;
using airplaneWar.GameLogic.Entities.Player;
using System.Numerics;
using airplaneWar.Core.Manager;
using DocumentFormat.OpenXml.Drawing;

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
        private Image bkgImage; // 添加背景图片字段
        public Vector2 position { get; private set; } = new Vector2(0, 0);
        public RectangleF ClientRectangle { get; private set; }
        public Size gameWorldSize { get; private set; } = new Size(1500, 1000);

        public Player Player { get; set; } = new Player();
        public List<IEnemy> ActiveEnemies { get; } = new List<IEnemy>();//敌人
        public List<IBullet> ActiveBullets { get; } = new List<IBullet>();//子弹
        public Camera camera { get; private set; } = new Camera();

        public ProjectTimer BulletSpawnTimer = new();
        public BulletPool bulletPool = new();
        public ProjectTimer EnemySpawnTimer = new();
        public EnemyPool enemyPool = new();
        private static readonly Font DefaultFont = new Font("Arial", 12);

        public void InitializeGameWorld()
        {
            //加载图片
            //bkgImage = Image.FromFile("Resources/bkg.png");
            bkgImage = Image.FromFile("D:\\code\\project\\airplaneWar\\airplaneWar\\Resources\\bkg.png");
            _Instance = this;
            //切换为MainForm的屏幕大小 
            ClientRectangle = new RectangleF(
                position.X, position.Y,
                gameWorldSize.Width, gameWorldSize.Height
                );
            //camera.size = new Size(2230, 1400);
            camera.size = new Size(MainForm.DesignWidth, MainForm.DesignHeight);
            camera.position = new Vector2(
                ClientRectangle.Width / 2 - camera.size.Width / 2,
                ClientRectangle.Height / 2 - camera.size.Height / 2
                );

            // 创建玩家实例（应在游戏世界初始化时创建）
            GameWorld.Instance.Player = new Player();
            var playerStartPos = new Vector2(
                ClientRectangle.Width / 2 - Player.size.Width / 2,
                ClientRectangle.Height / 2 - Player.size.Height / 2
                );
            GameWorld.Instance.Player.Position = playerStartPos;
            CollisionManager.Instance.add_collision(Player.hitbox);

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

        public void on_exit()
        {
            //释放资源

        }

        public void on_update(double deltaTime)
        {
            // 更新敌人位置
            for (int i = GameWorld.Instance.ActiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = GameWorld.Instance.ActiveEnemies[i];
                var x = Player.Position.X - enemy.Position.X;
                var y = Player.Position.Y - enemy.Position.Y;
                var distance = Math.Sqrt(x * x + y * y);
                enemy.Angle = Math.Atan2(y, x);
                enemy.on_update(deltaTime);
                if (!enemy.IsAlive)
                {
                    CollisionManager.Instance.remove_collision(enemy.Hitbox);
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
                    CollisionManager.Instance.remove_collision(bullet.Hitbox);
                    GameWorld.Instance.ActiveBullets.RemoveAt(i);
                    EventManager.score += 100;
                }
            }
            CollisionManager.Instance.on_collide(deltaTime);
            //更新玩家位置
            GameWorld.Instance.Player.on_update(deltaTime);
            //更新相机位置
            GameWorld.Instance.camera.on_update(deltaTime);
            // 更新时间
            BulletSpawnTimer.on_update(deltaTime);
            EnemySpawnTimer.on_update(deltaTime);
        }


        public void RenderGameWorld(Graphics g)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制背景
            using (var brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                //this.position -= camera.position;
                //g.FillRectangle(
                //    brush,
                //    this.position.X - camera.position.X, this.position.Y - camera.position.Y,
                //    gameWorldSize.Width, gameWorldSize.Height);
                g.DrawImage(bkgImage,
                    this.position.X - camera.position.X, this.position.Y - camera.position.Y,
                    gameWorldSize.Width, gameWorldSize.Height);
            }

            // 绘制敌人
            List<IEnemy> enemiesCopy = new List<IEnemy>(GameWorld.Instance.ActiveEnemies);
            foreach (var enemy in enemiesCopy)
            {
                enemy.on_render(g, camera.position);
            }

            // 绘制子弹
            List<IBullet> bulletsCopy = new List<IBullet>(GameWorld.Instance.ActiveBullets);
            foreach (var bullet in bulletsCopy)
            {
                bullet.on_render(g, camera.position);
            }

            // 绘制玩家
            Instance.Player.on_render(g, camera.position);

            //绘制UI
            g.DrawString($"HP {Player.HP:F1}", DefaultFont, Brushes.Red, 10, 40);
        }

        public void Shooting(Vector2 mouse_vector2, BulletType type)
        {
            if (Player.IsShooting == false)
            {
                return;
            }

            Vector2 position = new Vector2(Player.Position.X + Player.size.Width / 2, Player.Position.Y + Player.size.Height / 2);
            double angle = Math.Atan2(mouse_vector2.Y - Player.Position.Y + camera.position.Y, mouse_vector2.X - Player.Position.X + camera.position.X);

            IBullet bullet = bulletPool.GetBullet(type);
            bullet.Initialize(position, angle);
            CollisionManager.Instance.add_collision(bullet.Hitbox);
            GameWorld.Instance.ActiveBullets.Add(bullet);//添加到活动子弹列表
        }

        public void SpawnEnemy()
        {

            var type = EnemyType.normal;
            IEnemy enemy = enemyPool.GetEnemyType(type);
            //随机在屏幕外生成敌人
            Random random = new Random();
            int x = random.Next(0, (int)ClientRectangle.Width);
            int y = random.Next(0, (int)ClientRectangle.Height);
            enemy.Position = new Vector2(x, y);
            CollisionManager.Instance.add_collision(enemy.Hitbox);
            GameWorld.Instance.ActiveEnemies.Add(enemy);

        }
    }
}