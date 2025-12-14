
using System.Drawing.Drawing2D;
using System.Media;

namespace Assignment2
{

    public partial class Form1 : Form
    {
        private List<Point> snake = new List<Point>();  // List to store snake body segments as coordinates
        private int snakeDirectionX = 1;  // Initial movement: 1=right, -1=left, 0=no horizontal movement
        private int snakeDirectionY = 0;  // Initial movement: 1=down, -1=up, 0=no vertical movement
        public int cellSize = 20;         // Size of each grid cell in pixels
        int rows;                         // Number of rows in game grid
        int cols;                         // Number of columns in game grid
        private Bitmap gridBitmap = new Bitmap(1, 1); // Bitmap to draw grid (temporary small size)

        private Point food;               // Coordinates of current food position
        private Random rand = new Random(); // Random generator for food placement
        private int score = 0;            // Player's current score
        private int lives = 3;            // Number of lives/attempts remaining
        private int highScore = 0;        // Best score achieved
        private bool gameStarted = false; // Is game currently running?
        private bool gamePaused = false;  // Is game paused?
        private bool gameEnded = false;   // Has game ended?
                                          // UI Colors
                                          // UI Colors
        Color snakeColor = Color.FromArgb(255, 105, 180);      // hot pink body
        Color snakeHeadColor = Color.FromArgb(255, 20, 147);  // deep  pink head
        private Color foodColor = Color.FromArgb(255, 40, 40);       // Red for food
        private Color panelBackground = Color.FromArgb(15, 15, 20);
        // Dark gray for background
        private Color gridColor = Color.FromArgb(140, 140, 140);
        // Add to variable declarations section
        private enum FoodShape { Heart, Circle, Diamond, Square, Star }
        private FoodShape currentFoodShape = FoodShape.Heart; // Default shape
                                                              // Add these variables to your form class
        private Color customSnakeColor = Color.FromArgb(255, 105, 180);      // Hot pink body (default)
        private Color customSnakeHeadColor = Color.FromArgb(255, 20, 147);   // Deep pink head (default)
        private Color customFoodColor = Color.FromArgb(255, 40, 40);         // Red food (default)
        private Color customPanelBackground = Color.FromArgb(15, 15, 20);    // Dark gray background (default)
        public Form1()
        {
            InitializeComponent();
            pnlGame.DoubleBuffered(true); //removes flicker inside game panel

            // Style the existing controls
            StyleExistingControls();
            // Hook KeyDown event to ALL controls on the form
            HookAllControls(this);


        }
        private void StyleExistingControls()
        {
            // Style the form
            this.BackColor = Color.FromArgb(13, 17, 23);
            this.ForeColor = Color.White;
            this.Text = "🐍 Modern Snake Game";
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            // Style the game panel
            pnlGame.BackColor = panelBackground;

            // Style buttons
            StyleButton(btnStart, Color.FromArgb(76, 175, 80));  // Green
            StyleButton(btnPause, Color.FromArgb(33, 150, 243)); // Blue

            // Style labels in the GroupBox
            lblScore.ForeColor = Color.FromArgb(76, 175, 80);  // Green
            lblScore.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            lblLives.ForeColor = Color.FromArgb(244, 67, 54);  // Red
            lblLives.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblHighScore.ForeColor = Color.FromArgb(255, 193, 7);  // Amber
            lblHighScore.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            // Style the GroupBox (if you have one)
            // If your controls are in a GroupBox called gbControls, style it:
            gbControls.ForeColor = Color.White;
            gbControls.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gbControls.FlatStyle = FlatStyle.Flat;
            gbControls.ForeColor = Color.White;
            gbControls.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            gbControls.BackColor = Color.FromArgb(30, 30, 46);
            // Style instructions label
            lblInstructions.ForeColor = Color.LightGray;
            lblInstructions.Font = new Font("Segoe UI", 9);
        }

