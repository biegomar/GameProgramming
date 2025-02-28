using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    
    private readonly SKColor[] colors =
    [
        new SKColor(194, 178, 128), 
        new SKColor(210, 180, 140), 
        new SKColor(244, 164, 96),  
        new SKColor(222, 184, 135)
    ];
    
    private Vector cellSize => new ((int)cellSizeSelector.Value, (int)cellSizeSelector.Value);
    
    private Vector dimension;
    private SKColor aliveColor = SKColors.Chartreuse;
    private readonly SKColor emptyColor = SKColors.Black;
    
    private readonly ToolTip toolTip = new ();
    private readonly Timer toolTipTimer = new ();

    private readonly IList<long> generationTimes = new List<long>();
    private readonly IList<long> renderingTimes = new List<long>();
    
    private readonly Stopwatch generationStopwatch = new ();
    private readonly Stopwatch renderingStopwatch = new ();
    private readonly Stopwatch totalStopwatch = new ();
    private readonly Random random = new ();
    
    private bool timingEnabled = true;
    private int currentGeneration = 0;
    private int generation = 0;

    private IPlayGround<bool> playGroundBool;
    private IPlayGround<SandCellState> playGroundSand;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private Vector bitmapSize => new (this.GameOfLiveView.Width, this.GameOfLiveView.Height);
    
    public GameOfLiveForm()
    {
        InitializeComponent();
        InitializeLayout();
        InitializePlayGround();
        SetButtonState(false);
        InitializeTimer();
    }

    private void InitializeLayout()
    {
        cbPatternSand.Enabled = false;
        cbPatternSand.Visible = false;
        cbPatternSand.Location = cbPattern.Location;
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
        dimension = new Vector(bitmapSize.X / cellSize.X, bitmapSize.Y / cellSize.Y);

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

        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;

        switch (cbPatternSand.SelectedIndex)
        {
            case 0:
                GameOfLifeInitializer.Randomize(playGroundSand, (double)probabilitySelector.Value);
                break;
            case 1:
                GameOfLifeInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), SandCellState.Sand);

                // add some terrain
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), SandCellState.Solid);

                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), SandCellState.Solid);
                break;
        }
    }
    
    private void InitializeForSandArray()
    {
        playGroundSand = new PlayGroundArray<SandCellState>(dimension);

        ruleSet = new SandRuleSetArray();
        
        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;
        
        switch (cbPatternSand.SelectedIndex)
        {
            case 0:
                GameOfLifeInitializer.Randomize(playGroundSand, (double)probabilitySelector.Value);
                break;
            case 1:
                GameOfLifeInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), SandCellState.Sand);

                // add some terrain
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), SandCellState.Solid);

                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), SandCellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), SandCellState.Solid);
                break;
        }
    }

    private void InitializeForGameOfLive()
    {
        playGroundBool = new PlayGround<bool>(dimension);
        var gamePlayGround = (playGroundBool as PlayGround<bool>)!;
        
        ruleSet = new GameOfLifeRuleSet();
        
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
                GameOfLifeInitializer.AddSingleCell(gamePlayGround, new Vector(0, 0));
                GameOfLifeInitializer.AddSingleCell(gamePlayGround, new Vector(dimension.X - 1, dimension.Y - 1));
                break;
        }
    }
    
    private void InitializeForGameOfLiveArray()
    {
        playGroundBool = new PlayGroundArray<bool>(dimension);
        
        ruleSet = new GameOfLifeRuleSetArray();
        
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
                GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(0, 0));
                GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(dimension.X - 1, dimension.Y - 1));
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
            _ = ProcessNextGeneration();
        }
        
        SetButtonState(true);
    }

    private async Task ProcessNextGeneration()
    {
        generationTimes.Clear();
        renderingTimes.Clear();
        
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        var maxGenerations = (int)stopWatchCountSelector.Value;
        
        currentGeneration = 0;
        
        await Task.Run(async () =>
        {
            totalStopwatch.Restart();
            while (!token.IsCancellationRequested && currentGeneration < maxGenerations)
            {
                if (timingEnabled)
                {
                    generationStopwatch.Restart();
                    
                    GenerateNextPlaygroundState(ruleSetType);
                    
                    generationStopwatch.Stop();
                    generationTimes.Add(generationStopwatch.ElapsedMilliseconds);
                    
                    currentGeneration++;
                }
                else
                {
                    GenerateNextPlaygroundState(ruleSetType);
                }

                await InvokeAsync(RenderPlaygroundAndDisplayGeneration, token);
            }
            totalStopwatch.Stop();
        }, token);
        
        if (timingEnabled)
        {
            GenerateSimulationReport();
        }

        SetButtonState(false);

        cancellationTokenSource = null;
    }
    
    private void GenerateSimulationReport()
    {
        var totalStats = new StringBuilder();
        totalStats.AppendLine($"{currentGeneration} Generationen auf {Environment.ProcessorCount} Kernen:");
        totalStats.AppendLine($"Gesamtzeit: {FormatTime(totalStopwatch.ElapsedMilliseconds)} m");
        totalStats.AppendLine($"Gesamtzeit der Einzelmessungen: {FormatTime(generationTimes.Sum() + renderingTimes.Sum())} m");
            
        var generationStats = CalculateStatistics(generationTimes, "Generierung");
        var renderingStats = CalculateStatistics(renderingTimes, "Rendering");
            
        totalStats.AppendLine("");
        totalStats.AppendLine(generationStats);
        totalStats.AppendLine("");
        totalStats.AppendLine(renderingStats);
            
        // if (playGroundBool is PlayGroundArray<bool> playGroundWithStatistics)
        // {
        //     var initStatistics = CalculateStatistics(PlayGroundArray<bool>.GenerationTimes.ToArray(), currentGeneration, "Initialisierung");
        //     totalStats.AppendLine("");
        //     totalStats.AppendLine(initStatistics);
        // }
            
        tbStopWatch.Text = totalStats.ToString();
    }
    
    private string CalculateStatistics(IList<long> times, string type)
    {
        var statistics = new StringBuilder();
        
        var total = times.Sum();                
        var min = times.Min();                  
        var max = times.Max();                  
        var average = times.Average();        

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
        statistics.AppendLine("");
        foreach (var ruleCount in ruleSet.RuleCounter)
        {
            statistics.AppendLine($"- {ruleCount.Key}: {ruleCount.Value}");
        }

        times.Clear();
        
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
            RuleSetType.GameOfLife => Automata<bool>.NextGenerationParallel((playGroundBool as PlayGround<bool>)!, (ruleSet as GameOfLifeRuleSet)!, false, Environment.ProcessorCount),
            RuleSetType.GameOfLifeArray => AutomataArray<bool>.NextGenerationParallel((playGroundBool as PlayGroundArray<bool>)!,(ruleSet as GameOfLifeRuleSetArray)!, false, Environment.ProcessorCount),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => Automata<SandCellState>.NextGenerationParallel((playGroundSand as PlayGround<SandCellState>)!, (ruleSet as SandRuleSet)!, false, Environment.ProcessorCount),
            RuleSetType.SandArray => AutomataArray<SandCellState>.NextGenerationParallel((playGroundSand as PlayGroundArray<SandCellState>)!,(ruleSet as SandRuleSetArray)!, false, Environment.ProcessorCount),
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
        if (timingEnabled)
        {
            renderingStopwatch.Restart();
            VisualizerRender(ruleSetType, e.Surface.Canvas);
            renderingStopwatch.Stop();
            renderingTimes.Add(renderingStopwatch.ElapsedMilliseconds);
        }
        else
        {
            VisualizerRender(ruleSetType, e.Surface.Canvas);
        }
        
    }

    private void VisualizerRender(RuleSetType type, SKCanvas canvas)
    {
        canvas.Clear(emptyColor);
        
        switch (type)
        {
            case RuleSetType.GameOfLifeArray:
                PlayGroundArray<bool> localBoolPlayGroundArray = (playGroundBool as PlayGroundArray<bool>)!;
                SkiaVisualizer<bool>.Render(localBoolPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, b => b ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.Sand:
                PlayGround<SandCellState> localSandCellStatePlayGround = (playGroundSand as PlayGround<SandCellState>)!;
                SkiaVisualizer<SandCellState>.Render(localSandCellStatePlayGround, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, ChooseSandColor);
                break;
            case RuleSetType.GameOfLife:
                PlayGround<bool> localBoolPlayGround = (playGroundBool as PlayGround<bool>)!;
                SkiaVisualizer<bool>.Render(localBoolPlayGround, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, b => b ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.SandArray:
                PlayGroundArray<SandCellState> localSandCellStatePlayGroundArray = (playGroundSand as PlayGroundArray<SandCellState>)!;
                SkiaVisualizer<SandCellState>.Render(localSandCellStatePlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, ChooseSandColor);
                break;
            default:    
                break;
        }
    }

    private SKColor ChooseSandColor(SandCellState state)
    {
        return state switch
        {
            SandCellState.Empty => emptyColor,
            SandCellState.Sand => ChooseSandColor(),
            SandCellState.Solid => SKColors.Brown,
            _ => emptyColor
        };
    }
    
    private SKColor ChooseSandColor()
    {
        var index = random.Next(0, colors.Length);
        return colors[index];
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
        timingEnabled = cbStopWatch.Checked;
    }

    private void cbRuleSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbRuleSet.SelectedIndex % 2 == 0)
        {
            FlipPatternBoxes(false);
        }
        else
        {
            FlipPatternBoxes(true);
        }
    }

    private void FlipPatternBoxes(bool toSandPattern)
    {
        cbPattern.Visible = !toSandPattern;
        cbPattern.Enabled = !toSandPattern;
        cbPatternSand.Visible = toSandPattern;
        cbPatternSand.Enabled = toSandPattern;
    }

    private void cbEngine_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void cbPatternSand_SelectedValueChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }
}