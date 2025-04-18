using System.Collections.Concurrent;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;
using SkiaSharp;

namespace Visualizer;

public static class SkiaVisualizer
{
    public static void Render(PlayGround playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellColor, SKColor> typeToColor)
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
    
    public static void RenderSimplePlayGround(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, SKColor emptyColor, int maxDegreeOfParallelism, Func<bool, SKColor> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderSimpleRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            case 1:
                RenderSimplePixel(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
            default:
                RenderSimpleRectangles(playGround, cellSize, canvas, emptyColor, maxDegreeOfParallelism, stateToColor);
                break;
        }
    }
    
    private static void RenderPixel(PlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellColor, SKColor> typeToColor)
    {
        var colorBuckets = new ConcurrentDictionary<SKColor, ConcurrentBag<SKPoint>>();
        var dimensionX = playGround.Dimension.X;
        var dimensionY = playGround.Dimension.Y;
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        var halfCellHeight = cellHeight / 2;
        var halfCellWidth = cellWidth / 2;

    
        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        
        Parallel.For(0, dimensionX, parallelOptions,x =>
        {
            for (var y = 0; y < dimensionY; y++)
            {
                var color = typeToColor(playGround.GetCell(new Vector(x, y)).Type, playGround.GetCell(new Vector(x, y)).Color);
                if (color == emptyColor)
                    continue;
    
                var point = new SKPoint(x * cellHeight + halfCellHeight, y * cellWidth + halfCellWidth);
                colorBuckets.GetOrAdd(color, _ => new ConcurrentBag<SKPoint>()).Add(point);
            }
        });
    
        
        using var paint = new SKPaint();
        paint.IsAntialias = false;
        paint.Style = SKPaintStyle.Fill;
        paint.StrokeCap = SKStrokeCap.Square;
        paint.StrokeWidth = cellWidth;
    
        foreach (var (color, pointList) in colorBuckets)
        {
            paint.Color = color;
            canvas.DrawPoints(SKPointMode.Points, pointList.ToArray(), paint);
        }
    }
    
    private static void RenderSimplePixel(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<bool, SKColor> stateToColor)
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
                var color = stateToColor(playGround.GetState(new Vector(x, y)));
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
    
    private static void RenderAsRectangles(PlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<CellType, CellColor, SKColor> typeToColor)
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
                var color = typeToColor(cell.Type, cell.Color);
                if (color == emptyColor) continue;
            
                paint.Color = color;
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
    
    private static void RenderSimpleRectangles(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, SKColor emptyColor, int maxDegreeOfParallelism, Func<bool, SKColor> stateToColor)
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
                
                var color = stateToColor(playGround.GetState(new Vector(column, row)));
                if (color == emptyColor) continue;
            
                paint.Color = color;
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
}