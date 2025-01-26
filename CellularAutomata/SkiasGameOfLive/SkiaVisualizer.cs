using CellularAutomata;

namespace SkiasGameOfLive;

using SkiaSharp;

public static class SkiaVisualizer<T>
{
    public static void Render(PlayGround<T> playGround, Vector cellSize, PictureBox pictureBox, Func<T, SKColor> stateToColor)
    {
        using (var surface = SKSurface.Create(new SKImageInfo(pictureBox.Width, pictureBox.Height)))
        {
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Black); 
            RenderPlayGround(playGround, cellSize, canvas, stateToColor);

            using (SKImage imageFromSnapshot = surface.Snapshot())
            using (SKData data = imageFromSnapshot.Encode())
            using (MemoryStream stream = new MemoryStream(data.ToArray()))
            {
                Bitmap bitmap = new Bitmap(stream);
                pictureBox.Image = bitmap;
            }
        }
    }
    
    private static void RenderPlayGround(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var positionToCheck = new Vector(x, y, 0);
                
                SKColor cellColor = stateToColor(playGround[positionToCheck]);
                
                var rect = new SKRect(x * cellSize.X, y * cellSize.Y, (x + 1) * cellSize.X, (y + 1) * cellSize.Y);
                using (var paint = new SKPaint { Color = cellColor, Style = SKPaintStyle.Fill })
                {
                    canvas.DrawRect(rect, paint);
                }
            }
        }
    }
}