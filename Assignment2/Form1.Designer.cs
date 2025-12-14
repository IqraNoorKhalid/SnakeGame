namespace Assignment2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer tmrGame;
            pnlGame = new Panel();
            lblScore = new Label();
            lblLives = new Label();
            btnStart = new Button();
            lblHighScore = new Label();
            btnPause = new Button();
            lblInstructions = new Label();
            gbControls = new GroupBox();
            pnlColor = new Panel();
            btnResetColors = new Button();
            btnPanelColor = new Button();
            btnFoodColor = new Button();
            btnSnakeHeadColor = new Button();
            btnSnakeColor = new Button();
            lblColorTitle = new Label();
            pnlSettingsPanel = new Panel();
            lblFoodShape = new Label();
            tmrGame = new System.Windows.Forms.Timer(components);
            gbControls.SuspendLayout();
            pnlColor.SuspendLayout();
            pnlSettingsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tmrGame
            // 
            tmrGame.Interval = 150;
            tmrGame.Tick += tmrGame_Tick;
            // 
            // pnlGame
            // 
            pnlGame.BackColor = Color.FromArgb(0, 0, 64);
            pnlGame.BorderStyle = BorderStyle.FixedSingle;
            pnlGame.Location = new Point(20, 20);
            pnlGame.Name = "pnlGame";
            pnlGame.Size = new Size(600, 600);
            pnlGame.TabIndex = 0;
            pnlGame.TabStop = true;
            pnlGame.Paint += pnlGame_Paint;
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Location = new Point(20, 140);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(57, 20);
            lblScore.TabIndex = 1;
            lblScore.Text = "Score : ";
            // 
            // lblLives
            // 
            lblLives.AutoSize = true;
            lblLives.Location = new Point(20, 175);
            lblLives.Name = "lblLives";
            lblLives.Size = new Size(48, 20);
            lblLives.TabIndex = 2;
            lblLives.Text = "Lives :";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(20, 30);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(180, 40);
            btnStart.TabIndex = 3;
            btnStart.TabStop = false;
            btnStart.Text = "&Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // lblHighScore
            // 
            lblHighScore.AutoSize = true;
            lblHighScore.Location = new Point(20, 210);
            lblHighScore.Name = "lblHighScore";
            lblHighScore.Size = new Size(89, 20);
            lblHighScore.TabIndex = 4;
            lblHighScore.Text = "High Score :";
            // 
            // btnPause
            // 
            btnPause.Location = new Point(20, 80);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(180, 40);
            btnPause.TabIndex = 5;
            btnPause.Text = "&Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.Location = new Point(640, 333);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(84, 20);
            lblInstructions.TabIndex = 7;
            lblInstructions.Text = "Instructions";
            // 
            // gbControls
            // 
            gbControls.Controls.Add(lblHighScore);
            gbControls.Controls.Add(lblScore);
            gbControls.Controls.Add(btnPause);
            gbControls.Controls.Add(lblLives);
            gbControls.Controls.Add(btnStart);
            gbControls.Location = new Point(640, 20);
            gbControls.Name = "gbControls";
            gbControls.Size = new Size(220, 250);
            gbControls.TabIndex = 8;
            gbControls.TabStop = false;
            gbControls.Text = "Game Controls";
            // 
            // pnlColor
            // 
            pnlColor.Controls.Add(btnResetColors);
            pnlColor.Controls.Add(btnPanelColor);
            pnlColor.Controls.Add(btnFoodColor);
            pnlColor.Controls.Add(btnSnakeHeadColor);
            pnlColor.Controls.Add(btnSnakeColor);
            pnlColor.Controls.Add(lblColorTitle);
            pnlColor.Location = new Point(920, 33);
            pnlColor.Name = "pnlColor";
            pnlColor.Size = new Size(242, 285);
            pnlColor.TabIndex = 9;
            // 
            // btnResetColors
            // 
            btnResetColors.Location = new Point(22, 242);
            btnResetColors.Name = "btnResetColors";
            btnResetColors.Size = new Size(180, 30);
            btnResetColors.TabIndex = 5;
            btnResetColors.Text = "Reset";
            btnResetColors.UseVisualStyleBackColor = true;
            // 
            // btnPanelColor
            // 
            btnPanelColor.Location = new Point(22, 197);
            btnPanelColor.Name = "btnPanelColor";
            btnPanelColor.Size = new Size(180, 30);
            btnPanelColor.TabIndex = 4;
            btnPanelColor.Text = "Panel Color";
            btnPanelColor.UseVisualStyleBackColor = true;
            // 
            // btnFoodColor
            // 
            btnFoodColor.Location = new Point(22, 153);
            btnFoodColor.Name = "btnFoodColor";
            btnFoodColor.Size = new Size(180, 30);
            btnFoodColor.TabIndex = 3;
            btnFoodColor.Text = "Food Color";
            btnFoodColor.UseVisualStyleBackColor = true;
            // 
            // btnSnakeHeadColor
            // 
            btnSnakeHeadColor.Location = new Point(22, 102);
            btnSnakeHeadColor.Name = "btnSnakeHeadColor";
            btnSnakeHeadColor.Size = new Size(180, 30);
            btnSnakeHeadColor.TabIndex = 2;
            btnSnakeHeadColor.Text = "Snake Head Color";
            btnSnakeHeadColor.UseVisualStyleBackColor = true;
            // 
            // btnSnakeColor
            // 
            btnSnakeColor.Location = new Point(22, 55);
            btnSnakeColor.Name = "btnSnakeColor";
            btnSnakeColor.Size = new Size(180, 30);
            btnSnakeColor.TabIndex = 1;
            btnSnakeColor.Text = "Snake Color";
            btnSnakeColor.UseVisualStyleBackColor = true;
            // 
            // lblColorTitle
            // 
            lblColorTitle.AutoSize = true;
            lblColorTitle.Location = new Point(22, 22);
            lblColorTitle.Name = "lblColorTitle";
            lblColorTitle.Size = new Size(50, 20);
            lblColorTitle.TabIndex = 0;
            lblColorTitle.Text = "label1";
            // 
            // pnlSettingsPanel
            // 
            pnlSettingsPanel.Controls.Add(lblFoodShape);
            pnlSettingsPanel.Location = new Point(923, 341);
            pnlSettingsPanel.Name = "pnlSettingsPanel";
            pnlSettingsPanel.Size = new Size(250, 160);
            pnlSettingsPanel.TabIndex = 10;
            // 
            // lblFoodShape
            // 
            lblFoodShape.AutoSize = true;
            lblFoodShape.Location = new Point(16, 24);
            lblFoodShape.Name = "lblFoodShape";
            lblFoodShape.Size = new Size(88, 20);
            lblFoodShape.TabIndex = 0;
            lblFoodShape.Text = "Food Shape";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1209, 653);
            Controls.Add(pnlSettingsPanel);
            Controls.Add(pnlColor);
            Controls.Add(gbControls);
            Controls.Add(lblInstructions);
            Controls.Add(pnlGame);
            KeyPreview = true;
            MinimumSize = new Size(800, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            KeyDown += Form1_KeyDown;
            gbControls.ResumeLayout(false);
            gbControls.PerformLayout();
            pnlColor.ResumeLayout(false);
            pnlColor.PerformLayout();
            pnlSettingsPanel.ResumeLayout(false);
            pnlSettingsPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlGame;
        private System.Windows.Forms.Timer tmrGame;
        private Label lblScore;
        private Label lblLives;
        private Button btnStart;
        private Label lblHighScore;
        private Button btnPause;
        private Label lblInstructions;
        private GroupBox gbControls;
        private Panel pnlColor;
        private Label lblColorTitle;
        private Button btnSnakeColor;
        private Button btnSnakeHeadColor;
        private Button btnFoodColor;
        private Button btnPanelColor;
        private Button btnResetColors;
        private Panel pnlSettingsPanel;
        private Label lblFoodShape;
    }
}
