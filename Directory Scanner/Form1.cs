using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace Directory_Scanner
{
    public partial class Scanner : Form
    {
        public Scanner()
        {
            InitializeComponent();
        }

        public static int dirNum;
        public static int fileNum;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                dirNum = 0;
                fileNum = 0;

                // Initial
                btnOpen.Enabled = false;
                txtPath.Text = path;
                treeView1.Nodes.Clear();
                toolStripStatusLabel1.Text = "扫描中，请稍候...";
                Application.DoEvents();

                ListDirectory(treeView1, path);
                toolStripStatusLabel1.Text = string.Format("包含子文件夹：{0}，文件：{1}", dirNum, fileNum);
                btnOpen.Enabled = true;
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
                    dirNum++;
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name));
                    fileNum++;
                }
            }
            catch
            {
                // pass
            }
            return directoryNode;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // TODO txtPath.Text为根目录（如C:\\）或共享文件夹一级目录(一级目录例：\\10.10.18.16\MNs...与节点父目录(\MNs...\...)会重复）的话，Path.GetDirectoryName（）返回的值为null。
            string filestr;
            string path = Path.GetDirectoryName(txtPath.Text);
            if (path is null)
            {
                filestr = Path.Combine(txtPath.Text, e.Node.FullPath);
            }
            // TODO
            else
            {
                filestr = Path.Combine(path, e.Node.FullPath);
            }

            try
            {
                System.Diagnostics.Process.Start(filestr);
            }
            catch { }
        }
    }
}
