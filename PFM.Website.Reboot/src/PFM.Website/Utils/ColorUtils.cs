using System;
using ChartJs.Blazor.Util;
using System.Drawing;

namespace PFM.Website.Utils
{
    public static class ColorUtils
    {
        public static readonly Color Green = Color.FromArgb(60, 179, 113);
        public static readonly Color Blue = Color.FromArgb(72, 209, 204);
        public static readonly Color Golden = Color.FromArgb(218, 165, 32);

        public static string ConvertColor(string graphColor, int? transparency = null)
        {
            if (!transparency.HasValue && graphColor.StartsWith("#"))
            {
                return graphColor;
            }

            if (!transparency.HasValue)
            {
                return $"#{graphColor}";
            }

            if (transparency.HasValue && graphColor.StartsWith("#") && graphColor.Length == 9)
            {
                return graphColor;
            }

            if (!graphColor.StartsWith("#"))
            {
                graphColor = "#" + graphColor;
            }

            try
            {
                return ColorUtil.FromDrawingColor(Color.FromArgb(transparency.Value, ColorTranslator.FromHtml(graphColor)));
            }
            catch(Exception ex)
            {
                return ColorUtil.FromDrawingColor(Color.Black);
            }
        }
    }
}

