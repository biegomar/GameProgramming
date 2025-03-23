using System.Diagnostics;
using System.Text;
using CellularAutomata;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Supporter;
using Visualizer;
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
    private readonly Vector spawnPosition = new Vector(5,5);
    private readonly Vector brushSize = new Vector(5,5);
    
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
    private int maxDegreeOfParallelism = 2;

    private IPlayGround playGroundBool;
    private IPlayGround playGroundSand;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private Automata automataBool;
    private AutomataArray automataArrayBool;
    private Automata automataSandBool;
    private AutomataArray automataSandArrayBool;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private Vector bitmapSize => new (this.GameOfLiveView.Width, this.GameOfLiveView.Height);
    
    public GameOfLiveForm()
    {
        InitializeComponent();
        InitializeComponentValues();
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
    
    private void InitializeComponentValues()
    {
        processorCountSelector.Maximum = Environment.ProcessorCount;
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
        
        automataBool = new Automata(dimension);
        automataArrayBool = new AutomataArray(dimension);
        automataSandBool = new Automata(dimension);
        automataSandArrayBool = new AutomataArray(dimension);

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
        playGroundSand = new PlayGround(dimension);
        ruleSet = new SandRuleSet(dimension);

        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;

        switch (cbPatternSand.SelectedIndex)
        {
            case 0:
                GameOfLifeInitializer.Randomize(playGroundSand, maxDegreeOfParallelism, (double)probabilitySelector.Value);
                break;
            case 1:
                SandInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), CellState.Sand);

                // add some terrain
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), CellState.Solid);

                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), CellState.Solid);
                break;
        }
    }
    
    private void InitializeForSandArray()
    {
        playGroundSand = new PlayGroundArray(dimension);
        ruleSet = new SandRuleSetArray(dimension);
        
        var middle = playGroundSand.Dimension.X / 2;
        aliveColor = SKColors.Bisque;
        
        switch (cbPatternSand.SelectedIndex)
        {
            case 0:
                GameOfLifeInitializer.Randomize(playGroundSand, maxDegreeOfParallelism, (double)probabilitySelector.Value);
                break;
            case 1:
                SandInitializer.GenerateSandHourglass(playGroundSand);
                break;
            case 2:
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 0), CellState.Sand);

                // add some terrain
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle + 1, 10), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 11), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 12), CellState.Solid);

                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle, 20), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 1, 19), CellState.Solid);
                GameOfLifeInitializer.AddSandCellStateToCell(playGroundSand, new Vector(middle - 2, 18), CellState.Solid);
                break;
        }
    }

    private void InitializeForGameOfLive()
    {
        playGroundBool = new PlayGround(dimension);
        ruleSet = new GameOfLifeRuleSet(dimension);
        
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
    
    private void InitializeForGameOfLiveArray()
    {
        playGroundBool = new PlayGroundArray(dimension);
        ruleSet = new GameOfLifeRuleSetArray(dimension);
        
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
                    
                        currentGeneration++;
                    }
                    else
                    {
                        GenerateNextPlaygroundState(ruleSetType);
                    }

                    await InvokeAsync(RenderPlaygroundAndDisplayGeneration, token);
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
            RuleSetType.GameOfLife => automataBool.NextGenerationParallel((playGroundBool as PlayGround)!, (ruleSet as GameOfLifeRuleSet)!, false, spawnPosition, brushSize, maxDegreeOfParallelism),
            RuleSetType.GameOfLifeArray => automataArrayBool.NextGenerationParallel((playGroundBool as PlayGroundArray)!,(ruleSet as GameOfLifeRuleSetArray)!, false, spawnPosition, brushSize, maxDegreeOfParallelism),
            _ => playGroundBool
        };
        
        playGroundSand = type switch
        {
            RuleSetType.Sand => automataSandBool.NextGenerationParallel((playGroundSand as PlayGround)!, (ruleSet as SandRuleSet)!, false, spawnPosition, brushSize, maxDegreeOfParallelism),
            RuleSetType.SandArray => automataSandArrayBool.NextGenerationParallel((playGroundSand as PlayGroundArray)!,(ruleSet as SandRuleSetArray)!, false, spawnPosition, brushSize, maxDegreeOfParallelism),
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
            renderingTimes.Add(renderingStopwatch.ElapsedTicks);
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

    private void processorCountSelector_ValueChanged(object sender, EventArgs e)
    {
        maxDegreeOfParallelism = (int)processorCountSelector.Value;
    }
}