using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildVersionGenerator.Command
{
    public class CmdBase
    {
        public static CmdBase CreateInstance(string sCommand)
        {
            if (string.IsNullOrEmpty(sCommand))
                return null;

            sCommand = sCommand.ToLower();
            switch (sCommand)
            {
                case "all":
                default:
                    return new CmdAllChange();
                case "get":
                    return new CmdGetVersion();
                case "getpart":
                    return new CmdGetVersionPart();
            }
        }

        static protected Regex g_regexArgs = new Regex("(?<name>[A-Za-z]+)\\=(?<value>[0-9]+)");
        static protected Regex m_regexAssemblyVersion = new Regex("\\[assembly:[ ]*(?<nm>AssemblyVersion)\\(\"(?<mj>[0-9]+)\\.(?<mn>[0-9]+)\\.(?<bv>[0-9]+)[\\.]*(?<rv>[0-9*]*)\"\\)\\]");
        static protected Regex m_regexFileVersion = new Regex("\\[assembly:[ ]*(?<nm>AssemblyFileVersion)\\(\"(?<mj>[0-9]+)\\.(?<mn>[0-9]+)\\.(?<bv>[0-9]+)[\\.]*(?<rv>[0-9*]*)\"\\)\\]");

        protected int m_nVerMajor = 1;
        protected int m_nVerMinor = 0;
        protected int m_nVerRevision = 0;
        protected int m_nVerBuild = 0;
        protected string m_sSolutionPath = string.Empty;

        public void SetVersion(int nMajor, int nMinor, int nRevision, int nBuild)
        {
            m_nVerMajor = (nMajor > 0) ? nMajor : 1;
            m_nVerMinor = (nMinor > 0) ? nMinor : 0;
            m_nVerRevision = (nRevision > 0) ? nRevision : 0;
            m_nVerBuild = (nBuild > 0) ? nBuild : 0;
        }

        virtual public bool ParseArguments(List<string> args)
        {
            foreach (string arg in args)
            {
                Match match = g_regexArgs.Match(arg);
                if (match.Success)
                {
                    switch (match.Groups["name"].Value)
                    {
                        case "mj":
                            m_nVerMajor = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        case "mn":
                            m_nVerMinor = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        case "bv":
                            m_nVerBuild = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        case "rv":
                            m_nVerRevision = Convert.ToInt32(match.Groups["value"].Value);
                            break;
                        
                    }
                }
            }
            m_sSolutionPath = args[1];

            return false;
        }

        virtual public void Execute()
        {

        }

        protected List<FileInfo> FindAllFiles(string sPath, string sFindFilename)
        {
            List<FileInfo> aryResult = new List<FileInfo>();
            DirectoryInfo curDir = new DirectoryInfo(sPath);
            FindAllFiles(curDir, sFindFilename, ref aryResult);
            return aryResult;
        }

        protected void FindAllFiles(DirectoryInfo curDir, string sFindFilename, ref List<FileInfo> aryResults)
        {
            //"AssemblyInfo.cs"
            if (curDir.Exists)
            {
                
                FileInfo[] findFiles = curDir.GetFiles(sFindFilename);
                aryResults.AddRange(findFiles);
                foreach (DirectoryInfo childDir in curDir.GetDirectories())
                {
                    FindAllFiles(childDir, sFindFilename, ref aryResults);
                }
            }
        }

    }
}
