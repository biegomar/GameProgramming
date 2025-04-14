using System.Collections.Concurrent;
using CellularAutomata;
using CellularAutomata.Cells;
using SkiaSharp;

namespace Visualizer;

public static class SkiaVisualizer
{
    public static void Render(PlayGround playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellBrightness, SKColor> typeToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, typeToColor);
                break;
            case 1:
                RenderPixel(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, typeToColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, typeToColor);
                break;
        }
    }
    
    private static void RenderPixel(PlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellBrightness, SKColor> typeToColor)
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
                var color = typeToColor(playGround.GetCell(new Vector(x, y)).Type, playGround.GetCell(new Vector(x, y)).Brightness);
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
    
    private static void RenderAsRectangles(PlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellBrightness, SKColor> typeToColor)
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

                var cell = playGround.GetCell(new Vector(column, row));
                var color = typeToColor(cell.Type, cell.Brightness);
                if (color == emptyColor) continue;
            
                paint.Color = color;
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
}