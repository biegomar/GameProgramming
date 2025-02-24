using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;

namespace AvaloniaGameOfLive;

using Avalonia;
using Avalonia.Media;

public class AvaloniaVisualizer<T>
{
    public static void Render(PlayGround<T> playGround, Vector cellSize, DrawingContext context, int renderEngineIndex, Color emptyColor,
        Func<T, Color> stateToColor)
    {
        switch (renderEngineIndex)
        {
            case 0:
                RenderAsRectangles(playGround, cellSize, context, emptyColor, stateToColor);
                break;
            // case 1:
            //     RenderPixel(playGround, canvas, emptyColor, stateToColor);
            //     break;
            // case 2:
            //     RenderAsBitmap(playGround, canvas, emptyColor, stateToColor);
            //     break;
            default:
                RenderAsRectangles(playGround, cellSize, context, emptyColor, stateToColor);
                break;
        }
    }
    
    public static void Render(PlayGroundArray<T> playGround, Vector cellSize, DrawingContext context, int renderEngineIndex, Color emptyColor,
        Func<T, Color> stateToColor)
    {
        
    }
    
    private static void RenderAsRectangles(PlayGround<T> playGround, Vector cellSize, DrawingContext context, Color emptyColor, Func<T, Color> stateToColor)
    {
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        
        foreach (var cell in playGround.Cells.Values)
        {
            var top = cell.Y * cellHeight;
            var left = cell.X * cellWidth;
            
            var color = stateToColor(cell.State);
            if (color == emptyColor) continue;
            
            var brush = new SolidColorBrush(color);

            var rect = new Rect(left, top, cellWidth, cellHeight);
            context.FillRectangle(brush, rect);
        }
    }
    
    private static void RenderAsRectangles(PlayGroundArray<T> playGround, Vector cellSize, DrawingContext context, Color emptyColor, Func<T, Color> stateToColor)
    {
        var cellWidth = cellSize.X;
        var cellHeight = cellSize.Y;
        
        foreach (var cell in playGround.Cells)
        {
            var top = cell.Y * cellHeight;
            var left = cell.X * cellWidth;
            
            var color = stateToColor(cell.State);
            if (color == emptyColor) continue;
            
            var brush = new SolidColorBrush(color);

            var rect = new Rect(left, top, cellWidth, cellHeight);
            context.FillRectangle(brush, rect);
        }
    }

    
}