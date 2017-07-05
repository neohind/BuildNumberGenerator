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
#if DEBUG  
            //args = new string[] { @"d:\Works\90.PrivateProjects\WindowsFormsApplication6\", "5", "mj=2", "mn=5"};
            args = new string[] { @"d:\Works\90.PrivateProjects\Lotto\", "getver" };

#endif




            if (args.Length < 2)
            {
                Console.WriteLine("Not completed enter arguments!");
                Console.WriteLine("Arg#1 Workspace Path, arg#2 BuildVersion");
                return;
            }

            int nBuildVersion = 0;
            string sWorkspacePath = args[0];
            g_sBuildVersion = args[1];

            foreach (string arg in args)
            {
                Match match = g_regexArgs.Match(arg);
                if (match.Success)
                {
                    switch (match.Groups["name"].Value)
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


            List<FileInfo> aryAllAssemblyFiles = FindAllFiles(sWorkspacePath);
            g_regex = new Regex(g_sPattern);

            if ("getver".Equals(g_sBuildVersion))
            {
                string sLastedVersion = GetLastedVersion(aryAllAssemblyFiles);
                Console.WriteLine(sLastedVersion);
            }
            else if (int.TryParse(g_sBuildVersion, out nBuildVersion))
            {
                RewriteVersions(aryAllAssemblyFiles);                
            }
            else
            {
                Console.WriteLine("Invaild Build Version - not integer value");                
            }
        }

        static string GetLastedVersion(List<FileInfo> aryAllAssemblyFiles)
        {
            Version lastedVersion = new Version(0, 0, 0, 0);

            foreach (FileInfo file in aryAllAssemblyFiles)
            {
                string sAllContents = string.Empty;
                try
                {
                    using (StreamReader reader = File.OpenText(file.FullName))
                    {
                        sAllContents = reader.ReadToEnd();
                        Match m = g_regex.Match(sAllContents);
                        if (m.Success)
                        {
                            string sMajor = m.Groups["mj"].Value;
                            string sMinor = m.Groups["mn"].Value;
                            string sRevision = m.Groups["rv"].Value;
                            string sBuild = m.Groups["bd"].Value;

                            Version version = new Version(Convert.ToInt32(sMajor), Convert.ToInt32(sMinor), Convert.ToInt32(sRevision), Convert.ToInt32(sBuild));
                            if(version > lastedVersion)                            
                                lastedVersion = version;
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
            }

            return lastedVersion.ToString();
        }



        static void RewriteVersions(List<FileInfo> aryAllAssemblyFiles)
        {
            foreach (FileInfo file in aryAllAssemblyFiles)
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

                if (string.IsNullOrEmpty(sAllContents) == false)
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
            if (curDir.Exists)
            {
                FileInfo[] findFiles = curDir.GetFiles("AssemblyInfo.cs");
                aryResults.AddRange(findFiles);

                foreach (DirectoryInfo childDir in curDir.GetDirectories())
                {
                    FindAllFiles(childDir, ref aryResults);
                }
            }
        }
    }
}
