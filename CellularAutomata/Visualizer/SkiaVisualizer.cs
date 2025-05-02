using System.Collections.Concurrent;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;
using SkiaSharp;

namespace Visualizer;

public static class SkiaVisualizer
{
    private static CellColor[] blueShades =
    [
        CellColor.CoolBlue, 
        CellColor.OceanBlue, 
        CellColor.DeepSky, 
        CellColor.CrystalLake, 
        CellColor.BlueCurrent, 
        CellColor.SplashBlue, 
        CellColor.AzureDrift,
        CellColor.Wavestone, 
        CellColor.RippleBlue
    ];
    
    public static void Render(PlayGround playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, int maxDegreeOfParallelism, Func<CellColor, SKColor>? typeToColor = null)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, maxDegreeOfParallelism, typeToColor ?? ChooseMaterialColor);
                break;
            case 1:
                RenderPixel(playGround, cellSize, canvas, maxDegreeOfParallelism, typeToColor ?? ChooseMaterialColor);
                break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, maxDegreeOfParallelism, typeToColor ?? ChooseMaterialColor);
                break;
        }
    }
    
    public static void RenderSimplePlayGround(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, int renderEngineIndex, int maxDegreeOfParallelism, SKColor stateColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderSimpleRectangles(playGround, cellSize, canvas, maxDegreeOfParallelism, stateColor);
                break;
            case 1:
                RenderSimplePixel(playGround, cellSize, canvas, maxDegreeOfParallelism, stateColor);
                break;
            default:
                RenderSimpleRectangles(playGround, cellSize, canvas, maxDegreeOfParallelism, stateColor);
                break;
        }
    }
    
    private static void RenderPixel(PlayGround playGround, Vector cellSize, SKCanvas canvas, int maxDegreeOfParallelism, Func<CellColor, SKColor> typeToColor)
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
                var actualCell = playGround.GetCell(new Vector(x, y));
                
                var actualCellColor = actualCell.Color;
                if (actualCellColor == CellColor.Empty)
                    continue;
             
                var newCellColor = SparklingBlue(actualCellColor);
                if (newCellColor != actualCellColor)
                {
                    playGround.SetCell(new Vector(x, y), actualCell with { Color = newCellColor });
                }
                
                var point = new SKPoint(x * cellHeight + halfCellHeight, y * cellWidth + halfCellWidth);
                colorBuckets.GetOrAdd(typeToColor(actualCellColor), _ => new ConcurrentBag<SKPoint>()).Add(point);
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
    
    private static void RenderSimplePixel(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, int maxDegreeOfParallelism, SKColor stateColor)
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
                var actualState = playGround.GetState(new Vector(x, y));
                if (!actualState)
                    continue;
                
                var point = new SKPoint(x * cellHeight, y * cellWidth);
                colorBuckets.GetOrAdd(stateColor, _ => new ConcurrentBag<SKPoint>()).Add(point);
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
    
    private static void RenderAsRectangles(PlayGround playGround, Vector cellSize, SKCanvas canvas, int maxDegreeOfParallelism, Func<CellColor, SKColor> typeToColor)
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

                var actualCell = playGround.GetCell(new Vector(column, row));
                
                var actualCellColor = actualCell.Color;
                if (actualCellColor == CellColor.Empty)
                    continue;
             
                var newCellColor = SparklingBlue(actualCellColor);
                if (newCellColor != actualCellColor)
                {
                    playGround.SetCell(new Vector(column, row), actualCell with { Color = newCellColor });
                }
                
                paint.Color = typeToColor(newCellColor);
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
    
    private static void RenderSimpleRectangles(SimplePlayGround playGround, Vector cellSize, SKCanvas canvas, int maxDegreeOfParallelism, SKColor stateColor)
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

                var actualState = playGround.GetState(new Vector(column, row));
                if (!actualState)
                    continue;
            
                paint.Color = stateColor;
                var rect = new SKRect(left, top, right, bottom);
        
                canvas.DrawRect(rect, paint);
            }
        }
    }
    
    private static CellColor SparklingBlue(CellColor color)
    {
        if (blueShades.Contains(color))
        {
            int currentIndex = Array.IndexOf(blueShades, color);

            return blueShades[(currentIndex + 1) % blueShades.Length];
        }
        
        return color;
    }
    
    private static SKColor ChooseMaterialColor(CellColor color)
    {
        return color switch
        {
            CellColor.Solid => SKColors.Gray,
            CellColor.GoldenSand => new SKColor(210, 168, 105),
            CellColor.DesertGold => new SKColor(214, 171, 107),
            CellColor.Wheatfield => new SKColor(206, 165, 103),
            CellColor.SaharaDune => new SKColor(212, 170, 106),
            CellColor.HoneyBeige => new SKColor(208, 166, 104),
            CellColor.ToastedAlmond => new SKColor(207, 166, 104),
            CellColor.AmberGrain => new SKColor(216, 173, 108),
            CellColor.ClayOchre => new SKColor(209, 167, 104),
            CellColor.GoldenWheat => new SKColor(213, 170, 106),
            CellColor.SunlitSandstone => new SKColor(211, 169, 106),
            CellColor.CoolBlue => new SKColor(81, 130, 203, 255),
            CellColor.OceanBlue => new SKColor(87, 139, 217, 255),
            CellColor.DeepSky => new SKColor(80, 128, 200, 255),
            CellColor.CrystalLake => new SKColor(83, 133, 208, 255),
            CellColor.SurfBlue => new SKColor(84, 134, 209, 255),
            CellColor.BlueCurrent => new SKColor(82, 132, 206, 255),
            CellColor.SplashBlue => new SKColor(89, 142, 222, 255),
            CellColor.AzureDrift => new SKColor(88, 140, 219, 255),
            CellColor.Wavestone => new SKColor(85, 136, 212, 255),          
            CellColor.RippleBlue => new SKColor(81, 129, 202, 255),
            CellColor.SoftPlum     => new SKColor(132, 119, 132, 255),
            CellColor.SmokyLavender=> new SKColor(139, 125, 137, 255),
            CellColor.DustyMauve   => new SKColor(128, 116, 129, 255),
            CellColor.PaleOrchid   => new SKColor(133, 120, 133, 255),
            CellColor.MistyViolet  => new SKColor(130, 117, 130, 255),
            CellColor.AshRose      => new SKColor(140, 126, 138, 255),
            CellColor.TwilightGrey => new SKColor(127, 115, 128, 255),
            CellColor.MoonShadow   => new SKColor(126, 115, 128, 255),
            CellColor.FadedLilac   => new SKColor(131, 118, 131, 255),
            CellColor.DeepHeather  => new SKColor(142, 128, 139, 255),
            CellColor.IceBlue         => new SKColor(142, 180, 237, 255),
            CellColor.FrostBlue       => new SKColor(143, 181, 238, 255),
            CellColor.CrystalBlue     => new SKColor(140, 177, 233, 255),
            CellColor.PaleGlacier     => new SKColor(141, 179, 235, 255),
            CellColor.WinterSky       => new SKColor(145, 183, 241, 255),
            CellColor.SnowReflection  => new SKColor(145, 184, 242, 255),
            CellColor.DeepFreeze      => new SKColor(139, 176, 232, 255),
            CellColor.ArcticMist      => new SKColor(138, 175, 230, 255),
            CellColor.IcyHorizon      => new SKColor(140, 178, 234, 255),
            CellColor.GentleBlizzard  => new SKColor(141, 178, 234, 255),
            CellColor.SnowWhite     => new SKColor(255, 255, 255, 255),
            CellColor.FrostTint     => new SKColor(234, 246, 255, 255),
            CellColor.IceGrey       => new SKColor(221, 230, 232, 255),
            CellColor.PaleSnow      => new SKColor(247, 251, 253, 255),
            CellColor.BlueShadow    => new SKColor(200, 221, 235, 255),
            CellColor.GlacialGlow   => new SKColor(240, 248, 255, 255),
            CellColor.MorningFrost  => new SKColor(225, 236, 242, 255),
            CellColor.PowderWhite   => new SKColor(252, 252, 252, 255),
            CellColor.SilverMist    => new SKColor(211, 211, 211, 255),
            CellColor.ArcticLight   => new SKColor(245, 253, 255, 255),

        };
    }
}