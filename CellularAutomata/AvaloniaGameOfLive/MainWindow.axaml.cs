using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.GameOfLive;
using CellularAutomata.Interfaces;
using CellularAutomata.MaterialFlow;
using CellularAutomata.Wolfram;
using SkiaSharp;
using Supporter;
using Visualizer;
using Vector = CellularAutomata.Vector;

namespace AvaloniaGameOfLive;

public partial class MainWindow : Window
{
    private enum RuleSetType
    {
        Sand,
        GameOfLife,
        Wolfram,
        NoiseGrid,
    }
    
    private Vector cellSize => new ((int)cellSizeSelector.Value, (int)cellSizeSelector.Value);
    private Vector dimension;
    
    private SKColor aliveColor = SKColors.Chartreuse;
    private SKColor wolframColor = SKColors.CornflowerBlue;
    private SKColor noiseGridColor = SKColors.Aquamarine;
    private readonly SKColor emptyColor = SKColors.Black;

    private double initializationProbability;
    private double spawnProbability;
    
    private ToolTip toolTip;
    private readonly DispatcherTimer toolTipTimer = new ();
    private bool isTooltipVisible = false;
    private bool isSpawnActive = false;
    private Vector spawnPosition = new (0, 0);
    private Vector brushSize = new Vector(10,10);

    private IList<long> generationTimes;
    private IList<long> renderingTimes;
    
    private readonly Stopwatch generationStopwatch = new ();
    private readonly Stopwatch renderingStopwatch = new ();
    private readonly Stopwatch totalStopwatch = new ();

    private bool shouldDraw = true;
    private bool timingEnabled = true;
    private int currentGeneration = 0;
    private int generation = 0;
    private int maxDegreeOfParallelism = 2;
    
    private PlayGround playGroundBool;
    private PlayGround playGroundSand;
    private IRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private Automata automataBool;
    private AutomataMaterialGrid automataSand;
    private AutomataWolfram automataWolframBool;
    private AutomataNoiseGrid automataNoiseGrid;
    
    private CancellationTokenSource? cancellationTokenSource;
    private CancellationTokenSource? tooltipCancellationSource;
    
    private Vector bitmapSize => new ((int)this.GameOfLiveView.Width, (int)this.GameOfLiveView.Height);
    
    public MainWindow()
    {
        //var test = Marshal.SizeOf<CellularAutomata.MaterialFlow.MaterialMovement>();

        InitializeComponent();
        InitializeComponentValues();
        InitializeEventHandlers();
        InitializePlayGround();
        SetButtonState(false);
    }

    private void InitializeComponentValues()
    {
        processorCountSelector.Maximum = Environment.ProcessorCount;
        SetMaxDegreeOfParallelism();
        SetInitializationProbability();
        SetSpawnProbability();
    }

    private void InitializeEventHandlers()
    {
        cbPattern.SelectionChanged += cbPattern_SelectedValueChanged;
        cellSizeSelector.ValueChanged += cellSizeSelector_ValueChanged;
        brushSizeSelector.ValueChanged += brushSizeSelector_ValueChanged;
        probabilitySelector.ValueChanged += probabilitySelector_ValueChanged;
        cbRuleSet.SelectionChanged += cbRuleSet_SelectedValueChanged;
        cbStopWatch.IsCheckedChanged += cbStopWatch_CheckedChanged;
        cbUseProbability.IsCheckedChanged += cbUseProbability_IsCheckedChanged;
        cbEngine.SelectionChanged += cbEngine_SelectedIndexChanged;
        
        btnStart.Click += startGameOfLive_Click;
        btnStop.Click += btnStop_Click;
        btnSingleStep.Click += btnSingleStep_Click;
        
        processorCountSelector.ValueChanged += processorCountSelector_ValueChanged;
        
        GameOfLiveView.PaintSurface += GameOfLiveView_PaintSurface;
        GameOfLiveView.PointerPressed += GameOfLiveView_PointerPressed;
        GameOfLiveView.PointerReleased += GameOfLiveView_PointerReleased;
        GameOfLiveView.PointerMoved += GameOfLiveView_PointerMoved;
    }

