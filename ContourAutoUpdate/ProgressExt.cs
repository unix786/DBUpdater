using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContourAutoUpdate
{
    internal interface IProgressExt<T> : IProgress<T>
    {
        Task WaitForPauseRequest();
        Task Cancellation();
    }

    internal class ProgressExt<T> : Progress<T>, IProgressExt<T>, IDisposable
    {
        //private Microsoft.VisualStudio.Threading.AsyncManualResetEvent mre;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public bool RequestPause { get; set; }

        public void Cancel() => cancellationTokenSource.Cancel();

        public void Dispose()
        {
            cancellationTokenSource.Dispose();
        }

        async Task IProgressExt<T>.WaitForPauseRequest()
        {
            while (RequestPause) await Task.Delay(10);
        }

        Task IProgressExt<T>.Cancellation()
        {
            return cancellationTokenSource.IsCancellationRequested ?
                Task.FromCanceled(cancellationTokenSource.Token)
                : Task.CompletedTask;
        }
    }
}
