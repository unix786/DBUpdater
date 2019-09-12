using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using PaintDotNet.Effects;
using PaintDotNet;

namespace AlphaMaskEffect
{
    /// <summary>
    /// Structure used to hold options selected for the photo flood fill effect.
    /// </summary>
    public class AlphaMaskImportEffectConfigToken : EffectConfigToken
    {
        #region Fields
        /// <summary>
        /// File name of photo to use in fill process
        /// </summary>
        private String _PhotoFileName = String.Empty;
        /// <summary>
        /// Image to Flood Fill
        /// </summary>
        private Surface _Image = null;
        private bool _Invert = false;
        private bool _AlphaMix = false;
        private bool _Clipboard = true;
        #endregion

        #region Properties
        /// <summary>
        /// The file name of the photo to use in fill process
        /// </summary>
        public String PhotoFileName
        {
          get { return _PhotoFileName; }
          set 
          { 
              _PhotoFileName = value;
              _Image = null;
          }
        }



        /*private void kill(AlphaMaskImportEffectConfigToken AlphaMaskImportEffectConfigToken)
        {
            throw new Exception("The method or operation is not implemented.");
        }*/
        //[STAThread]
        private Surface GetImageFromClipboard()
        {
            Bitmap img = null;
            IDataObject clippy;
            try
            {
                clippy = Clipboard.GetDataObject();
                //MessageBox.Show(clippy.GetFormats()[0]);
                if (clippy != null)
                {
                    img = (Bitmap)clippy.GetData(typeof(System.Drawing.Bitmap));
                }
            }
            catch (Exception/* ex*/)
            {
                //MessageBox.Show("Image retrieval failure: "+ex.Message);
            }
            if (img != null)
            {
                //MessageBox.Show("Image retrieval success.");
                return Surface.CopyFromBitmap(img);
            }
            else
            {
                return null;
            }
        }
        public bool Invert
        {
            get { return _Invert; }
            set { _Invert = value; }
        }
        public bool AlphaMix
        {
            get { return _AlphaMix; }
            set { _AlphaMix = value; }
        }
        public bool inClipboard
        {
            get { return _Clipboard; }
            set { _Clipboard = value;
            if (value == true)
                _Image = GetImageFromClipboard();
            }
        }
        #endregion

        /*public Bitmap Fallback
        {
            get { return new Bitmap(typeof(System.Drawing.Bitmap), "AlphaMaskWhitePixel.png"); }
        }*/

        public Surface Image
        {
            get
            {
                if (_Image != null)
                {
                    return _Image;
                }
                else
                {
                    _Image = GetImage(PhotoFileName);
                    return _Image;
                }
            }
        }

        protected Surface GetImage(string aFileName)
        {
            Surface idest = new Surface(1,1);
            idest[0,0] = ColorBgra.Transparent;
            Bitmap img;
            if (String.IsNullOrEmpty(aFileName))
            {
                
                return idest;
            }
            else
            {
                try
                {
                    img = (Bitmap)Bitmap.FromFile(aFileName, true);
                }
                catch (OutOfMemoryException)
                {
                    return null;
                }
                catch (System.IO.FileNotFoundException)
                {
                    return null;
                }
                if (img != null)
                {
                    return Surface.CopyFromBitmap(img);
                }
                return idest;
            }
        }

        #region Constructors and Clone Override
        /// <summary>
        /// Default Constuctor
        /// </summary>
        public AlphaMaskImportEffectConfigToken()
            : base()
        {
        }

        /// <summary>
        ///  Copy Constructor
        /// </summary>
        /// <param name="aOther"></param>
        protected AlphaMaskImportEffectConfigToken(AlphaMaskImportEffectConfigToken aOther)
            : base(aOther)
        {
            this.PhotoFileName = aOther.PhotoFileName;
            this.Invert = aOther.Invert;
            this.AlphaMix = aOther.AlphaMix;
            this.inClipboard = aOther.inClipboard;
            this._Image = aOther.Image;
        }

        /// <summary>
        /// Custom Clone (Copy Override)
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new AlphaMaskImportEffectConfigToken(this);
        }
        #endregion
    }
}
