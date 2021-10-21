using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace fake_terminal {
    class Program {
        public static string[] userdata = {"",""};
        public static bool hideSelf;
        public static int fileCount = 0;
        public static int dirCount = 0;
        static void Main(string[] args) {

            GetData();
            Console.Clear();
            Console.WriteLine(".");
            foreach (string file in hideSelf ? Directory.GetFiles(Directory.GetCurrentDirectory()).Where(s => !s.Contains("settings.txt") && !s.Contains($"{Assembly.GetExecutingAssembly().GetName().Name}.exe")) : Directory.GetFiles(Directory.GetCurrentDirectory())) {
                Console.WriteLine((IsLastElem(file) && !ParentHasDirs(file) ? "└── " : "├── ") + file.Split('\\').Last());
                fileCount++;
            }
            foreach (string subdir in Directory.GetDirectories(Directory.GetCurrentDirectory())) {
                WriteSubElems(subdir);
            }
            ResetColor(); Console.WriteLine($"\n{dirCount} directories, {fileCount} files\n{userdata[0]}@{userdata[1]}:~$");
            WriteData();
            Console.ReadKey();
        }
        static void GetData() {
            ConsoleKey key;
            do {
                Console.Clear();
                Console.Write("Hide the program and its settings from the tree? (Y/N)");
                key = Console.ReadKey().Key;
            }
            while(key!=ConsoleKey.Y && key!=ConsoleKey.N);
            hideSelf = key==ConsoleKey.Y;
            do {
                Console.Clear();
                Console.Write("Read previous username and hostname? (Y/N)");
                key = Console.ReadKey().Key;
            }
            while (key != ConsoleKey.Y && key != ConsoleKey.N);
            if (key==ConsoleKey.Y && File.Exists("settings.txt") && File.ReadAllLines("settings.txt").Length==2) {
                userdata = File.ReadAllLines("settings.txt");
            }
            while(userdata[0].Length<1) {
                Console.Clear();
                Console.Write("Set username: ");
                userdata[0] = Console.ReadLine();
            }
            while(userdata[1].Length<1) {
                Console.Clear();
                Console.Write("Set hostname: ");
                userdata[1] = Console.ReadLine();
            }
        }
        static void WriteData() => File.WriteAllLines("settings.txt", userdata);
        static void WriteSubElems(string path) {
            dirCount++;
            ResetColor(); Console.Write(new string(' ',(path.Replace(Directory.GetCurrentDirectory(),"").Split('\\').Length - 2) * 4) + (IsLastDir(path) ? "└── " : "├── "));
            SetBlue(); Console.WriteLine(path.Split('\\').Last());
            foreach (string file in Directory.GetFiles(path)) {
                ResetColor(); Console.WriteLine(new string(' ',(file.Replace(Directory.GetCurrentDirectory(),"").Split('\\').Length - 2) * 4) + (IsLastElem(file) && !ParentHasDirs(file) ? "└── " : "├── ") + file.Split('\\').Last());
                fileCount++;
            }
            foreach (string subdir in Directory.GetDirectories(path)) {
                WriteSubElems(subdir);
            }
        }
        static bool IsLastElem(string path) => Directory.GetFileSystemEntries(Directory.GetParent(path).ToString()).ToList().Last().Split('\\').Last() == path.Split('\\').Last();
        static bool IsLastDir(string path) => Directory.GetDirectories(Directory.GetParent(path).ToString()).ToList().Last().Split('\\').Last() == path.Split('\\').Last();
        static bool ParentHasDirs(string path) => Directory.GetDirectories(Directory.GetParent(path).ToString()).Count()>0;
        static void SetBlue() => Console.ForegroundColor = ConsoleColor.Blue;
        static void ResetColor() => Console.ForegroundColor = ConsoleColor.Gray;
    }
}
