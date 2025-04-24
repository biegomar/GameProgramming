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
using CellularAutomata.Cells;
using CellularAutomata.GameOfLive;
using CellularAutomata.MaterialFlow;
using CellularAutomata.NoiseGrid;
using CellularAutomata.PlayGrounds;
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
    private bool isTooltipVisible = false;
    
    private bool isSpawnActive = false;
    private Vector spawnPosition = new (0, 0);
    private Vector brushSize = new (10,10);
    private CellType actualSpawnType = CellType.Empty;

    private IList<long> generationTimes;
    private IList<long> renderingTimes;

    private bool shouldDraw = true;
    private bool timingEnabled = true;
    private int currentGeneration = 0;
    private int generation = 0;
    private int maxDegreeOfParallelism = 2;
    
    private PlayGround playGroundSand;
    private RuleSetType ruleSetType;
    
    private SimplePlayGround playGroundBool;
    private Automata automataBool;
    private GameOfLifeRuleSet ruleSetBool;
    
    private AutomataMaterialGrid automataSand;
    private MaterialRuleSet ruleSetMaterial;
    
    private AutomataWolfram automataWolframBool;
    private WolframRuleSet ruleSetWolfram;
    
    private AutomataNoiseGrid automataNoiseGrid;
    private NoiseGridRuleSet ruleSetNoise;
    
    private CancellationTokenSource? cancellationTokenSource;
    private CancellationTokenSource? tooltipCancellationSource;
    
    private Vector bitmapSize => new ((int)this.GameOfLiveView.Width, (int)this.GameOfLiveView.Height);
    
    public MainWindow()
    {
        //var test = Marshal.SizeOf<Cell>();

        InitializeComponent();
        InitializeComponentValues();
        InitializeEventHandlers();
        InitializePlayGround();
        SetButtonState(false);
    }

    private void InitializeComponentValues()
    {
        SetProcessorCountSelectorMax();
        SetMaxDegreeOfParallelism();
        SetInitializationProbability();
        SetSpawnProbability();
    }

    private void SetProcessorCountSelectorMax()
    {
        processorCountSelector.Maximum = Environment.ProcessorCount;
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
        cbMaterial.SelectionChanged += cbMaterial_SelectedValueChanged;
        
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
    
    private void cbMaterial_SelectedValueChanged(object? sender, EventArgs e)
    {
        SetSpawnType();
    }

    private void SetBrushSize()
    {
        var brushSquare = (int)brushSizeSelector.Value!;
        brushSize = new Vector(brushSquare, brushSquare);
    }

    private void SetSpawnType()
    {
        actualSpawnType = (CellType)cbMaterial.SelectedIndex;
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
        
        var cellX = (int)Math.Floor(mouseX / viewWidth * dimension.X); 
        var cellY = (int)Math.Floor(mouseY / viewHeight * dimension.Y);
        
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
        return playGroundBool.GetState(cellPosition).ToString();
    }

    private string GetCellStateFromSand(Vector cellPosition)
    {
        return playGroundSand.GetCell(cellPosition).Type.ToString();
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
        playGroundBool = new SimplePlayGround(dimension);
        ruleSetBool = new GameOfLifeRuleSet(dimension);
        
        InitializeGameOfLifePattern();
    }
    
    private void InitializeGameOfLifePattern()
    {
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(playGroundBool, initializationProbability);
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
        ruleSetMaterial = new MaterialRuleSet(dimension);
        
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
        InitializeWolframPattern();
        
        automataWolframBool = new AutomataWolfram(playGroundBool);
        ruleSetWolfram = new WolframRuleSet(cbPattern.SelectedIndex);
    }
    
    private void InitializeWolframPattern()
    {
        playGroundBool = new SimplePlayGround(dimension);
        GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(dimension.X/2, 0));
    }

    private void InitializeNoise()
    {
        automataNoiseGrid = new AutomataNoiseGrid(dimension);
        playGroundBool = new SimplePlayGround(dimension);
        ruleSetNoise = new NoiseGridRuleSet(dimension);
        
        InitializeNoisePattern();
    }

    private void InitializeNoisePattern()
    {
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                NoiseGridInitializer.Randomize(playGroundBool, initializationProbability);
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
                SkiaVisualizer.RenderSimplePlayGround(playGroundBool, cellSize, canvas, cbEngine.SelectedIndex, maxDegreeOfParallelism, b => b ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.Wolfram:
                SkiaVisualizer.RenderSimplePlayGround(playGroundBool, cellSize, canvas, cbEngine.SelectedIndex, maxDegreeOfParallelism, b => b  ? this.wolframColor : emptyColor);
                break;
            case RuleSetType.NoiseGrid:
                SkiaVisualizer.RenderSimplePlayGround(playGroundBool, cellSize, canvas, cbEngine.SelectedIndex, maxDegreeOfParallelism, b => b  ? this.noiseGridColor : emptyColor);
                break;
            case RuleSetType.Sand:
                SkiaVisualizer.Render(playGroundSand, cellSize, canvas, cbEngine.SelectedIndex, maxDegreeOfParallelism, ChooseSandColor);
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

        var totalTicks = 0L;
        
        currentGeneration = 0;
        
        generationTimes = new List<long>(maxGenerations);
        renderingTimes = new List<long>(maxGenerations);
        
        
        await Task.Run(async () =>
        {
            var totalStartTimestamp = Stopwatch.GetTimestamp();
            try
            {
                while (!token.IsCancellationRequested && currentGeneration < maxGenerations)
                {
                    if (timingEnabled)
                    {
                        var generationStartTimestamp = Stopwatch.GetTimestamp();
                    
                        GenerateNextPlaygroundState(ruleSetType);
                        
                        generationTimes.Add(Stopwatch.GetElapsedTime(generationStartTimestamp).Ticks);
                        
                        var renderingStartTimestamp = Stopwatch.GetTimestamp();
                        
                        if (currentGeneration % 10 == 0)
                        {
                            await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);    
                        }
                        
                        renderingTimes.Add(Stopwatch.GetElapsedTime(renderingStartTimestamp).Ticks);
                    
                        currentGeneration++;
                        generation++;
                    }
                    else
                    {
                        GenerateNextPlaygroundState(ruleSetType);
                        if (currentGeneration % 10 == 0)
                        {
                            await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);    
                        }
                    }
                }
            }
            finally
            {
                totalTicks = Stopwatch.GetElapsedTime(totalStartTimestamp).Ticks;
                await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);
            }
        }, token);
        
        GenerateStatisticsReport(totalTicks);
        
        SetButtonState(false);

        cancellationTokenSource = null;
    }

    private void GenerateStatisticsReport(long totalTicks)
    {
        if (timingEnabled)
        {
            var statisticGenerator = new StatisticGenerator();

            tbStopWatch.Text = statisticGenerator.Generate(new AutomataStatistics(totalTicks, currentGeneration, maxDegreeOfParallelism,
                renderingTimes, generationTimes));
        }
    }

    private void GenerateNextPlaygroundState(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => automataBool.NextGenerationParallel(playGroundBool, ruleSetBool, isSpawnActive, spawnPosition, brushSize, maxDegreeOfParallelism),
            RuleSetType.Wolfram => automataWolframBool.NextGenerationParallel(playGroundBool, ruleSetWolfram, generation - 1, maxDegreeOfParallelism),
            RuleSetType.NoiseGrid => automataNoiseGrid.NextGenerationParallel(playGroundBool, ruleSetNoise, maxDegreeOfParallelism),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSand.NextGenerationParallel(playGroundSand, ruleSetMaterial, maxDegreeOfParallelism),
            _ => playGroundSand
        };
    }
    
    private void SpawnCells(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => automataBool.ApplySpawnRules(playGroundBool, ruleSetBool, spawnPosition, brushSize, spawnProbability),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSand.ApplySpawnRules(playGroundSand, ruleSetMaterial, actualSpawnType, spawnPosition, brushSize, spawnProbability),
            _ => playGroundSand
        };
    }
    
    private SKColor ChooseSandColor(CellColor color) =>
        color switch
        {
            CellColor.Empty => emptyColor,
            CellColor.Solid => SKColors.Gray,
            CellColor.GoldenSand => new SKColor(210, 168, 105), 
            CellColor.DesertGold => new SKColor(214, 171, 107),
            CellColor.Wheatfield => new SKColor(206, 165, 103),
            CellColor.SaharaDune => new SKColor(212, 170, 106),   
            CellColor.HoneyBeige => new SKColor(208, 166, 104), 
            CellColor.ToastedAlmond => new SKColor(207, 166, 104), 
            CellColor.AmberGrain => new SKColor(216, 173, 108), 
            CellColor.ClayOchre => new SKColor(209, 167, 104), 
            CellColor.GoldenWheat => new SKColor(213, 170, 106), 
            CellColor.SunlitSandstone => new SKColor(211, 169, 106),
            CellColor.CoolBlue => new SKColor(81, 130, 203, 255), 
            CellColor.OceanBlue => new SKColor(87, 139, 217, 255), 
            CellColor.DeepSky => new SKColor(80, 128, 200, 255), 
            CellColor.CrystalLake => new SKColor(83, 133, 208, 255), 
            CellColor.SurfBlue => new SKColor(84, 134, 209, 255), 
            CellColor.BlueCurrent => new SKColor(82, 132, 206, 255), 
            CellColor.SplashBlue => new SKColor(89, 142, 222, 255), 
            CellColor.AzureDrift => new SKColor(88, 140, 219, 255), 
            CellColor.Wavestone => new SKColor(85, 136, 212, 255), 
            CellColor.RippleBlue => new SKColor(81, 129, 202, 255),
            _ => emptyColor
        };
}