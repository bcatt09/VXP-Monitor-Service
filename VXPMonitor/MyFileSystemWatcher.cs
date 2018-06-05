using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXPMonitor
{
    public class MyFileSystemWatcher : FileSystemWatcher
    {
		string moveToFolder = @"C:\Gating";
		//string moveToFolder = @"C:\Users\bcatt\Desktop\MonitorTest\LogTest";
		string sourceVRT;
		string sourceRGSC;
		string target;

		public MyFileSystemWatcher()
        {
            Init();
        }

        public MyFileSystemWatcher(String inDirectoryPath)
            : base(inDirectoryPath)
        {
            Init();
        }

        public MyFileSystemWatcher(String inDirectoryPath, string inFilter)
            : base(inDirectoryPath, inFilter)
        {
            Init();
        }

        private void Init()
        {
            IncludeSubdirectories = false;
            // Eliminate duplicates when timestamp doesn't change
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size; // The default also has NotifyFilters.LastWrite
            EnableRaisingEvents = true;
            Created += Watcher_Created;

			//grab config settings for current version numbers
			sourceVRT = System.Configuration.ConfigurationManager.AppSettings["sourceVRT"];
			sourceRGSC = System.Configuration.ConfigurationManager.AppSettings["sourceRGSC"];
			target = System.Configuration.ConfigurationManager.AppSettings["target"];
		}

        public void Watcher_Created(object source, FileSystemEventArgs inArgs)
        {
			Log.WriteLine(System.DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")) + " - Added to folder: " + inArgs.Name);
			if(System.IO.Path.GetExtension(inArgs.FullPath) == ".vxp")
			{
				string str = File.ReadAllText(inArgs.FullPath);
				//convert all VRT version numbers to RGSC version and then RGSC versions to Philips CT version so that either VRT or RGSC will get converted
				str = str.Replace("Version=" + sourceVRT, "Version=" + sourceRGSC);
				str = str.Replace("Version=" + sourceRGSC, "Version=" + target);
				File.WriteAllText(inArgs.FullPath, str);
				Log.WriteLine(System.DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")) + " - File edited: " + inArgs.Name + ", SourceVRT=" + sourceVRT + ", SourceRGSC=" + sourceRGSC + ", Target=" + target);
				File.Move(inArgs.FullPath, System.IO.Path.Combine(moveToFolder, System.IO.Path.GetFileName(inArgs.FullPath)));
				Log.WriteLine(System.DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")) + " - File moved: " + inArgs.FullPath + " to " + System.IO.Path.Combine(moveToFolder, System.IO.Path.GetFileName(inArgs.FullPath)));
			}
        }
    }
}
