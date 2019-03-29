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
            this.QuitGameButton = new System.Windows.Forms.Button();
            this.WordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.cell1 = new System.Windows.Forms.Label();
            this.cell2 = new System.Windows.Forms.Label();
            this.cell3 = new System.Windows.Forms.Label();
            this.cell4 = new System.Windows.Forms.Label();
            this.cell5 = new System.Windows.Forms.Label();
            this.cell6 = new System.Windows.Forms.Label();
            this.cell7 = new System.Windows.Forms.Label();
            this.cell8 = new System.Windows.Forms.Label();
            this.cell9 = new System.Windows.Forms.Label();
            this.cell10 = new System.Windows.Forms.Label();
            this.cell11 = new System.Windows.Forms.Label();
            this.cell12 = new System.Windows.Forms.Label();
            this.cell13 = new System.Windows.Forms.Label();
            this.cell14 = new System.Windows.Forms.Label();
            this.cell15 = new System.Windows.Forms.Label();
            this.cell16 = new System.Windows.Forms.Label();
            this.BogleGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            this.NameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameTextBox.Location = new System.Drawing.Point(304, 51);
            this.NameTextBox.MaxLength = 100;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(313, 26);
            this.NameTextBox.TabIndex = 0;
            this.NameTextBox.TextChanged += new System.EventHandler(this.Registration_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(241, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerTextBox.Location = new System.Drawing.Point(304, 83);
            this.ServerTextBox.MaxLength = 100;
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(313, 26);
            this.ServerTextBox.TabIndex = 2;
            this.ServerTextBox.TextChanged += new System.EventHandler(this.Registration_TextChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(237, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // RegisterUserButton
            // 
            this.RegisterUserButton.Enabled = false;
            this.RegisterUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegisterUserButton.Location = new System.Drawing.Point(241, 115);
            this.RegisterUserButton.Name = "RegisterUserButton";
            this.RegisterUserButton.Size = new System.Drawing.Size(376, 28);
            this.RegisterUserButton.TabIndex = 4;
            this.RegisterUserButton.Text = "Register User";
            this.RegisterUserButton.UseVisualStyleBackColor = true;
            this.RegisterUserButton.Click += new System.EventHandler(this.RegisterUserButton_Click);
            // 
            // TimeTextBox
            // 
            this.TimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeTextBox.Location = new System.Drawing.Point(304, 158);
            this.TimeTextBox.MaxLength = 100;
            this.TimeTextBox.Name = "TimeTextBox";
            this.TimeTextBox.Size = new System.Drawing.Size(313, 26);
            this.TimeTextBox.TabIndex = 5;
            this.TimeTextBox.TextChanged += new System.EventHandler(this.TimeLimit_TextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(210, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Time Limit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JoinGameButton
            // 
            this.JoinGameButton.Enabled = false;
            this.JoinGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JoinGameButton.Location = new System.Drawing.Point(241, 199);
            this.JoinGameButton.Name = "JoinGameButton";
            this.JoinGameButton.Size = new System.Drawing.Size(376, 28);
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
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.BogleGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
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
            this.BogleGrid.Location = new System.Drawing.Point(241, 242);
            this.BogleGrid.Name = "BogleGrid";
            this.BogleGrid.RowCount = 4;
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.BogleGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.BogleGrid.Size = new System.Drawing.Size(376, 384);
            this.BogleGrid.TabIndex = 8;
            // 
            // QuitGameButton
            // 
            this.QuitGameButton.Enabled = false;
            this.QuitGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuitGameButton.Location = new System.Drawing.Point(241, 642);
            this.QuitGameButton.Name = "QuitGameButton";
            this.QuitGameButton.Size = new System.Drawing.Size(376, 28);
            this.QuitGameButton.TabIndex = 9;
            this.QuitGameButton.Text = "Quit Game";
            this.QuitGameButton.UseVisualStyleBackColor = true;
            this.QuitGameButton.Click += new System.EventHandler(this.QuitGameButton_Click);
            // 
            // WordTextBox
            // 
            this.WordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordTextBox.Location = new System.Drawing.Point(304, 689);
            this.WordTextBox.MaxLength = 100;
            this.WordTextBox.Name = "WordTextBox";
            this.WordTextBox.Size = new System.Drawing.Size(313, 26);
            this.WordTextBox.TabIndex = 10;
            this.WordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordTextBox_EnterPressed);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(245, 689);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 26);
            this.label4.TabIndex = 11;
            this.label4.Text = "Word";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(632, 158);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(88, 69);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel Join Game";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Visible = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // cell1
            // 
            this.cell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell1.Location = new System.Drawing.Point(4, 1);
            this.cell1.Name = "cell1";
            this.cell1.Size = new System.Drawing.Size(84, 94);
            this.cell1.TabIndex = 0;
            this.cell1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell2
            // 
            this.cell2.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell2.Location = new System.Drawing.Point(95, 1);
            this.cell2.Name = "cell2";
            this.cell2.Size = new System.Drawing.Size(84, 94);
            this.cell2.TabIndex = 1;
            this.cell2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell3
            // 
            this.cell3.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell3.Location = new System.Drawing.Point(186, 1);
            this.cell3.Name = "cell3";
            this.cell3.Size = new System.Drawing.Size(84, 94);
            this.cell3.TabIndex = 2;
            this.cell3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell4
            // 
            this.cell4.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell4.Location = new System.Drawing.Point(280, 1);
            this.cell4.Name = "cell4";
            this.cell4.Size = new System.Drawing.Size(84, 94);
            this.cell4.TabIndex = 3;
            this.cell4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell5
            // 
            this.cell5.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell5.Location = new System.Drawing.Point(4, 96);
            this.cell5.Name = "cell5";
            this.cell5.Size = new System.Drawing.Size(84, 94);
            this.cell5.TabIndex = 4;
            this.cell5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell6
            // 
            this.cell6.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell6.Location = new System.Drawing.Point(95, 96);
            this.cell6.Name = "cell6";
            this.cell6.Size = new System.Drawing.Size(84, 94);
            this.cell6.TabIndex = 5;
            this.cell6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell7
            // 
            this.cell7.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell7.Location = new System.Drawing.Point(186, 96);
            this.cell7.Name = "cell7";
            this.cell7.Size = new System.Drawing.Size(84, 94);
            this.cell7.TabIndex = 6;
            this.cell7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell8
            // 
            this.cell8.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell8.Location = new System.Drawing.Point(280, 96);
            this.cell8.Name = "cell8";
            this.cell8.Size = new System.Drawing.Size(84, 94);
            this.cell8.TabIndex = 7;
            this.cell8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell9
            // 
            this.cell9.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell9.Location = new System.Drawing.Point(4, 191);
            this.cell9.Name = "cell9";
            this.cell9.Size = new System.Drawing.Size(84, 94);
            this.cell9.TabIndex = 8;
            this.cell9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell10
            // 
            this.cell10.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell10.Location = new System.Drawing.Point(95, 191);
            this.cell10.Name = "cell10";
            this.cell10.Size = new System.Drawing.Size(84, 94);
            this.cell10.TabIndex = 9;
            this.cell10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell11
            // 
            this.cell11.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell11.Location = new System.Drawing.Point(186, 191);
            this.cell11.Name = "cell11";
            this.cell11.Size = new System.Drawing.Size(84, 94);
            this.cell11.TabIndex = 10;
            this.cell11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell12
            // 
            this.cell12.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell12.Location = new System.Drawing.Point(280, 191);
            this.cell12.Name = "cell12";
            this.cell12.Size = new System.Drawing.Size(84, 94);
            this.cell12.TabIndex = 11;
            this.cell12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell13
            // 
            this.cell13.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell13.Location = new System.Drawing.Point(4, 286);
            this.cell13.Name = "cell13";
            this.cell13.Size = new System.Drawing.Size(84, 94);
            this.cell13.TabIndex = 12;
            this.cell13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell14
            // 
            this.cell14.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell14.Location = new System.Drawing.Point(95, 286);
            this.cell14.Name = "cell14";
            this.cell14.Size = new System.Drawing.Size(84, 94);
            this.cell14.TabIndex = 13;
            this.cell14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell15
            // 
            this.cell15.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell15.Location = new System.Drawing.Point(186, 286);
            this.cell15.Name = "cell15";
            this.cell15.Size = new System.Drawing.Size(84, 94);
            this.cell15.TabIndex = 14;
            this.cell15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cell16
            // 
            this.cell16.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cell16.Location = new System.Drawing.Point(280, 286);
            this.cell16.Name = "cell16";
            this.cell16.Size = new System.Drawing.Size(84, 94);
            this.cell16.TabIndex = 15;
            this.cell16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BoggleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 752);
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
    }
}

