using CellularAutomata;

namespace SkiasGameOfLive;

using SkiaSharp;

public static class SkiaVisualizer<T>
{
    
    public static void Render(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, canvas, emptyColor, stateToColor);
                break;
            case 2:
                RenderAsBitmap(playGround, canvas, emptyColor, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
        }
    }
    
    public static void Render(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, canvas, emptyColor, stateToColor);
                break;
            case 2:
                RenderAsBitmap(playGround, canvas, emptyColor, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
        }
    }
    
    private static void RenderAsBitmap(PlayGround<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        using var bitmap = new SKBitmap(playGround.Dimension.X, playGround.Dimension.Y);
        //var pixels = bitmap.Pixels;
        
        var positionToCheck = new Vector(0, 0);
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var color = stateToColor(playGround[positionToCheck]);
                if (color == emptyColor) continue;
                
                positionToCheck.X = x;
                positionToCheck.Y = y;
                bitmap.SetPixel(x, y, color);
                //pixels[y * (int)playGround.Dimension.X + x] = color;
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        {
            canvas.DrawImage(image, 0, 0);
        }
    }
    
    private static void RenderAsBitmap(PlayGroundArray<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        using var bitmap = new SKBitmap(playGround.Dimension.X, playGround.Dimension.Y);
        //var pixels = bitmap.Pixels;
        
        var positionToCheck = new Vector(0, 0);
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var color = stateToColor(playGround[positionToCheck]);
                if (color == emptyColor) continue;
                
                positionToCheck.X = x;
                positionToCheck.Y = y;
                
                bitmap.SetPixel(x, y, color);
                //pixels[y * (int)playGround.Dimension.X + x] = color;
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        {
            canvas.DrawImage(image, 0, 0);
        }
    }
    
    private static void RenderPixel(PlayGround<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        var positionToCheck = new Vector(0, 0);

        using var paint = new SKPaint();
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var color = stateToColor(playGround[positionToCheck]);
                if (color == emptyColor) continue;
                
                positionToCheck.X = x;
                positionToCheck.Y = y;

                paint.Color = color;
                canvas.DrawPoint(x, y, paint);
            }
        }
    }
    
    private static void RenderPixel(PlayGroundArray<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint();
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var color = stateToColor(playGround[(x,y)]);
                if (color == emptyColor) continue;
                
                //var point = new SKPoint(x,y);
                paint.Color = color;
                canvas.DrawPoint(x, y, paint);
            }
        }
    }
    
    private static void RenderAsRectangles(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint();
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        
        foreach (var cell in playGround.Cells.Values)
        {
            var top = cell.Y * cellHeight;
            var bottom = top + cellHeight;
            var left = cell.X * cellWidth;
            var right = left + cellWidth;
            
            var color = stateToColor(cell.State);
            if (color == emptyColor) continue;
            
            paint.Color = color;
            var rect = new SKRect(left, top, right, bottom);
        
            canvas.DrawRect(rect, paint);
        }
        
        canvas.Flush();
    }
    
    private static void RenderAsRectangles(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        using var paint = new SKPaint();
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        
        foreach (var cell in playGround.Cells)
        {
            var top = cell.Y * cellHeight;
            var bottom = top + cellHeight;
            var left = cell.X * cellWidth;
            var right = left + cellWidth;
            
            var color = stateToColor(cell.State);
            if (color == emptyColor) continue;
            
            paint.Color = color;
            var rect = new SKRect(left, top, right, bottom);
        
            canvas.DrawRect(rect, paint);
        }
        
        canvas.Flush();
        
        // for (var y = 0; y < playGround.Dimension.Y; y++)
        // {
        //     var top = y * cellHeight;
        //     var bottom = top + cellHeight;
        //
        //     for (var x = 0; x < playGround.Dimension.X; x++)
        //     {
        //         var left = x * cellWidth;
        //         var right = left + cellWidth;
        //         
        //         paint.Color = stateToColor(playGround[(x,y,0)]);
        //
        //         var rect = new SKRect(left, top, right, bottom);
        //         canvas.DrawRect(rect, paint);
        //     }
        // }
    }
}