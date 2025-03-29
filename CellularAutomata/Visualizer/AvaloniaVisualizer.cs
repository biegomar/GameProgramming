using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using CellularAutomata;
using Color = Avalonia.Media.Color;

namespace Visualizer;

using Vector = CellularAutomata.Vector;

public static class AvaloniaVisualizer
{
    public static void Render(PlayGround playGround, Vector cellSize, Canvas canvas, int renderEngineIndex, Color emptyColor, Func<CellState, Color> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
            // case 1:
            //     RenderPixel(playGround, canvas, emptyColor, stateToColor);
            //     break;
            // case 2:
            //     RenderAsBitmap(playGround, canvas, emptyColor, stateToColor);
            //     break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
        }
    }
    
    public static void Render(PlayGroundArray playGround, Vector cellSize, Canvas canvas, int renderEngineIndex, Color emptyColor, Func<CellState, Color> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
            // case 1:
            //     RenderPixel(playGround, canvas, emptyColor, stateToColor);
            //     break;
            // case 2:
            //     RenderAsBitmap(playGround, canvas, emptyColor, stateToColor);
            //     break;
            default:
                RenderAsRectangles(playGround, cellSize, canvas, emptyColor, stateToColor);
                break;
        }
    }
    
    private static void RenderAsRectangles(PlayGround playGround, Vector cellSize, Canvas canvas, Color emptyColor, Func<CellState, Color> stateToColor)
    {
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;

        for (var column = 0; column < playGround.Dimension.X; column++)
        {
            for (var row = 0; row < playGround.Dimension.Y; row++)
            {
                var top = row * cellHeight;
                var left = column * cellWidth;
            
                var color = stateToColor(playGround[new Vector(column, row)]);
                if (color == emptyColor) continue;
            
                var brush = new SolidColorBrush(color);
            
                var rect = new Rectangle
                {
                    Width = cellWidth,
                    Height = cellHeight,
                    Fill = brush
                };

                Canvas.SetLeft(rect, left); 
                Canvas.SetTop(rect, top); 

                canvas.Children.Add(rect);
            }
        }
    }
    
    private static void RenderAsRectangles(PlayGroundArray playGround, Vector cellSize, Canvas canvas, Color emptyColor, Func<CellState, Color> stateToColor)
    {
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;

        for (var column = 0; column < playGround.Dimension.X; column++)
        {
            for (var row = 0; row < playGround.Dimension.Y; row++)
            {
                var top = row * cellHeight;
                var left = column * cellWidth;
            
                var color = stateToColor(playGround[new Vector(column, row)]);
                if (color == emptyColor) continue;
            
                var brush = new SolidColorBrush(color);

                var rect = new Rectangle
                {
                    Width = cellWidth,
                    Height = cellHeight,
                    Fill = brush
                };

                Canvas.SetLeft(rect, left); 
                Canvas.SetTop(rect, top); 

                canvas.Children.Add(rect);
            }
        }
    }

    
}