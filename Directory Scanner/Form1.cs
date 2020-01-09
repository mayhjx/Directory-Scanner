using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Directory_Scanner
{
    public partial class Scanner : Form
    {
        public Scanner()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPath.Text = "";
                string path = folderBrowserDialog1.SelectedPath;
                txtPath.Text = path;
                ListDirectory(treeView1, path);
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name));
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                // pass
            }
            return directoryNode;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // TODO txtPath.Text为根目录（如C:\）的话，Path.GetDirectoryName（）返回的值为null，需增加判断条件。
            string filestr = Path.Combine(Path.GetDirectoryName(txtPath.Text), e.Node.FullPath);
            System.Diagnostics.Process.Start(filestr);
        }
    }
}
