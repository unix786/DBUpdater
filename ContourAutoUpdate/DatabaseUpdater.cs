using System.Threading.Tasks;

namespace ContourAutoUpdate
{
    internal class DatabaseUpdater
    {
        public DatabaseUpdater(PatchProvider patchProvider)
        {

        }

        public Task Update(DatabaseServerInfo serverInfo, string databaseName, string patchGroupName)
        {
            return Task.CompletedTask;
        }
    }
}
