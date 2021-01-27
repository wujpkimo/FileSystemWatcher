using System;
using System.IO;
using System.Text;
using System.Security.Permissions;
using System.Linq;

namespace Watcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Watcher.exe (directory)");
                return;
            }
            Console.WriteLine($"Monitor path:{args[1]}");

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = args[1];

                watcher.NotifyFilter = NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName;

                watcher.Filter = "*.log";
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;
                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = true;

                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            //Console.WriteLine(File.ReadAllText(e.FullPath));
            string[] lines = File.ReadAllLines(e.FullPath);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            //lines.Select(l => { Console.WriteLine(l); return l; });
        }

        private static void OnRenamed(object source, RenamedEventArgs e) => Console.WriteLine($"File: {e.OldFullPath} renames to {e.FullPath}");
    }
}