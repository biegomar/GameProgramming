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
        GameOfLife
    }
    
    private Vector cellSize => new Vector((int)cellSizeSelector.Value, (int)cellSizeSelector.Value, 0);
    
    private Vector dimension;
    private SKColor aliveColor = SKColors.Chartreuse;
    private SKColor emptyColor = SKColors.Black;
    
    private ToolTip toolTip = new ToolTip();
    private Timer toolTipTimer = new Timer();


    private IPlayGround playGround;
    private IBaseRuleSet ruleSet;
    private RuleSetType ruleSetType;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private int generation = 0;
    private int systemSpeed => (int)(systemSpeedSelector.Maximum - systemSpeedSelector.Value);
    private Vector bitmapSize => new Vector(this.GameOfLiveView.Width, this.GameOfLiveView.Height, 0);
    
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
            case RuleSetType.Sand:
                InitializeForSand();
                break;
            case RuleSetType.GameOfLife:
                InitializeForGameOfLive();
                break;
        }
        
        RenderPlaygroundAndDisplayGeneration();
    }

    private void InitializeForSand()
    {
        playGround = new PlayGround<SandCellState>(dimension);
        var sandPlayGround = (playGround as PlayGround<SandCellState>)!;

        ruleSet = new SandRuleSet();

        var middle = (int)(sandPlayGround.Dimension.X / 2);
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
        
        GameOfLifeInitializer.GenerateSandHourglass(sandPlayGround);
        //GameOfLifeInitializer.TestCaseOne(sandPlayGround);
    }

    private void InitializeForGameOfLive()
    {
        playGround = new PlayGround<bool>(dimension);
        var gamePlayGround = (playGround as PlayGround<bool>)!;
        
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
    
    private RuleSetType GetTypeFromSelection() 
    {
        return cbRuleSet.SelectedIndex switch
        {
            1 => RuleSetType.Sand,        
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
        
        await Task.Run(() =>
        {
            while (!token.IsCancellationRequested)
            {
                GenerateNextPlaygroundState(ruleSetType);
                
                Invoke(RenderPlaygroundAndDisplayGeneration);

                if (systemSpeed > 0)
                {
                    Thread.Sleep(systemSpeed);    
                }
            }
        }, token);
        
        cancellationTokenSource = null;
    }

    private void GenerateNextPlaygroundState(RuleSetType type)
    {
        playGround = type switch
        {
            RuleSetType.Sand => Automata<SandCellState>.NextGeneration((playGround as PlayGround<SandCellState>)!,
                (ruleSet as SandRuleSet)!, false),
            RuleSetType.GameOfLife => Automata<bool>.NextGenerationParallel((playGround as PlayGround<bool>)!,
                (ruleSet as GameOfLifeRuleSet)!, false),
            _ => playGround
        };
    }

    private void SetButtonState(bool isRunning)
    {
        btnStart.Enabled = !isRunning;
        btnReset.Enabled = !isRunning;
        btnSingleStep.Enabled = !isRunning;
        
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
                SkiaVisualizer<SandCellState>.Render((playGround as PlayGround<SandCellState>)!, cellSize, canvas, b =>
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
                SkiaVisualizer<bool>.Render((playGround as PlayGround<bool>)!, cellSize, canvas, b => b ? this.aliveColor : emptyColor);
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
}