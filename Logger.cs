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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MCForge
{
    public static class Logger
    {
        public static void Write(string str) //Kept for backwards compatibility
        {
            PidgeonLogger.LogMessage(str);
        }
        public static void WriteError(Exception ex)
        {
            PidgeonLogger.LogError(ex);
        }
        public static string LogPath { get { return PidgeonLogger.MessageLogPath; } set { PidgeonLogger.MessageLogPath = value; } }
        public static string ErrorLogPath { get { return PidgeonLogger.ErrorLogPath; } set { PidgeonLogger.ErrorLogPath = value; } }

        //Everything is static..!
        public static void Dispose()
        {
            PidgeonLogger.Dispose();
        }

    }
    /// <summary>
    /// Temporary class, will replace Logger completely once satisfied
    /// </summary>
    static class PidgeonLogger
    {
        //TODO: Implement report back feature

        static Boolean NeedRestart = false;
        static System.Timers.Timer RestartTimer = new System.Timers.Timer(30000);

        static bool _disposed;
        static bool _reportBack = false;
        static string _messagePath = "logs/" + DateTime.Now.ToString("yyyy-MM-dd").Replace("/", "-") + ".txt";
        static string _errorPath = "logs/errors/" + DateTime.Now.ToString("yyyy-MM-dd").Replace("/", "-") + "error.log";

        static object _lockObject = new object();
        static object _fileLockObject = new object();
        static Thread _workingThread;
        static Queue<string> _messageCache = new Queue<string>();
        static Queue<string> _errorCache = new Queue<string>(); //always handle this first!

        static public void Init()
        {
            _reportBack = Server.reportBack;
            //Should be done as part of the config
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            if (!Directory.Exists("logs/errors")) Directory.CreateDirectory("logs/errors");

            _workingThread = new Thread(new ThreadStart(WorkerThread));
            _workingThread.IsBackground = true;
            _workingThread.Start();
        }

        public static string MessageLogPath
        {
            get { return _messagePath; }
            set { _messagePath = value; }
        }
        public static string ErrorLogPath
        {
            get { return _errorPath; }
            set { _errorPath = value; }
        }

        public static void LogMessage(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                    lock (_lockObject)
                    {
                        _messageCache.Enqueue(message);
                        Monitor.Pulse(_lockObject);
                    }
            }
            catch
            {

            }
            //Should it error or passed null or zero string?
        }
        public static void LogError(Exception ex)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Exception e = ex;

                sb.AppendLine("----" + DateTime.Now + " ----");
                while (e != null)
                {
                    sb.AppendLine(getErrorText(e));
                    e = e.InnerException;
                }

                sb.AppendLine(new string('-', 25));

                if(Server.s != null)
                    Server.s.ErrorCase(sb.ToString());

                lock (_lockObject)
                {
                    _errorCache.Enqueue(sb.ToString());
                    Monitor.Pulse(_lockObject);
                }



                if (NeedRestart)
                {
                    Server.listen.Close();
                    Server.Setup();
                    //http://alltheragefaces.com/img/faces/large/misc-jackie-chan-l.png

                    NeedRestart = false;
                }
            } catch (Exception e) {
                try
                {
                    File.AppendAllText("ErrorLogError.log", getErrorText(e));
                }
                catch (Exception _ex)
                {
                    MessageBox.Show("ErrorLogError Error:\n Could not log the error logs error. This is a big error. \n" + _ex.Message);
                }
            }
        }


        static void WorkerThread()
        {
            while (!_disposed)
            {
                lock (_lockObject)
                {
                    if (_errorCache.Count > 0)
                        FlushCache(_errorPath, _errorCache);

                    if (_messageCache.Count > 0)
                        FlushCache(_messagePath, _messageCache);
                    //Monitor.Wait(_lockObject, 500);
                }
                Thread.Sleep(500);
            }
        }

        //Only call from within synchronised code or all hell will break loose
        static void FlushCache(string path, Queue<string> cache)
        {
            // Extra layer of protection
            lock (_fileLockObject)
            {
                FileStream fs = null;
                try
                {
                    //TODO: not happy about constantly opening and closing a stream like this but I suppose its ok (Pidgeon)
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    while (cache.Count > 0)
                    {
                        byte[] tmp = Encoding.Default.GetBytes(cache.Dequeue());
                        fs.Write(tmp, 0, tmp.Length);
                    }
                    fs.Close();
                }
                finally
                {
                    fs.Dispose();
                }
            }
        }
        static string getErrorText(Exception e)
        {
            if(e == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();

            // Attempt to gather this info.  Skip anything that you can't read for whatever reason
            try { sb.AppendLine("Type: " + e.GetType().Name); } catch { }
            try { sb.AppendLine("Source: " + e.Source); } catch { }
            try { sb.AppendLine("Message: " + e.Message); } catch { }
            try { sb.AppendLine("Target: " + e.TargetSite.Name); } catch { }
            try { sb.AppendLine("Trace: " + e.StackTrace); } catch { }

            if (e.Message != null && e.Message.IndexOf("An existing connection was forcibly closed by the remote host") != -1)
            {
                NeedRestart = true;
            }

            return sb.ToString();
        }

        #region IDisposable Members

        public static void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            lock (_lockObject)
            {
                if (_errorCache.Count > 0)
                {
                    FlushCache(_errorPath, _errorCache);
                }

                _messageCache.Clear();
                Monitor.Pulse(_lockObject);
            }
        }

        #endregion
    }
}
