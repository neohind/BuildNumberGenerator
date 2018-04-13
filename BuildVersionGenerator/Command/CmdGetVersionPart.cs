using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildVersionGenerator.Command
{
    public class CmdGetVersionPart : CmdBase
    {
        string m_sTargetProjectName = string.Empty;
        string m_sResultLevel = "rv";

        public override bool ParseArguments(List<string> args)
        {
            base.ParseArguments(args);
            if (args.Count > 3)
            {
                m_sTargetProjectName = args[2];
                m_sResultLevel = args[3];
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            base.Execute();

            string sTargetPath = Path.Combine(m_sSolutionPath, m_sTargetProjectName);
            List<FileInfo> aryAllAssemblyInfoFiles = FindAllFiles(sTargetPath, "AssemblyInfo.cs");

            //Console.WriteLine(sTargetPath);
            //Console.WriteLine(aryAllAssemblyInfoFiles.Count);
            foreach (FileInfo file in aryAllAssemblyInfoFiles)
            {
                string sAllContents = string.Empty;
                try
                {

                    using (StreamReader reader = File.OpenText(file.FullName))
                    {
                        string sVersion = string.Empty;
                        sAllContents = reader.ReadToEnd();
                        Match m = m_regexAssemblyVersion.Match(sAllContents);
                        if (m.Success)
                        {
                            m_nVerMajor = Convert.ToInt32(m.Groups["mj"].Value);
                            m_nVerMinor = Convert.ToInt32(m.Groups["mn"].Value);
                            if (m.Groups["bv"].Value != "*")
                                m_nVerBuild = Convert.ToInt32(m.Groups["bv"].Value);
                            if (m.Groups["rv"].Value != "*")
                                m_nVerRevision = Convert.ToInt32(m.Groups["rv"].Value);



                            switch (m_sResultLevel)
                            {
                                default:
                                case "rv":
                                    sVersion = Convert.ToString(m_nVerRevision);
                                    break;
                                case "bv":
                                    sVersion = Convert.ToString(m_nVerBuild);
                                    break;
                                case "mn":
                                    sVersion = Convert.ToString(m_nVerMinor);
                                    break;
                                case "mj":
                                    sVersion = Convert.ToString(m_nVerMajor);
                                    break;
                            }
                            Console.WriteLine(sVersion);
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
