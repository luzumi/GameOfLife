using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DoppelteDateien
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            const string pfad = @"D:\tmp";
            Console.WriteLine(args);
            foreach (var pair in FindDoubles(new DirectoryInfo(pfad)))
            {
                Console.WriteLine($"{pair.Item1.Name} == {pair.Item2.Name}");
                //pair.Item1.Delete();
            }
            Console.ReadLine();
        }

        private static void TraverseDirectory(DirectoryInfo directory, List<Tuple<FileInfo, FileInfo>> doubles, Dictionary<string, FileInfo> hashes)
        {
            using (var md5 = MD5.Create())
            {
                foreach (var file in directory.GetFiles())
                {
                    using (var stream = file.OpenRead())
                    {
                        var hash = BitConverter.ToString(md5.ComputeHash(stream));
                        if (hashes.TryGetValue(hash, out var alreadyFound))
                        {
                            doubles.Add(Tuple.Create(alreadyFound, file));
                        }
                        else
                        {
                            hashes.Add(hash, file);
                        }
                    }
                }
            }

            foreach (var sub in directory.GetDirectories())
            {
                TraverseDirectory(sub, doubles, hashes);
            }
        }

        private static List<Tuple<FileInfo, FileInfo>> FindDoubles(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException();
            }
            if (!directory.Exists)
            {
                throw new FileNotFoundException();
            }

            var doubles = new List<Tuple<FileInfo, FileInfo>>();
            var hashes = new Dictionary<string, FileInfo>();

            TraverseDirectory(directory, doubles, hashes);

            return doubles;
        }
    }
}