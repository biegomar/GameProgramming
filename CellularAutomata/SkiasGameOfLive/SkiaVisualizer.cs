using CellularAutomata;

namespace SkiasGameOfLive;

using SkiaSharp;

public static class SkiaVisualizer<T>
{
    public static void Render(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        bool cellSizeIsDot = (int)cellSize.X == 1 && (int)cellSize.Y == 1;
        
        if (cellSizeIsDot)
        {
            RenderAsBitmap(playGround, canvas, stateToColor);
        }
        else
        {
            RenderAsRectangles(playGround, cellSize, canvas, stateToColor);
        }
    }
    
    private static void RenderAsBitmap(PlayGround<T> playGround, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        using var bitmap = new SKBitmap((int)playGround.Dimension.X, (int)playGround.Dimension.Y);
        //var pixels = bitmap.Pixels;
        
        var positionToCheck = Vector.Zero;
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                positionToCheck.X = x;
                positionToCheck.Y = y;
                SKColor color = stateToColor(playGround[positionToCheck]);
                bitmap.SetPixel(x, y, color);
                //pixels[y * (int)playGround.Dimension.X + x] = color;
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        {
            canvas.DrawImage(image, 0, 0);
        }
    }
    
    private static void RenderAsRectangles(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint { Style = SKPaintStyle.Fill };
        float cellWidth = cellSize.X;
        float cellHeight = cellSize.Y;

        var positionToCheck = Vector.Zero;

        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            float top = y * cellHeight;
            float bottom = top + cellHeight;

            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                float left = x * cellWidth;
                float right = left + cellWidth;

                positionToCheck.X = x;
                positionToCheck.Y = y;
                paint.Color = stateToColor(playGround[positionToCheck]);

                var rect = new SKRect(left, top, right, bottom);
                canvas.DrawRect(rect, paint);
            }
        }
    }
}