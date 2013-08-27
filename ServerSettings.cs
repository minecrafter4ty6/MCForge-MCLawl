/*
Copyright (C) 2010-2013 David Mitchell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Net;

namespace MCForge {
	//derp idk just need to edit this so I can commit :/
	public static class ServerSettings {
        /// <summary>
        /// The url MCForge downloads additional URL's from
        /// </summary>
		public const string UrlsUrl = "http://server.mcforge.net/urls.txt";

		private static string _RevisionList = "http://www.mcforge.net/revs.txt";
		private static string _HeartbeatAnnounce = "http://server.mcforge.net/hbannounce.php";
		private static string _ArchivePath = "http://www.mcforge.net/archives/exe/";

		static ServerSettings() {
			using ( var client = new WebClient() ) {
				client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
				client.DownloadStringAsync(new Uri(UrlsUrl));
			}
		}

		static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
			if ( e.Cancelled || e.Error != null ) {
				Server.s.Log("Error getting urls. Using defaults.");
				return;
			}

			if ( e.Result.Split('@').Length != 3 ) {
				Server.s.Log("Recieved Malformed data from server...");
				return;
			}

			string[] lines = e.Result.Split('@');

			_RevisionList = lines[0];
			_HeartbeatAnnounce = lines[1];
			_ArchivePath = lines[2];
		}
        /// <summary>
        /// returns the MCForge archives url
        /// </summary>
		public static string ArchivePath {
			get {
				return _ArchivePath;
			}
		}
        /// <summary>
        /// returns the MCForge heartbeat announce URL
        /// </summary>
		public static string HeartbeatAnnounce {
			get {
				return _HeartbeatAnnounce;
			}
		}
        /// <summary>
        ///  returns the MCForge revision list URL
        /// </summary>
		public static string RevisionList {
			get {
				return _RevisionList;
			}
		}
	}
}
