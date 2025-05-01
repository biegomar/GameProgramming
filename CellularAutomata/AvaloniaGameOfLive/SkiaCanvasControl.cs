using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace AvaloniaGameOfLive;

public class SkiaCanvasControl : Control
{
    private static readonly SKColor EmptyColor = new SKColor(243, 229, 229, 255);
    private WriteableBitmap? _bitmap;

    public event Action<SKCanvas>? PaintSurface;

    public SKColor? ClearColor { get; set; }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (Bounds.Width <= 0 || Bounds.Height <= 0)
            return;

        var width = (int)Bounds.Width;
        var height = (int)Bounds.Height;
        
        _bitmap ??= new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), Avalonia.Platform.PixelFormat.Bgra8888);

        using (var framebuffer = _bitmap.Lock())
        {
            using (var surface = SKSurface.Create(
                       new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul),
                       framebuffer.Address,
                       framebuffer.RowBytes))
            {
                var canvas = surface.Canvas;
                
                canvas.Clear(ClearColor ?? SKColors.Black);

                PaintSurface?.Invoke(canvas);

                canvas.Flush();
            }
        }
        
        if (_bitmap != null)
        {
            context.DrawImage(_bitmap, new Rect(0, 0, width, height), new Rect(0, 0, width, height));
        }

    }
}