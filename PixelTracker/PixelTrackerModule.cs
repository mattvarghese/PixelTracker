using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PixelTracker
{
	public class PixelTrackerModule : IHttpModule
	{
		public void Dispose() { }



		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(PixelTrackEvent);
		}



		public void PixelTrackEvent(Object source, EventArgs e)
		{
			try
			{
				HttpApplication app = source as HttpApplication;
				// Add cache control headers so that the pixel does not get cached.
				app.Context.Response.Headers.Add("Cache-Control", "no-store");
				app.Context.Response.Headers.Add("Cache-Control", "max-age=0");
				app.Context.Response.Headers.Add("Cache-Control", "no-cache");
				if (app.Context.Request.QueryString.AllKeys.Contains("param"))
				{
					List<string> paramsToSkip = Properties.Settings.Default.SkipParams.Split(',').ToList();
					string param = app.Context.Request.QueryString["param"];
					// Fist check if it is a param which we've excluded
					if (!(paramsToSkip.Contains(param)))
					{
						// Set a max limit on the file to prevent Denial of Service attacks.
						FileInfo oFileInfo = new FileInfo(Properties.Settings.Default.TrackerLogFile);
						if (oFileInfo.Length < (Properties.Settings.Default.FileSizeLimitMB * 1024 * 1024))
						{
							// Write the log entry into a Comma Separated file
							// Format:  Timestamp, param, Referring URL, Hostname
							using (StreamWriter oStreamWriter = File.AppendText(Properties.Settings.Default.TrackerLogFile))
							{
								oStreamWriter.WriteLine(string.Format("{0}, {1}, {2}, {3}",
														DateTime.Now.ToString(),
														param,
														app.Context.Request.UrlReferrer,
														DetermineCompName(app.Context.Request.UserHostName)));
							}
						}
					}
				}
			}
			catch { /* Oh well ! */ }
		}



		private static string DetermineCompName(string ipAddress)
		{
			// This doesn't always work - especially if the client is behind a NAT
			IPAddress oIPAddress = IPAddress.Parse(ipAddress);
			IPHostEntry oHostEntry = Dns.GetHostEntry(oIPAddress);
			return oHostEntry.HostName.ToString();
		}
	}
}
