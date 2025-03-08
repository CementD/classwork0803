using System.ComponentModel;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private BackgroundWorker worker;
        private int progressiveValue = 0;
        private bool pause = false;

        public Form1()
        {
            InitializeComponent();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                progressiveValue = 0;
                pause = false;
                statusStrip1.Visible = true;
                worker.RunWorkerAsync();
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            for (int i = progressiveValue; i < toolStripProgressBar1.Maximum; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    progressiveValue = i;
                    return;
                }
                if (pause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                worker.ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                MessageBox.Show("Progress finished");
                statusStrip1.Visible = false;
            }
        }

        private void stopToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            pause = true;
        }

        private void continueToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pause)
            {
                pause = false;
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }
            }
        }
    }
}
