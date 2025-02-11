using System.Text;
using CellularAutomata;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Timer = System.Windows.Forms.Timer;

namespace SkiasGameOfLive;

public partial class GameOfLiveForm : Form
{
    private enum RuleSetType
    {
        Sand,
        GameOfLife,
        SandArray,
        GameOfLifeArray,
    }
    
    private Vector cellSize => new ((int)cellSizeSelector.Value, (int)cellSizeSelector.Value, 0);
    
    private Vector dimension;
    private SKColor aliveColor = SKColors.Chartreuse;
    private SKColor emptyColor = SKColors.Black;
    
    private ToolTip toolTip = new ();
    private Timer toolTipTimer = new ();


    private IPlayGround<bool> playGroundBool;
    private IPlayGround<SandCellState> playGroundSand;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private int generation = 0;
    private int systemSpeed => (int)(systemSpeedSelector.Maximum - systemSpeedSelector.Value);
    private Vector bitmapSize => new (this.GameOfLiveView.Width, this.GameOfLiveView.Height, 0);
    
    public GameOfLiveForm()
    {
        InitializeComponent();
        InitializePlayGround();
        SetButtonState(false);
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        toolTipTimer.Interval = 3000; 
        toolTipTimer.Tick += (s, e) =>
        {
            toolTip.Hide(GameOfLiveView);
            toolTipTimer.Stop(); 
        };

    }

    private void InitializePlayGround()
    {
        ruleSetType = GetTypeFromSelection();
        generation = 0;
        dimension = new Vector((int)(bitmapSize.X / cellSize.X), (int)(bitmapSize.Y / cellSize.Y), 0);

        switch (ruleSetType)
        {
            case RuleSetType.GameOfLife:
                InitializeForGameOfLive();
                break;
            case RuleSetType.Sand:
                InitializeForSand();
                break;
            case RuleSetType.GameOfLifeArray:
                InitializeForGameOfLiveArray();
                break;
            case RuleSetType.SandArray:
                InitializeForSandArray();
                break;
        }
        
        RenderPlaygroundAndDisplayGeneration();
    }

