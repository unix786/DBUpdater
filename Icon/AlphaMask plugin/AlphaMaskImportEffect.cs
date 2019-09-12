using System;
using System.Resources;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using PaintDotNet;
using System.Drawing.Imaging;
using System.Reflection;
using PaintDotNet.Effects;

namespace AlphaMaskEffect
{
    /// <summary>
    /// Effect class. Implements a gradient fill draw when the effect is called from the Paint.Net code.
    /// </summary>
    [EffectCategory(EffectCategory.Effect)]
    public class AlphaMaskImportEffect : Effect
    {
        public static Bitmap Icon
        {
            get
            {
                return new Bitmap(typeof(AlphaMaskImportEffect), "AlphaMaskImportIcon.png");
            }
        }
        /// <summary>
        /// Base class override, re-uses curves effect icon, to lazy to redraw one now.
        /// </summary>
        public AlphaMaskImportEffect()
            : base("Alpha Mask", AlphaMaskImportEffect.Icon, EffectFlags.SingleThreaded | EffectFlags.Configurable)
        {
        }

        /// <summary>
        /// Main Render function. Called multiple times to render sections of the overall selected region.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="dstArgs"></param>
        /// <param name="srcArgs"></param>
        /// <param name="rois"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public override void Render(EffectConfigToken parameters, PaintDotNet.RenderArgs dstArgs, PaintDotNet.RenderArgs srcArgs, System.Drawing.Rectangle[] rois, int startIndex, int length)
        {
            AlphaMaskImportEffectConfigToken token = (AlphaMaskImportEffectConfigToken)parameters;

            // Determine Drawing Colors and Selection Rectangle

            Rectangle selection = this.EnvironmentParameters.GetSelection(dstArgs.Surface.Bounds).GetBoundsInt();

            // Setup Facts needed For drawing various gradients that do not change per point x and y.

            int lSelectionLeft = selection.Left;
            int lSelectionTop = selection.Top;

            
            for (int lRectangleIndex = startIndex; lRectangleIndex < startIndex + length; ++lRectangleIndex)
            {
                Rectangle lRectangle = rois[lRectangleIndex];

                // do something for every row in the current rectangle

                for (int y = lRectangle.Top; y < lRectangle.Bottom; y++)
                {

                    for (int x = lRectangle.Left; x < lRectangle.Right; x++)
                    {
                        // Get point's current color
                        ColorBgra point = srcArgs.Surface[x, y];

                        if (token.Image != null)
                        {
                            point = token.Image[x%token.Image.Width, y%token.Image.Height];
                        }

                        // Set the newly derived color if it was changed
                        if (token.Invert == false && token.Image != null)
                        {
                            ColorBgra col = srcArgs.Surface[x, y];
                            float preCol = (float)col.A;
                            int luma = (int)(point.R * 0.3 + point.G * 0.59 + point.B * 0.11);
                            col.A = (byte)(luma);
                            if (token.AlphaMix == true) { col.A = (byte)((preCol / 255) * luma); }
                            dstArgs.Surface[x, y] = col;
                        }
                        if (token.Invert == true && token.Image != null)
                        {
                            ColorBgra col = srcArgs.Surface[x, y];
                            float preCol = (float)col.A;
                            int luma = (int)(point.R * 0.3 + point.G * 0.59 + point.B * 0.11);
                            col.A = (byte)(255 - luma);
                            if (token.AlphaMix == true) { col.A = (byte)((preCol / 255) * (255-luma)); }
                            dstArgs.Surface[x, y] = col;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Constructs a new custom dialog box for gradient effect options.
        /// </summary>
        /// <returns></returns>
        public override EffectConfigDialog CreateConfigDialog()
        {
            return new AlphaMaskImportEffectConfigDialog();
        }
        
    }
}