using System.Collections.Concurrent;
using CellularAutomata;
using SkiaSharp;

namespace Visualizer;

public static class SkiaVisualizer<T>
{
    
    public static void Render(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
        }
    }
    
    public static void Render(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            case 1:
                RenderPixel(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
        }
    }
    
    private static void RenderPixel(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
    {
        var colorBuckets = new ConcurrentDictionary<SKColor, ConcurrentBag<SKPoint>>();
        var dimensionX = playGround.Dimension.X;
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;

        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        
        Parallel.For(0, dimensionX, parallelOptions, x =>
        {
            for (var y = 0; y < playGround.Dimension.Y; y++)
            {
                var color = stateToColor(playGround[(x, y)]);
                if (color == emptyColor)
                    continue;

                var point = new SKPoint(x * cellHeight, y * cellWidth);
                colorBuckets.GetOrAdd(color, _ => new ConcurrentBag<SKPoint>()).Add(point);
            }
        });

        
        using var paint = new SKPaint();
        paint.IsAntialias = false;
        paint.Style = SKPaintStyle.Fill;
        paint.StrokeCap = SKStrokeCap.Square;
        paint.StrokeWidth = cellSize.X;

        foreach (var (color, pointList) in colorBuckets)
        {
            paint.Color = color;
            canvas.DrawPoints(SKPointMode.Points, pointList.ToArray(), paint);
        }
    }
    
    private static void RenderPixel(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
    {
        var colorBuckets = new ConcurrentDictionary<SKColor, ConcurrentBag<SKPoint>>();
        var dimensionX = playGround.Dimension.X;
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
    
        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        
        Parallel.For(0, dimensionX, parallelOptions,x =>
        {
            for (var y = 0; y < playGround.Dimension.Y; y++)
            {
                var color = stateToColor(playGround[(x, y)]);
                if (color == emptyColor)
                    continue;
    
                var point = new SKPoint(x * cellHeight, y * cellWidth);
                colorBuckets.GetOrAdd(color, _ => new ConcurrentBag<SKPoint>()).Add(point);
            }
        });
    
        
        using var paint = new SKPaint();
        paint.IsAntialias = false;
        paint.Style = SKPaintStyle.Fill;
        paint.StrokeCap = SKStrokeCap.Square;
        paint.StrokeWidth = cellSize.X;
    
        foreach (var (color, pointList) in colorBuckets)
        {
            paint.Color = color;
            canvas.DrawPoints(SKPointMode.Points, pointList.ToArray(), paint);
        }

        
        // Parallel.ForEach(colorBuckets, parallelOptions, bucket =>
        // {
        //     var (color, pointList) = bucket;
        //
        //     // Lokaler Paint für jeden Thread
        //     var localPaint = new SKPaint
        //     {
        //         IsAntialias = false,
        //         Style = SKPaintStyle.Fill,
        //         StrokeCap = SKStrokeCap.Square,
        //         Color = color
        //     };
        //
        //     canvas.DrawPoints(SKPointMode.Points, pointList.ToArray(), localPaint);
        // });


    }
    
    private static void RenderAsRectangles(PlayGround<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
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
    
    private static void RenderAsRectangles(PlayGroundArray<T> playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<T, SKColor> stateToColor)
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