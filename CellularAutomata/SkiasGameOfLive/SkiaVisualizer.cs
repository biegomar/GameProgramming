using CellularAutomata;

namespace SkiasGameOfLive;

using SkiaSharp;

public static class SkiaVisualizer<T>
{
    public static void Render(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, canvas, stateToColor);
                break;
            case 2:
                RenderAsBitmap(playGround, canvas, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, stateToColor);
                break;
        }
    }
    
    public static void Render(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, canvas, stateToColor);
                break;
            case 2:
                RenderAsBitmap(playGround, canvas, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, stateToColor);
                break;
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
    
    private static void RenderAsBitmap(PlayGroundArray<T> playGround, SKCanvas canvas, Func<T, SKColor> stateToColor)
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
    
    private static void RenderPixel(PlayGround<T> playGround, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint { Style = SKPaintStyle.Fill };

        var positionToCheck = Vector.Zero;

        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                positionToCheck.X = x;
                positionToCheck.Y = y;

                var point = new SKPoint(x,y);
                SKColor color = stateToColor(playGround[positionToCheck]);
                canvas.DrawPoint(point, color);
            }
        }
    }
    
    private static void RenderPixel(PlayGroundArray<T> playGround, SKCanvas canvas, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint { Style = SKPaintStyle.Fill };

        var positionToCheck = Vector.Zero;

        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                positionToCheck.X = x;
                positionToCheck.Y = y;

                var point = new SKPoint(x,y);
                SKColor color = stateToColor(playGround[positionToCheck]);
                canvas.DrawPoint(point, color);
            }
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
    
    private static void RenderAsRectangles(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, Func<T, SKColor> stateToColor)
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