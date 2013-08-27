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
using System.IO;
using System.Reflection;
using System.Threading;

namespace MCForge
{
    /// <summary>
    /// This is the same as a normal plugin, except it contains less info.
    /// </summary>
    public abstract class Plugin_Simple
    {
        #region Abstract
        /// <summary>
        /// Use this to load all your events and everything you need.
        /// </summary>
        /// <param name="startup">True if this was used from the server startup and not loaded from the command.</param>
        public abstract void Load(bool startup);
        /// <summary>
        /// Use this method to dispose of everything you used.
        /// </summary>
        /// <param name="shutdown">True if this was used by the server shutting down and not a command.</param>
        public abstract void Unload(bool shutdown);
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public abstract string name { get; }
        /// <summary>
        /// The creator/author of this plugin. (Your name)
        /// </summary>
        public abstract string creator { get; }
        /// <summary>
        /// Oldest version of MCForge the plugin is compatible with.
        /// </summary>
        public abstract string MCForge_Version { get; }
        #endregion

        #region Loading
        /// <summary>
        /// Load a simple plugin
        /// </summary>
        /// <param name="pluginname">The filepath to load</param>
        /// <param name="startup">Whether the server is starting up or not</param>
        /// <returns>Whether the plugin loaded or not</returns>
        public static bool Load(string pluginname, bool startup)
        {
            String creator = "";
            object instance = null;
            Assembly lib = null;
            using (FileStream fs = File.Open(pluginname, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int read = 0;
                    while ((read = fs.Read(buffer, 0, 1024)) > 0)
                        ms.Write(buffer, 0, read);
                    lib = Assembly.Load(ms.ToArray());
                    ms.Close();
                    ms.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }
            try
            {
                foreach (Type t in lib.GetTypes())
                {
                    if (t.BaseType == typeof(Plugin_Simple))
                    {
                        instance = Activator.CreateInstance(lib.GetTypes()[0]);
                        break;
                    }
                }
            }
            catch { }
            if (instance == null)
            {
                Server.s.Log("The plugin " + pluginname + " couldn't be loaded!");
                return false;
            }
            String plugin_version = ((Plugin_Simple)instance).MCForge_Version;
            if (plugin_version != "" && new Version(plugin_version) != Server.Version)
            {
                Server.s.Log("This plugin (" + ((Plugin_Simple)instance).name + ") isn't compatible with this version of MCForge!");
                Thread.Sleep(1000);
                if (Server.unsafe_plugin)
                {
                    Server.s.Log("Will attempt to load!");
                    goto here;
                }
                else
                {
                    return false;
                }
            }
            here:
            Plugin.all_simple.Add((Plugin_Simple)instance);
            creator = ((Plugin_Simple)instance).creator;
            ((Plugin_Simple)instance).Load(startup);
            Server.s.Log("Plugin: " + ((Plugin_Simple)instance).name + " loaded...");
            Thread.Sleep(1000);
            return true;
        }
        #endregion
    }
}
