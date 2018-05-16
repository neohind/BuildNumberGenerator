
using BuildVersionGenerator.Command;
using System;
using System.Collections.Generic;

namespace BuildVersionGenerator
{
    class Program
    {
        static void Main(string[]args)
        {
            List<string> aryArgs = ParseArgs();


            //foreach (string s in aryArgs)
            //    Console.WriteLine(s);
            //args = new string[] { "all", @"d:\Works\90.PrivateProjects\GetFileVersion\", "rv=15" };
            //aryArgs.Clear();
            //aryArgs.Add("get");
            //aryArgs.Add(@"d:\Works\20.UMMC\Client\");
            //aryArgs.Add("UMMs");
            //aryArgs.Add("mj");

            bool bCheckArguments = aryArgs.Count > 1;

            if (bCheckArguments)
            {

                string sCommand = aryArgs[0];
                CmdBase cmd = CmdBase.CreateInstance(sCommand);
                if (cmd != null)
                {
                    bCheckArguments = cmd.ParseArguments(aryArgs);
                    if (bCheckArguments)
                    {
                        cmd.Execute();
                    }
                }
            }

            if (bCheckArguments == false)
            {
                Console.WriteLine("Not completed enter arguments!");
                Console.WriteLine("Arguments");
                Console.WriteLine("   Type#1 :  all {solution Path} [mj={n}][mn={n}][bv={n}][rv={n}]");
                Console.WriteLine("   Type#2 :  project {solution Path} {project name} [mj={n}][mn={n}][bv={n}][rv={n}]");
                Console.WriteLine("   Type#3 :  get {solution Path} {project name} [mj|mn|bv|rv]");
                Console.WriteLine("   Type#4 :  getpart {solution Path} {project name} [mj|mn|bv|rv]");
                Console.WriteLine();
                Console.WriteLine("   all : Update all assembly, fileversion update and publish version");
                Console.WriteLine("   project : Update specific project settings that is assembly, fileversion update and publish version.");
                Console.WriteLine("   get : Get version by level. ex) 1.2.3.4 :  mj -> 1. / mn -> 1.2. / bv -> 1.2.3. / rv -> 1.2.3.4");
                Console.WriteLine("   getpart : Get part version by level. ex) 1.2.3.4 :  mj -> 1 / mn -> 2 / bv -> 3 / rv -> 4");
                Console.WriteLine("   - {solution Path} : Solution folder full path name");
                Console.WriteLine("   - {Project Name} : Project Name. .csproj Filename");
                Console.WriteLine("   - mj : Major Version");
                Console.WriteLine("   - mn : Minor Version");
                Console.WriteLine("   - bv : Build Version");
                Console.WriteLine("   - rv : Revision Version");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Type#1 : BuildVersionGenerator.exe all \"C:\\Projects\\TestProj\" mj=1 mn=0 bv=16 rv =1");
                Console.WriteLine("Type#2 : BuildVersionGenerator.exe project \"C:\\Projects\\TestProj\" mainapp mj=1 mn=0 bv=16 rv=1");
                Console.WriteLine("Type#2 : BuildVersionGenerator.exe get \"C:\\Projects\\TestProj\" mainapp bv");
                Console.WriteLine("Type#2 : BuildVersionGenerator.exe getpart \"C:\\Projects\\TestProj\" Test  mainapp mj");
                
            }
        }

        static List<string> ParseArgs()
        {
            List<string> aryArgs = new List<string>();

            int nIndex = 0;
            string sArg = Environment.CommandLine;
            nIndex = sArg.IndexOf(".exe") + 4;
            if (sArg.Length > nIndex && sArg[nIndex] == '\"')
                nIndex = nIndex + 1;
            sArg = sArg.Substring(nIndex);
            sArg = sArg.Trim();


            string sCurArg = string.Empty;
            bool bOpen = false;
            for (int i = 0; i < sArg.Length; i++)
            {
                char ch = sArg[i];
                if (ch == '\"')
                {
                    bOpen = !bOpen;
                }

                if (ch == ' ' && bOpen == false)
                {
                    sCurArg = sCurArg.Trim();
                    if (string.IsNullOrEmpty(sCurArg) == false)
                        aryArgs.Add(sCurArg);
                    sCurArg = string.Empty;
                }
                else if (ch == '\"')
                    continue;
                sCurArg += ch;
            }

            sCurArg = sCurArg.Trim();
            if (string.IsNullOrEmpty(sCurArg) == false)
                aryArgs.Add(sCurArg);

            return aryArgs;
        }
    }
}
