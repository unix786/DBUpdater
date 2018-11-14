namespace ContourAutoUpdate
{
    internal class PatchVersion
    {
        public int Version { get; }
        public int Build { get; }
        public int Patch { get; }

        public PatchVersion(int version, int build, int patch)
        {
            Version = version;
            Build = build;
            Patch = patch;
        }

        public override string ToString() => $"{Version:D2}.{Build:D3}.{Patch:D3}";
    }
}
