
/*
 * Portions of this file are derived from Cosmos.
 *
 * Copyright (c) 2007-2026, CosmosOS, COSMOS Project
 * Licensed under the BSD 3-Clause License.
 *
 * TFS-specific modifications and additional code:
 * Copyright (c) 2026 Tinos3 contributors.
 *
 * The full BSD 3-Clause license terms and disclaimer
 * are available in THIRD-PARTY-LICENSES/Cosmos-BSD-3-Clause.txt.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    internal sealed class TfsDirEntry
    {
        public string Name { get; }
        public string ShortName { get; }
        public TFSAttr Attributes { get; }
        public uint FirstCluster { get; }
        public uint Size { get; }
        public int ByteOffset { get; }
        public int LfnEntryCount { get; }
        public bool IsDirectory => (Attributes & TFSAttr.Directory) != 0;
        public bool IsVolumeId => (Attributes & TFSAttr.VolumeID) != 0;

        public TfsDirEntry(
            string name,
            string shortName,
            TFSAttr attributes,
            uint firstCluster,
            uint size,
            int byteOffset,
            int lfnEntryCount)
        {
            Name = name;
            ShortName = shortName;
            Attributes = attributes;
            FirstCluster = firstCluster;
            Size = size;
            ByteOffset = byteOffset;
            LfnEntryCount = lfnEntryCount;
        }
    }

    internal class TfsDirectory
    {
        internal const int EntrySize = 32;
        internal const byte DeletedMarker = 0xE5;
        internal const byte EndOfDirectoryMarker = 0x00;
        internal const int AttributesOffset = 11;
        internal const int FirstClusterHighOffset = 20;
        internal const int FirstClusterLowOffset = 26;
        internal const uint EmptyFirstCluster = 0u;
        internal const int SizeOffset = 28;
        internal const int ClusterWordBytes = 2;
        internal const int SizeFieldBytes = 4;
        internal const uint DirectorySizeOnDisk = 0u;
        internal const int ClusterHighShift = 16;
        internal const uint ClusterWordMask = 0xFFFFu;
        internal const int MaxLfnNameLength = 255;
        internal const int ShortNameLength = 11;
        private const int CaseDistance = 32;
        private const byte LfnLastBit = 0x40;
        private const int LfnCharsPerEntry = 13;
        private const int MaxLfnEntries = 20;
        private const byte LfnOrdinalMask = 0x3F;
        private const int LfnFirstOrdinal = 1;
        private const int LfnSequenceOffset = 0;
        private const int LfnTypeOffset = 12;
        private const byte LfnTypeNameEntry = 0;
        private const int LfnChecksumOffset = 13;
        private const byte ChecksumCarryBit = 0x80;
        private const int LfnName1Offset = 1;
        private const int LfnName1Bytes = 10;
        private const int LfnName2Offset = 14;
        private const int LfnName2Bytes = 12;
        private const int LfnName3Offset = 28;
        private const int LfnName3Bytes = 4;
        private const int LfnName1Chars = 5;
        private const int LfnName2Chars = 6;
        private const int LfnName3Chars = 2;
        private const char LfnPadChar = (char)0xFFFF;
        private const int Ucs2BytesPerChar = 2;
        private const int Ucs2HighByteOffset = 1;
        private const int BitsPerByte = 8;
        private const int LowByteMask = 0xFF;
        private const byte PadByte = 0x20;
        private const byte KanjiLeadSubstitute = 0x05;
        private const int ShortBaseLength = 8;
        private const int ShortExtLength = 3;
        private const int MaxShortNameChars = 12;
        private const int MaxShortNameDots = 1;
        private const uint NumericTailRadix = 10;
        private const uint FirstNumericTail = 1;
        private const int NumericTailTildeChars = 1;

        internal static List<TfsDirEntry> Parse(ReadOnlySpan<byte> buffer, bool fat32)
        {
            List<TfsDirEntry> result = [];
            Span<char> lfnAccum = stackalloc char[LfnCharsPerEntry * MaxLfnEntries];
            int lfnLength = 0;
            int lfnEntryCount = 0;
            byte lfnChecksum = 0;

            for (int offset = 0; offset + EntrySize <= buffer.Length; offset += EntrySize)
            {
                byte first = buffer[offset];
                if (first == EndOfDirectoryMarker)
                {
                    break;
                }

                if (first == DeletedMarker)
                {
                    lfnAccum.Clear();
                    lfnLength = 0;
                    lfnEntryCount = 0;
                    continue;
                }

                TFSAttr attributes = (TFSAttr)buffer[offset + AttributesOffset];

                if (attributes == TFSAttr.Lfn)
                {
                    // LFN metadata is untrusted: validate the 6-bit ordinal
                    // and require one checksum across the chain, or a stale
                    // accumulator splices two names together.
                    byte sequence = buffer[offset];
                    int sequenceIndex = (sequence & LfnOrdinalMask) - LfnFirstOrdinal;
                    byte checksum = buffer[offset + LfnChecksumOffset];
                    if (sequenceIndex < 0 || sequenceIndex >= MaxLfnEntries
                        || (lfnEntryCount > 0 && checksum != lfnChecksum))
                    {
                        lfnAccum.Clear();
                        lfnLength = 0;
                        lfnEntryCount = 0;
                        continue;
                    }

                    lfnChecksum = checksum;
                    int destBase = sequenceIndex * LfnCharsPerEntry;
                    ReadLfnChars(buffer.Slice(offset, EntrySize), lfnAccum.Slice(destBase, LfnCharsPerEntry));

                    int candidateLength = destBase + LfnCharsPerEntry;
                    if (candidateLength > lfnLength)
                    {
                        lfnLength = candidateLength;
                    }
                    lfnEntryCount++;
                    continue;
                }

                string shortName = DecodeShortName(buffer.Slice(offset, ShortNameLength), first);

                bool lfnValid = lfnLength > 0
                    && lfnChecksum == ComputeShortChecksum(buffer.Slice(offset, ShortNameLength));
                string longName = lfnValid
                    ? TrimLfn(lfnAccum.Slice(0, lfnLength))
                    : shortName;

                uint firstClusterLow = BitConverter.ToUInt16(buffer.Slice(offset + FirstClusterLowOffset, ClusterWordBytes));
                uint firstCluster = firstClusterLow;
                if (fat32)
                {
                    uint firstClusterHigh = BitConverter.ToUInt16(buffer.Slice(offset + FirstClusterHighOffset, ClusterWordBytes));
                    firstCluster |= firstClusterHigh << ClusterHighShift;
                }
                uint size = BitConverter.ToUInt32(buffer.Slice(offset + SizeOffset, SizeFieldBytes));

                result.Add(new TfsDirEntry(
                    longName,
                    shortName,
                    attributes,
                    firstCluster,
                    size,
                    offset,
                    lfnValid ? lfnEntryCount : 0));

                lfnAccum.Clear();
                lfnLength = 0;
                lfnEntryCount = 0;
            }

            return result;
        }

        internal static int FindFreeRun(ReadOnlySpan<byte> buffer, int entriesNeeded, out bool consumedTerminator)
        {
            consumedTerminator = false;
            int run = 0;
            int runStart = -1;

            for (int offset = 0; offset + EntrySize <= buffer.Length; offset += EntrySize)
            {
                byte first = buffer[offset];
                if (first == EndOfDirectoryMarker)
                {
                    if (run == 0)
                    {
                        runStart = offset;
                    }
                    int available = run + (buffer.Length - offset) / EntrySize;
                    if (available >= entriesNeeded)
                    {
                        consumedTerminator = runStart + entriesNeeded * EntrySize > offset;
                        return runStart;
                    }
                    return -1;
                }
                if (first == DeletedMarker)
                {
                    if (run == 0)
                    {
                        runStart = offset;
                    }
                    run++;
                    if (run >= entriesNeeded)
                    {
                        return runStart;
                    }
                }
                else
                {
                    run = 0;
                    runStart = -1;
                }
            }

            return -1;
        }

        internal static int LfnEntryCountFor(ReadOnlySpan<char> name)
        {
            if (FitsInShortName(name))
            {
                return 0;
            }
            return (name.Length + LfnCharsPerEntry - 1) / LfnCharsPerEntry;
        }

        internal static void WriteShortEntry(
            Span<byte> dest,
            int offset,
            ReadOnlySpan<char> shortName11,
            TFSAttr attributes,
            uint firstCluster,
            uint size)
        {
            Span<byte> entry = dest.Slice(offset, EntrySize);
            entry.Clear();

            for (int i = 0; i < ShortNameLength && i < shortName11.Length; i++)
            {
                entry[i] = (byte)shortName11[i];
            }
            for (int i = shortName11.Length; i < ShortNameLength; i++)
            {
                entry[i] = PadByte;
            }

            entry[AttributesOffset] = (byte)attributes;
            BitConverter.TryWriteBytes(entry.Slice(FirstClusterHighOffset, ClusterWordBytes), (ushort)((firstCluster >> ClusterHighShift) & ClusterWordMask));
            BitConverter.TryWriteBytes(entry.Slice(FirstClusterLowOffset, ClusterWordBytes), (ushort)(firstCluster & ClusterWordMask));
            BitConverter.TryWriteBytes(entry.Slice(SizeOffset, SizeFieldBytes), size);
        }

        internal static bool NameEqualsIgnoreCase(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                char ac = a[i];
                char bc = b[i];
                if (ac is >= 'a' and <= 'z')
                {
                    ac = (char)(ac - CaseDistance);
                }
                if (bc is >= 'a' and <= 'z')
                {
                    bc = (char)(bc - CaseDistance);
                }
                if (ac != bc)
                {
                    return false;
                }
            }
            return true;
        }

        internal static void WriteLfnEntries(
        Span<byte> dest,
        int offset,
        ReadOnlySpan<char> longName,
        ReadOnlySpan<char> shortName11)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(longName.Length, MaxLfnNameLength, nameof(longName));

            int entries = LfnEntryCountFor(longName);
            if (entries == 0)
            {
                return;
            }

            byte checksum = ComputeShortChecksum(shortName11);
            Span<char> chunk = stackalloc char[LfnCharsPerEntry];
            for (int i = 0; i < entries; i++)
            {
                int ordinal = entries - i;
                int sourceStart = (ordinal - LfnFirstOrdinal) * LfnCharsPerEntry;
                for (int c = 0; c < LfnCharsPerEntry; c++)
                {
                    int sourceIndex = sourceStart + c;
                    if (sourceIndex < longName.Length)
                    {
                        chunk[c] = longName[sourceIndex];
                    }
                    else if (sourceIndex == longName.Length)
                    {
                        chunk[c] = '\0';
                    }
                    else
                    {
                        chunk[c] = LfnPadChar;
                    }
                }

                byte sequence = (byte)ordinal;
                if (i == 0)
                {
                    sequence |= LfnLastBit;
                }

                Span<byte> entry = dest.Slice(offset + i * EntrySize, EntrySize);
                entry.Clear();
                entry[LfnSequenceOffset] = sequence;
                entry[AttributesOffset] = (byte)TFSAttr.Lfn;
                entry[LfnTypeOffset] = LfnTypeNameEntry;
                entry[LfnChecksumOffset] = checksum;

                WriteUcs2(chunk.Slice(0, LfnName1Chars), entry.Slice(LfnName1Offset, LfnName1Bytes));
                WriteUcs2(chunk.Slice(LfnName1Chars, LfnName2Chars), entry.Slice(LfnName2Offset, LfnName2Bytes));
                WriteUcs2(chunk.Slice(LfnName1Chars + LfnName2Chars, LfnName3Chars), entry.Slice(LfnName3Offset, LfnName3Bytes));
            }
        }

        internal static byte ComputeShortChecksum(ReadOnlySpan<char> shortName11)
        {
            byte sum = 0;
            for (int i = 0; i < ShortNameLength; i++)
            {
                byte b = i < shortName11.Length ? (byte)shortName11[i] : PadByte;
                sum = (byte)(((sum & 1) != 0 ? ChecksumCarryBit : 0) + (sum >> 1) + b);
            }
            return sum;
        }

        internal static byte ComputeShortChecksum(ReadOnlySpan<byte> raw11)
        {
            byte sum = 0;
            for (int i = 0; i < ShortNameLength; i++)
            {
                sum = (byte)(((sum & 1) != 0 ? ChecksumCarryBit : 0) + (sum >> 1) + raw11[i]);
            }
            return sum;
        }

        internal static void BuildShortName(string longName, Span<char> dest11, ReadOnlySpan<byte> directoryData)
        {
            for (int i = 0; i < ShortNameLength; i++)
            {
                dest11[i] = ' ';
            }

            if (longName.Length == 0)
            {
                return;
            }

            int dot = longName.LastIndexOf('.');
            ReadOnlySpan<char> baseName = dot >= 0 ? longName.AsSpan(0, dot) : longName.AsSpan();
            ReadOnlySpan<char> extension = dot >= 0 && dot + 1 < longName.Length
                ? longName.AsSpan(dot + 1)
                : [];

            int e = 0;
            for (int i = 0; i < extension.Length && e < ShortExtLength; i++)
            {
                char c = NormalizeShort(extension[i]);
                if (c != '\0')
                {
                    dest11[ShortBaseLength + e++] = c;
                }
            }

            if (FitsInShortName(longName))
            {
                int b = 0;
                for (int i = 0; i < baseName.Length && b < ShortBaseLength; i++)
                {
                    char c = NormalizeShort(baseName[i]);
                    if (c != '\0')
                    {
                        dest11[b++] = c;
                    }
                }
                return;
            }

            // Collect the normalized base once, then probe ~1, ~2, ...
            // (shrinking the kept base as the tail widens) until the result
            // is unique within the directory.
            Span<char> normalizedBase = stackalloc char[ShortBaseLength];
            int normalizedLength = 0;
            for (int i = 0; i < baseName.Length && normalizedLength < ShortBaseLength; i++)
            {
                char c = NormalizeShort(baseName[i]);
                if (c != '\0')
                {
                    normalizedBase[normalizedLength++] = c;
                }
            }

            for (uint tail = FirstNumericTail; ; tail++)
            {
                int digits = CountDigits(tail);
                int keep = ShortBaseLength - NumericTailTildeChars - digits;
                if (keep > normalizedLength)
                {
                    keep = normalizedLength;
                }
                if (keep < 0)
                {
                    keep = 0;
                }

                int b = 0;
                for (; b < keep; b++)
                {
                    dest11[b] = normalizedBase[b];
                }
                dest11[b++] = '~';
                uint value = tail;
                for (int i = digits - 1; i >= 0; i--)
                {
                    dest11[b + i] = (char)('0' + value % NumericTailRadix);
                    value /= NumericTailRadix;
                }
                b += digits;
                for (int i = b; i < ShortBaseLength; i++)
                {
                    dest11[i] = ' ';
                }

                if (!ShortNameExists(directoryData, dest11))
                {
                    return;
                }
            }
        }

        internal static bool FitsInShortName(ReadOnlySpan<char> name)
        {
            if (name.Length == 0 || name.Length > MaxShortNameChars)
            {
                return false;
            }

            int dot = name.LastIndexOf('.');
            int baseLength = dot >= 0 ? dot : name.Length;
            int extensionLength = dot >= 0 ? name.Length - dot - 1 : 0;
            if (baseLength == 0 || baseLength > ShortBaseLength || extensionLength > ShortExtLength)
            {
                return false;
            }

            int dots = 0;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c == '.')
                {
                    dots++;
                    if (dots > MaxShortNameDots)
                    {
                        return false;
                    }
                    continue;
                }
                // Any character the 8.3 encoder would alter (upcase, mangle
                // to '_', or drop) must take the LFN path, or the requested
                // spelling exists nowhere on the volume after a re-parse.
                if (NormalizeShort(c) != c)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ShortNameExists(ReadOnlySpan<byte> directoryData, ReadOnlySpan<char> short11)
        {
            for (int offset = 0; offset + EntrySize <= directoryData.Length; offset += EntrySize)
            {
                byte first = directoryData[offset];
                if (first == EndOfDirectoryMarker)
                {
                    return false;
                }
                if (first == DeletedMarker || (TFSAttr)directoryData[offset + AttributesOffset] == TFSAttr.Lfn)
                {
                    continue;
                }
                bool same = true;
                for (int i = 0; i < ShortNameLength && same; i++)
                {
                    same = directoryData[offset + i] == (byte)short11[i];
                }
                if (same)
                {
                    return true;
                }
            }
            return false;
        }

        private static int CountDigits(uint value)
        {
            int digits = 1;
            while (value >= NumericTailRadix)
            {
                value /= NumericTailRadix;
                digits++;
            }
            return digits;
        }

        internal static void MarkDeleted(Span<byte> dest, int offset, int entryCount)
        {
            for (int i = 0; i < entryCount; i++)
            {
                dest[offset + i * EntrySize] = DeletedMarker;
            }
        }

        private static void ReadLfnChars(ReadOnlySpan<byte> entry, Span<char> dest13)
        {
            ReadUcs2(entry.Slice(LfnName1Offset, LfnName1Bytes), dest13.Slice(0, LfnName1Chars));
            ReadUcs2(entry.Slice(LfnName2Offset, LfnName2Bytes), dest13.Slice(LfnName1Chars, LfnName2Chars));
            ReadUcs2(entry.Slice(LfnName3Offset, LfnName3Bytes), dest13.Slice(LfnName1Chars + LfnName2Chars, LfnName3Chars));
        }

        private static void ReadUcs2(ReadOnlySpan<byte> src, Span<char> dest)
        {
            for (int i = 0; i < dest.Length; i++)
            {
                dest[i] = (char)(src[i * Ucs2BytesPerChar] | (src[i * Ucs2BytesPerChar + Ucs2HighByteOffset] << BitsPerByte));
            }
        }

        private static void WriteUcs2(ReadOnlySpan<char> src, Span<byte> dest)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dest[i * Ucs2BytesPerChar] = (byte)(src[i] & LowByteMask);
                dest[i * Ucs2BytesPerChar + Ucs2HighByteOffset] = (byte)((src[i] >> BitsPerByte) & LowByteMask);
            }
        }

        private static string TrimLfn(ReadOnlySpan<char> chars)
        {
            int end = chars.Length;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] is '\0' or LfnPadChar)
                {
                    end = i;
                    break;
                }
            }
            return new string(chars.Slice(0, end));
        }

        private static string DecodeShortName(ReadOnlySpan<byte> raw11, byte firstByte)
        {
            Span<char> chars = stackalloc char[MaxShortNameChars];
            int length = 0;

            byte effectiveFirst = firstByte == KanjiLeadSubstitute ? DeletedMarker : firstByte;
            chars[length++] = (char)effectiveFirst;

            for (int i = 1; i < ShortBaseLength; i++)
            {
                if (raw11[i] == PadByte)
                {
                    break;
                }
                chars[length++] = (char)raw11[i];
            }

            bool hasExtension = false;
            for (int i = ShortBaseLength; i < ShortNameLength; i++)
            {
                if (raw11[i] != PadByte)
                {
                    hasExtension = true;
                    break;
                }
            }

            if (hasExtension)
            {
                chars[length++] = '.';
                for (int i = ShortBaseLength; i < ShortNameLength; i++)
                {
                    if (raw11[i] == PadByte)
                    {
                        break;
                    }
                    chars[length++] = (char)raw11[i];
                }
            }

            return new string(chars.Slice(0, length));
        }

        private static char NormalizeShort(char c)
        {
            if (c == ' ')
            {
                return '\0';
            }
            if (c is >= 'a' and <= 'z')
            {
                return (char)(c - CaseDistance);
            }
            if (c is (>= 'A' and <= 'Z') or (>= '0' and <= '9'))
            {
                return c;
            }
            if (c is '$' or '%' or '\'' or '-' or '_'
                or '@' or '~' or '`' or '!' or '('
                or ')' or '{' or '}' or '^' or '#' or '&')
            {
                return c;
            }
            return '_';
        }
    }
}
