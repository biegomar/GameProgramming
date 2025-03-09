using CellularAutomata;
using SkiaSharp;

namespace Visualizer;

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
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
        }
    }
    
    private static void RenderPixel(PlayGround<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        //var positionToCheck = new Vector(0, 0);

        using var paint = new SKPaint();
        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var color = stateToColor(playGround[(x,y)]);
                if (color == emptyColor) continue;
                
                paint.Color = color;
                canvas.DrawPoint(x, y, paint);
            }
        }
    }
    
    private static void RenderPixel(PlayGroundArray<T> playGround, SKCanvas canvas, SKColor emptyColor, Func<T, SKColor> stateToColor)
    {
        var points = new List<SKPoint>(playGround.Dimension.X * playGround.Dimension.Y);
        //var points = new SKPoint[playGround.Dimension.X * playGround.Dimension.Y];

        SKColor drawingColor = emptyColor;
        var index = 0;
        for (var x = 0; x < playGround.Dimension.X; x++)
        {
            for (var y = 0; y < playGround.Dimension.Y; y++)
            {
                var color = stateToColor(playGround[(x,y)]);
                if (color == emptyColor) continue;
                
                if (!drawingColor.Equals(color)) drawingColor = color;
                
                //points[index++] = new SKPoint(x, y); 
                points.Add(new SKPoint(x, y));
            }
        }
        
        using var paint = new SKPaint();
        paint.Color = drawingColor;
        paint.IsAntialias = false;
        paint.Style = SKPaintStyle.Fill;
        paint.StrokeCap = SKStrokeCap.Square;
        canvas.DrawPoints(SKPointMode.Points, points.ToArray(), paint);
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
    }
}