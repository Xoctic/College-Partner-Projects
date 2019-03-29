namespace BoggleClient
{
    partial class BoggleView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RegisterUserButton = new System.Windows.Forms.Button();
            this.TimeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.JoinGameButton = new System.Windows.Forms.Button();
            this.BogleGrid = new System.Windows.Forms.TableLayoutPanel();
            this.cell16 = new System.Windows.Forms.Label();
            this.cell15 = new System.Windows.Forms.Label();
            this.cell14 = new System.Windows.Forms.Label();
            this.cell13 = new System.Windows.Forms.Label();
            this.cell12 = new System.Windows.Forms.Label();
            this.cell11 = new System.Windows.Forms.Label();
            this.cell10 = new System.Windows.Forms.Label();
            this.cell9 = new System.Windows.Forms.Label();
            this.cell8 = new System.Windows.Forms.Label();
            this.cell7 = new System.Windows.Forms.Label();
            this.cell6 = new System.Windows.Forms.Label();
            this.cell5 = new System.Windows.Forms.Label();
            this.cell4 = new System.Windows.Forms.Label();
            this.cell3 = new System.Windows.Forms.Label();
            this.cell2 = new System.Windows.Forms.Label();
            this.cell1 = new System.Windows.Forms.Label();
            this.QuitGameButton = new System.Windows.Forms.Button();
            this.WordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.Player1Title = new System.Windows.Forms.Label();
            this.Player2Title = new System.Windows.Forms.Label();
            this.SecondsLabel = new System.Windows.Forms.Label();
            this.TimeLeftLabel = new System.Windows.Forms.Label();
            this.Player1NameLabel = new System.Windows.Forms.Label();
            this.Player2NameLabel = new System.Windows.Forms.Label();
            this.Player1ScoreLabel = new System.Windows.Forms.Label();
            this.Player2ScoreLabel = new System.Windows.Forms.Label();
            this.Player1WordsPlayedTitle = new System.Windows.Forms.Label();
            this.Player2WordsPlayedTitle = new System.Windows.Forms.Label();
            this.Player1WordsPlayed = new System.Windows.Forms.Label();
            this.Player2WordsPlayed = new System.Windows.Forms.Label();
            this.BogleGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            this.NameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameTextBox.Location = new System.Drawing.Point(456, 78);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NameTextBox.MaxLength = 100;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(468, 35);
            this.NameTextBox.TabIndex = 0;
            this.NameTextBox.TextChanged += new System.EventHandler(this.Registration_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(362, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerTextBox.Location = new System.Drawing.Point(456, 128);
            this.ServerTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ServerTextBox.MaxLength = 100;
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(468, 35);
            this.ServerTextBox.TabIndex = 2;
            this.ServerTextBox.TextChanged += new System.EventHandler(this.Registration_TextChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(356, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 40);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // RegisterUserButton
            // 
            this.RegisterUserButton.Enabled = false;
            this.RegisterUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegisterUserButton.Location = new System.Drawing.Point(362, 177);
            this.RegisterUserButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RegisterUserButton.Name = "RegisterUserButton";
            this.RegisterUserButton.Size = new System.Drawing.Size(564, 43);
            this.RegisterUserButton.TabIndex = 4;
            this.RegisterUserButton.Text = "Register User";
            this.RegisterUserButton.UseVisualStyleBackColor = true;
            this.RegisterUserButton.Click += new System.EventHandler(this.RegisterUserButton_Click);
            // 
            // TimeTextBox
            // 
            this.TimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeTextBox.Location = new System.Drawing.Point(456, 243);
            this.TimeTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TimeTextBox.MaxLength = 100;
            this.TimeTextBox.Name = "TimeTextBox";
            this.TimeTextBox.Size = new System.Drawing.Size(468, 35);
            this.TimeTextBox.TabIndex = 5;
            this.TimeTextBox.TextChanged += new System.EventHandler(this.TimeLimit_TextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(315, 243);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 40);
            this.label3.TabIndex = 6;
            this.label3.Text = "Time Limit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JoinGameButton
            // 
            this.JoinGameButton.Enabled = false;
            this.JoinGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JoinGameButton.Location = new System.Drawing.Point(362, 306);
            this.JoinGameButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.JoinGameButton.Name = "JoinGameButton";
            this.JoinGameButton.Size = new System.Drawing.Size(374, 43);
            this.JoinGameButton.TabIndex = 7;
            this.JoinGameButton.Text = "Join Game";
            this.JoinGameButton.UseVisualStyleBackColor = true;
            this.JoinGameButton.Click += new System.EventHandler(this.JoinGameButton_Click);
            // 
            // BogleGrid
            // 
            this.BogleGrid.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.BogleGrid.ColumnCount = 4;
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.BogleGrid.Controls.Add(this.cell16, 3, 3);
            this.BogleGrid.Controls.Add(this.cell15, 2, 3);
            this.BogleGrid.Controls.Add(this.cell14, 1, 3);
            this.BogleGrid.Controls.Add(this.cell13, 0, 3);
            this.BogleGrid.Controls.Add(this.cell12, 3, 2);
            this.BogleGrid.Controls.Add(this.cell11, 2, 2);
            this.BogleGrid.Controls.Add(this.cell10, 1, 2);
            this.BogleGrid.Controls.Add(this.cell9, 0, 2);
            this.BogleGrid.Controls.Add(this.cell8, 3, 1);
            this.BogleGrid.Controls.Add(this.cell7, 2, 1);
            this.BogleGrid.Controls.Add(this.cell6, 1, 1);
            this.BogleGrid.Controls.Add(this.cell5, 0, 1);
            this.BogleGrid.Controls.Add(this.cell4, 3, 0);
            this.BogleGrid.Controls.Add(this.cell3, 2, 0);
            this.BogleGrid.Controls.Add(this.cell2, 1, 0);
            this.BogleGrid.Controls.Add(this.cell1, 0, 0);
            this.BogleGrid.Location = new System.Drawing.Point(362, 372);
            this.BogleGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BogleGrid.Name = "BogleGrid";
            this.BogleGrid.RowCount = 4;
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.BogleGrid.Size = new System.Drawing.Size(564, 591);
            this.BogleGrid.TabIndex = 8;
            // 
            // cell16
            // 
            this.cell16.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell16.Location = new System.Drawing.Point(418, 441);
            this.cell16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell16.Name = "cell16";
            this.cell16.Size = new System.Drawing.Size(126, 145);
            this.cell16.TabIndex = 15;
            this.cell16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell15
            // 
            this.cell15.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell15.Location = new System.Drawing.Point(277, 441);
            this.cell15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell15.Name = "cell15";
            this.cell15.Size = new System.Drawing.Size(126, 145);
            this.cell15.TabIndex = 14;
            this.cell15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell14
            // 
            this.cell14.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell14.Location = new System.Drawing.Point(141, 441);
            this.cell14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell14.Name = "cell14";
            this.cell14.Size = new System.Drawing.Size(126, 145);
            this.cell14.TabIndex = 13;
            this.cell14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell13
            // 
            this.cell13.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell13.Location = new System.Drawing.Point(5, 441);
            this.cell13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell13.Name = "cell13";
            this.cell13.Size = new System.Drawing.Size(126, 145);
            this.cell13.TabIndex = 12;
            this.cell13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell12
            // 
            this.cell12.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell12.Location = new System.Drawing.Point(418, 295);
            this.cell12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell12.Name = "cell12";
            this.cell12.Size = new System.Drawing.Size(126, 145);
            this.cell12.TabIndex = 11;
            this.cell12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell11
            // 
            this.cell11.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell11.Location = new System.Drawing.Point(277, 295);
            this.cell11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell11.Name = "cell11";
            this.cell11.Size = new System.Drawing.Size(126, 145);
            this.cell11.TabIndex = 10;
            this.cell11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell10
            // 
            this.cell10.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell10.Location = new System.Drawing.Point(141, 295);
            this.cell10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell10.Name = "cell10";
            this.cell10.Size = new System.Drawing.Size(126, 145);
            this.cell10.TabIndex = 9;
            this.cell10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell9
            // 
            this.cell9.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell9.Location = new System.Drawing.Point(5, 295);
            this.cell9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell9.Name = "cell9";
            this.cell9.Size = new System.Drawing.Size(126, 145);
            this.cell9.TabIndex = 8;
            this.cell9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell8
            // 
            this.cell8.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell8.Location = new System.Drawing.Point(418, 148);
            this.cell8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell8.Name = "cell8";
            this.cell8.Size = new System.Drawing.Size(126, 145);
            this.cell8.TabIndex = 7;
            this.cell8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell7
            // 
            this.cell7.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell7.Location = new System.Drawing.Point(277, 148);
            this.cell7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell7.Name = "cell7";
            this.cell7.Size = new System.Drawing.Size(126, 145);
            this.cell7.TabIndex = 6;
            this.cell7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell6
            // 
            this.cell6.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell6.Location = new System.Drawing.Point(141, 148);
            this.cell6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell6.Name = "cell6";
            this.cell6.Size = new System.Drawing.Size(126, 145);
            this.cell6.TabIndex = 5;
            this.cell6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell5
            // 
            this.cell5.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell5.Location = new System.Drawing.Point(5, 148);
            this.cell5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell5.Name = "cell5";
            this.cell5.Size = new System.Drawing.Size(126, 145);
            this.cell5.TabIndex = 4;
            this.cell5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell4
            // 
            this.cell4.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell4.Location = new System.Drawing.Point(418, 1);
            this.cell4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell4.Name = "cell4";
            this.cell4.Size = new System.Drawing.Size(126, 145);
            this.cell4.TabIndex = 3;
            this.cell4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell3
            // 
            this.cell3.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell3.Location = new System.Drawing.Point(277, 1);
            this.cell3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell3.Name = "cell3";
            this.cell3.Size = new System.Drawing.Size(126, 145);
            this.cell3.TabIndex = 2;
            this.cell3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell2
            // 
            this.cell2.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell2.Location = new System.Drawing.Point(141, 1);
            this.cell2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell2.Name = "cell2";
            this.cell2.Size = new System.Drawing.Size(126, 145);
            this.cell2.TabIndex = 1;
            this.cell2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell1
            // 
            this.cell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell1.Location = new System.Drawing.Point(5, 1);
            this.cell1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cell1.Name = "cell1";
            this.cell1.Size = new System.Drawing.Size(126, 145);
            this.cell1.TabIndex = 0;
            this.cell1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QuitGameButton
            // 
            this.QuitGameButton.Enabled = false;
            this.QuitGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuitGameButton.Location = new System.Drawing.Point(362, 988);
            this.QuitGameButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.QuitGameButton.Name = "QuitGameButton";
            this.QuitGameButton.Size = new System.Drawing.Size(564, 43);
            this.QuitGameButton.TabIndex = 9;
            this.QuitGameButton.Text = "Quit Game";
            this.QuitGameButton.UseVisualStyleBackColor = true;
            this.QuitGameButton.Click += new System.EventHandler(this.QuitGameButton_Click);
            // 
            // WordTextBox
            // 
            this.WordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordTextBox.Location = new System.Drawing.Point(456, 1060);
            this.WordTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WordTextBox.MaxLength = 100;
            this.WordTextBox.Name = "WordTextBox";
            this.WordTextBox.Size = new System.Drawing.Size(468, 35);
            this.WordTextBox.TabIndex = 10;
            this.WordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordTextBox_EnterPressed);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(368, 1060);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 40);
            this.label4.TabIndex = 11;
            this.label4.Text = "Word";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(761, 307);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(165, 43);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel Join Game";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Visible = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Player1Title
            // 
            this.Player1Title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Player1Title.AutoSize = true;
            this.Player1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1Title.Location = new System.Drawing.Point(78, 30);
            this.Player1Title.Name = "Player1Title";
            this.Player1Title.Size = new System.Drawing.Size(127, 32);
            this.Player1Title.TabIndex = 13;
            this.Player1Title.Text = "Player 1";
            this.Player1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player2Title
            // 
            this.Player2Title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Player2Title.AutoSize = true;
            this.Player2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2Title.Location = new System.Drawing.Point(1032, 30);
            this.Player2Title.Name = "Player2Title";
            this.Player2Title.Size = new System.Drawing.Size(127, 32);
            this.Player2Title.TabIndex = 14;
            this.Player2Title.Text = "Player 2";
            this.Player2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SecondsLabel
            // 
            this.SecondsLabel.AutoSize = true;
            this.SecondsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondsLabel.ForeColor = System.Drawing.Color.Red;
            this.SecondsLabel.Location = new System.Drawing.Point(692, 14);
            this.SecondsLabel.Name = "SecondsLabel";
            this.SecondsLabel.Size = new System.Drawing.Size(73, 52);
            this.SecondsLabel.TabIndex = 15;
            this.SecondsLabel.Text = "55";
            // 
            // TimeLeftLabel
            // 
            this.TimeLeftLabel.AutoSize = true;
            this.TimeLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLeftLabel.Location = new System.Drawing.Point(461, 14);
            this.TimeLeftLabel.Name = "TimeLeftLabel";
            this.TimeLeftLabel.Size = new System.Drawing.Size(218, 46);
            this.TimeLeftLabel.TabIndex = 16;
            this.TimeLeftLabel.Text = "Time Left :";
            // 
            // Player1NameLabel
            // 
            this.Player1NameLabel.AutoSize = true;
            this.Player1NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1NameLabel.Location = new System.Drawing.Point(24, 89);
            this.Player1NameLabel.Name = "Player1NameLabel";
            this.Player1NameLabel.Size = new System.Drawing.Size(96, 29);
            this.Player1NameLabel.TabIndex = 17;
            this.Player1NameLabel.Text = "Name1";
            // 
            // Player2NameLabel
            // 
            this.Player2NameLabel.AutoSize = true;
            this.Player2NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2NameLabel.Location = new System.Drawing.Point(972, 89);
            this.Player2NameLabel.Name = "Player2NameLabel";
            this.Player2NameLabel.Size = new System.Drawing.Size(103, 29);
            this.Player2NameLabel.TabIndex = 18;
            this.Player2NameLabel.Text = "Name 2";
            // 
            // Player1ScoreLabel
            // 
            this.Player1ScoreLabel.AutoSize = true;
            this.Player1ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1ScoreLabel.Location = new System.Drawing.Point(25, 138);
            this.Player1ScoreLabel.Name = "Player1ScoreLabel";
            this.Player1ScoreLabel.Size = new System.Drawing.Size(49, 32);
            this.Player1ScoreLabel.TabIndex = 19;
            this.Player1ScoreLabel.Text = "10";
            // 
            // Player2ScoreLabel
            // 
            this.Player2ScoreLabel.AutoSize = true;
            this.Player2ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2ScoreLabel.Location = new System.Drawing.Point(971, 138);
            this.Player2ScoreLabel.Name = "Player2ScoreLabel";
            this.Player2ScoreLabel.Size = new System.Drawing.Size(49, 32);
            this.Player2ScoreLabel.TabIndex = 20;
            this.Player2ScoreLabel.Text = "10";
            // 
            // Player1WordsPlayedTitle
            // 
            this.Player1WordsPlayedTitle.AutoSize = true;
            this.Player1WordsPlayedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1WordsPlayedTitle.Location = new System.Drawing.Point(64, 220);
            this.Player1WordsPlayedTitle.Name = "Player1WordsPlayedTitle";
            this.Player1WordsPlayedTitle.Size = new System.Drawing.Size(203, 32);
            this.Player1WordsPlayedTitle.TabIndex = 21;
            this.Player1WordsPlayedTitle.Text = "Words Played";
            // 
            // Player2WordsPlayedTitle
            // 
            this.Player2WordsPlayedTitle.AutoSize = true;
            this.Player2WordsPlayedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2WordsPlayedTitle.Location = new System.Drawing.Point(989, 220);
            this.Player2WordsPlayedTitle.Name = "Player2WordsPlayedTitle";
            this.Player2WordsPlayedTitle.Size = new System.Drawing.Size(203, 32);
            this.Player2WordsPlayedTitle.TabIndex = 22;
            this.Player2WordsPlayedTitle.Text = "Words Played";
            // 
            // Player1WordsPlayed
            // 
            this.Player1WordsPlayed.AutoSize = true;
            this.Player1WordsPlayed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1WordsPlayed.Location = new System.Drawing.Point(111, 307);
            this.Player1WordsPlayed.Name = "Player1WordsPlayed";
            this.Player1WordsPlayed.Size = new System.Drawing.Size(75, 29);
            this.Player1WordsPlayed.TabIndex = 23;
            this.Player1WordsPlayed.Text = "Poop";
            // 
            // Player2WordsPlayed
            // 
            this.Player2WordsPlayed.AutoSize = true;
            this.Player2WordsPlayed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2WordsPlayed.Location = new System.Drawing.Point(1050, 313);
            this.Player2WordsPlayed.Name = "Player2WordsPlayed";
            this.Player2WordsPlayed.Size = new System.Drawing.Size(75, 29);
            this.Player2WordsPlayed.TabIndex = 24;
            this.Player2WordsPlayed.Text = "Poop";
            // 
            // BoggleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 1157);
            this.Controls.Add(this.Player2WordsPlayed);
            this.Controls.Add(this.Player1WordsPlayed);
            this.Controls.Add(this.Player2WordsPlayedTitle);
            this.Controls.Add(this.Player1WordsPlayedTitle);
            this.Controls.Add(this.Player2ScoreLabel);
            this.Controls.Add(this.Player1ScoreLabel);
            this.Controls.Add(this.Player2NameLabel);
            this.Controls.Add(this.Player1NameLabel);
            this.Controls.Add(this.TimeLeftLabel);
            this.Controls.Add(this.SecondsLabel);
            this.Controls.Add(this.Player2Title);
            this.Controls.Add(this.Player1Title);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.WordTextBox);
            this.Controls.Add(this.QuitGameButton);
            this.Controls.Add(this.BogleGrid);
            this.Controls.Add(this.JoinGameButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TimeTextBox);
            this.Controls.Add(this.RegisterUserButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTextBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BoggleView";
            this.Text = "Form1";
            this.BogleGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RegisterUserButton;
        private System.Windows.Forms.TextBox TimeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button JoinGameButton;
        private System.Windows.Forms.TableLayoutPanel BogleGrid;
        private System.Windows.Forms.Button QuitGameButton;
        private System.Windows.Forms.TextBox WordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label cell16;
        private System.Windows.Forms.Label cell15;
        private System.Windows.Forms.Label cell14;
        private System.Windows.Forms.Label cell13;
        private System.Windows.Forms.Label cell12;
        private System.Windows.Forms.Label cell11;
        private System.Windows.Forms.Label cell10;
        private System.Windows.Forms.Label cell9;
        private System.Windows.Forms.Label cell8;
        private System.Windows.Forms.Label cell7;
        private System.Windows.Forms.Label cell6;
        private System.Windows.Forms.Label cell5;
        private System.Windows.Forms.Label cell4;
        private System.Windows.Forms.Label cell3;
        private System.Windows.Forms.Label cell2;
        private System.Windows.Forms.Label cell1;
        private System.Windows.Forms.Label Player1Title;
        private System.Windows.Forms.Label Player2Title;
        private System.Windows.Forms.Label SecondsLabel;
        private System.Windows.Forms.Label TimeLeftLabel;
        private System.Windows.Forms.Label Player1NameLabel;
        private System.Windows.Forms.Label Player2NameLabel;
        private System.Windows.Forms.Label Player1ScoreLabel;
        private System.Windows.Forms.Label Player2ScoreLabel;
        private System.Windows.Forms.Label Player1WordsPlayedTitle;
        private System.Windows.Forms.Label Player2WordsPlayedTitle;
        private System.Windows.Forms.Label Player1WordsPlayed;
        private System.Windows.Forms.Label Player2WordsPlayed;
    }
}

