using System;

namespace ContourAutoUpdate
{
    internal class PatchVersion : IComparable<PatchVersion>, IEquatable<PatchVersion>
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

        public int CompareTo(PatchVersion other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (Version < other.Version) return -1;
            else if (other.Version < Version) return 1;
            else if (Build < other.Build) return -1;
            else if (other.Build < Build) return 1;
            else if (Patch < other.Patch) return -1;
            else if (other.Patch < Patch) return 1;
            else return 0;
        }

        public override int GetHashCode()
        {
            return (Version << 24) + (Build << 16) + Patch;
        }

        public override bool Equals(object obj) => Equals(obj as PatchVersion);

        public bool Equals(PatchVersion other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return other.Version == Version && other.Build == Build && other.Patch == Patch;
        }
    }
}
