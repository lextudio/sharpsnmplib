//Created by Roy Osherove, Team Agile
// Blog: www.ISerializable.com
// Roy@TeamAgile.com

#pragma warning disable 1591
namespace TeamAgile.Samples.Threading
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using System.Threading;
    using System.ComponentModel;

/// <summary>
/// Replaces the standard BackgroundWorker Component in .NET 2.0 Winforms
/// To support the ability of aborting the thread the worker is using, 
///and supporting the fast proporgation of ProgressChanged events without locking up
/// </summary>
/// <remarks></remarks> 
    public class BackgroundWorkerEx : Component
    {
        // Events
        public event DoWorkEventHandler DoWork
        {
            add
            {
                base.Events.AddHandler(BackgroundWorkerEx.doWorkKey, value);
            }
            remove
            {
                base.Events.RemoveHandler(BackgroundWorkerEx.doWorkKey, value);
            }
        }
        public event ProgressChangedEventHandler ProgressChanged
        {
            add
            {
                base.Events.AddHandler(BackgroundWorkerEx.progressChangedKey, value);
            }
            remove
            {
                base.Events.RemoveHandler(BackgroundWorkerEx.progressChangedKey, value);
            }
        }
        public event RunWorkerCompletedEventHandler RunWorkerCompleted
        {
            add
            {
                base.Events.AddHandler(BackgroundWorkerEx.runWorkerCompletedKey, value);
            }
            remove
            {
                base.Events.RemoveHandler(BackgroundWorkerEx.runWorkerCompletedKey, value);
            }
        }

        // Methods
        static BackgroundWorkerEx()
        {
            BackgroundWorkerEx.doWorkKey = new object();
            BackgroundWorkerEx.runWorkerCompletedKey = new object();
            BackgroundWorkerEx.progressChangedKey = new object();
        }

        public BackgroundWorkerEx()
        {
            this.threadStart = new WorkerThreadStartDelegate(this.WorkerThreadStart);
            this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
            this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
        }

        private void AsyncOperationCompleted(object arg)
        {
            this.isRunning = false;
            this.cancellationPending = false;
            this.OnRunWorkerCompleted((RunWorkerCompletedEventArgs)arg);
        }

        public void CancelAsync()
        {
            if (!this.WorkerSupportsCancellation)
            {
                throw new InvalidOperationException("BackgroundWorker_WorkerDoesntSupportCancellation");
            }
            this.cancellationPending = true;
        }

        protected virtual void OnDoWork(DoWorkEventArgs e)
        {
            mThread = Thread.CurrentThread;
            DoWorkEventHandler workStartDelegate = (DoWorkEventHandler)base.Events[BackgroundWorkerEx.doWorkKey];
            if (workStartDelegate != null)
            {
                try
                {
                    workStartDelegate(this, e);
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
            }
        }

        private object tempLock = new object();
        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            ProgressChangedEventHandler progressChangedDelegate = (ProgressChangedEventHandler)base.Events[BackgroundWorkerEx.progressChangedKey];
            if (progressChangedDelegate != null)
            {
                progressChangedDelegate(this, e);
            }
        }

        protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            RunWorkerCompletedEventHandler workderCompletedDelegate = (RunWorkerCompletedEventHandler)base.Events[BackgroundWorkerEx.runWorkerCompletedKey];
            if (workderCompletedDelegate != null)
            {
                workderCompletedDelegate(this, e);
            }
        }

        private void ProgressReporter(object arg)
        {
            this.OnProgressChanged((ProgressChangedEventArgs)arg);
        }

        public void ReportProgress(int percentProgress)
        {
            this.ReportProgress(percentProgress, null);
        }

        public void ReportProgress(int percentProgress, object userState)
        {
            if (!this.WorkerReportsProgress)
            {
                throw new InvalidOperationException("BackgroundWorker_WorkerDoesntReportProgress");
            }
            ProgressChangedEventArgs progressArgs = new ProgressChangedEventArgs(percentProgress, userState);
            object lockTarget = new object();
            if (this.asyncOperation != null)
            {
                this.asyncOperation.Post(this.progressReporter, progressArgs);
               // Thread.Sleep(10);
            }
            else
            {
                this.progressReporter(progressArgs);
            }
        }

        public void RunWorkerAsync()
        {
            this.RunWorkerAsync(null);
        }

        private Thread mThread;
        public void StopImmediately()
        {
            if (!IsBusy || mThread == null)
            {
                return;
            }
            else
            {
                mThread.Abort();
                //there is no need to catch a threadAbortException 
                //since we are catching it and resetting it inside the OnDoWork method
            }
            

            RunWorkerCompletedEventArgs completedArgs =
                new RunWorkerCompletedEventArgs(null, null, true);
            if (asyncOperation != null)
            {
                //invoke operation on the correct thread
                asyncOperation.PostOperationCompleted(operationCompleted, completedArgs);
            }
            else
            {
                //invoke operation directly
                operationCompleted(completedArgs);
            }
        }

        public void RunWorkerAsync(object argument)
        {
            if (this.isRunning)
            {
                throw new InvalidOperationException("BackgroundWorker_WorkerAlreadyRunning");
            }
            this.isRunning = true;
            this.cancellationPending = false;
            this.asyncOperation = AsyncOperationManager.CreateOperation(null);
            this.threadStart.BeginInvoke(argument, null, null);
        }

        private void WorkerThreadStart(object userState)
        {
            object result = null;
            Exception workerException = null;
            bool cancel = false;
            try
            {
                DoWorkEventArgs workArgs = new DoWorkEventArgs(userState);
                this.OnDoWork(workArgs);
                if (workArgs.Cancel)
                {
                    cancel = true;
                }
                else
                {
                    result = workArgs.Result;
                }
            }
            catch (Exception ex)
            {
                workerException = ex;
            }
            RunWorkerCompletedEventArgs completedArgs = new RunWorkerCompletedEventArgs(result, workerException, cancel);
            if (isRunning)
            {
                this.asyncOperation.PostOperationCompleted(this.operationCompleted, completedArgs);
            }
        }


        // Properties
        public bool CancellationPending
        {
            get
            {
                return this.cancellationPending;
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isRunning;
            }
        }

        public bool WorkerReportsProgress
        {
            get
            {
                return this.workerReportsProgress;
            }
            set
            {
                this.workerReportsProgress = value;
            }
        }

        public bool WorkerSupportsCancellation
        {
            get
            {
                return this.canCancelWorker;
            }
            set
            {
                this.canCancelWorker = value;
            }
        }


        // Fields
        private AsyncOperation asyncOperation;
        private bool canCancelWorker;
        private bool cancellationPending;
        private static readonly object doWorkKey;
        private bool isRunning;
        private readonly SendOrPostCallback operationCompleted;
        private static readonly object progressChangedKey;
        private readonly SendOrPostCallback progressReporter;
        private static readonly object runWorkerCompletedKey;
        private readonly WorkerThreadStartDelegate threadStart;
        private bool workerReportsProgress;

        // Nested Types
        private delegate void WorkerThreadStartDelegate(object argument);

    }
}
#pragma warning restore 1591
