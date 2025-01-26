using CellularAutomata;
using SkiaSharp;

namespace SkiasGameOfLive;

public partial class GameOfLiveForm : Form
{
    private readonly Vector dimension;
    private readonly Vector screenSize;
    private readonly GameOfLifeRuleSet ruleSet;
    
    private PlayGround<bool> playGround;
    private Bitmap? bitmap;
    
    private CancellationTokenSource? cancellationTokenSource;
    
    private int generation = 0;

    private int systemSpeed => (int)(systemSpeedSelector.Maximum - systemSpeedSelector.Value);
    
    public GameOfLiveForm()
    {
        InitializeComponent();
        
        dimension = new Vector(100,40,0);
        screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
        playGround = new PlayGround<bool>(dimension);
        ruleSet = new GameOfLifeRuleSet();
        
        InitializePlayGroundWithRandomValues((double)probabilitySelector.Value);
        RenderPlaygroundAndDisplayGeneration();
    }

    private void InitializePlayGroundWithRandomValues(double probability)
    {
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
        SkiaVisualizer<bool>.Render(playGround, 20, this.GameOfLiveView, b => b ? SKColors.Chartreuse : SKColors.Black);
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