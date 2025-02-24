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
    private Color aliveColor = Colors.Chartreuse;
    
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
    
    private int systemSpeed => (int)(systemSpeedSelector.Maximum - systemSpeedSelector.Value);
    private Vector bitmapSize => new ((int)this.GameOfLiveView.Width, (int)this.GameOfLiveView.Height);
    
    public MainWindow()
    {
        InitializeComponent();
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
        
        aliveColor = Colors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(gamePlayGround, probabilitySelector.Value);
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
        
        aliveColor = Colors.Chartreuse;
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
        aliveColor = Colors.Bisque;

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
        aliveColor = Colors.Bisque;
        
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
    
    private async Task ProcessNextGeneration()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        var maxGenerations = stopWatchCountSelector.Value == null ? 50 : (int)stopWatchCountSelector.Value;
        
        currentGeneration = 0;

        totalStopwatch.Restart();
        
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
                    generationTimes.Add(generationStopwatch.ElapsedMilliseconds);
                }

                Dispatcher.UIThread.Post(RenderPlaygroundAndDisplayGeneration);
                
                if (timingEnabled)
                {
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
            var totalStats = new StringBuilder();
            totalStats.AppendLine($"Simulation abgeschlossen nach {currentGeneration} Generationen auf {Environment.ProcessorCount} Kernen:");
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
    
    private void btnReset_Click(object? sender, RoutedEventArgs e
    )
    {
        InitializePlayGround();
    }
}