    private void InitializeForSand()
    {
        playGroundSand = new PlayGround<SandCellState>(dimension);

        ruleSet = new SandRuleSet();

        var middle = (int)(playGroundSand.Dimension.X / 2);
        cbPattern.Enabled = false;
        aliveColor = SKColors.Bisque;
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle, 0, 0), SandCellState.Sand);
        //
        //
        // // add some terrain
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle + 1, 10, 0), SandCellState.Solid);
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle , 11, 0), SandCellState.Solid);
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle - 1, 12, 0), SandCellState.Solid);
        //
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle, 20, 0), SandCellState.Solid);
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle -1 , 19, 0), SandCellState.Solid);
        // GameOfLifeInitializer.AddSandCellStateToCell(sandPlayGround, new Vector(middle - 2, 18, 0), SandCellState.Solid);
        
        GameOfLifeInitializer.GenerateSandHourglass(playGroundSand);
        //GameOfLifeInitializer.TestCaseOne(sandPlayGround);
    }
    
    private void InitializeForSandArray()
    {
        playGroundSand = new PlayGroundArray<SandCellState>(dimension);

        ruleSet = new SandRuleSetArray();
        
        cbPattern.Enabled = false;
        aliveColor = SKColors.Bisque;
        
        GameOfLifeInitializer.GenerateSandHourglass(playGroundSand);
    }

    private void InitializeForGameOfLive()
    {
        playGroundBool = new PlayGround<bool>(dimension);
        var gamePlayGround = (playGroundBool as PlayGround<bool>)!;
        
        ruleSet = new GameOfLifeRuleSet();
        
        cbPattern.Enabled = true;
        aliveColor = SKColors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(gamePlayGround, (double)probabilitySelector.Value);
                break;
            case 1: 
                GameOfLifeInitializer.AddCheckerboard(gamePlayGround);
                break;
            case 2: 
                GameOfLifeInitializer.AddSingleLineWithCellOnEveryXColumn(gamePlayGround, 10, 10);
                GameOfLifeInitializer.AddSingleColumnWithCellOnEveryYRow(gamePlayGround, 10, 10);
                GameOfLifeInitializer.AddSingleCell(gamePlayGround, Vector.Zero);
                GameOfLifeInitializer.AddSingleCell(gamePlayGround, new Vector(dimension.X - 1, dimension.Y - 1, 0));
                break;
        }
    }
    
    private void InitializeForGameOfLiveArray()
    {
        playGroundBool = new PlayGroundArray<bool>(dimension);
        //var gamePlayGround = (playGround as PlayGroundArray<bool>)!;
        
        ruleSet = new GameOfLifeRuleSetArray();
        
        cbPattern.Enabled = true;
        aliveColor = SKColors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(playGroundBool, (double)probabilitySelector.Value);
                break;
            case 1: 
                GameOfLifeInitializer.AddCheckerboard(playGroundBool);
                break;
            case 2: 
                GameOfLifeInitializer.AddSingleLineWithCellOnEveryXColumn(playGroundBool, 10, 10);
                GameOfLifeInitializer.AddSingleColumnWithCellOnEveryYRow(playGroundBool, 10, 10);
                GameOfLifeInitializer.AddSingleCell(playGroundBool, Vector.Zero);
                GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(dimension.X - 1, dimension.Y - 1, 0));
                break;
        }
    }
    private RuleSetType GetTypeFromSelection() 
    {
        return cbRuleSet.SelectedIndex switch
        {
            0 => RuleSetType.GameOfLife,
            1 => RuleSetType.Sand,
            2 => RuleSetType.GameOfLifeArray,
            3 => RuleSetType.SandArray,
            _ => RuleSetType.GameOfLife,
        };
    }

    private void startGameOfLive_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSource == null)
        {
            ProcessNextGeneration();
        }
        
        SetButtonState(true);
    }

    private async Task ProcessNextGeneration()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        // Maximalanzahl der Schleifen (einstellbare Generationsgrenze)
        var maxGenerations = (int)stopWatchCountSelector.Value;
        
        var timingEnabled = cbStopWatch.Checked;

        // Listen zur Speicherung der Zeiten
        var generationTimes = new long[maxGenerations];
        var renderingTimes = new long[maxGenerations];


        // Allgemeine Stoppuhren für kumulative Zeit
        var totalStopwatch = new System.Diagnostics.Stopwatch();

        // Stoppuhren für einzelne Durchläufe
        var generationStopwatch = new System.Diagnostics.Stopwatch();
        var renderingStopwatch = new System.Diagnostics.Stopwatch();

        int currentGeneration = 0;

        totalStopwatch.Start();
        
        await Task.Run(() =>
        {
            while (!token.IsCancellationRequested && currentGeneration < maxGenerations)
            {
                if (timingEnabled)
                {
                    generationStopwatch.Restart();
                }

                GenerateNextPlaygroundState(ruleSetType);
                
                if (timingEnabled)
                {
                    generationStopwatch.Stop();
                    generationTimes[currentGeneration] = generationStopwatch.ElapsedMilliseconds;
                }

                
                if (timingEnabled)
                {
                    renderingStopwatch.Restart(); 
                }

                Invoke(RenderPlaygroundAndDisplayGeneration);
                
                if (timingEnabled)
                {
                    renderingStopwatch.Stop();
                    renderingTimes[currentGeneration] = renderingStopwatch.ElapsedMilliseconds;
                    
                    currentGeneration++;
                }

                if (systemSpeed > 0)
                {
                    Thread.Sleep(systemSpeed);    
                }
            }
        }, token);
        
        totalStopwatch.Stop();
        
        if (timingEnabled)
        {
            var generationStats = CalculateStatistics(generationTimes, currentGeneration, "Generierung");
            var renderingStats = CalculateStatistics(renderingTimes, currentGeneration, "Rendering");
            var totalStats = new StringBuilder();
            totalStats.AppendLine($"Simulation abgeschlossen nach {currentGeneration} Generationen:");
            totalStats.AppendLine($"Gesamtzeit: {FormatTime(totalStopwatch.ElapsedMilliseconds)} m");
            totalStats.AppendLine("");
            totalStats.AppendLine(generationStats);
            totalStats.AppendLine("");
            totalStats.AppendLine(renderingStats);
            
            tbStopWatch.Text = totalStats.ToString();
            
            SetButtonState(false);
        }


        cancellationTokenSource = null;
    }
    
    private string CalculateStatistics(long[] times, int count, string type)
    {
        if (count == 0)
        {
            return $"{type}-Statistik: Keine Messdaten vorhanden.";
        }

        var statistics = new StringBuilder();
        
        var total = times.Take(count).Sum();                // Gesamtzeit
        var min = times.Take(count).Min();                  // Schnellste Zeit
        var max = times.Take(count).Max();                  // Langsamste Zeit
        var average = times.Take(count).Average();        // Durchschnittszeit

        var totalFormatted = FormatTime(total);
        var minFormatted = FormatTime(min);
        var maxFormatted = FormatTime(max);
        var averageFormatted = FormatTime((long)average);

        // Ausgabe
        statistics.AppendLine($"{type}-Statistik:");
        statistics.AppendLine($"- Gesamtzeit: {totalFormatted} m");
        statistics.AppendLine($"- Langsamste: {maxFormatted} m");
        statistics.AppendLine($"- Schnellste: {minFormatted} m");
        statistics.AppendLine($"- Durchschnitt: {averageFormatted} m");

        
        return statistics.ToString();
    }

    private string FormatTime(long milliseconds)
    {
        var timespan = TimeSpan.FromMilliseconds(milliseconds);
        return $"{(int)timespan.TotalMinutes:D2}:{timespan.Seconds:D2}.{timespan.Milliseconds:D3}";
    }


    private void GenerateNextPlaygroundState(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => Automata<bool>.NextGenerationParallel((playGroundBool as PlayGround<bool>)!, (ruleSet as GameOfLifeRuleSet)!, false),
            RuleSetType.GameOfLifeArray => AutomataArray<bool>.NextGeneration((playGroundBool as PlayGroundArray<bool>)!,(ruleSet as GameOfLifeRuleSetArray)!, false),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => Automata<SandCellState>.NextGenerationParallel((playGroundSand as PlayGround<SandCellState>)!, (ruleSet as SandRuleSet)!, false),
            RuleSetType.SandArray => AutomataArray<SandCellState>.NextGeneration((playGroundSand as PlayGroundArray<SandCellState>)!,(ruleSet as SandRuleSetArray)!, false),
            _ => playGroundSand
        };
    }

    private void SetButtonState(bool isRunning)
    {
        btnStart.Enabled = !isRunning;
        btnReset.Enabled = !isRunning;
        btnSingleStep.Enabled = !isRunning;
        
        cbStopWatch.Enabled = !isRunning;
        cbPattern.Enabled = !isRunning;
        cbRuleSet.Enabled = !isRunning;
        
        cellSizeSelector.Enabled = !isRunning;
        stopWatchCountSelector.Enabled = !isRunning;
        
        cbEngine.Enabled = !isRunning && cellSizeSelector.Value == 1;
        
        btnStop.Enabled = isRunning;
    }

    private void RenderPlaygroundAndDisplayGeneration()
    {
        GameOfLiveView.Invalidate();
        this.DisplayGeneration();
    }

    private void DisplayGeneration()
    {
        statusLabel.Text = $"Generation: {generation++}";
        statusLabel.Update();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        SetButtonState(false);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void GameOfLiveView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        VisualizerRender(ruleSetType, e.Surface.Canvas);
    }

    private void VisualizerRender(RuleSetType type, SKCanvas canvas)
    {
        canvas.Clear(emptyColor);
        
        switch (type)
        {
            case RuleSetType.Sand:
                PlayGround<SandCellState> localSandCellStatePlayGround = (playGroundSand as PlayGround<SandCellState>)!;
                SkiaVisualizer<SandCellState>.Render(localSandCellStatePlayGround, cellSize, canvas, cbEngine.SelectedIndex, b =>
                {
                    return b switch
                    {
                        SandCellState.Empty => this.emptyColor,
                        SandCellState.Sand => this.aliveColor,
                        SandCellState.Solid => SKColors.Brown,
                        _ => this.emptyColor
                    };
                });
                break;
            case RuleSetType.GameOfLife:
                PlayGround<bool> localBoolPlayGround = (playGroundBool as PlayGround<bool>)!;
                SkiaVisualizer<bool>.Render(localBoolPlayGround, cellSize, canvas, cbEngine.SelectedIndex, b => b ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.SandArray:
                PlayGroundArray<SandCellState> localSandCellStatePlayGroundArray = (playGroundSand as PlayGroundArray<SandCellState>)!;
                SkiaVisualizer<SandCellState>.Render(localSandCellStatePlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, b =>
                {
                    return b switch
                    {
                        SandCellState.Empty => this.emptyColor,
                        SandCellState.Sand => this.aliveColor,
                        SandCellState.Solid => SKColors.Brown,
                        _ => this.emptyColor
                    };
                });
                break;
            case RuleSetType.GameOfLifeArray:
                PlayGroundArray<bool> localBoolPlayGroundArray = (playGroundBool as PlayGroundArray<bool>)!;
                SkiaVisualizer<bool>.Render(localBoolPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, b => b ? this.aliveColor : emptyColor);
                break;
            default:    
                break;
        }
    }

    private void cbRuleSet_SelectedValueChanged(object sender, EventArgs e)
    {
        cbPattern.Enabled = true;
        if (cbRuleSet.SelectedIndex == 1)
        {
            cbPattern.Enabled = false;
        }
        
        ruleSetType = GetTypeFromSelection();
        
        InitializePlayGround();
    }

    private void btnSingleStep_Click(object sender, EventArgs e)
    {
        GenerateNextPlaygroundState(ruleSetType);
                
        this.Invoke(RenderPlaygroundAndDisplayGeneration);
    }

    private void cbPattern_SelectedValueChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void cellSizeSelector_ValueChanged(object sender, EventArgs e)
    {
        cbEngine.Enabled = cellSizeSelector.Value == 1;
        if (cellSizeSelector.Value != 1)
        {
            cbEngine.SelectedIndex = 0;    
        }
        
        InitializePlayGround();
    }

    private void GameOfLiveView_MouseClick(object sender, MouseEventArgs e)
    {
        float viewWidth = GameOfLiveView.Width;
        float viewHeight = GameOfLiveView.Height;
        
        float mouseX = e.Location.X;
        float mouseY = e.Location.Y;
        
        int cellX = (int)(mouseX / viewWidth * dimension.X);
        int cellY = (int)(mouseY / viewHeight * dimension.Y);
        
        if (cellX >= dimension.X || cellY >= dimension.Y || cellX < 0 || cellY < 0)
        {
            toolTip.Hide(GameOfLiveView);
            return;
        }
        
        string toolTipText = $"Zelle: [{cellX}, {cellY}]";
        
        toolTip.Show(toolTipText, GameOfLiveView, e.Location);
        
        toolTipTimer.Start();

    }

    private void cbStopWatch_CheckedChanged(object sender, EventArgs e)
    {
        paStopWatch.Visible = cbStopWatch.Checked;
        stopWatchCountSelector.Enabled = cbStopWatch.Checked;
    }

    private void lblSum_Click(object sender, EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void cbRuleSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    private void cbEngine_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }
}