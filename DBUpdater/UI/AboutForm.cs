using System.Text;
using System.Windows.Forms;

namespace DBUpdater.UI
{
    public partial class AboutForm : Form
    {
        private const string number = "012943";

        public AboutForm()
        {
            InitializeComponent();

            edText.Text = new StringBuilder()
                .AppendLine(GetInfo<AboutForm>())
                .AppendLine(GetInfo<CECommon.CEVar>())
                .AppendLine()
                .AppendFormat(edText.Text, number)
                .ToString();

            edText.MouseDoubleClick += EdText_MouseDoubleClick;
        }

        private void EdText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (edText.SelectedText.StartsWith(number) && edText.SelectedText.Length == number.Length + 1)
            {
                var lastChar = edText.SelectedText[edText.SelectedText.Length - 1];
                if (lastChar == '.' || lastChar == ' ') edText.SelectionLength -= 1;
            }
        }

        private static string GetInfo<T>()
        {
            var assemblyName = typeof(T).Assembly.GetName();
            return $"{assemblyName.Name} v{assemblyName.Version}";
        }

        private void AboutForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void EdText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
