using CellularAutomata;

namespace SkiasGameOfLive;

using SkiaSharp;

public static class SkiaVisualizer<T>
{
    public static void Render(PlayGround<T> playGround, int cellSize, PictureBox pictureBox, Func<T, SKColor> stateToColor)
    {
        using (var surface = SKSurface.Create(new SKImageInfo(pictureBox.Width, pictureBox.Height)))
        {
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Black); 
            RenderPlayGround(playGround, canvas, stateToColor);

            using (SKImage imageFromSnapshot = surface.Snapshot())
            using (SKData data = imageFromSnapshot.Encode())
            using (MemoryStream stream = new MemoryStream(data.ToArray()))
            {
                Bitmap bitmap = new Bitmap(stream);
                pictureBox.Image = bitmap;
            }
        }
    }
    
    private static void RenderPlayGround(PlayGround<T> playGround, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        int cellSize = 20;
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var positionToCheck = new Vector(x, y, 0);
                
                SKColor cellColor = stateToColor(playGround[positionToCheck]);
                
                var rect = new SKRect(x * cellSize, y * cellSize, (x + 1) * cellSize, (y + 1) * cellSize);
                using (var paint = new SKPaint { Color = cellColor, Style = SKPaintStyle.Fill })
                {
                    canvas.DrawRect(rect, paint);
                }
            }
        }
    }
}