using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace AvaloniaGameOfLive;

public class SkiaCanvasControl : Control
{
    private WriteableBitmap? _bitmap;

    public event Action<SKCanvas>? PaintSurface;

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

                // Lösche das Canvas mit einem leeren Hintergrund
                canvas.Clear(SKColors.Black);

                // Lasse das PaintSurface-Event die Zeichnung ausführen
                PaintSurface?.Invoke(canvas);

                // Einen Flush aufrufen, um sicherzustellen, dass alles gerendert wird
                canvas.Flush();
            }
        }

        // Zeichne das aktualisierte Bitmap auf das Control
        if (_bitmap != null)
        {
            context.DrawImage(_bitmap, new Rect(0, 0, width, height), new Rect(0, 0, width, height));
        }

    }

    // Eine Methode, um manuell ein Neuzeichnen auszulösen
    public void Redraw()
    {
        this.InvalidateVisual(); // Fordert eine Neuzeichnung an
    }

}