using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContourAutoUpdate
{
    internal class DatabaseUpdater
    {
        public DatabaseUpdater(PatchProvider patchProvider)
        {

        }

        public Task Update(DatabaseServerInfo serverInfo, string databaseName, string patchGroupName, IProgress<string> progress)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    progress.Report($"Step {i + 1}.");
                    Thread.Sleep(400);
                    progress.Report("Step complete.");
                }
                progress.Report("Process complete.");
            });
        }
    }
}
