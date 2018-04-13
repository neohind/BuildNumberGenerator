using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BuildVersionGenerator.Command
{
    public class CmdProjectange : CmdBase
    {
        string m_sTargetProjectName = string.Empty;

        public override bool ParseArguments(List<string> args)
        {
            base.ParseArguments(args);

            if (Directory.Exists(m_sSolutionPath) == false)
                return false;

            if (args.Count > 3)
            {
                m_sTargetProjectName = args[2];
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();
            string sPath = Path.Combine(m_sSolutionPath, m_sTargetProjectName);
            List<FileInfo> aryAllAssemblyInfoFiles = FindAllFiles(sPath, "AssemblyInfo.cs");
            List<FileInfo> aryAllProjectFiles = FindAllFiles(sPath, "*.csproj");

            foreach (FileInfo infoAssemblyInfo in aryAllAssemblyInfoFiles)
            {
                string sAllContents = string.Empty;
                try
                {
                    using (StreamReader reader = File.OpenText(infoAssemblyInfo.FullName))
                    {
                        sAllContents = reader.ReadToEnd();
                        if (m_regexFileVersion.IsMatch(sAllContents))
                            sAllContents = m_regexFileVersion.Replace(sAllContents, new MatchEvaluator(matchEval));

                        if (m_regexAssemblyVersion.IsMatch(sAllContents))
                            sAllContents = m_regexAssemblyVersion.Replace(sAllContents, new MatchEvaluator(matchEval));
                        else
                            sAllContents = string.Empty;


                    }
                    if (string.IsNullOrEmpty(sAllContents) == false)
                    {
                        infoAssemblyInfo.Delete();
                        using (StreamWriter writer = File.CreateText(infoAssemblyInfo.FullName))
                            writer.Write(sAllContents);
                    }
                }
                catch
                {
                }
            }

            foreach (FileInfo infoAssemblyInfo in aryAllProjectFiles)
            {
                string sAllContents = string.Empty;
                try
                {
                    XmlDocument doc = new XmlDocument();
                    using (StreamReader reader = File.OpenText(infoAssemblyInfo.FullName))
                    {
                        sAllContents = reader.ReadToEnd();
                        doc.LoadXml(sAllContents);
                        string sUrl = doc.DocumentElement.NamespaceURI;
                        XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
                        mgr.AddNamespace("x", sUrl);

                        XmlNode node = doc.SelectSingleNode("//x:ApplicationVersion", mgr);
                        if (node != null)
                        {
                            node.InnerText = string.Format("{0}.{1}.{2}.{3}", m_nVerMajor, m_nVerMinor, m_nVerBuild, m_nVerRevision);
                        }


                        node = doc.SelectSingleNode("//x:ApplicationRevision", mgr);
                        if (node != null)
                            node.InnerText = Convert.ToString(m_nVerRevision);
                    }
                    infoAssemblyInfo.Delete();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    using (StreamWriter writer = File.CreateText(infoAssemblyInfo.FullName))
                    using (XmlWriter writerXml = XmlWriter.Create(writer, settings))
                        doc.WriteTo(writerXml);
                }
                catch
                {
                }
            }

        }

        string matchEval(Match m)
        {
            string sName = m.Groups["nm"].Value;
            string sNewText = string.Format("[assembly: {4}(\"{0}.{1}.{2}.{3}\")]"
                , m_nVerMajor, m_nVerMinor, m_nVerBuild, m_nVerRevision, sName);

            return sNewText;
        }
    }
}
