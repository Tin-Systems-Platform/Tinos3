
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
using Cosmos.Kernel.System.FileSystem;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    public static class TFSAttributes
    {
        private static VfsMode DirPerms = 
            VfsMode.OwnerRead | VfsMode.OwnerWrite | VfsMode.OwnerExecute |
            VfsMode.GroupRead | VfsMode.GroupExecute |
            VfsMode.OtherWrite | VfsMode.OtherRead | VfsMode.OtherExecute;

        private const VfsMode FilePermissions =
            VfsMode.OwnerRead | VfsMode.OwnerWrite |
            VfsMode.GroupRead |
            VfsMode.OtherRead;

        private const VfsMode WriteMask =
            VfsMode.OwnerWrite |
            VfsMode.GroupWrite |
            VfsMode.OtherWrite;

        private const int TfsEpochYear = 1970;

        private const int DateYearShift = 9;

        private const int DateYearMask = 0x7F;

        private const int DateMonthShift = 5;

        private const int DateMonthMask = 0x0F;


        private const int DateDayMask = 0x1F;

        private const int TimeHourShift = 11;

        private const int TimeHourMask = 0x1F;

        private const int TimeMinuteShift = 5;

        private const int TimeMinuteMask = 0x3F;

        private const int TimeTwoSecondMask = 0x1F;

        private const int TwoSecondGranularity = 2;

        private const int TenthsPerSecond = 100;

        private const long NanosecondsPerHundredth = 10_000_000L;

        private const long SecondsPerDay = 86_400L;
        private const long SecondsPerHour = 3600L;
        private const long SecondsPerMinute = 60L;

        private const int UnixEpochYear = 1970;

        private const int MonthsPerYear = 12;

        private const int MaxDaysInMonth = 31;

        private const int DaysPerLeapYear = 366;

        private const int DaysPerCommonYear = 365;

        private const int February = 2;

        private const int LeapYearInterval = 4;

        private const int LeapCenturyException = 100;

        private const int LeapQuadCenturyInterval = 400;

        private static ReadOnlySpan<byte> DaysInMonth => [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        public static VfsMode ToMode(TFSAttr attr)
        {
            bool isDirectory = (attr & TFSAttr.Directory) != 0;

            VfsMode mode = isDirectory
                ? VfsMode.Directory | DirPerms
                : VfsMode.RegularFile | FilePermissions;

            if ((attr & TFSAttr.ReadOnly) != 0)
            {
                mode &= ~WriteMask;
            }
            return mode;
        }

        public static TFSAttr ToFatAttr(VfsMode mode)
        {
            TFSAttr attr = (mode & VfsMode.FileTypeMask) == VfsMode.Directory
                ? TFSAttr.Directory
                : TFSAttr.None;

            if ((mode & VfsMode.OwnerWrite) == 0 && (mode & VfsMode.FileTypeMask) != VfsMode.Directory)
            {
                attr |= TFSAttr.ReadOnly;
            }

            return attr;
        }

        public static VfsTimespec UnpackDateTime(ushort fatDate, ushort fatTime, byte tenths)
        {
            if (fatDate == 0)
            {
                return new VfsTimespec(0, 0);
            }

            int year = ((fatDate >> DateYearShift) & DateYearMask) + TfsEpochYear;
            int month = (fatDate >> DateMonthShift) & DateMonthMask;
            int day = fatDate & DateDayMask;

            int hour = (fatTime >> TimeHourShift) & TimeHourMask;
            int minute = (fatTime >> TimeMinuteShift) & TimeMinuteMask;
            int second = (fatTime & TimeTwoSecondMask) * TwoSecondGranularity + (tenths >= TenthsPerSecond ? 1 : 0);

            long epochSeconds = ToUnixSeconds(year, month, day, hour, minute, second);
            long nanoseconds = (long)(tenths % TenthsPerSecond) * NanosecondsPerHundredth;
            return new VfsTimespec(epochSeconds, nanoseconds);
        }

        private static long ToUnixSeconds(int year, int month, int day, int hour, int minute, int second)
        {
            if (month < 1 || month > MonthsPerYear || day < 1 || day > MaxDaysInMonth)
            {
                return 0;
            }

            long days = 0;
            for (int y = UnixEpochYear; y < year; y++)
            {
                days += IsLeap(y) ? DaysPerLeapYear : DaysPerCommonYear;
            }
            for (int m = 1; m < month; m++)
            {
                days += DaysInMonth[m - 1];
                if (m == February && IsLeap(year))
                {
                    days++;
                }
            }
            days += day - 1;
            return days * SecondsPerDay + hour * SecondsPerHour + minute * SecondsPerMinute + second;
        }

        private static bool IsLeap(int year)
        {
            return (year % LeapYearInterval == 0 && year % LeapCenturyException != 0) || year % LeapQuadCenturyInterval == 0;
        }
    }
}
