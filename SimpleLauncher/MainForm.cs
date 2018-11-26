using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace SimpleLauncher
{
    public partial class MainForm : Form
    {
        private List<KeyValuePair<string, string>> Commands = new List<KeyValuePair<string, string>>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var xdoc = XDocument.Load("config.xml");

            foreach (var item in xdoc.Document.Root.Elements("Item"))
            {
                var label = item.Element("Label").Value;
                var command = item.Element("Command").Value;

                Commands.Add(new KeyValuePair<string, string>(label, command));
            }

            foreach (var command in Commands)
            {
                lbItems.Items.Add(command.Key);
            }
        }

        private void lbItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbItems.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var command = Commands[index].Value;

                Process.Start(command);
            }
        }

        private void lbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var command = Commands[lbItems.SelectedIndex].Value;

                Process.Start(command);
            }
        }
    }
}
