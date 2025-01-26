namespace SkiasGameOfLive;

partial class GameOfLiveForm
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnStart = new System.Windows.Forms.Button();
        GameOfLiveView = new System.Windows.Forms.PictureBox();
        statusLabel = new System.Windows.Forms.Label();
        btnStop = new System.Windows.Forms.Button();
        probabilitySelector = new System.Windows.Forms.NumericUpDown();
        lblProbability = new System.Windows.Forms.Label();
        lblSpeed = new System.Windows.Forms.Label();
        systemSpeedSelector = new System.Windows.Forms.NumericUpDown();
        cbPattern = new System.Windows.Forms.ComboBox();
        lblPattern = new System.Windows.Forms.Label();
        btnReset = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)GameOfLiveView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).BeginInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).BeginInit();
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.Location = new System.Drawing.Point(838, 48);
        btnStart.Name = "btnStart";
        btnStart.Size = new System.Drawing.Size(92, 23);
        btnStart.TabIndex = 0;
        btnStart.Text = "Start";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += startGameOfLive_Click;
        // 
        // GameOfLiveView
        // 
        GameOfLiveView.Location = new System.Drawing.Point(13, 49);
        GameOfLiveView.Name = "GameOfLiveView";
        GameOfLiveView.Size = new System.Drawing.Size(800, 600);
        GameOfLiveView.TabIndex = 1;
        GameOfLiveView.TabStop = false;
        // 
        // statusLabel
        // 
        statusLabel.Location = new System.Drawing.Point(13, 23);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new System.Drawing.Size(100, 23);
        statusLabel.TabIndex = 2;
        statusLabel.Text = "Generation";
        // 
        // btnStop
        // 
        btnStop.Location = new System.Drawing.Point(838, 78);
        btnStop.Name = "btnStop";
        btnStop.Size = new System.Drawing.Size(92, 23);
        btnStop.TabIndex = 4;
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;
        // 
        // probabilitySelector
        // 
        probabilitySelector.DecimalPlaces = 2;
        probabilitySelector.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
        probabilitySelector.Location = new System.Drawing.Point(1112, 49);
        probabilitySelector.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        probabilitySelector.Name = "probabilitySelector";
        probabilitySelector.Size = new System.Drawing.Size(66, 23);
        probabilitySelector.TabIndex = 5;
        probabilitySelector.Value = new decimal(new int[] { 2, 0, 0, 65536 });
        // 
        // lblProbability
        // 
        lblProbability.Location = new System.Drawing.Point(954, 49);
        lblProbability.Name = "lblProbability";
        lblProbability.Size = new System.Drawing.Size(152, 23);
        lblProbability.TabIndex = 6;
        lblProbability.Text = "Anfangswahrscheinlichkeit";
        // 
        // lblSpeed
        // 
        lblSpeed.Location = new System.Drawing.Point(954, 78);
        lblSpeed.Name = "lblSpeed";
        lblSpeed.Size = new System.Drawing.Size(152, 23);
        lblSpeed.TabIndex = 8;
        lblSpeed.Text = "Geschwindigkeit";
        // 
        // systemSpeedSelector
        // 
        systemSpeedSelector.Location = new System.Drawing.Point(1112, 78);
        systemSpeedSelector.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
        systemSpeedSelector.Name = "systemSpeedSelector";
        systemSpeedSelector.Size = new System.Drawing.Size(66, 23);
        systemSpeedSelector.TabIndex = 7;
        systemSpeedSelector.Value = new decimal(new int[] { 100, 0, 0, 0 });
        // 
        // cbPattern
        // 
        cbPattern.FormattingEnabled = true;
        cbPattern.Items.AddRange(new object[] { "Random", "Schachbrett", "Free Style" });
        cbPattern.Location = new System.Drawing.Point(1059, 138);
        cbPattern.Name = "cbPattern";
        cbPattern.Size = new System.Drawing.Size(119, 23);
        cbPattern.TabIndex = 9;
        cbPattern.Text = "Random";
        // 
        // lblPattern
        // 
        lblPattern.Location = new System.Drawing.Point(954, 138);
        lblPattern.Name = "lblPattern";
        lblPattern.Size = new System.Drawing.Size(99, 23);
        lblPattern.TabIndex = 10;
        lblPattern.Text = "Muster";
        // 
        // btnReset
        // 
        btnReset.Location = new System.Drawing.Point(838, 138);
        btnReset.Name = "btnReset";
        btnReset.Size = new System.Drawing.Size(92, 23);
        btnReset.TabIndex = 11;
        btnReset.Text = "Reset";
        btnReset.UseVisualStyleBackColor = true;
        btnReset.Click += btnReset_Click;
        // 
        // GameOfLiveForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(1264, 681);
        Controls.Add(btnReset);
        Controls.Add(lblPattern);
        Controls.Add(cbPattern);
        Controls.Add(lblSpeed);
        Controls.Add(systemSpeedSelector);
        Controls.Add(lblProbability);
        Controls.Add(probabilitySelector);
        Controls.Add(btnStop);
        Controls.Add(statusLabel);
        Controls.Add(GameOfLiveView);
        Controls.Add(btnStart);
        Location = new System.Drawing.Point(15, 15);
        MaximumSize = new System.Drawing.Size(1280, 720);
        MinimumSize = new System.Drawing.Size(1280, 720);
        ((System.ComponentModel.ISupportInitialize)GameOfLiveView).EndInit();
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).EndInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnReset;

    private System.Windows.Forms.ComboBox cbPattern;
    private System.Windows.Forms.Label lblPattern;

    private System.Windows.Forms.Label lblProbability;
    private System.Windows.Forms.Label lblSpeed;
    private System.Windows.Forms.NumericUpDown systemSpeedSelector;

    private System.Windows.Forms.NumericUpDown probabilitySelector;

    private System.Windows.Forms.Button btnStop;

    private System.Windows.Forms.PictureBox GameOfLiveView;
    private System.Windows.Forms.Label statusLabel;

    private System.Windows.Forms.Button btnStart;

    #endregion
}