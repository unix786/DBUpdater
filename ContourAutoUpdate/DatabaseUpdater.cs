﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECommon.DataAccess;

namespace ContourAutoUpdate
{
    internal class DatabaseUpdater
    {
        private readonly PatchProvider patchProvider;
        public DatabaseUpdater(PatchProvider patchProvider)
        {
            this.patchProvider = patchProvider;
        }

        private readonly HashSet<string> unknownCodes = new HashSet<string>();

        /// <summary>Может вернуть null.</summary>
        private string GetPatchCode(PatchProvider.PatchInfo patch, IProgress<string> progress)
        {
            var archiveCode = patch.ArchiveCode;
            if (patchProvider.PatchCodes.TryGetDBCode(archiveCode, out string code))
            {
                return code;
            }

            if (!unknownCodes.Contains(archiveCode))
            {
                unknownCodes.Add(archiveCode);
                progress.Report($"Warning: unknown patch code {archiveCode}, patch {patch}.");
            }
            return null;
        }

        private static CEContext CreateContext(DatabaseServerInfo serverInfo, string databaseName)
        {
            var dbProvider = CEProvider.CreateProvider(DBSettings.DBProvider, serverInfo.Address, databaseName, 2);
            CEContext ctx;
            if (serverInfo.UseDBLogin) ctx = dbProvider.CreateContext(serverInfo.UserName, serverInfo.Password, null);
            else ctx = dbProvider.CreateContext();
            return ctx;
        }

        private static Dictionary<string, PatchVersion> GetDBVersions(CEContext ctx)
        {
            var versions = new Dictionary<string, PatchVersion>();

            var selInstalledVersions = new SCmd(ctx, "SELECT * FROM ABIServApplication");
            using (var table = selInstalledVersions.RunRetDataTable())
            using (var reader = table.CreateDataReader())
            {
                while (reader.Read())
                {
                    string code = (string)reader["Code"];
                    var version = new PatchVersion(
                        (int)reader["Version"],
                        (int)reader["Build"],
                        (int)reader["Patch"]
                        );
                    versions[code] = version;
                }
            }

            return versions;
        }

        private int GetNumber()
        {


            //if(...)
            return 0;
        }

        public Task Update(DatabaseServerInfo serverInfo, string databaseName, string patchGroupName, IProgress<string> progress)
        {
            return Task.Run(async () =>
            {
                CEContext ctx = CreateContext(serverInfo, databaseName);
                Dictionary<string, PatchVersion> installedVersions = GetDBVersions(ctx);

                //progress.Report($"Warning: unknown patch code \"{code}\" in database (patch version {version})!");

                progress.Report("Versions in " + databaseName);
                foreach (var item in installedVersions) progress.Report($"{item.Key}: {item.Value}");

                var patchListStream = patchProvider.GetPatchList(patchGroupName, progress);

                var patches = patchListStream.OrderBy((p) => p.Number).ToList();

                var latestVersions = new Dictionary<string, PatchProvider.PatchInfo>();
                var unknownCodes = new HashSet<string>();
                PatchProvider.PatchInfo lastInstalledPatch = null;
                int lastInstalledPatchIndex = -1;
                var newPatches = new Stack<PatchProvider.PatchInfo>();
                var skipped = new Stack<PatchProvider.PatchInfo>();
                var modified = new Stack<PatchProvider.PatchInfo>(); // Был установлен, но изменился с времени последнего патча.
                for (int i = patches.Count - 1; i >= 0; i--)
                {
                    var patch = patches[i];

                    if (!latestVersions.ContainsKey(patch.ArchiveCode)) latestVersions.Add(patch.ArchiveCode, patch);

                    var code = GetPatchCode(patch, progress);
                    if (code != null)
                    {
                        if (installedVersions.TryGetValue(code, out var serverVersion) && patch.Version.CompareTo(serverVersion) <= 0)
                        {
                            if (lastInstalledPatch == null)
                            {
                                lastInstalledPatch = patch;
                                lastInstalledPatchIndex = i;
                                progress.Report($"Last installed patch: {patch}." + (newPatches.Count == 0 ? " Database is up to date." : $"There are {newPatches.Count} new patches."));
                            }
                            else if (lastInstalledPatch.Timestamp < patch.Timestamp)
                            {
                                modified.Push(patch);
                            }
                        }
                        else if (lastInstalledPatch == null)
                        {
                            newPatches.Push(patch);
                        }
                        else
                        {
                            skipped.Push(patch);
                        }
                    }
                }

                // Report:
                {
                    var sbReport = new StringBuilder();
                    int initialReportSize = sbReport.Length;
                    if (skipped.Count > 0)
                    {
                        sbReport.AppendLine(" * Naujiniai, kurie buvo praleisti (neįdiegti):");
                        foreach (var item in skipped)
                        {
                            //if (...) sbReport.AppendLine($"Visa {item.ArchiveCode} serija"); else
                            sbReport.AppendLine(item.ToString());
                        }
                    }

                    if (modified.Count > 0)
                    {
                        sbReport.AppendLine($" * Naujiniai, kurie buvo modifikuoti serveryje po paskutinio atnaujinimo ({lastInstalledPatch.Timestamp})");
                        foreach (var item in modified) sbReport.AppendLine($"{item}, modifikuotas {item.Timestamp}");
                    }

                    if (sbReport.Length > initialReportSize) progress.Report(sbReport.Append("****").ToString());
                }

                if (newPatches.Count > 0)
                {
                    foreach (IPatch item in patchProvider.Prepare(newPatches, progress))
                    {
                        await PatchExecuter.ApplyPatch(ctx, item);
                    }
                }

                progress.Report("Process complete.");
            });
        }
    }
}