    private void cbUseProbability_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        SetSpawnProbability();
    }

    private void btnReset_Click(object? sender, RoutedEventArgs e)
    {
        InitializePlayGround();
    }
    
    private void btnSingleStep_Click(object? sender, RoutedEventArgs e)
    {
        GenerateNextPlaygroundState(ruleSetType);
        
        generation++;
                
        RenderPlaygroundAndDisplayGeneration();
    }
    
    private void cbPattern_SelectedValueChanged(object? sender, EventArgs e)
    {
        InitializePlayGround();
    }
    
    private void startGameOfLive_Click(object? sender, EventArgs e)
    {
        if (cancellationTokenSource == null)
        {
            ProcessNextGenerationAsync().ConfigureAwait(false);
        }
        
        SetButtonState(true);
    }
    
    private void btnStop_Click(object? sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        SetButtonState(false);
    }
    
    private void cbRuleSet_SelectedValueChanged(object? sender, EventArgs e)
    {
        SetTypeFromSelection();
        SetPatternItems();
        
        InitializePlayGround();
    }

    private void probabilitySelector_ValueChanged(object? sender, EventArgs e)
    {
        SetInitializationProbability();
        SetSpawnProbability();
    }
    
    private void cellSizeSelector_ValueChanged(object? sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void brushSizeSelector_ValueChanged(object? sender, EventArgs e)
    {
        SetBrushSize();
    }

    private void SetBrushSize()
    {
        var brushSquare = (int)brushSizeSelector.Value!;
        brushSize = new Vector(brushSquare, brushSquare);
    }

    private void processorCountSelector_ValueChanged(object? sender, EventArgs e)
    {
        SetMaxDegreeOfParallelism();
    }

    private void SetMaxDegreeOfParallelism()
    {
        maxDegreeOfParallelism = (int)processorCountSelector.Value!;
    }

    private void SetInitializationProbability()
    {
        initializationProbability = (double)probabilitySelector.Value!;
    }

    private void SetSpawnProbability()
    {
        spawnProbability = cbUseProbability.IsChecked!.Value ? initializationProbability : 1f;
    }

    private void cbStopWatch_CheckedChanged(object? sender, EventArgs e)
    {
        SetStopWatchVisibility();
    }

    private void SetStopWatchVisibility()
    {
        tbStopWatch.IsVisible = cbStopWatch.IsChecked!.Value;
        stopWatchCountSelector.IsEnabled = cbStopWatch.IsChecked!.Value;
        timingEnabled = cbStopWatch.IsChecked!.Value;
    }

    private void cbEngine_SelectedIndexChanged(object? sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void GameOfLiveView_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (isSpawnActive)
        {
            GetSpawnPositionFromMouseCursor(e);
            
            HandleCellSpawnAndRender();
        }
    }

    private void GetSpawnPositionFromMouseCursor(PointerEventArgs e)
    {
        spawnPosition = GetCellPositionFromMouseCursor(e.GetPosition(GameOfLiveView));
    }

    private void GameOfLiveView_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (IsRightButtonPressed(e))
        {
            HandleRightClickOnGameView(e);
        }
        else if (IsLeftButtonPressed(e))
        {
            HandleLeftClickOnGameView(e);
        }
    }
    
    private void GameOfLiveView_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (IsLeftButtonReleased(e))
        {
            HandleLeftReleasedOnGameView();
        }
    }
    
    private bool IsRightButtonPressed(PointerPressedEventArgs e)
    {
        return e.GetCurrentPoint(GameOfLiveView).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed;
    }
    
    private bool IsLeftButtonReleased(PointerReleasedEventArgs e)
    {
        return e.GetCurrentPoint(GameOfLiveView).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased;
    }
    
    private bool IsLeftButtonPressed(PointerPressedEventArgs e)
    {
        return e.GetCurrentPoint(GameOfLiveView).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed;
    }
    
    private Task HandleRightClickOnGameView(PointerPressedEventArgs e)
    {
        CloseTooltip(GameOfLiveView);

        var position = e.GetPosition(GameOfLiveView);
        var tooltipText = FormatTooltipText(position);

        return ShowTooltipWithTimeout(tooltipText);
    }
    
    private void HandleLeftClickOnGameView(PointerPressedEventArgs e)
    {
        if (!isSpawnActive) isSpawnActive = cbBrushActive.IsChecked!.Value;

        if (isSpawnActive)
        {
            GetSpawnPositionFromMouseCursor(e);

            HandleCellSpawnAndRender();
        }
    }

    private void HandleCellSpawnAndRender()
    {
        if (cancellationTokenSource == null)
        {
            SpawnCells(ruleSetType);
            RenderPlayground();
        }
    }

    private void HandleLeftReleasedOnGameView()
    {
        if (isSpawnActive) isSpawnActive = false;
    }
    
    private string FormatTooltipText(Point position)
    {
        var cellPosition = GetCellPositionFromMouseCursor(position);

        var cellState = GetCellState(cellPosition);
        
        return $"Zelle: [{cellPosition.X}, {cellPosition.Y}]: {cellState}";
    }

    private Vector GetCellPositionFromMouseCursor(Point position)
    {
        var viewWidth = GameOfLiveView.Width;
        var viewHeight = GameOfLiveView.Height;
        
        var mouseX = position.X;
        var mouseY = position.Y;
        
        var cellX = (int)(mouseX / viewWidth * dimension.X);
        var cellY = (int)(mouseY / viewHeight * dimension.Y);
        
        return new Vector(cellX, cellY);
    }

    private string GetCellState(Vector cellPosition)
    {
        switch (ruleSetType)
        {
            case RuleSetType.Sand:
                return GetCellStateFromSand(cellPosition);
            case RuleSetType.GameOfLife:
            case RuleSetType.Wolfram:
            case RuleSetType.NoiseGrid:
                return GetCellStateForGameOfLive(cellPosition);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private string GetCellStateForGameOfLive(Vector cellPosition)
    {
        return playGroundBool[cellPosition].ToString();
    }

    private string GetCellStateFromSand(Vector cellPosition)
    {
        return playGroundSand[cellPosition].ToString();
    }

    private async Task ShowTooltipWithTimeout(string tooltipText)
    {
        tooltipCancellationSource = new CancellationTokenSource();
        try
        {
            OpenTooltip(GameOfLiveView, tooltipText);
            isTooltipVisible = true;
                
            await Task.Delay(TimeSpan.FromSeconds(3), tooltipCancellationSource.Token);
        }
        catch (TaskCanceledException)
        {
        }
        finally
        {
            if (isTooltipVisible)
            {
                CloseTooltip(GameOfLiveView);
            }
        }
    }

    private void OpenTooltip(Control control, string text)
    {
        toolTip = new ToolTip { Content = text };
        ToolTip.SetTip(control, toolTip);
        ToolTip.SetIsOpen(control, true);
        isTooltipVisible = true;

    }
    
    private void CloseTooltip(Control control)
    {
        tooltipCancellationSource?.Cancel();
        ToolTip.SetIsOpen(control, false);
        ToolTip.SetTip(control, null);
        isTooltipVisible = false;
    }


    private void InitializePlayGround()
    {
        SetTypeFromSelection();
        
        ResetGenerationCount();
        
        InitializeDimension();

        InitializeSimulationRules();

        RenderPlaygroundAndDisplayGeneration();
    }

    private void ResetGenerationCount()
    {
        generation = 0;
    }

    private void InitializeDimension()
    {
        dimension = new Vector(bitmapSize.X / cellSize.X, bitmapSize.Y / cellSize.Y);
    }

    private void InitializeSimulationRules()
    {
        switch (ruleSetType)
        {
            case RuleSetType.GameOfLife:
                InitializeForGameOfLive();
                break;
            case RuleSetType.Sand:
                InitializeForSand();
                break;
            case RuleSetType.Wolfram:
                InitializeWolfram();
                break;
            case RuleSetType.NoiseGrid:
                InitializeNoise();
                break;
        }
    }

    private void RenderPlayground()
    {
        GameOfLiveView.InvalidateVisual();
    }
    
    private void RenderPlaygroundAndDisplayGeneration()
    {
        RenderPlayground();
        DisplayGeneration();
    }

    private void DisplayGeneration()
    {
        statusLabel.Text = $"Generation: {generation}";
    }
    
    private void SetTypeFromSelection() 
    {
        ruleSetType = cbRuleSet.SelectedIndex switch
        {
            0 => RuleSetType.GameOfLife,
            1 => RuleSetType.Sand,
            2 => RuleSetType.Wolfram,
            3 => RuleSetType.NoiseGrid,
            _ => RuleSetType.GameOfLife,
        };
    }

    private void SetPatternItems()
    {
        cbPattern.Items.Clear();

        switch (ruleSetType)
        {
            case RuleSetType.Sand:
                AddPatternItems(["Random", "Sanduhr", "Freestyle"]);
                break;
            case RuleSetType.Wolfram:
                foreach (var item in Enumerable.Range(0, 256).Select(n => n.ToString()))
                {
                    cbPattern.Items.Add(item);
                }
                break;
            case RuleSetType.NoiseGrid:
            case RuleSetType.GameOfLife:
            default:
                AddPatternItems(["Random", "Schachbrett", "Freestyle"]);
                break;
        }

        cbPattern.SelectedIndex = 0;
    }
    
    private void AddPatternItems(List<string> items)
    {
        items.ForEach(item => cbPattern.Items.Add(item));
    }
    
    private void InitializeForGameOfLive()
    {
        automataBool = new Automata(dimension);
        playGroundBool = new PlayGround(dimension);
        ruleSet = new GameOfLifeRuleSet(dimension);
        
        InitializeGameOfLifePattern();
    }
    
    private void InitializeGameOfLifePattern()
    {
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(playGroundBool, maxDegreeOfParallelism, initializationProbability);
                break;
            case 1: 
                GameOfLifeInitializer.AddCheckerboard(playGroundBool);
                break;
            case 2: 
                GameOfLifeInitializer.PrepareFreestyle(playGroundBool);
                break;
        }
    }

    private void InitializeForSand()
    {
        automataSand = new AutomataMaterialGrid(dimension);
        playGroundSand = new PlayGround(dimension);
        ruleSet = new SandRuleSet(dimension);
        
        InitializeSandPattern();
    }
    
    private void InitializeSandPattern()
    {
        switch (cbPattern.SelectedIndex)
        {
            case 0:
                SandInitializer.Randomize(playGroundSand, maxDegreeOfParallelism, initializationProbability);
                break;
            case 1:
                SandInitializer.GenerateSandHourglass(playGroundSand);
                break;
        }
    }
    
    private void InitializeWolfram()
    {
        automataWolframBool = new AutomataWolfram();
        playGroundBool = new PlayGround(dimension);
        ruleSet = new WolframRuleSet(cbPattern.SelectedIndex);
        
        InitializeWolframPattern();
    }
    
    private void InitializeWolframPattern()
    {
        GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(dimension.X/2, 0));
    }

    private void InitializeNoise()
    {
        automataNoiseGrid = new AutomataNoiseGrid(dimension);
        playGroundBool = new PlayGround(dimension);
        ruleSet = new NoiseGridRuleSet(dimension);
        
        InitializeNoisePattern();
    }

    private void InitializeNoisePattern()
    {
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                NoiseGridInitializer.Randomize(playGroundBool, maxDegreeOfParallelism, initializationProbability);
                break;
            case 1: 
                NoiseGridInitializer.AddCheckerboard(playGroundBool);
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
        processorCountSelector.IsEnabled = !isRunning;
        
        cbEngine.IsEnabled = !isRunning && cellSizeSelector.Value == 1;
        cbEngine.IsEnabled = true;
        
        btnStop.IsEnabled = isRunning;
    }
    
    private void GameOfLiveView_PaintSurface(SKCanvas e)
    {
        VisualizerRender(e);
    }
    
    private void VisualizerRender(SKCanvas canvas)
    {
        switch (ruleSetType)
        {
            case RuleSetType.GameOfLife:
                var localBoolPlayGroundArray = (playGroundBool as PlayGround)!;
                SkiaVisualizer.Render(localBoolPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, (b, _) => b == CellType.Solid ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.Wolfram:
                var localWolframPlayGroundArray = (playGroundBool as PlayGround)!;
                SkiaVisualizer.Render(localWolframPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, (b, _) => b == CellType.Solid ? this.wolframColor : emptyColor);
                break;
            case RuleSetType.NoiseGrid:
                var localNoiseGridPlayGroundArray = (playGroundBool as PlayGround)!;
                SkiaVisualizer.Render(localNoiseGridPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, (b, _) => b == CellType.Solid ? this.noiseGridColor : emptyColor);
                break;
            case RuleSetType.Sand:
                var localSandCellStatePlayGroundArray = (playGroundSand as PlayGround)!;
                SkiaVisualizer.Render(localSandCellStatePlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, (_, b) => ChooseSandColor(b));
                break;
            default:    
                break;
        }
    }
    
    
    private async Task ProcessNextGenerationAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        
        var maxGenerations = stopWatchCountSelector.Value == null ? 50 : (int)stopWatchCountSelector.Value;
        
        currentGeneration = 0;
        
        generationTimes = new List<long>(maxGenerations);
        renderingTimes = new List<long>(maxGenerations);
        
        await Task.Run(async () =>
        {
            totalStopwatch.Restart();
            try
            {
                while (!token.IsCancellationRequested && currentGeneration < maxGenerations)
                {
                    if (timingEnabled)
                    {
                        generationStopwatch.Restart();
                    
                        GenerateNextPlaygroundState(ruleSetType);
                    
                        generationStopwatch.Stop();
                        generationTimes.Add(generationStopwatch.ElapsedTicks);
                        
                        renderingStopwatch.Restart();

                        //await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);
                        if (currentGeneration % 10 == 0)
                        {
                            await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);    
                        }
                        
                        renderingStopwatch.Stop();
                        renderingTimes.Add(renderingStopwatch.ElapsedTicks);
                    
                        currentGeneration++;
                        generation++;
                    }
                    else
                    {
                        GenerateNextPlaygroundState(ruleSetType);
                        await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);
                    }
                }
            }
            finally
            {
                totalStopwatch.Stop();
                await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);
            }
        }, token);
        
        GenerateStatisticsReport();

        SetButtonState(false);

        cancellationTokenSource = null;
    }

    private void GenerateStatisticsReport()
    {
        if (timingEnabled)
        {
            var statisticGenerator = new StatisticGenerator();

            tbStopWatch.Text = statisticGenerator.Generate(new AutomataStatistics(totalStopwatch.ElapsedTicks, currentGeneration, maxDegreeOfParallelism,
                renderingTimes, generationTimes));
        }
    }

    private void GenerateNextPlaygroundState(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => automataBool.NextGenerationParallel(playGroundBool,(ruleSet as GameOfLifeRuleSet)!, isSpawnActive, spawnPosition, brushSize, maxDegreeOfParallelism),
            RuleSetType.Wolfram => automataWolframBool.NextGenerationParallel(playGroundBool, (ruleSet as WolframRuleSet)!, generation - 1, maxDegreeOfParallelism),
            RuleSetType.NoiseGrid => automataNoiseGrid.NextGenerationParallel(playGroundBool, (ruleSet as NoiseGridRuleSet)!, maxDegreeOfParallelism),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSand.NextGenerationParallel(playGroundSand,(ruleSet as SandRuleSet)!, isSpawnActive, spawnPosition, brushSize, maxDegreeOfParallelism),
            _ => playGroundSand
        };
    }
    
    private void SpawnCells(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => automataBool.ApplySpawnRules(playGroundBool,(ruleSet as GameOfLifeRuleSet)!, spawnPosition, brushSize, spawnProbability),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSand.ApplySpawnRules(playGroundSand,(ruleSet as SandRuleSet)!, spawnPosition, brushSize, spawnProbability),
            _ => playGroundSand
        };
    }
    
    private SKColor ChooseSandColor(CellBrightness brightness)
    {
        return brightness switch
        {
            CellBrightness.Empty => emptyColor,
            CellBrightness.Normal => SKColors.Goldenrod,
            CellBrightness.Dark => SKColors.DarkGoldenrod,
            CellBrightness.Light => SKColors.LightGoldenrodYellow,
            CellBrightness.Medium => SKColors.Chocolate,
            CellBrightness.Solid => SKColors.Gray,
            _ => emptyColor
        };
    }
}