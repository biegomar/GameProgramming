using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CellularAutomata;
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
        SandArray,
        GameOfLifeArray,
        Wolfram,
    }
    
    private Vector cellSize => new ((int)cellSizeSelector.Value, (int)cellSizeSelector.Value);
    private Vector dimension;
    private SKColor aliveColor = SKColors.Chartreuse;
    private readonly SKColor emptyColor = SKColors.Red;
    
    private ToolTip toolTip;
    private readonly DispatcherTimer toolTipTimer = new ();
    private bool isTooltipVisible = false;
    private bool isSpawnActive = false;
    private Vector spawnPosition = new (0, 0);

    private IList<long> generationTimes;
    private IList<long> renderingTimes;
    
    private readonly Stopwatch generationStopwatch = new ();
    private readonly Stopwatch renderingStopwatch = new ();
    private readonly Stopwatch totalStopwatch = new ();
    private readonly Random random = new ();
    
    private bool timingEnabled = true;
    private int currentGeneration = 0;
    private int generation = 0;
    private int maxDegreeOfParallelism = 2;
    
    private IPlayGround playGroundBool;
    private IPlayGround playGroundSand;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private Automata automataBool;
    private AutomataArray automataArrayBool;
    private Automata automataSand;
    private AutomataArray automataSandArray;
    private AutomataWolfram automataWolframBool;
    
    private CancellationTokenSource? cancellationTokenSource;
    private CancellationTokenSource? tooltipCancellationSource;

    
    private Vector bitmapSize => new ((int)this.GameOfLiveView.Width, (int)this.GameOfLiveView.Height);
    
    public MainWindow()
    {
        InitializeComponent();
        InitializeComponentValues();
        InitializeEventHandlers();
        InitializePlayGround();
        SetButtonState(false);
    }

    private void InitializeComponentValues()
    {
        processorCountSelector.Maximum = Environment.ProcessorCount;
        maxDegreeOfParallelism = (int)processorCountSelector.Value!;
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
        processorCountSelector.ValueChanged += processorCountSelector_ValueChanged;
        
        GameOfLiveView.PaintSurface += GameOfLiveView_PaintSurface;
        GameOfLiveView.PointerPressed += GameOfLiveView_PointerPressed;
        GameOfLiveView.PointerReleased += GameOfLiveView_PointerReleased;
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
        ruleSetType = GetTypeFromSelection();
        SetPatternItems(ruleSetType);
        
        InitializePlayGround();
    }
    
    private void cellSizeSelector_ValueChanged(object sender, EventArgs e)
    {
        InitializePlayGround();
    }

    private void processorCountSelector_ValueChanged(object sender, EventArgs e)
    {
        maxDegreeOfParallelism = (int)processorCountSelector.Value!;
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
        if (!isSpawnActive) isSpawnActive = true;

        var position = e.GetPosition(GameOfLiveView);
        spawnPosition = GetCellPositionFromMouseCursor(position);
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
            case RuleSetType.SandArray:
                return GetCellStateFromSand(cellPosition);
            case RuleSetType.GameOfLife:
            case RuleSetType.GameOfLifeArray:
            case RuleSetType.Wolfram:
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
            case RuleSetType.Wolfram:
                InitializeWolfram();
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
    }
    
    private RuleSetType GetTypeFromSelection() 
    {
        return cbRuleSet.SelectedIndex switch
        {
            0 => RuleSetType.GameOfLife,
            1 => RuleSetType.Sand,
            2 => RuleSetType.GameOfLifeArray,
            3 => RuleSetType.SandArray,
            4 => RuleSetType.Wolfram,
            _ => RuleSetType.GameOfLife,
        };
    }

    private void SetPatternItems(RuleSetType ruleSetType)
    {
        cbPattern.Items.Clear();

        switch (ruleSetType)
        {
            case RuleSetType.Sand:
            case RuleSetType.SandArray:
                cbPattern.Items.Add("Random");
                cbPattern.Items.Add("Sanduhr");
                cbPattern.Items.Add("Freestyle");
                break;
            case RuleSetType.Wolfram:
                foreach (var item in Enumerable.Range(0, 256).Select(n => n.ToString()))
                {
                    cbPattern.Items.Add(item);
                }
                break;
            case RuleSetType.GameOfLife:
            case RuleSetType.GameOfLifeArray:
            default:
                cbPattern.Items.Add("Random");
                cbPattern.Items.Add("Schachbrett");
                cbPattern.Items.Add("Freestyle");
                break;
        }

        cbPattern.SelectedIndex = 0;
    }

    private void InitializeWolfram()
    {
        automataWolframBool = new AutomataWolfram();
        playGroundBool = new PlayGroundArray(dimension);

        ruleSet = new WolframRuleSet(cbPattern.SelectedIndex);
        
        aliveColor = SKColors.CornflowerBlue;
        
        GameOfLifeInitializer.AddSingleCell(playGroundBool, new Vector(dimension.X/2, 0));

    }
    private void InitializeForGameOfLive()
    {
        automataBool = new Automata(dimension);
        playGroundBool = new PlayGround(dimension);
        
        ruleSet = new GameOfLifeRuleSet();
        
        aliveColor = SKColors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(playGroundBool, maxDegreeOfParallelism, (double)probabilitySelector.Value!);
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
    
    private void InitializeForGameOfLiveArray()
    {
        automataArrayBool = new AutomataArray(dimension);
        playGroundBool = new PlayGroundArray(dimension);
        
        ruleSet = new GameOfLifeRuleSetArray();
        
        aliveColor = SKColors.Chartreuse;
        switch (cbPattern.SelectedIndex)
        {
            case 0: 
                GameOfLifeInitializer.Randomize(playGroundBool, maxDegreeOfParallelism, (double)probabilitySelector.Value);
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
        automataSand = new Automata(dimension);
        playGroundSand = new PlayGround(dimension);

        ruleSet = new SandRuleSet();

        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;

        switch (cbPattern.SelectedIndex)
        {
            case 0:
                SandInitializer.Randomize(playGroundSand, maxDegreeOfParallelism, (double)probabilitySelector.Value);
                break;
            case 1:
                SandInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), CellState.Sand);

                // add some terrain
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), CellState.Solid);

                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), CellState.Solid);
                break;
        }
    }
    
    private void InitializeForSandArray()
    {
        automataSandArray = new AutomataArray(dimension);
        playGroundSand = new PlayGroundArray(dimension);

        ruleSet = new SandRuleSetArray();
        
        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;
        
        switch (cbPattern.SelectedIndex)
        {
            case 0:
                SandInitializer.Randomize(playGroundSand, maxDegreeOfParallelism, (double)probabilitySelector.Value);
                break;
            case 1:
                SandInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), CellState.Sand);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 1), CellState.SandDark);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 2), CellState.SandMedium);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 2), CellState.SandLight);

                // add some terrain
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), CellState.Solid);

                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), CellState.Solid);
                SandInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), CellState.Solid);
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
        VisualizerRender(ruleSetType, e);
    }
    
    private void VisualizerRender(RuleSetType type, SKCanvas canvas)
    {
        switch (type)
        {
            case RuleSetType.GameOfLifeArray:
            case RuleSetType.Wolfram:
                PlayGroundArray localBoolPlayGroundArray = (playGroundBool as PlayGroundArray)!;
                SkiaVisualizer.Render(localBoolPlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, b => b == CellState.Solid ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.Sand:
                PlayGround localSandCellStatePlayGround = (playGroundSand as PlayGround)!;
                SkiaVisualizer.Render(localSandCellStatePlayGround, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, ChooseSandColor);
                break;
            case RuleSetType.GameOfLife:
                PlayGround localBoolPlayGround = (playGroundBool as PlayGround)!;
                SkiaVisualizer.Render(localBoolPlayGround, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, b => b == CellState.Solid ? this.aliveColor : emptyColor);
                break;
            case RuleSetType.SandArray:
                PlayGroundArray localSandCellStatePlayGroundArray = (playGroundSand as PlayGroundArray)!;
                SkiaVisualizer.Render(localSandCellStatePlayGroundArray, cellSize, canvas, cbEngine.SelectedIndex, emptyColor, maxDegreeOfParallelism, ChooseSandColor);
                break;
            default:    
                break;
        }
    }
    
    
    private async Task ProcessNextGenerationAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        var maxGenerations = stopWatchCountSelector.Value == null ? 50 : (int)stopWatchCountSelector.Value;
        
        currentGeneration = 0;
        
        generationTimes = new List<long>(2000);
        renderingTimes = new List<long>(2000);
        
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
                        
                        await Dispatcher.UIThread.InvokeAsync(RenderPlaygroundAndDisplayGeneration, DispatcherPriority.MaxValue);
                        
                        renderingStopwatch.Stop();
                        renderingTimes.Add(renderingStopwatch.ElapsedTicks);
                    
                        currentGeneration++;
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
                generationTimes, renderingTimes));
        }
    }

    private void GenerateNextPlaygroundState(RuleSetType type)
    {
        playGroundBool = type switch
        {
            RuleSetType.GameOfLife => automataBool.NextGenerationParallel((playGroundBool as PlayGround)!, (ruleSet as GameOfLifeRuleSet)!, isSpawnActive, spawnPosition, maxDegreeOfParallelism),
            RuleSetType.GameOfLifeArray => automataArrayBool.NextGenerationParallel((playGroundBool as PlayGroundArray)!,(ruleSet as GameOfLifeRuleSetArray)!, isSpawnActive, spawnPosition, maxDegreeOfParallelism),
            RuleSetType.Wolfram => automataWolframBool.NextGenerationParallel((playGroundBool as PlayGroundArray)!, (ruleSet as WolframRuleSet)!, generation - 1, maxDegreeOfParallelism),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSand.NextGenerationParallel((playGroundSand as PlayGround)!, (ruleSet as SandRuleSet)!, isSpawnActive, spawnPosition, maxDegreeOfParallelism),
            RuleSetType.SandArray => automataSandArray.NextGenerationParallel((playGroundSand as PlayGroundArray)!,(ruleSet as SandRuleSetArray)!, isSpawnActive, spawnPosition, maxDegreeOfParallelism),
            _ => playGroundSand
        };
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
    
    private SKColor ChooseSandColor(CellState state)
    {
        return state switch
        {
            CellState.Empty => emptyColor,
            CellState.Sand => SKColors.Goldenrod,
            CellState.SandDark => SKColors.DarkGoldenrod,
            CellState.SandLight => SKColors.LightGoldenrodYellow,
            CellState.SandMedium => SKColors.Chocolate,
            CellState.Solid => SKColors.Gray,
            _ => emptyColor
        };
    }
}