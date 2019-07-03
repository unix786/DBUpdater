using System;
using System.Drawing;
using System.Windows.Forms;

namespace ContourAutoUpdate
{
    internal static class App
    {
        static App()
        {
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (icon != null) icon.Dispose();
            if (iconStream != null) iconStream.Dispose();
        }

        private static bool needLoadIcon = true;
        private static Icon icon;
        private static IDisposable iconStream;
        public static Icon Icon
        {
            get
            {
                if (needLoadIcon)
                {
                    needLoadIcon = false;
                    var t = typeof(App);
                    try
                    {
                        // Может произвести исключение при запуске через сеть.
                        icon = Icon.ExtractAssociatedIcon(t.Assembly.Location);
                    }
                    catch
                    {
                        try
                        {
                            var resourceName = t.FullName + ".ico";
                            var stream = t.Assembly.GetManifestResourceStream(resourceName);
                            iconStream = stream;
                            icon = new Icon(stream);
                        }
                        catch
                        {
                            icon = null;
                        }
                    }
                }
                return icon;
            }
        }
    }
}
