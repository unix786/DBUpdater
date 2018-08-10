using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ContourAutoUpdate.UI
{
    [DesignerCategory("Code")]
    internal class TextBoxExt : TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (TextChangedAction != null) TextChangedAction(Text);
        }

        public Action<string> TextChangedAction { get; set; }

        public void UpdateText(string text)
        {
            Action<string> textChangedAction = TextChangedAction;
            try
            {
                TextChangedAction = null;
                Text = text;
            }
            finally
            {
                TextChangedAction = textChangedAction;
            }
        }
    }
}
