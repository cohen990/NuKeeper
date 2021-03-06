﻿using System;
using System.IO;
using System.Linq;

namespace NuKeeper
{
    public static class TempFiles
    {
        private static string NuKeeperTempFilesPath()
        {
            return Path.Combine(Path.GetTempPath(), "NuKeeper");
        }

        public static void DeleteExistingTempDirs()
        {
            var dirInfo = new DirectoryInfo(NuKeeperTempFilesPath());
            var dirs = dirInfo.Exists ? dirInfo.EnumerateDirectories() : Enumerable.Empty<DirectoryInfo>();
            foreach (var dir in dirs)
            {
                TryDelete(dir);
            }
        }

        public static string GetUniqueTemporaryPath()
        {
            var uniqueName = Guid.NewGuid().ToString("N");
            return Path.Combine(NuKeeperTempFilesPath(), uniqueName);
        }

        public static string MakeUniqueTemporaryPath()
        {
            var uniquePath = GetUniqueTemporaryPath();
            Directory.CreateDirectory(uniquePath);
            return uniquePath;
        }

        public static void TryDelete(DirectoryInfo tempDir)
        {
            Console.WriteLine($"Attempting delete of temp dir {tempDir}");

            try
            {
                tempDir.Delete(true);
            }
            catch (Exception)
            {
                Console.WriteLine("Delete failed. Continuing");
            }
        }
    }
}