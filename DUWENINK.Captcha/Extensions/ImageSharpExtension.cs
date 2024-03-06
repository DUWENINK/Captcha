using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace DUWENINK.Captcha.Extensions
{
    public static class ImageSharpExtension
    {

        public static IImageProcessingContext DrawingCnText(this IImageProcessingContext processingContext, int containerWidth, int containerHeight, string text, Color color, Font font)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Random random = new();
                var textWidth = (containerWidth / text.Length);
                var img2Size = Math.Min(textWidth, containerHeight);
                var fontMiniSize = (int)(img2Size * 0.6);
                var fontMaxSize = (int)(img2Size * 0.95);

                for (int i = 0; i < text.Length; i++)
                {
                    var scaledFont = new Font(font, random.Next(fontMiniSize, fontMaxSize));
                    processingContext.DrawText( text[i].ToString(), scaledFont, color, new PointF(i * textWidth, (containerHeight - img2Size) / 2));
                }
            }

            return processingContext;
        }

        public static IImageProcessingContext DrawingCircles(this IImageProcessingContext processingContext, int containerWidth, int containerHeight, int count, int miniR, int maxR, Color color, bool canOverlap = false)
        {
            Random random = new Random();

            for (var i = 0; i < count; i++)
            {
                var radius = random.Next(miniR, maxR);
                var center = new PointF(random.Next(radius, containerWidth - radius), random.Next(radius, containerHeight - radius));

                // 使用 Draw 方法绘制圆形，直接指定颜色和线条宽度
                processingContext.Draw(color, 1, new EllipsePolygon(center.X, center.Y, radius));
            }

            return processingContext;
        }

        public static IImageProcessingContext DrawingGrid(this IImageProcessingContext processingContext, int containerWidth, int containerHeight, Color color, int count, float thickness)
        {
            var points = new List<PointF> { new PointF(0, 0) };
            Random random = new Random();

            // Assuming GetCirclePoginF generates random points; replace with actual implementation
            for (int i = 0; i < count; i++)
            {
                points.Add(new PointF(random.Next(0, containerWidth), random.Next(0, containerHeight)));
            }
            points.Add(new PointF(containerWidth, containerHeight));

            processingContext.DrawLine(color, thickness, [.. points]);

            return processingContext;
        }

        private static PointF GetNonOverlappingPoint(int width, int height, int radius, List<PointF> existingPoints, Random random)
        {
            PointF newPoint;
            bool overlaps;
            do
            {
                newPoint = new PointF(random.Next(radius, width - radius), random.Next(radius, height - radius));
                overlaps = existingPoints.Any(p => Math.Sqrt(Math.Pow(p.X - newPoint.X, 2) + Math.Pow(p.Y - newPoint.Y, 2)) < 2 * radius);
            }
            while (overlaps);
            existingPoints.Add(newPoint);
            return newPoint;
        }
        /// <summary>
        /// 散 随机点
        /// </summary>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="lapR"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static PointF GetCirclePoginF(int containerWidth, int containerHeight, double lapR, ref List<PointF> list)
        {
            var random = new Random();
            var newPoint = new PointF();
            var retryTimes = 10;

            do
            {
                newPoint.X = random.Next(0, containerWidth);
                newPoint.Y = random.Next(0, containerHeight);
                var tooClose = false;
                foreach (var p in list)
                {
                    tooClose = false;
                    var tempDistance = Math.Sqrt((Math.Pow((p.X - newPoint.X), 2) + Math.Pow((p.Y - newPoint.Y), 2)));
                    if (!(tempDistance < lapR)) continue;
                    tooClose = true;
                    break;
                }

                if (tooClose != false) continue;
                list.Add(newPoint);
                break;
            }
            while (retryTimes-- > 0);

            if (retryTimes <= 0)
            {
                list.Add(newPoint);
            }
            return newPoint;
        }
    }
}
