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

        private string hiddenText;
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

        internal void Disable()
        {
            Enabled = false;
            if (String.IsNullOrEmpty(Text))
            {
                hiddenText = null;
            }
            else
            {
                hiddenText = Text;
                UpdateText(String.Empty);
            }
        }

        internal void Enable()
        {
            Enabled = true;
            if (hiddenText != null)
            {
                UpdateText(hiddenText);
                hiddenText = null;
            }
        }
    }
}
