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
                if (txtKeyword.Text != "")
                {
                    toolStripStatusLabel1.Text = string.Format("包含关键字（{0}）的子文件夹：{1}个，子文件：{2}个", txtKeyword.Text, dirNum, fileNum);
                }
                else
                {
                    toolStripStatusLabel1.Text = string.Format("包含子文件夹：{0}个，子文件：{1}个", dirNum, fileNum);
                }
                btnOpen.Enabled = true;
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            var rootDirectoryInfo = new DirectoryInfo(path);
            string keyword = txtKeyword.Text;
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo, keyword));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, string keyword)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    //if (directory.Name.Contains(keyword))
                    //{
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory, keyword));
                    dirNum++;
                    //}
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    if (file.Name.Contains(keyword))
                    {
                        directoryNode.Nodes.Add(new TreeNode(file.Name));
                        fileNum++;
                    }         
                }
            }
            catch (Exception ex)
            {
                // pass
                MessageBox.Show(ex.Message);
            }
            return directoryNode;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode != null)
                {
                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
                    treeView1.SelectedNode = CurrentNode;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string path = "";
            path = Path.Combine(Path.GetDirectoryName(this.txtPath.Text), this.treeView1.SelectedNode.FullPath);
            System.Diagnostics.Process.Start(path);
        }
    }
}
