using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using CellularAutomata;
using SkiaSharp;

namespace AvaloniaGameOfLive;

public partial class MainWindow : Window
{
    private enum RuleSetType
    {
        Sand,
        GameOfLife,
        SandArray,
        GameOfLifeArray,
    }
    
    private Vector cellSize => new ((int)cellSizeSelector.Value, (int)cellSizeSelector.Value);
    private Vector dimension;
    private SKColor aliveColor = SKColors.Chartreuse;
    private readonly SKColor emptyColor = SKColors.Red;
    
    private readonly ToolTip toolTip = new ();
    private readonly DispatcherTimer toolTipTimer = new ();

    private readonly IList<long> generationTimes = new List<long>();
    private readonly IList<long> renderingTimes = new List<long>();
    
    private readonly Stopwatch generationStopwatch = new ();
    private readonly Stopwatch renderingStopwatch = new ();
    private readonly Stopwatch totalStopwatch = new ();
    
    private bool timingEnabled = true;
    private int currentGeneration = 0;
    private int generation = 0;
    
    private IPlayGround<bool> playGroundBool;
    private IPlayGround<SandCellState> playGroundSand;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private Vector bitmapSize => new ((int)this.GameOfLiveView.Width, (int)this.GameOfLiveView.Height);
    
    public MainWindow()
    {
        InitializeComponent();
        InitializeEventHandlers();
        InitializePlayGround();
        SetButtonState(false);
        InitializeTimer();
    }
    
    private void InitializeTimer()
    {
        toolTipTimer.Interval = TimeSpan.FromSeconds(3); 
        toolTipTimer.Tick += (s, e) =>
        {
            toolTip.IsVisible = false;
            toolTipTimer.Stop(); 
        };
    }

    private void InitializeEventHandlers()
    {
        cbPattern.SelectionChanged += cbPattern_SelectedValueChanged;
        cellSizeSelector.ValueChanged += cellSizeSelector_ValueChanged;
        cbRuleSet.SelectionChanged += cbRuleSet_SelectedValueChanged;
        cbStopWatch.IsCheckedChanged += cbStopWatch_CheckedChanged;
        cbEngine.SelectionChanged += cbEngine_SelectedIndexChanged;
        btnStart.Click += startGameOfLive_Click;
        btnStop.Click += btnStop_Click;
        
        GameOfLiveView.PaintSurface += GameOfLiveView_PaintSurface;
    }
    
