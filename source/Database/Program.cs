using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Database
{
    class Program
    {
        static void Main()
        {
            // Change to your number of menuitems.
            const int maxMenuItems = 4;
            var selector = 0;
            while (selector != maxMenuItems)
            {
                Console.Clear();
                DrawMenu();
                bool good = int.TryParse(Console.ReadLine(), out selector);
                if (good)
                {
                    const string localdb = ".\\sqlexpress";
                    const string databaseName = "Demo";
                    var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
                    var parentDirectory = currentDirectory.Parent.Parent.FullName;
                    var scriptspath = parentDirectory + "\\scripts\\";
                    var deployerpath = parentDirectory + "\\databasedeployer\\databasedeployer.exe";
                    var p = new Process();

                    switch (selector)
                    {
                        case 1:
                        case 2:
                        case 3:
                            string cmdArguments = string.Format("{0} {1} {2} {3}", GetVerbForCase(selector), localdb, databaseName, scriptspath);
                            p.StartInfo.FileName = deployerpath;
                            p.StartInfo.Arguments = cmdArguments;
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.Start();
                            Console.WriteLine(p.StandardOutput.ReadToEnd());
                            Console.WriteLine("Press any key to continue.");
                            break;
                        default:
                            if (selector != maxMenuItems)
                            {
                                ErrorMessage();
                            }
                            break;
                    }
                }
                else
                {
                    ErrorMessage();
                }
                Console.ReadKey();
            }
        }
        private static void ErrorMessage()
        {
            Console.WriteLine("Typing error, press key to continue.");
        }

        private static void DrawMenu()
        {
            Console.WriteLine(" 1. Rebuild Database on SQLExpress");
            Console.WriteLine(" 2. Update Database on SQL Express");
            Console.WriteLine(" 3. Populate Seed Data on SQL Express");
            Console.WriteLine(" 4. Exit program");

        }

        /// <summary>
        ///returns project name and removes the word "database"
        /// </summary>
        /// <returns></returns>
        private static string GetProjectName()
        {
            string fullName = Assembly.GetEntryAssembly().Location;
            var projectname = Path.GetFileNameWithoutExtension(fullName);
            return projectname.Replace("Database", "");
        }

        private static string GetVerbForCase(int selector)
        {
            if (selector == 1) return "Rebuild";
            if (selector == 2) return "Update";
            if (selector == 3) return "Seed";
            throw new Exception("invalid selector");
        }
    }
}