        private void StyleButton(System.Windows.Forms.Button button, Color backColor)
        {
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;

            // Add hover effects
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(backColor.R + 20, 255),
                Math.Min(backColor.G + 20, 255),
                Math.Min(backColor.B + 20, 255)
            );
        }
        private void HookAllControls(Control control)
        {
            control.KeyDown += Form1_KeyDown; // Use your existing KeyDown handler
            control.PreviewKeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                    e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    e.IsInputKey = true;
                }
            };

            foreach (Control child in control.Controls)
            {
                HookAllControls(child);
            }
        }
        private void pnlGame_Paint(object sender, PaintEventArgs e)
        {
            // Calculate how many full cells fit inside the panel
            cols = pnlGame.Width / cellSize;
            rows = pnlGame.Height / cellSize;

            // Draw the grid from bitmap
            e.Graphics.DrawImage(gridBitmap, 0, 0);
            // Draw snake with improved colors
            for (int i = 0; i < snake.Count; i++)
            {
                Point segment = snake[i];
                Color segmentColor = (i == 0) ? snakeHeadColor : snakeColor;

                using (Brush segmentBrush = new SolidBrush(segmentColor))
                {
                    int padding = 2;
                    e.Graphics.FillRectangle(segmentBrush,
                        segment.X * cellSize + padding,
                        segment.Y * cellSize + padding,
                        cellSize - padding * 2,
                        cellSize - padding * 2);
                }
            }
            // Draw food with shine effect
            // Draw food as a perfect heart
            // Draw food based on selected shape
            using (Brush foodBrush = new SolidBrush(foodColor))
            {
                int padding = 1; // Adjust this for size
                Rectangle r = new Rectangle(
                    food.X * cellSize + padding,
                    food.Y * cellSize + padding,
                    cellSize - padding * 2,
                    cellSize - padding * 2
                );

                float x = r.X;
                float y = r.Y;
                float w = r.Width;
                float h = r.Height;

                using (GraphicsPath path = new GraphicsPath())
                {
                    switch (currentFoodShape)
                    {
                        case FoodShape.Heart:
                            // Heart shape
                            path.AddArc(x + w * 0.10f, y, w * 0.40f, h * 0.40f, 140, 200);
                            path.AddArc(x + w * 0.50f, y, w * 0.40f, h * 0.40f, 200, 200);
                            path.AddLine(x + w * 0.90f, y + h * 0.32f, x + w * 0.50f, y + h);
                            path.AddLine(x + w * 0.50f, y + h, x + w * 0.10f, y + h * 0.32f);
                            break;

                        case FoodShape.Circle:
                            // Circle shape
                            path.AddEllipse(r);
                            break;

                        case FoodShape.Diamond:
                            // Diamond shape
                            PointF[] diamondPoints = new PointF[]
                            {
                    new PointF(x + w / 2, y),           // Top
                    new PointF(x + w, y + h / 2),       // Right
                    new PointF(x + w / 2, y + h),       // Bottom
                    new PointF(x, y + h / 2)            // Left
                            };
                            path.AddPolygon(diamondPoints);
                            break;

                        case FoodShape.Square:
                            // Square shape
                            path.AddRectangle(r);
                            break;
                        case FoodShape.Star:
                            // Star shape (5-pointed) - CORRECTED
                            PointF center = new PointF(x + w / 2, y + h / 2);
                            float outerRadius = Math.Min(w, h) * 0.45f;  // Slightly larger
                            float innerRadius = outerRadius * 0.4f;

                            // Create star points
                            PointF[] starPoints = new PointF[10];

                            for (int i = 0; i < 10; i++)
                            {
                                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                                double angle = Math.PI / 2 + i * Math.PI / 5;

                                starPoints[i] = new PointF(
                                    center.X + radius * (float)Math.Cos(angle),
                                    center.Y + radius * (float)Math.Sin(angle)
                                );
                            }

                            path.AddPolygon(starPoints);
                            break;
                    }

                    path.CloseFigure();
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(foodBrush, path);

                    // Add shine effect for all shapes except square
                    if (currentFoodShape != FoodShape.Square)
                    {
                        using (Brush shine = new SolidBrush(Color.FromArgb(120, 255, 255, 255)))
                        {
                            e.Graphics.FillEllipse(
                                shine,
                                x + w * 0.28f,
                                y + h * 0.16f,
                                w * 0.26f,
                                h * 0.26f
                            );
                        }
                    }
                }
            }

            // Draw "PAUSED" text with better styling
            if (gamePaused)
            {
                using (Font pauseFont = new Font("Segoe UI", 24, FontStyle.Bold))
                {
                    // Draw shadow
                    e.Graphics.DrawString("PAUSED", pauseFont,
                        Brushes.Black,
                        pnlGame.Width / 2 - 60 + 2,
                        pnlGame.Height / 2 - 15 + 2);

                    // Draw main text
                    e.Graphics.DrawString("PAUSED", pauseFont,
                        Brushes.Yellow,
                        pnlGame.Width / 2 - 60,
                        pnlGame.Height / 2 - 15);
                }
            }
        }
        private void InitializeColorControls()
        {
            // Create a panel for color customization
          
            pnlColor.BackColor = Color.FromArgb(30, 30, 46);
            pnlColor.BorderStyle = BorderStyle.FixedSingle;
            pnlColor.Size = new Size(200, 220);
            pnlColor.Name = "pnlColorSettings";

            // Title label
            Label lblColorTitle = new Label();
            lblColorTitle.Text = "🎨 Customize Colors";
            lblColorTitle.ForeColor = Color.White;
            lblColorTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblColorTitle.Location = new Point(10, 10);
            lblColorTitle.AutoSize = true;
            pnlColor.Controls.Add(lblColorTitle);

            int yPos = 40;

            // 1. Snake Body Color
            Button btnSnakeColor = new Button();
            btnSnakeColor.Text = "🐍 Snake Body";
            btnSnakeColor.Size = new Size(180, 30);
            btnSnakeColor.Location = new Point(10, yPos);
            btnSnakeColor.BackColor = customSnakeColor;
            btnSnakeColor.ForeColor = GetContrastColor(customSnakeColor);
            btnSnakeColor.FlatStyle = FlatStyle.Flat;
            btnSnakeColor.FlatAppearance.BorderSize = 1;
            btnSnakeColor.FlatAppearance.BorderColor = Color.White;

            btnSnakeColor.Click += (s, e) =>
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = customSnakeColor;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        customSnakeColor = colorDialog.Color;
                        btnSnakeColor.BackColor = customSnakeColor;
                        btnSnakeColor.ForeColor = GetContrastColor(customSnakeColor);
                        snakeColor = customSnakeColor;
                        pnlGame.Invalidate();
                    }
                }
            };

            pnlColor.Controls.Add(btnSnakeColor);
            yPos += 35;

            // 2. Snake Head Color
            Button btnSnakeHeadColor = new Button();
            btnSnakeHeadColor.Text = "🐍 Snake Head";
            btnSnakeHeadColor.Size = new Size(180, 30);
            btnSnakeHeadColor.Location = new Point(10, yPos);
            btnSnakeHeadColor.BackColor = customSnakeHeadColor;
            btnSnakeHeadColor.ForeColor = GetContrastColor(customSnakeHeadColor);
            btnSnakeHeadColor.FlatStyle = FlatStyle.Flat;
            btnSnakeHeadColor.FlatAppearance.BorderSize = 1;
            btnSnakeHeadColor.FlatAppearance.BorderColor = Color.White;

            btnSnakeHeadColor.Click += (s, e) =>
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = customSnakeHeadColor;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        customSnakeHeadColor = colorDialog.Color;
                        btnSnakeHeadColor.BackColor = customSnakeHeadColor;
                        btnSnakeHeadColor.ForeColor = GetContrastColor(customSnakeHeadColor);
                        snakeHeadColor = customSnakeHeadColor;
                        pnlGame.Invalidate();
                    }
                }
            };

            pnlColor.Controls.Add(btnSnakeHeadColor);
            yPos += 35;

            // 3. Food Color
            Button btnFoodColor = new Button();
            btnFoodColor.Text = "🍎 Food Color";
            btnFoodColor.Size = new Size(180, 30);
            btnFoodColor.Location = new Point(10, yPos);
            btnFoodColor.BackColor = customFoodColor;
            btnFoodColor.ForeColor = GetContrastColor(customFoodColor);
            btnFoodColor.FlatStyle = FlatStyle.Flat;
            btnFoodColor.FlatAppearance.BorderSize = 1;
            btnFoodColor.FlatAppearance.BorderColor = Color.White;

            btnFoodColor.Click += (s, e) =>
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = customFoodColor;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        customFoodColor = colorDialog.Color;
                        btnFoodColor.BackColor = customFoodColor;
                        btnFoodColor.ForeColor = GetContrastColor(customFoodColor);
                        foodColor = customFoodColor;
                        pnlGame.Invalidate();
                    }
                }
            };

            pnlColor.Controls.Add(btnFoodColor);
            yPos += 35;

            // 4. Panel Background Color
            Button btnPanelColor = new Button();
            btnPanelColor.Text = "🎯 Panel Background";
            btnPanelColor.Size = new Size(180, 30);
            btnPanelColor.Location = new Point(10, yPos);
            btnPanelColor.BackColor = customPanelBackground;
            btnPanelColor.ForeColor = GetContrastColor(customPanelBackground);
            btnPanelColor.FlatStyle = FlatStyle.Flat;
            btnPanelColor.FlatAppearance.BorderSize = 1;
            btnPanelColor.FlatAppearance.BorderColor = Color.White;

            btnPanelColor.Click += (s, e) =>
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = customPanelBackground;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        customPanelBackground = colorDialog.Color;
                        btnPanelColor.BackColor = customPanelBackground;
                        btnPanelColor.ForeColor = GetContrastColor(customPanelBackground);
                        panelBackground = customPanelBackground;

                        // Redraw grid with new background color
                        DrawEnhancedGrid();
                        pnlGame.Invalidate();
                    }
                }
            };

            pnlColor.Controls.Add(btnPanelColor);
            yPos += 40;

            // 5. Reset Colors Button
            Button btnResetColors = new Button();
            btnResetColors.Text = "🔄 Reset to Default";
            btnResetColors.Size = new Size(180, 30);
            btnResetColors.Location = new Point(10, yPos);
            StyleButton(btnResetColors, Color.FromArgb(100, 100, 100));

            btnResetColors.Click += (s, e) =>
            {
                ResetColorsToDefault();

                // Update button colors
                btnSnakeColor.BackColor = customSnakeColor;
                btnSnakeColor.ForeColor = GetContrastColor(customSnakeColor);
                btnSnakeHeadColor.BackColor = customSnakeHeadColor;
                btnSnakeHeadColor.ForeColor = GetContrastColor(customSnakeHeadColor);
                btnFoodColor.BackColor = customFoodColor;
                btnFoodColor.ForeColor = GetContrastColor(customFoodColor);
                btnPanelColor.BackColor = customPanelBackground;
                btnPanelColor.ForeColor = GetContrastColor(customPanelBackground);

                // Redraw everything
                DrawEnhancedGrid();
                pnlGame.Invalidate();
            };

            pnlColor.Controls.Add(btnResetColors);

            // Position color panel
            Control settingsPanel = this.Controls.Find("pnlFoodSettings", true).FirstOrDefault();
            if (settingsPanel != null)
            {
                pnlColor.Location = new Point(
                    settingsPanel.Right + 20,
                    settingsPanel.Top
                );
            }
            else
            {
                pnlColor.Location = new Point(
                    gbControls.Left,
                    gbControls.Bottom + 20
                );
            }

            // Add to form
            this.Controls.Add(pnlColor);
            pnlColor.BringToFront();
        }
        private void InitializeSettingsPanel()
        {
            // Create a panel for settings
            Panel settingsPanel = new Panel();
            settingsPanel.BackColor = Color.FromArgb(30, 30, 46);
            settingsPanel.BorderStyle = BorderStyle.FixedSingle;
            settingsPanel.Size = new Size(180, 160);
            settingsPanel.Name = "pnlFoodSettings"; // Add name for reference

            // Title label
            Label lblFoodShape = new Label();
            lblFoodShape.Text = "🍎 Food Shape";
            lblFoodShape.ForeColor = Color.White;
            lblFoodShape.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFoodShape.Location = new Point(10, 10);
            lblFoodShape.AutoSize = true;
            settingsPanel.Controls.Add(lblFoodShape);

            // Create radio buttons
            int yPos = 40;
            string[] shapeNames = { "Heart", "Circle", "Diamond", "Square", "Star" };
            string[] shapeEmojis = { "❤️", "🔵", "💎", "⬜", "⭐" };

            // Store the first radio button reference
            RadioButton firstRadioButton = null;

            foreach (FoodShape shape in Enum.GetValues(typeof(FoodShape)))
            {
                RadioButton rb = new RadioButton();
                rb.Text = $"{shapeEmojis[(int)shape]} {shapeNames[(int)shape]}";
                rb.Tag = shape;
                rb.Location = new Point(15, yPos);
                rb.ForeColor = Color.LightGray;
                rb.BackColor = Color.Transparent;
                rb.AutoSize = true;

                // Set checked based on currentFoodShape
                rb.Checked = (shape == currentFoodShape);

                if (firstRadioButton == null)
                    firstRadioButton = rb;

                rb.CheckedChanged += (s, e) =>
                {
                    if (rb.Checked)
                    {
                        currentFoodShape = (FoodShape)rb.Tag;
                        pnlGame.Invalidate(); // Redraw with new shape
                    }
                };

                settingsPanel.Controls.Add(rb);
                yPos += 25;
            }

            // Ensure at least one is checked
            if (firstRadioButton != null && !settingsPanel.Controls.OfType<RadioButton>().Any(rb => rb.Checked))
            {
                firstRadioButton.Checked = true;
            }

            // Position settings panel
            settingsPanel.Location = new Point(
                gbControls.Left,
                gbControls.Bottom + 20
            );

            // Add to form
            this.Controls.Add(settingsPanel);
            settingsPanel.BringToFront();
        }
        private Color GetContrastColor(Color color)
        {
            // Calculate the perceptive luminance
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            // Return black for bright colors, white for dark colors
            return luminance > 0.5 ? Color.Black : Color.White;
        }
        private void ResetColorsToDefault()
        {
            // Reset to default colors
            customSnakeColor = Color.FromArgb(255, 105, 180);      // Hot pink body
            customSnakeHeadColor = Color.FromArgb(255, 20, 147);   // Deep pink head
            customFoodColor = Color.FromArgb(255, 40, 40);         // Red food
            customPanelBackground = Color.FromArgb(15, 15, 20);    // Dark gray background
            gridColor = Color.FromArgb(140, 140, 140);             // Grid color

            // Update the actual colors used in drawing
            snakeColor = customSnakeColor;
            snakeHeadColor = customSnakeHeadColor;
            foodColor = customFoodColor;
            panelBackground = customPanelBackground;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            pnlGame.Width = (pnlGame.Width / cellSize) * cellSize;
            pnlGame.Height = (pnlGame.Height / cellSize) * cellSize;

            // FIX: Set global cols/rows
            cols = pnlGame.Width / cellSize;
            rows = pnlGame.Height / cellSize;

            gridBitmap = new Bitmap(pnlGame.Width, pnlGame.Height);

            // Initialize snake in center
            int startX = cols / 2;
            int startY = rows / 2;

            snake.Clear();
            snake.Add(new Point(startX, startY));
            snake.Add(new Point(startX - 1, startY));
            snake.Add(new Point(startX - 2, startY));

            // Create grid bitmap
            DrawEnhancedGrid();

            // Stylish fonts
            lblScore.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLives.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblHighScore.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Add emojis
            lblScore.Text = "⭐ Score: 0";
            lblLives.Text = "❤️ Lives: 3";
            lblHighScore.Text = "🏆 High Score: 0";

            // FIX: Generate first food
            GenerateFood();

            // ADD THIS LINE TO INITIALIZE THE SHAPE CONTROLS
            // InitializeShapeControls(); // <-- ADD THIS LINE
            InitializeSettingsPanel();

            // ADD THIS: Initialize color controls
            InitializeColorControls();

            // ensure designer timer exists; if not, create and wire it up
            if (tmrGame == null)
            {
                // this.components usually exists when form has other components
                this.tmrGame = new System.Windows.Forms.Timer(this.components);
                this.tmrGame.Interval = 150;
                this.tmrGame.Tick += new System.EventHandler(this.tmrGame_Tick);
            }

            // Initial button states
            btnStart.Enabled = true;
            btnPause.Enabled = true;

            lblInstructions.Text = "?? GAME CONTROLS\n\n" +
                             "Use Arrow Keys to Move\n" +
                             "Eat red food to grow\n" +
                             "Avoid walls and yourself\n" +
                             "You have 3 lives\n\n" +
                             "?? TIPS\n\n" +
                             "• Spacebar to Pause/Resume\n" +
                             "• Plan your moves ahead\n" +
                             "• Try to get high score!\n\n" +
                             "Click 'Start Game' to begin!";
            lblInstructions.AutoSize = true;
            lblInstructions.Font = new Font("Arial", 10);
            lblInstructions.Visible = true;

        }
        private void DrawEnhancedGrid()
        {
            using (Graphics g = Graphics.FromImage(gridBitmap))
            using (Pen gridPen = new Pen(gridColor, 1))
            {
                g.Clear(panelBackground); // Uses the custom panel background

                // Draw grid lines
                for (int x = 0; x < cols; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        g.DrawRectangle(gridPen, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }

                // Draw border around the grid
                using (Pen borderPen = new Pen(Color.FromArgb(100, 100, 100), 2))
                {
                    g.DrawRectangle(borderPen, 0, 0, pnlGame.Width - 1, pnlGame.Height - 1);
                }
            }
        }

        private void tmrGame_Tick(object sender, EventArgs e)
        {
            // Calculate new head
            Point newHead = new Point(
                snake[0].X + snakeDirectionX,
                snake[0].Y + snakeDirectionY
            );

            // Check collision with walls first
            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= cols || newHead.Y >= rows || snake.Contains(newHead))
            {
                System.Media.SystemSounds.Hand.Play();

                lives--;  // lose one life

                lblLives.Text = "❤️ Lives: " + lives;
                if (lives <= 0)
                {
                    if (tmrGame != null)
                        tmrGame.Stop();

                    MessageBox.Show("Game Over! Final Score: " + score);
                    if (score > highScore)
                    {
                        highScore = score;
                        lblHighScore.Text = "🏆 High Score: " + highScore;
                    }
                    return; // stop further execution
                }
                else
                {
                    // Reset snake to center
                    int startX = cols / 2;
                    int startY = rows / 2;
                    snake.Clear();
                    snake.Add(new Point(startX, startY));
                    snake.Add(new Point(startX - 1, startY));
                    snake.Add(new Point(startX - 2, startY));

                    // Reset direction
                    snakeDirectionX = 1;
                    snakeDirectionY = 0;

                    pnlGame.Invalidate(); // redraw
                    return; // IMPORTANT: stop further execution this tick
                }
            }

            // Move snake normally
            snake.Insert(0, newHead);

            // Check food collision
            if (newHead == food)
            {
                score += 10;
                lblScore.Text = "Score: " + score;
                UpdateGameUI(); // Update UI after scoring
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            pnlGame.Invalidate();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (snakeDirectionY != 1)
                    {
                        snakeDirectionX = 0;
                        snakeDirectionY = -1;
                    }
                    break;
                case Keys.Down:
                    if (snakeDirectionY != -1)
                    {
                        snakeDirectionX = 0;
                        snakeDirectionY = 1;
                    }
                    break;
                case Keys.Left:
                    if (snakeDirectionX != 1)
                    {
                        snakeDirectionX = -1;
                        snakeDirectionY = 0;
                    }
                    break;
                case Keys.Right:
                    if (snakeDirectionX != -1)
                    {
                        snakeDirectionX = 1;
                        snakeDirectionY = 0;
                    }
                    break;
                case Keys.Space:
                    // Toggle pause with spacebar
                    if (gameStarted && !gameEnded)
                        btnPause.PerformClick();
                    break;
            }
        }
        private void GenerateFood()
        {
            Point newFood;
            do
            {
                int x = rand.Next(0, cols);
                int y = rand.Next(0, rows);
                newFood = new Point(x, y);
            }
            while (snake.Contains(newFood));

            food = newFood;
        }
        private void UpdateGameUI()
        {
            // Update labels with emojis for better visual appeal
            lblScore.Text = $"⭐ Score: {score}";
            lblLives.Text = $"❤️ Lives: {lives}";
            lblHighScore.Text = $"🏆 High Score: {highScore}";
        }
        private void InitializeSnake()
        {
            int startX = cols / 2;
            int startY = rows / 2;

            snake.Clear();
            snake.Add(new Point(startX, startY));
            snake.Add(new Point(startX - 1, startY));
            snake.Add(new Point(startX - 2, startY));

            snakeDirectionX = 1;
            snakeDirectionY = 0;
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (gameStarted && !gameEnded)
            {
                if (!gamePaused)
                {
                    // Pause game
                    gamePaused = true;
                    tmrGame.Stop();
                    btnPause.Text = "? Resume"; // Changed text
                    pnlGame.Invalidate();
                }
                else
                {
                    // Resume game
                    gamePaused = false;
                    tmrGame.Start();
                    btnPause.Text = "? Pause"; // Changed text
                    pnlGame.Invalidate();
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblInstructions.Visible = false;
            // Reset game stats
            score = 0;
            lives = 3;
            UpdateGameUI();

            // Initialize game objects
            InitializeSnake();
            GenerateFood();

            // Set game states
            gameStarted = true;
            gamePaused = false;
            gameEnded = false;
            // Start timer
            tmrGame.Start();

            // Update UI
            btnStart.Enabled = true;
            btnPause.Enabled = true;

            // lblInstructions.Visible = false;

            pnlGame.Invalidate();
            pnlGame.Focus();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Minimum size check
            if (this.ClientSize.Width < 900 || this.ClientSize.Height < 650) // Increased minimum size
                return;

            // Maintain aspect ratio (16:9 ya 4:3)
            float aspectRatio = 16f / 9f;
            int targetWidth = (int)(this.ClientSize.Height * aspectRatio);

            // Calculate total width including all panels
            Control settingsPanel = this.Controls.Find("pnlFoodSettings", true).FirstOrDefault();
            Control colorPanel = this.Controls.Find("pnlColorSettings", true).FirstOrDefault();

            int extraWidth = 0;
            if (settingsPanel != null) extraWidth += settingsPanel.Width + 20;
            if (colorPanel != null) extraWidth += colorPanel.Width + 20;

            int totalWidth = pnlGame.Width + gbControls.Width + 40 + extraWidth;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int startY = (this.ClientSize.Height - Math.Max(pnlGame.Height, gbControls.Height)) / 2;

            // Game panel position
            pnlGame.Location = new Point(startX, startY);

            // Controls group position
            gbControls.Location = new Point(startX + pnlGame.Width + 20, startY);

            // Food shape panel position
            if (settingsPanel != null)
            {
                settingsPanel.Location = new Point(
                    gbControls.Right + 20,
                    startY
                );
            }

            // Color panel position
            if (colorPanel != null)
            {
                if (settingsPanel != null)
                {
                    colorPanel.Location = new Point(
                        settingsPanel.Right + 20,
                        startY
                    );
                }
                else
                {
                    colorPanel.Location = new Point(
                        gbControls.Right + 20,
                        startY
                    );
                }
            }

            // Position instructions label below everything
            if (!gbControls.Controls.Contains(lblInstructions))
            {
                int bottomMost = Math.Max(
                    pnlGame.Bottom,
                    Math.Max(
                        gbControls.Bottom,
                        Math.Max(
                            settingsPanel?.Bottom ?? 0,
                            colorPanel?.Bottom ?? 0
                        )
                    )
                );

                lblInstructions.Location = new Point(
                    pnlGame.Left,
                    bottomMost + 20
                );

                // Calculate total width of all side panels
                int sidePanelsWidth = 0;
                if (settingsPanel != null) sidePanelsWidth = Math.Max(sidePanelsWidth, settingsPanel.Right);
                if (colorPanel != null) sidePanelsWidth = Math.Max(sidePanelsWidth, colorPanel.Right);

                lblInstructions.Width = Math.Max(
                    pnlGame.Width,
                    (sidePanelsWidth > 0 ? sidePanelsWidth - pnlGame.Left : pnlGame.Width + gbControls.Width + 40)
                );

                lblInstructions.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var property = typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            property?.SetValue(control, enable, null);
        }
    }

}