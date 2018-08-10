using System.Threading.Tasks;
using PatchDatabase = CECommon.MetaAccess.PatchDatabase;

namespace ContourAutoUpdate
{
    internal class PatchHelper
    {
        public static Task ApplyPatch(IContext context, IPatch patch)
        {
            var patcher = new PatchDatabase(context.AsLegacyImplementer());
            string patchPath = patch.GetFolderPath();
            patcher.ApplyPatch(patchPath);
            return Task.CompletedTask;
        }
    }
}
