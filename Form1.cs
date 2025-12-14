using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    enum GameState { Start, Running, Paused, GameOver }
    enum Direction { Up, Down, Left, Right }

    public class Form1 : Form
    {
        // ===== UI =====
        Panel? gamePanel;
        Button? btnStart, btnPause, btnColor;
        Label? lblScore, lblHigh, lblLevel, lblStatus;

        // ===== Game =====
        List<Point> snake = new();
        Point food;
        Direction direction, nextDirection;
        GameState state = GameState.Start;

        System.Windows.Forms.Timer timer = new();
        Random rand = new();

        // ===== Configuration =====
        const int gridSize = 20;
        const int cellSize = 20;

        const int baseSpeed = 150;
        const int minSpeed = 60;

        int score = 0;
        int highScore = 0;
        int level = 1;

        Color snakeColor = Color.Lime;

        public Form1()
        {
            Text = "Snake Game - C# .NET";
            ClientSize = new Size(600, 470);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            KeyPreview = true;
            Focus();

            InitializeUI();
            InitializeGame();
        }

        // ================= UI SETUP =================
        void InitializeUI()
        {
            gamePanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(gridSize * cellSize, gridSize * cellSize),
                BackColor = Color.Black
            };
            gamePanel.Paint += DrawGame;
            Controls.Add(gamePanel);

            btnStart = CreateButton("Start / Restart", 430, 20, StartGame);
            btnPause = CreateButton("Pause", 430, 60, TogglePause);
            btnColor = CreateButton("Change Color", 430, 100, ChangeSnakeColor);

            lblScore = CreateLabel("Score: 0", 430, 150);
            lblHigh = CreateLabel("High Score: 0", 430, 175);
            lblLevel = CreateLabel("Level: 1", 430, 200);
            lblStatus = CreateLabel("Press Start", 10, 435);

            timer.Interval = baseSpeed;
            timer.Tick += UpdateGame;
        }

        void InitializeGame()
        {
            snake.Clear();
            direction = Direction.Right;
            nextDirection = Direction.Right;
        }

        // ================= GAME CONTROL =================
        void StartGame(object? sender, EventArgs? e)
        {
            snake.Clear();
            snake.Add(new Point(10, 10));
            snake.Add(new Point(9, 10));
            snake.Add(new Point(8, 10));

            score = 0;
            level = 1;
            timer.Interval = baseSpeed;

            lblScore!.Text = "Score: 0";
            lblLevel!.Text = "Level: 1";
            lblStatus!.Text = "";

            SpawnFood();

            state = GameState.Running;
            timer.Start();
            Focus();
        }

        void TogglePause(object? sender, EventArgs? e)
        {
            if (state == GameState.Running)
            {
                timer.Stop();
                state = GameState.Paused;
                lblStatus!.Text = "Paused";
            }
            else if (state == GameState.Paused)
            {
                timer.Start();
                state = GameState.Running;
                lblStatus!.Text = "";
                Focus();
            }
        }

        // ================= GAME LOOP =================
        void UpdateGame(object? sender, EventArgs e)
        {
            direction = nextDirection;

            Point head = snake[0];
            Point newHead = direction switch
            {
                Direction.Up => new(head.X, head.Y - 1),
                Direction.Down => new(head.X, head.Y + 1),
                Direction.Left => new(head.X - 1, head.Y),
                _ => new(head.X + 1, head.Y)
            };

            // Collision detection
            if (newHead.X < 0 || newHead.Y < 0 ||
                newHead.X >= gridSize || newHead.Y >= gridSize ||
                snake.Contains(newHead))
            {
                GameOver();
                return;
            }

            snake.Insert(0, newHead);

            if (newHead == food)
            {
                score += 10;
                lblScore!.Text = $"Score: {score}";
                UpdateLevelAndSpeed();
                SpawnFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            gamePanel!.Invalidate();
        }

        void UpdateLevelAndSpeed()
        {
            int newLevel = (score / 50) + 1;
            if (newLevel != level)
            {
                level = newLevel;
                lblLevel!.Text = $"Level: {level}";

                int newSpeed = baseSpeed - (level - 1) * 15;
                timer.Interval = Math.Max(minSpeed, newSpeed);
            }
        }

        void GameOver()
        {
            timer.Stop();
            state = GameState.GameOver;

            if (score > highScore)
                highScore = score;

            lblHigh!.Text = $"High Score: {highScore}";
            lblStatus!.Text = $"Game Over! Score: {score}";
        }

        // ================= DRAWING =================
        void DrawGame(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            using (SolidBrush brush = new SolidBrush(snakeColor))
            {
                foreach (Point p in snake)
                    g.FillRectangle(brush,
                        p.X * cellSize, p.Y * cellSize,
                        cellSize, cellSize);
            }

            g.FillEllipse(Brushes.Red,
                food.X * cellSize, food.Y * cellSize,
                cellSize, cellSize);
        }

        // ================= INPUT =================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (state != GameState.Running)
                return base.ProcessCmdKey(ref msg, keyData);

            switch (keyData)
            {
                case Keys.Up:
                    if (direction != Direction.Down) nextDirection = Direction.Up;
                    return true;
                case Keys.Down:
                    if (direction != Direction.Up) nextDirection = Direction.Down;
                    return true;
                case Keys.Left:
                    if (direction != Direction.Right) nextDirection = Direction.Left;
                    return true;
                case Keys.Right:
                    if (direction != Direction.Left) nextDirection = Direction.Right;
                    return true;
                case Keys.Space:
                    TogglePause(null, null);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ================= HELPERS =================
        void SpawnFood()
        {
            do
            {
                food = new Point(rand.Next(gridSize), rand.Next(gridSize));
            }
            while (snake.Contains(food));
        }

        void ChangeSnakeColor(object? sender, EventArgs? e)
        {
            using ColorDialog cd = new();
            cd.Color = snakeColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                snakeColor = cd.Color;
                gamePanel!.Invalidate();
                Focus();
            }
        }

        Button CreateButton(string text, int x, int y, EventHandler handler)
        {
            Button b = new()
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(140, 30)
            };
            b.Click += handler;
            Controls.Add(b);
            return b;
        }

        Label CreateLabel(string text, int x, int y)
        {
            Label l = new()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true
            };
            Controls.Add(l);
            return l;
        }
    }
}
