using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Character;
using Collision_Detection;
using Bullets;

namespace airplaneWar
{
    public partial class MainForm : Form
    {
        // 游戏元素
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<stdBullet> bullets = new List<stdBullet>();

        //private Collision collision = new Collision();

        // 游戏状态
        private int score = 0;
        private int gameInterval = 0;
        private bool isGameOver = false;

        // 控制状态
        private bool leftPressed, rightPressed, upPressed, downPressed;

        public MainForm()
        {
            InitializeComponent();
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            InitializeGame();

            // 设置窗口大小
            this.ClientSize = new Size(1600, 1200);

            // 设置游戏区域
            gamePanel.Size = new Size(1200, 800);
            gamePanel.Paint += GamePanel_Paint;
            gamePanel.MouseMove += MainForm_MouseMove;

            // 初始化计时器
            gameTimer.Interval = 30;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            //每1秒发射一颗子弹
            shootTimer.Interval = 200;
            shootTimer.Tick += (sender, e) => Shoot();
            shootTimer.Start();

            // 启用双缓冲
            DoubleBuffered = true;
        }

        private void InitializeGame()
        {
            player = new Player
            {
                Position = new Point(400, 500),
                Size = new Size(40, 40),
                Speed = 5
            };


            enemies = new List<Enemy>();
            bullets = new List<stdBullet>();

            score = 0;
            isGameOver = false;
            leftPressed = false;
            rightPressed = false;
            upPressed = false;
            downPressed = false;
        }

        private void restartGame()
        {
            player = new Player
            {
                Position = new Point(400, 500),
                Size = new Size(40, 40),
                Speed = 5
            };
            enemies.Clear();
            bullets.Clear();
            score = 0;
            isGameOver = false;
            leftPressed = false;
            rightPressed = false;
            upPressed = false;
            downPressed = false;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isGameOver)
            {
                UpdatePlayer();
                //Shoot();
                UpdateBullets();
                UpdateEnemies();
                SpawnEnemies();
                //CheckCollisions(player, enemies, bullets);
                Collision.CheckCollisions(player, enemies, bullets);

            }
            gamePanel.Invalidate();
        }

        private void UpdatePlayer()
        {
            if (player != null)
            {
                if (!player.IsAlive)
                {
                    isGameOver = true;
                    return;
                }
            }
            // 处理玩家移动
            //player.Position = new Point(
            //    Math.Max(0, Math.Min(gamePanel.Width - player.Size.Width, player.Position.X + (rightPressed ? player.Speed : 0) - (leftPressed ? player.Speed : 0))),
            //    Math.Max(0, Math.Min(gamePanel.Height - player.Size.Height, player.Position.Y + (downPressed ? player.Speed : 0) - (upPressed ? player.Speed : 0))));

            gamePanel.MouseMove += MainForm_MouseMove;
        }

        private void UpdateBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                //bullets[i].Position.Y -= bullets[i].Speed;
                bullets[i].Position = new Point(
                    bullets[i].Position.X,
                    bullets[i].Position.Y - bullets[i].Speed);
                if (bullets[i].Position.Y < 0)
                    bullets.RemoveAt(i);
            }
        }

        private void UpdateEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!enemies[i].IsAlive)
                {
                    enemies.RemoveAt(i);
                    score += 10;
                    continue;
                }
                enemies[i].Position = new Point(
                    enemies[i].Position.X,
                    enemies[i].Position.Y + enemies[i].Speed);
                if (enemies[i].Position.Y > gamePanel.Height)
                    enemies.RemoveAt(i);
            }
        }


        private void SpawnEnemies()//生成敌人
        {
            if (new Random().Next(0, 100) < 5) // 5%概率生成敌人
            {
                enemies.Add(new Enemy
                {
                    Position = new Point(new Random().Next(0, gamePanel.Width - 30), -30),
                    Size = new Size(30, 30),
                    Speed = 3
                });
            }
        }



        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            // 绘制玩家
            g.FillRectangle(Brushes.Cyan, player.Bounds);

            // 绘制子弹
            foreach (var bullet in bullets)
                g.FillRectangle(Brushes.Yellow, bullet.Bounds);

            // 绘制敌人
            foreach (var enemy in enemies)
                g.FillRectangle(Brushes.Red, enemy.Bounds);

            // 绘制分数
            g.DrawString($"Score: {score}", Font, Brushes.White, 10, 10);
            g.DrawString($"interval: {gameInterval}", Font, Brushes.White, 10, 30);
            g.DrawString($"HP: {player.Health}", Font, Brushes.White, 10, 50);


            // 游戏结束提示
            if (isGameOver)
            {
                g.DrawString("Game Over! Press R to restart",
                    new Font("Arial", 10), Brushes.White,
                    gamePanel.Width / 2 - 150, gamePanel.Height / 2 - 20);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: leftPressed = true; break;
                case Keys.Right: rightPressed = true; break;
                case Keys.Up: upPressed = true; break;
                case Keys.Down: downPressed = true; break;
                case Keys.R: if (isGameOver) InitializeGame(); restartGame(); break;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: leftPressed = false; break;
                case Keys.Right: rightPressed = false; break;
                case Keys.Up: upPressed = false; break;
                case Keys.Down: downPressed = false; break;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            player.Position = new Point(
                Math.Max(0, Math.Min(gamePanel.Width - player.Size.Width, e.X - player.Size.Width / 2)),
                Math.Max(0, Math.Min(gamePanel.Height - player.Size.Height, e.Y - player.Size.Height / 2)));
        }

        private void Shoot()
        {
            bullets.Add(new stdBullet
            {
                Position = new Point(
                    player.Position.X + player.Size.Width / 2 - 2,
                    player.Position.Y),
                Size = new Size(4, 10),
                Speed = 10
            });
        }
    }


}
