using System.IO;
using System.ServiceProcess;

namespace VXPMonitor
{
    public partial class DirectoryMonitoringService : ServiceBase
    {
        protected FileSystemWatcher Watcher;

		// Monitor folder
		string watchFolder = @"C:\GatingTemp";
		//string watchFolder = @"C:\Users\bcatt\Desktop\MonitorTest";


		public DirectoryMonitoringService()
		{
			Log.Instance.LogPath = @"C:\Users\VRT\Desktop\VXPMonitor";
			//Log.Instance.LogPath = @"C: \Users\bcatt\Desktop\MonitorTest\LogTest";

			Log.Instance.LogFileName = "VXPMonitor";
			//delete old log file from yesterday
			File.WriteAllText(Log.Instance.LogFullPath,System.DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")) + " - Service Started\r\n");
			Watcher = new MyFileSystemWatcher(watchFolder);
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
