using System.Threading.Tasks;
using CECommon.DataAccess;
using PatchDatabase = CECommon.MetaAccess.PatchDatabase;

namespace DBUpdater
{
    internal class PatchExecuter
    {
        public static Task ApplyPatch(IContext context, IPatch patch) => ApplyPatch(context.AsLegacyImplementer(), patch);

        internal static Task ApplyPatch(CEContext ctx, IPatch patch)
        {
            var patcher = new PatchDatabase(ctx);
            string patchPath = patch.GetFolderPath();
            patcher.ApplyPatch(patchPath);
            return Task.CompletedTask;
        }
    }
}
