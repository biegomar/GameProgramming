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
        InitializePlayGroundWithRandomValues((double)probabilitySelector.Value);
        RenderPlaygroundAndDisplayGeneration();
    }

    private void InitializePlayGroundWithRandomValues(double probability)
    {
        dimension = new Vector((int)(bitmapSize.X / cellSize.X), (int)(bitmapSize.Y / cellSize.Y), 0);
        playGround = new PlayGround<bool>(dimension);
        ruleSet = new GameOfLifeRuleSet();
        
        GameOfLifeInitializer.Randomize(playGround, probability);
    }

    private void startGameOfLive_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSource == null)
        {
            NextGeneration();
        }
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

    private void RenderPlaygroundAndDisplayGeneration()
    {
        SkiaVisualizer<bool>.Render(playGround, cellSize, this.GameOfLiveView, b => b ? SKColors.Chartreuse : SKColors.Black);
        this.DisplayGeneration();
    }

    private void btnPause_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
    }

    private void DisplayGeneration()
    {
        statusLabel.Text = $"Generation: {generation++}";
    }

    private void btnStopReset_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        
        Thread.Sleep(100);
        
        generation = 0;
        InitializePlayGroundWithRandomValues((double)probabilitySelector.Value);
        RenderPlaygroundAndDisplayGeneration();
    }
}