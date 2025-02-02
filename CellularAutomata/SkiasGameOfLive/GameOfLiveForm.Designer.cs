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
        statusLabel = new System.Windows.Forms.Label();
        btnStop = new System.Windows.Forms.Button();
        probabilitySelector = new System.Windows.Forms.NumericUpDown();
        lblProbability = new System.Windows.Forms.Label();
        lblSpeed = new System.Windows.Forms.Label();
        systemSpeedSelector = new System.Windows.Forms.NumericUpDown();
        cbPattern = new System.Windows.Forms.ComboBox();
        lblPattern = new System.Windows.Forms.Label();
        btnReset = new System.Windows.Forms.Button();
        skControl1 = new SkiaSharp.Views.Desktop.SKControl();
        lblCellSize = new System.Windows.Forms.Label();
        cellSizeSelector = new System.Windows.Forms.NumericUpDown();
        GameOfLiveView = new SkiaSharp.Views.Desktop.SKControl();
        lblRuleSet = new System.Windows.Forms.Label();
        cbRuleSet = new System.Windows.Forms.ComboBox();
        btnSingleStep = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).BeginInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).BeginInit();
        ((System.ComponentModel.ISupportInitialize)cellSizeSelector).BeginInit();
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
        btnStop.Location = new System.Drawing.Point(838, 77);
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
        systemSpeedSelector.Value = new decimal(new int[] { 500, 0, 0, 0 });
        // 
        // cbPattern
        // 
        cbPattern.FormattingEnabled = true;
        cbPattern.Items.AddRange(new object[] { "Random", "Schachbrett", "Free Style" });
        cbPattern.Location = new System.Drawing.Point(1059, 164);
        cbPattern.Name = "cbPattern";
        cbPattern.Size = new System.Drawing.Size(119, 23);
        cbPattern.TabIndex = 9;
        cbPattern.Text = "Random";
        cbPattern.SelectedValueChanged += cbPattern_SelectedValueChanged;
        // 
        // lblPattern
        // 
        lblPattern.Location = new System.Drawing.Point(954, 164);
        lblPattern.Name = "lblPattern";
        lblPattern.Size = new System.Drawing.Size(99, 23);
        lblPattern.TabIndex = 10;
        lblPattern.Text = "Muster";
        // 
        // btnReset
        // 
        btnReset.Location = new System.Drawing.Point(838, 135);
        btnReset.Name = "btnReset";
        btnReset.Size = new System.Drawing.Size(92, 23);
        btnReset.TabIndex = 11;
        btnReset.Text = "Reset";
        btnReset.UseVisualStyleBackColor = true;
        btnReset.Click += btnReset_Click;
        // 
        // skControl1
        // 
        skControl1.Location = new System.Drawing.Point(0, 0);
        skControl1.Name = "skControl1";
        skControl1.Size = new System.Drawing.Size(0, 0);
        skControl1.TabIndex = 0;
        // 
        // lblCellSize
        // 
        lblCellSize.Location = new System.Drawing.Point(954, 193);
        lblCellSize.Name = "lblCellSize";
        lblCellSize.Size = new System.Drawing.Size(99, 23);
        lblCellSize.TabIndex = 14;
        lblCellSize.Text = "Zellgröße";
        // 
        // cellSizeSelector
        // 
        cellSizeSelector.Location = new System.Drawing.Point(1112, 193);
        cellSizeSelector.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        cellSizeSelector.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        cellSizeSelector.Name = "cellSizeSelector";
        cellSizeSelector.Size = new System.Drawing.Size(66, 23);
        cellSizeSelector.TabIndex = 13;
        cellSizeSelector.Value = new decimal(new int[] { 4, 0, 0, 0 });
        cellSizeSelector.ValueChanged += cellSizeSelector_ValueChanged;
        // 
        // GameOfLiveView
        // 
        GameOfLiveView.Location = new System.Drawing.Point(14, 54);
        GameOfLiveView.Name = "GameOfLiveView";
        GameOfLiveView.Size = new System.Drawing.Size(800, 600);
        GameOfLiveView.TabIndex = 15;
        GameOfLiveView.Text = "skControl2";
        GameOfLiveView.PaintSurface += GameOfLiveView_PaintSurface;
        // 
        // lblRuleSet
        // 
        lblRuleSet.Location = new System.Drawing.Point(954, 135);
        lblRuleSet.Name = "lblRuleSet";
        lblRuleSet.Size = new System.Drawing.Size(99, 23);
        lblRuleSet.TabIndex = 17;
        lblRuleSet.Text = "Regeln";
        // 
        // cbRuleSet
        // 
        cbRuleSet.FormattingEnabled = true;
        cbRuleSet.Items.AddRange(new object[] { "Game of Life", "Sand" });
        cbRuleSet.Location = new System.Drawing.Point(1059, 135);
        cbRuleSet.Name = "cbRuleSet";
        cbRuleSet.Size = new System.Drawing.Size(119, 23);
        cbRuleSet.TabIndex = 16;
        cbRuleSet.Text = "Game of Life";
        cbRuleSet.SelectedValueChanged += cbRuleSet_SelectedValueChanged;
        // 
        // btnSingleStep
        // 
        btnSingleStep.Location = new System.Drawing.Point(838, 106);
        btnSingleStep.Name = "btnSingleStep";
        btnSingleStep.Size = new System.Drawing.Size(92, 23);
        btnSingleStep.TabIndex = 18;
        btnSingleStep.Text = "Next Step";
        btnSingleStep.UseVisualStyleBackColor = true;
        btnSingleStep.Click += btnSingleStep_Click;
        // 
        // GameOfLiveForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(1264, 681);
        Controls.Add(btnSingleStep);
        Controls.Add(lblRuleSet);
        Controls.Add(cbRuleSet);
        Controls.Add(GameOfLiveView);
        Controls.Add(lblCellSize);
        Controls.Add(cellSizeSelector);
        Controls.Add(btnReset);
        Controls.Add(lblPattern);
        Controls.Add(cbPattern);
        Controls.Add(lblSpeed);
        Controls.Add(systemSpeedSelector);
        Controls.Add(lblProbability);
        Controls.Add(probabilitySelector);
        Controls.Add(btnStop);
        Controls.Add(statusLabel);
        Controls.Add(btnStart);
        Location = new System.Drawing.Point(15, 15);
        MaximumSize = new System.Drawing.Size(1280, 720);
        MinimumSize = new System.Drawing.Size(1280, 720);
        ((System.ComponentModel.ISupportInitialize)probabilitySelector).EndInit();
        ((System.ComponentModel.ISupportInitialize)systemSpeedSelector).EndInit();
        ((System.ComponentModel.ISupportInitialize)cellSizeSelector).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnSingleStep;

    private System.Windows.Forms.Label lblRuleSet;
    private System.Windows.Forms.ComboBox cbRuleSet;

    private SkiaSharp.Views.Desktop.SKControl GameOfLiveView;

    private System.Windows.Forms.Label lblCellSize;
    private System.Windows.Forms.NumericUpDown cellSizeSelector;

    private SkiaSharp.Views.Desktop.SKControl skControl1;

    private System.Windows.Forms.Button btnReset;

    private System.Windows.Forms.ComboBox cbPattern;
    private System.Windows.Forms.Label lblPattern;

    private System.Windows.Forms.Label lblProbability;
    private System.Windows.Forms.Label lblSpeed;
    private System.Windows.Forms.NumericUpDown systemSpeedSelector;

    private System.Windows.Forms.NumericUpDown probabilitySelector;

    private System.Windows.Forms.Button btnStop;

    private System.Windows.Forms.Label statusLabel;

    private System.Windows.Forms.Button btnStart;

    #endregion
}