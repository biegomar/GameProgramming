using CellularAutomata;
using SkiaSharp;

namespace SkiasGameOfLive;

public partial class GameOfLiveForm : Form
{
    private static readonly Vector cellSize = new Vector(8, 8, 0);
    
    private Vector dimension;
    private GameOfLifeRuleSet ruleSet;
    private PlayGround<bool> playGround;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private int generation = 0;
    private int systemSpeed => (int)(systemSpeedSelector.Maximum - systemSpeedSelector.Value);
    private Vector bitmapSize => new Vector(this.GameOfLiveView.Width, this.GameOfLiveView.Height, 0);
    
    public GameOfLiveForm()
    {
        InitializeComponent();
        InitializePlayGround();
        RenderPlaygroundAndDisplayGeneration();
        SetButtonState(false);
    }

    private void InitializePlayGround()
    {
        dimension = new Vector((int)(bitmapSize.X / cellSize.X), (int)(bitmapSize.Y / cellSize.Y), 0);
        playGround = new PlayGround<bool>(dimension);
        ruleSet = new GameOfLifeRuleSet();

        var probability = (double)probabilitySelector.Value;

        switch (cbPattern.SelectedIndex)
        {
            case 0: // Random
                GameOfLifeInitializer.Randomize(playGround, probability);
                break;
            case 1: // Checkerboard
                GameOfLifeInitializer.AddCheckerboard(playGround);
                break;
            case 2: // Free Style
                GameOfLifeInitializer.AddSingleLineWithCellOnEveryXColumn(playGround, 10, 10);
                GameOfLifeInitializer.AddSingleColumnWithCellOnEveryYRow(playGround, 10, 10);
                GameOfLifeInitializer.AddSingleCell(playGround, Vector.Zero);
                GameOfLifeInitializer.AddSingleCell(playGround, new Vector(dimension.X - 1, dimension.Y - 1, 0));
                break;
        }
        
    }

    private void startGameOfLive_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSource == null)
        {
            NextGeneration();
        }
        
        SetButtonState(true);
    }

    private async void NextGeneration()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        
        await Task.Run(() =>
        {
            while (!token.IsCancellationRequested)
            {
                this.Invoke(RenderPlaygroundAndDisplayGeneration);
                
                playGround = Automata<bool>.NextGeneration(playGround, ruleSet);
                
                Thread.Sleep(systemSpeed);
            }
        }, token);
        
        cancellationTokenSource = null;
    }

    private void SetButtonState(bool isRunning)
    {
        btnStart.Enabled = !isRunning;
        btnReset.Enabled = !isRunning;
        
        btnStop.Enabled = isRunning;
    }

    private void RenderPlaygroundAndDisplayGeneration()
    {
        SkiaVisualizer<bool>.Render(playGround, cellSize, this.GameOfLiveView, b => b ? SKColors.Chartreuse : SKColors.Black);
        this.DisplayGeneration();
    }

    private void DisplayGeneration()
    {
        statusLabel.Text = $"Generation: {generation++}";
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        SetButtonState(false);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        generation = 0;
        InitializePlayGround();
        RenderPlaygroundAndDisplayGeneration();
    }
}