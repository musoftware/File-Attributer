using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace File_Attributer
{
    public partial class MainForm : DevComponents.DotNetBar.Office2007Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    open.Title = Application.ProductName;
                    open.Filter = "All Files|*.*";
                    if (open.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        int index = checkedListBox1.Items.Add(open.FileName);
                        checkedListBox1.SetItemChecked(index, true);
                        textBoxX1.Text = open.FileName;
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog open = new FolderBrowserDialog())
                {
                    open.SelectedPath = Path.GetDirectoryName(Application.ExecutablePath);
                    if (open.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        progressBarX2.Visible = true;
                        buttonX2.Enabled = false;
                        textBoxX2.Text = open.SelectedPath;
                        folderBrowserScan.RunWorkerAsync(open.SelectedPath);
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarX2.Visible = false;
            buttonX2.Enabled = true;
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBoxX1.Checked);
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonX3.Text == "Stop")
                {
                    if (FileChangingProcess.IsBusy && !FileChangingProcess.CancellationPending)
                    {
                        FileChangingProcess.CancelAsync();
                    }
                    return;
                }
                buttonX3.Text = "Stop";
                FilesClasses files = new FilesClasses();
                foreach (var item in checkedListBox1.Items)
                {
                    try
                    {
                        string file = item.ToString();
                        if (File.Exists(file))
                        {
                            files.files.Add(file);
                        }
                        if (checkBoxItem1.Checked)
                        {
                            files.AttributeSelected = FileAttributes.Normal;
                        }
                        else
                        {
                            files.AttributeSelected = FileAttributes.System | FileAttributes.Hidden;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                FileChangingProcess.RunWorkerAsync(files);
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void folderBrowserScan_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = e.Argument.ToString();
            if (Directory.Exists(filename))
            {
                var list = File_Attributer.FoldersWorking.RecursiveScan2(filename);
                if (checkedListBox1.IsHandleCreated)
                {
                    checkedListBox1.BeginInvoke((MethodInvoker)delegate
                    {
                        foreach (var item in list)
                        {
                            int index = checkedListBox1.Items.Add(item.Path);
                            checkedListBox1.SetItemChecked(index, true);
                        }
                    });
                }
            }
        }

        private void FileChangingProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = new FullResults();
            try
            {
                FilesClasses files = e.Argument as FilesClasses;

                if (files == null) throw new ThereIsNoFilesExpection();

                foreach (var file in files.files)
                {
                    try
                    {
                        string folder = Path.GetDirectoryName(file);
                        File.SetAttributes(file, files.AttributeSelected);
                        new DirectoryInfo(folder).Attributes = files.AttributeSelected;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ThereIsNoFilesExpection)
                {
                    (e.Result as FullResults).Errors.Add("There Is No Files");
                }
            }
            finally
            {

            }
        }

        private void groupPanel2_Click(object sender, EventArgs e)
        {

        }

        private void FileChangingProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonX3.Text = "Start";
        }

        private void Attributes_Click(object sender, EventArgs e)
        {

        }

        private void FileChangingProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarX1.Value = e.ProgressPercentage;
        }
    }
}
