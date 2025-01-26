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
        button1 = new System.Windows.Forms.Button();
        GameOfLiveView = new System.Windows.Forms.PictureBox();
        statusLabel = new System.Windows.Forms.Label();
        btnPause = new System.Windows.Forms.Button();
        btnStopReset = new System.Windows.Forms.Button();
        probabilitySelector = new System.Windows.Forms.NumericUpDown();
        lblProbability = new System.Windows.Forms.Label();
        lblSpeed = new System.Windows.Forms.Label();
        systemSpeedSelector = new System.Windows.Forms.NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)GameOfLiveView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).BeginInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).BeginInit();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(838, 49);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(92, 23);
        button1.TabIndex = 0;
        button1.Text = "Start";
        button1.UseVisualStyleBackColor = true;
        button1.Click += startGameOfLive_Click;
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
        // btnPause
        // 
        btnPause.Location = new System.Drawing.Point(838, 78);
        btnPause.Name = "btnPause";
        btnPause.Size = new System.Drawing.Size(92, 23);
        btnPause.TabIndex = 3;
        btnPause.Text = "Pause";
        btnPause.UseVisualStyleBackColor = true;
        btnPause.Click += btnPause_Click;
        // 
        // btnStopReset
        // 
        btnStopReset.Location = new System.Drawing.Point(838, 138);
        btnStopReset.Name = "btnStopReset";
        btnStopReset.Size = new System.Drawing.Size(92, 23);
        btnStopReset.TabIndex = 4;
        btnStopReset.Text = "Stop && Reset";
        btnStopReset.UseVisualStyleBackColor = true;
        btnStopReset.Click += btnStopReset_Click;
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
        // GameOfLiveForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(1264, 681);
        Controls.Add(lblSpeed);
        Controls.Add(systemSpeedSelector);
        Controls.Add(lblProbability);
        Controls.Add(probabilitySelector);
        Controls.Add(btnStopReset);
        Controls.Add(btnPause);
        Controls.Add(statusLabel);
        Controls.Add(GameOfLiveView);
        Controls.Add(button1);
        Location = new System.Drawing.Point(15, 15);
        MaximumSize = new System.Drawing.Size(1280, 720);
        MinimumSize = new System.Drawing.Size(1280, 720);
        ((System.ComponentModel.ISupportInitialize)GameOfLiveView).EndInit();
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).EndInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblProbability;
    private System.Windows.Forms.Label lblSpeed;
    private System.Windows.Forms.NumericUpDown systemSpeedSelector;

    private System.Windows.Forms.NumericUpDown probabilitySelector;

    private System.Windows.Forms.Button btnStopReset;

    private System.Windows.Forms.Button btnPause;

    private System.Windows.Forms.PictureBox GameOfLiveView;
    private System.Windows.Forms.Label statusLabel;

    private System.Windows.Forms.Button button1;

    #endregion
}