using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Lab.Extensions;

namespace TauCode.Lab.Data
{
    public readonly struct Version : IEquatable<Version>, IComparable<Version>
    {
        private const int MaxPart = 65535;
        private const int MaxPartDigitCount = 5;

        private static readonly HashSet<char> AcceptableSuffixChars;

        static Version()
        {
            var chars = new List<char>();
            chars.Add('-');

            for (var c = 'a'; c <= 'z'; c++)
            {
                chars.Add(c);
            }

            for (var c = 'A'; c <= 'Z'; c++)
            {
                chars.Add(c);
            }

            for (var c = '0'; c <= '9'; c++)
            {
                chars.Add(c);
            }

            AcceptableSuffixChars = new HashSet<char>(chars);

        }

        public Version(int major, int minor, int patch, string suffix)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Suffix = CheckSuffix(suffix);
        }

        public Version(string version)
        {
            var v = Version.Parse(version);
            this.Major = v.Major;
            this.Minor = v.Minor;
            this.Patch = v.Patch;
            this.Suffix = v.Suffix;
        }

        private static string CheckSuffix(string suffix)
        {
            if (suffix == null)
            {
                return null;
            }

            if (suffix.Length == 0)
            {
                throw new NotImplementedException();
            }

            if (suffix.Any(x => !AcceptableSuffixChars.Contains(x)))
            {
                throw new NotImplementedException();
            }

            return suffix;
        }

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string Suffix { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(this.Major);
            sb.Append('.');
            sb.Append(this.Minor);
            sb.Append('.');
            sb.Append(this.Patch);

            if (this.Suffix != null)
            {
                sb.Append('-');
                sb.Append(this.Suffix);
            }

            var res = sb.ToString();
            return res;
        }

        public bool Equals(Version other)
        {
            var eq =
                this.Major == other.Major &&
                this.Minor == other.Minor &&
                this.Patch == other.Patch &&
                this.Suffix == other.Suffix;

            return eq;
        }

        public override bool Equals(object obj)
        {
            return obj is Version other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.Major,
                this.Minor,
                this.Patch,
                this.Suffix);
        }

        public int CompareTo(Version other)
        {
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            var patchComparison = Patch.CompareTo(other.Patch);
            if (patchComparison != 0) return patchComparison;
            return string.Compare(Suffix, other.Suffix, StringComparison.Ordinal);
        }

        public static Version Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return Parse(s.AsSpan());
        }

        public static Version Parse(ReadOnlySpan<char> span)
        {
            var parsed = TryParse(span, out var v);
            if (parsed)
            {
                return v;
            }

            throw new NotImplementedException();
        }

        public static bool TryParse(string s, out Version v)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> span, out Version v)
        {
            if (span.Length == 0)
            {
                v = default;
                return false;
            }

            Span<int> parts = stackalloc int[3];
            var numberPartCount = 0;

            var start = 0;
            var index = 0;

            var gotHyphen = false;

            while (true)
            {
                var delta = index - start;

                if (delta > MaxPartDigitCount)
                {
                    v = default;
                    return false;
                }

                if (index == span.Length)
                {
                    if (delta == 0)
                    {
                        v = default;
                        return false;
                    }

                    var numberPart = ExtractNumberPart(span.Slice(start, delta));
                    if (numberPart == null)
                    {
                        v = default;
                        return false;
                    }

                    parts[numberPartCount] = numberPart.Value;
                    numberPartCount++;

                    break;
                }

                var c = span[index];
                if (c.IsDecimalDigit())
                {
                    index++;
                    continue;
                }

                if (c == '.')
                {
                    if (numberPartCount == 3)
                    {
                        throw new NotImplementedException();
                    }
                    
                    if (delta == 0)
                    {
                        v = default;
                        return false;
                    }

                    var numberPart = ExtractNumberPart(span.Slice(start, delta));
                    if (numberPart == null)
                    {
                        v = default;
                        return false;
                    }

                    parts[numberPartCount] = numberPart.Value;
                    numberPartCount++;

                    index++;
                    start = index;
                    continue;
                }

                if (c == '-')
                {
                    gotHyphen = true;

                    if (delta == 0)
                    {
                        v = default;
                        return false;
                    }

                    var numberPart = ExtractNumberPart(span.Slice(start, delta));
                    if (numberPart == null)
                    {
                        v = default;
                        return false;
                    }

                    parts[numberPartCount] = numberPart.Value;
                    numberPartCount++;

                    index++;
                    break;
                }

                v = default;
                return false;
            }

            string suffix = null;

            if (gotHyphen)
            {
                var suffixSpan = span.Slice(index);
                if (suffixSpan.Length == 0)
                {
                    v = default;
                    return false;
                }

                suffix = suffixSpan.ToString();
            }

            v = new Version(parts[0], parts[1], parts[2], suffix);
            return true;
        }

        private static int? ExtractNumberPart(ReadOnlySpan<char> span)
        {
            if (span.Length > MaxPartDigitCount)
            {
                return null;
            }

            var n = int.Parse(span);
            if (n > MaxPart)
            {
                return null;
            }

            return n;
        }

        public static bool operator ==(Version v1, Version v2) => v1.Equals(v2);

        public static bool operator !=(Version v1, Version v2)
        {
            return !(v1 == v2);
        }

        public static bool operator <(Version v1, Version v2)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(Version v1, Version v2)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(Version v1, Version v2)
        {
            return !(v1 > v2);
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return !(v1 < v2);
        }
    }
}
