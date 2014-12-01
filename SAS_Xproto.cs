using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;


namespace SAS
{

public static class SAS
{

static Process sortProcess = new Process();

static int ok = 0;
static int count = 0;
//static string code = null;
static string pass = null;
static string logi = null;
static string hostname = null;
static string ip = null;
static string cmdexec = null;
static string cmdargs = null;
static string cmdregex = null;
static string cmdwrite = null;

static string reg1 = null;
static string reg2 = null;

static StringBuilder sortOutput = null;
static StringBuilder sortError = null;
static int numOutputLines = 0;
//static Regex log = new Regex("\\blogin\\b|\\blogon\\b");
//static Regex pwd = new Regex("\\bpassword\\b|\\bpasswd\\b");

static string D ="@done";
static Regex exit = new Regex("(?ix)"+D);

static List<KeyValuePair<string, string>> cmdlst = new List<KeyValuePair<string, string>>();

	public static void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
	{

	Regex log = new Regex("(?ix)\\b"+ reg1 +"\\b");
	Regex pwd = new Regex("(?ix)\\b" + reg2 +"\\b");

		StreamWriter sortStreamWriter = sortProcess.StandardInput;
		sortStreamWriter.AutoFlush = true;

		if (numOutputLines > 99) {
			//sortOutput.Clear()
			sortOutput.Length = 0;
			sortError.Length = 0;
			numOutputLines = 1;
			Console.WriteLine(Environment.NewLine + "[[ BUFFER ROLL @99 ]]");
		}

		if (!string.IsNullOrEmpty(outLine.Data)) {
			numOutputLines += 1;
			Console.Write("[" + numOutputLines.ToString() + "] - " + outLine.Data + Environment.NewLine);

			if (log.IsMatch(outLine.Data)) {
				Console.Write("MATCH FOUND(LOG): " + outLine.Data + Environment.NewLine);
				sortStreamWriter.WriteLine(logi);
			}
			if (pwd.IsMatch(outLine.Data)) {
				Console.Write("MATCH FOUND(PWD): " + outLine.Data + Environment.NewLine);
				sortStreamWriter.WriteLine(pass);
			 ok += 1;
			Console.Write(ok +"\n");
			}

			if (exit.IsMatch(outLine.Data)) {
				Console.Write("MATCH FOUND(EXIT): " + outLine.Data + Environment.NewLine);
				//code = "done";
				count += 1;
			Console.Write(count +"\n");
			}

		}
	}



private static void CMD_XML(){

            XmlReader xmlReader = XmlReader.Create("cmd.xml");
            while(xmlReader.Read())
            {

                if((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "host"))
                {
                    if(xmlReader.HasAttributes)
                        Console.WriteLine(xmlReader.GetAttribute("name") + ": " + xmlReader.GetAttribute("ip")); 
                        hostname = xmlReader.GetAttribute("name");    
                        ip = xmlReader.GetAttribute("ip");
                }

                if((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "exec"))
                {
                    if(xmlReader.HasAttributes)
                        Console.WriteLine(xmlReader.GetAttribute("name") + ": " + xmlReader.GetAttribute("args"));        
                        cmdexec = xmlReader.GetAttribute("name");    
                        cmdargs = xmlReader.GetAttribute("args");
                }

                if((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cmd"))
                {
                    if(xmlReader.HasAttributes)
                        Console.WriteLine(xmlReader.GetAttribute("regex") + ": " + xmlReader.GetAttribute("write"));  
                        cmdregex = xmlReader.GetAttribute("regex");    
                        cmdwrite = xmlReader.GetAttribute("write");    
                        cmdlst.Add(new KeyValuePair<string, string>(cmdregex, cmdwrite));
			if (cmdregex == "login") {
				logi = cmdwrite;
                                reg1 = cmdregex;
			}
			if (cmdregex == "password") {
				pass = cmdwrite;
				reg2 = cmdregex;
			}
                }

            }
}


	public static void Main()
	{

		CMD_XML();
//Console.WriteLine("WAX ON; WAX OFF");

		//int numInputLines = 0;
		//string inputText = null;

                sortProcess.StartInfo.FileName = cmdexec;
		sortProcess.StartInfo.Arguments = cmdargs;
		//sortProcess.StartInfo.WorkingDirectory = "/tmp";
		sortProcess.StartInfo.UseShellExecute = false;
		sortProcess.StartInfo.RedirectStandardOutput = true;
		sortProcess.StartInfo.RedirectStandardInput = true;
		sortProcess.StartInfo.RedirectStandardError = true;

		sortOutput = new StringBuilder();
		sortError = new StringBuilder();

		sortProcess.OutputDataReceived += new DataReceivedEventHandler (SortOutputHandler);
		sortProcess.ErrorDataReceived += new DataReceivedEventHandler (SortOutputHandler);

		sortProcess.Start();

		StreamWriter sortStreamWriter = sortProcess.StandardInput;
		sortStreamWriter.AutoFlush = true;

		//ASYNC IO = BeginOutputReadLine
		sortProcess.BeginOutputReadLine();
		sortProcess.BeginErrorReadLine();
		sortStreamWriter.WriteLine("y \n");

		do {
			while (ok <= 2){
				if(ok == 2){
					foreach( KeyValuePair<string, string> kvp in cmdlst )
					{
						if ((kvp.Key != "password") && (kvp.Key != "login")){
    							sortStreamWriter.WriteLine(kvp.Value);
						ok += 1;
						}  
					}
				}


			}
		} while (count < 3);

		sortProcess.CancelErrorRead();
		sortProcess.CancelOutputRead();
		sortStreamWriter.Close();
                sortProcess.Dispose();
                sortProcess.Close();

	} 

   }


}


