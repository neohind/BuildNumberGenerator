using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildVersionGenerator
{
    class Program
    {
        static string g_sPattern = "\\[assembly: AssemblyFileVersion\\(\"(?<mj>[0-9]+)\\.(?<mn>[0-9]+)\\.(?<rv>[0-9*]+)[\\.]*(?<bd>[0-9]*)\"\\)\\]";
        static Regex g_regex = null;
        static string g_sBuildVersion = string.Empty;
        static int g_nMajor = -1;
        static int g_nMinor = -1;
        static int g_nRevision = -1;

        static Regex g_regexArgs = new Regex("(?<name>[A-Za-z]+)\\=(?<value>[0-9]+)");

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not completed enter arguments!");
                Console.WriteLine("Arg#1 Workspace Path, arg#2 BuildVersion");
                return;
            }

            int nBuildVersion = 0;
            string sWorkspacePath = args[0];
            g_sBuildVersion = args[1];

            foreach(string arg in args)
            {
                Match match = g_regexArgs.Match(arg);
                if(match.Success)
                {                    
                    switch(match.Groups["name"].Value)
                    {
                        case "mj":
                            g_nMajor = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        case "mn":
                            g_nMinor = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        case "rv":
                            g_nRevision = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                    }
                }
            }


            if (string.IsNullOrEmpty(g_sBuildVersion))
            {
                Console.WriteLine("Invaild Build Version - empty");                
                return;
            }

            if (int.TryParse(g_sBuildVersion, out nBuildVersion) == false)
            {
                Console.WriteLine("Invaild Build Version - not integer value");
                return;
            }

            List<FileInfo> aryAllAssemblyFiles = FindAllFiles(sWorkspacePath);
            g_regex = new Regex(g_sPattern);
            
            foreach(FileInfo file in aryAllAssemblyFiles)
            {
                string sAllContents = string.Empty;
                try
                {
                    using (StreamReader reader = File.OpenText(file.FullName))
                    {
                        sAllContents = reader.ReadToEnd();
                        if (g_regex.IsMatch(sAllContents))
                        {
                            sAllContents = g_regex.Replace(sAllContents, new MatchEvaluator(matchEval));
                        }
                        else
                        {
                            sAllContents = string.Empty;
                        }
                    }                 
                }
                catch
                {
                }

                if(string.IsNullOrEmpty(sAllContents) == false)
                {
                    try
                    {
                        file.Delete();
                        using (StreamWriter writer = File.CreateText(file.FullName))
                        {
                            writer.Write(sAllContents);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        static string matchEval(Match m)
        {
            string sMajor = m.Groups["mj"].Value;
            string sMinor = m.Groups["mn"].Value;
            string sRevision = m.Groups["rv"].Value;

            if (g_nMajor > 0)
                sMajor = g_nMajor.ToString();

            if (g_nMinor > 0)
                sMinor = g_nMinor.ToString();

            if (g_nRevision > 0)
                sRevision = g_nRevision.ToString();

            string sNewText = string.Format("[assembly: AssemblyFileVersion(\"{0}.{1}.{2}.{3}\")]"
                , sMajor, sMinor, sRevision, g_sBuildVersion);

            return sNewText;
        }


        static List<FileInfo> FindAllFiles(string sPath)
        {
            List<FileInfo> aryResult = new List<FileInfo>();
            DirectoryInfo curDir = new DirectoryInfo(sPath);
            FindAllFiles(curDir, ref aryResult);
            return aryResult;
        }

        static void FindAllFiles(DirectoryInfo curDir, ref List<FileInfo> aryResults)
        {
            FileInfo [] findFiles = curDir.GetFiles("AssemblyInfo.cs");
            aryResults.AddRange(findFiles);

            foreach(DirectoryInfo childDir in curDir.GetDirectories())
            {
                FindAllFiles(childDir, ref aryResults);
            }
        }
    }
}