    private void startGameOfLive_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSource == null)
        {
            ProcessNextGenerationAsync().ConfigureAwait(false);
        }
        
        SetButtonState(true);
    }
    
    private void btnStop_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        SetButtonState(false);
    }
    
    private void cbRuleSet_SelectedValueChanged(object sender, EventArgs e)
    {
        cbPattern.IsEnabled = true;
        if (cbRuleSet.SelectedIndex == 1)
        {
            cbPattern.IsEnabled = false;
        }
        
        ruleSetType = GetTypeFromSelection();
        
        InitializePlayGround();
    }
    
    private void cellSizeSelector_ValueChanged(object sender, EventArgs e)
    {
        cbEngine.IsEnabled = cellSizeSelector.Value == 1;
        if (cellSizeSelector.Value != 1)
        {
            cbEngine.SelectedIndex = 0;    
        }
        
        InitializePlayGround();
    }
    
    private void cbStopWatch_CheckedChanged(object sender, EventArgs e)
    {
        tbStopWatch.IsVisible = cbStopWatch.IsChecked!.Value;
        stopWatchCountSelector.IsEnabled = cbStopWatch.IsChecked!.Value;
        timingEnabled = cbStopWatch.IsChecked!.Value;
    }

    private void cbEngine_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
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
    
    private void RenderPlaygroundAndDisplayGeneration()
    {
        GameOfLiveView.InvalidateVisual();
        this.DisplayGeneration();
    }

    private void DisplayGeneration()
    {
        statusLabel.Text = $"Generation: {generation++}";
        //statusLabel.Update();
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
    
    private void InitializeForGameOfLive()
    {
        playGroundBool = new PlayGround<bool>(dimension);
        var gamePlayGround = (playGroundBool as PlayGround<bool>)!;
        
        ruleSet = new GameOfLifeRuleSet();
        
        aliveColor = SKColors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(gamePlayGround, (double)probabilitySelector.Value!);
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
    
    private void InitializeForSand()
    {
        playGroundSand = new PlayGround<SandCellState>(dimension);

        ruleSet = new SandRuleSet();

        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;

        switch (cbPattern.SelectedIndex)
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
        
        switch (cbPattern.SelectedIndex)
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
    
    private void SetButtonState(bool isRunning)
    {
        btnStart.IsEnabled = !isRunning;
        btnReset.IsEnabled = !isRunning;
        btnSingleStep.IsEnabled = !isRunning;
        
        cbStopWatch.IsEnabled = !isRunning;
        cbPattern.IsEnabled = !isRunning;
        cbRuleSet.IsEnabled = !isRunning;
        
        cellSizeSelector.IsEnabled = !isRunning;
        stopWatchCountSelector.IsEnabled = !isRunning;
        
        cbEngine.IsEnabled = !isRunning && cellSizeSelector.Value == 1;
        cbEngine.IsEnabled = true;
        
        btnStop.IsEnabled = isRunning;
    }
    
    private void GameOfLiveView_PaintSurface(SKCanvas e)
    {
        VisualizerRender(ruleSetType, e);
    }
    
    private void VisualizerRender(RuleSetType type, SKCanvas canvas)
    {
        if (timingEnabled)
        {
            renderingStopwatch.Restart(); 
        }
        
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
        
        if (timingEnabled)
        {
            renderingStopwatch.Stop();
            renderingTimes.Add(renderingStopwatch.ElapsedMilliseconds);
        }
    }
    
    private async Task ProcessNextGenerationAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        var maxGenerations = stopWatchCountSelector.Value == null ? 50 : (int)stopWatchCountSelector.Value;
        
        currentGeneration = 0;

        totalStopwatch.Restart();
        
        await Task.Run(async () =>
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
                    generationTimes.Add(generationStopwatch.ElapsedMilliseconds);
                }

                await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);

                if (timingEnabled)
                {
                    currentGeneration++;
                }
            }
        }, token);
        
        totalStopwatch.Stop();
        
        if (timingEnabled)
        {
            var totalStats = new StringBuilder();
            totalStats.AppendLine($"{currentGeneration} Generationen auf {Environment.ProcessorCount} Kernen:");
            totalStats.AppendLine($"Gesamtzeit: {FormatTime(totalStopwatch.ElapsedMilliseconds)} m");
            totalStats.AppendLine($"Gesamtzeit der Einzelmessungen: {FormatTime(generationTimes.Sum() + renderingTimes.Sum())} m");
            
            var generationStats = CalculateStatistics(generationTimes, "Generierung");
            var renderingStats = CalculateStatistics(renderingTimes, "Rendering");
            
            totalStats.AppendLine(generationStats);
            totalStats.AppendLine(renderingStats);
            
            // if (playGroundBool is PlayGroundArray<bool> playGroundWithStatistics)
            // {
            //     var initStatistics = CalculateStatistics(PlayGroundArray<bool>.GenerationTimes.ToArray(), currentGeneration, "Initialisierung");
            //     totalStats.AppendLine("");
            //     totalStats.AppendLine(initStatistics);
            // }
            
            tbStopWatch.Text = totalStats.ToString();
        }

        SetButtonState(false);

        cancellationTokenSource = null;
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
        //statistics.AppendLine("");
        // foreach (var ruleCount in ruleSet.RuleCounter)
        // {
        //     statistics.AppendLine($"- {ruleCount.Key}: {ruleCount.Value}");
        // }

        times.Clear();
        
        return statistics.ToString();
    }

    private string FormatTime(long milliseconds)
    {
        var timespan = TimeSpan.FromMilliseconds(milliseconds);
        return $"{(int)timespan.TotalMinutes:D2}:{timespan.Seconds:D2}.{timespan.Milliseconds:D3}";
    }
    
    private void btnReset_Click(object? sender, RoutedEventArgs e)
    {
        InitializePlayGround();
    }
    
    private void btnSingleStep_Click(object sender, RoutedEventArgs e)
    {
        GenerateNextPlaygroundState(ruleSetType);
                
        RenderPlaygroundAndDisplayGeneration();
    }
    
    private void cbPattern_SelectedValueChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }
    
    private SKColor ChooseSandColor(SandCellState state)
    {
        if (state == SandCellState.Empty) return emptyColor;
        if (state == SandCellState.Sand) return ChooseSandColor();
        if (state == SandCellState.Solid) return SKColors.Brown;
        
        return emptyColor;
    }
    
    private SKColor ChooseSandColor()
    {
        var colors = new SKColor[]
        {
            new SKColor(194, 178, 128), 
            new SKColor(210, 180, 140), 
            new SKColor(244, 164, 96),  
            new SKColor(222, 184, 135)  
        };
        
        var random = new Random();
        int index = random.Next(0, colors.Length);
        
        return colors[index];

    }
}