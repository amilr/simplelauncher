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
        private List<KeyValuePair<string, CommandItem>> Commands = new List<KeyValuePair<string, CommandItem>>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var xdoc = XDocument.Load("config.xml");

            Text = xdoc.Document.Root.Attribute("Title").Value;

            foreach (var item in xdoc.Document.Root.Elements("Item"))
            {
                var label = item.Element("Label").Value;
                var command = item.Element("Command").Value;
                var arguments = item.Element("Arguments") == null  ? null : item.Element("Arguments").Value;

                var ci = new CommandItem { Command = command, Arguments = arguments };

                Commands.Add(new KeyValuePair<string, CommandItem>(label, ci));
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
                RunCommand(index);
            }
        }

        private void lbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RunCommand(lbItems.SelectedIndex);
            }
        }

        private void RunCommand(int index)
        {
            var command = Commands[lbItems.SelectedIndex].Value;

            try
            {
                if (command.Arguments == null)
                {
                    Process.Start(command.Command);
                }
                else
                {
                    Process.Start(command.Command, command.Arguments);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
    }
}
