using System.Collections.Concurrent;
using CellularAutomata;
using SkiaSharp;

namespace Visualizer;

public static class SkiaVisualizer
{
    public static void Render(PlayGroundArray playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellState, SKColor> stateToColor)
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
    
    private static void RenderPixel(PlayGroundArray playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellState, SKColor> stateToColor)
    {
        var colorBuckets = new ConcurrentDictionary<SKColor, ConcurrentBag<SKPoint>>();
        var dimensionX = playGround.Dimension.X;
        var dimensionY = playGround.Dimension.Y;
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
    
        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        
        Parallel.For(0, dimensionX, parallelOptions,x =>
        {
            for (var y = 0; y < dimensionY; y++)
            {
                var color = stateToColor(playGround[new Vector(x, y)]);
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
    
    private static void RenderAsRectangles(PlayGroundArray playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellState, SKColor> stateToColor)
    {
        using var paint = new SKPaint();
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        var dimensionX = playGround.Dimension.X;
        var dimensionY = playGround.Dimension.Y;

        for (var column = 0; column < dimensionX; column++)
        {
            for (var row = 0; row < dimensionY; row++)
            {
                var top = row * cellHeight;
                var bottom = top + cellHeight;
                var left = column * cellWidth;
                var right = left + cellWidth;
            
                var color = stateToColor(playGround[new Vector(column, row)]);
                if (color == emptyColor) continue;
            
                paint.Color = color;
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
}