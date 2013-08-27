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
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace MCForge
{
    public sealed class LevelCollection : List<Level>, ITypedList
    {
        protected ILevelViewBuilder _viewBuilder;

        public LevelCollection(ILevelViewBuilder viewBuilder)
        {
            _viewBuilder = viewBuilder;
        }

        #region ITypedList Members

        protected PropertyDescriptorCollection _props;

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (_props == null)
            {
                _props = _viewBuilder.GetView();
            }
            return _props;
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return ""; // was used by 1.1 datagrid
        }

        #endregion
    }

    public interface ILevelViewBuilder
    {
        PropertyDescriptorCollection GetView();
    }

    public class LevelListView : ILevelViewBuilder
    {
        public PropertyDescriptorCollection GetView()
        {
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            LevelMethodDelegate del = l => l.name;
            props.Add(new LevelMethodDescriptor("Name", del, typeof(string)));

            del = l => l.players.Count;
            props.Add(new LevelMethodDescriptor("Players", del, typeof(int)));

            del = l => l.physics;
            props.Add(new LevelMethodDescriptor("Physics", del, typeof(int)));

            del = delegate(Level l)
            {
                //return l.permissionvisit.ToString();
                Group grp = Group.GroupList.Find(g => g.Permission == l.permissionvisit);
                return grp == null ? l.permissionvisit.ToString() : grp.name;
            };
            props.Add(new LevelMethodDescriptor("PerVisit", del, typeof(string)));

            del = delegate(Level l)
                      {
                          //return l.permissionbuild.ToString();
                Group grp = Group.GroupList.Find(g => g.Permission == l.permissionbuild);
                          return grp == null ? l.permissionbuild.ToString() : grp.name;
                      };
            props.Add(new LevelMethodDescriptor("PerBuild", del, typeof(string)));

            PropertyDescriptor[] propArray = new PropertyDescriptor[props.Count];
            props.CopyTo(propArray);
            return new PropertyDescriptorCollection(propArray);
        }
    }

    public class LevelListViewForTab : ILevelViewBuilder
    {
        public PropertyDescriptorCollection GetView()
        {
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            LevelMethodDelegate del = l => l.name;
            props.Add(new LevelMethodDescriptor("Name", del, typeof(string)));

            del = l => l.players.Count;
            props.Add(new LevelMethodDescriptor("Players", del, typeof(int)));

            del = l => l.physics;
            props.Add(new LevelMethodDescriptor("Physics", del, typeof(int)));

            del = l => l.motd;
            props.Add(new LevelMethodDescriptor("MOTD", del, typeof(string)));

            del = l => l.GrassGrow;
            props.Add(new LevelMethodDescriptor("Grass", del, typeof(bool)));

            del = l => l.Killer;
            props.Add(new LevelMethodDescriptor("Killer-Blocks", del, typeof(bool)));

            del = l => l.worldChat;
            props.Add(new LevelMethodDescriptor("World-Chat", del, typeof(bool)));

            del = l => l.Death;
            props.Add(new LevelMethodDescriptor("Death", del, typeof(bool)));

            del = l => l.finite;
            props.Add(new LevelMethodDescriptor("Finite", del, typeof(bool)));

            del = l => l.randomFlow;
            props.Add(new LevelMethodDescriptor("Random Flow", del, typeof(bool)));

            del = l => l.edgeWater;
            props.Add(new LevelMethodDescriptor("Edge-Water", del, typeof(bool)));

            del = l => l.ai ? "Hunt" : "Flee";
            props.Add(new LevelMethodDescriptor("AI", del, typeof(string)));

            del = l => l.guns;
            props.Add(new LevelMethodDescriptor("Guns", del, typeof(bool)));

            del = l => l.drown;
            props.Add(new LevelMethodDescriptor("Drown", del, typeof(int)));

            del = l => l.fall;
            props.Add(new LevelMethodDescriptor("Fall", del, typeof(int)));

            del = l => l.loadOnGoto;
            props.Add(new LevelMethodDescriptor("Load on /goto", del, typeof(bool)));

            del = l => l.unload;
            props.Add(new LevelMethodDescriptor("Unload Empty", del, typeof(bool)));

            del =
                l =>
                (File.Exists("text/autoload.txt") &&
                 (File.ReadAllLines("text/autoload.txt").Contains(l.name) ||
                  File.ReadAllLines("text/autoload.txt").Contains(l.name.ToLower())));
            props.Add(new LevelMethodDescriptor("Autoload", del, typeof(bool)));

            del = delegate(Level l)
                      {
                          //return l.permissionvisit.ToString();
                Group grp = Group.GroupList.Find(g => g.Permission == l.permissionvisit);
                          return grp == null ? l.permissionvisit.ToString() : grp.name;
                      };
            props.Add(new LevelMethodDescriptor("PerVisit", del, typeof(string)));

            del = delegate(Level l)
                      {
                          //return l.permissionbuild.ToString();
                Group grp = Group.GroupList.Find(g => g.Permission == l.permissionbuild);
                          return grp == null ? l.permissionbuild.ToString() : grp.name;
                      };
            props.Add(new LevelMethodDescriptor("PerBuild", del, typeof(string)));

            PropertyDescriptor[] propArray = new PropertyDescriptor[props.Count];
            props.CopyTo(propArray);
            return new PropertyDescriptorCollection(propArray);


        }
    }

    public delegate object LevelMethodDelegate(Level l);

    public class LevelMethodDescriptor : PropertyDescriptor
    {
        protected LevelMethodDelegate _method;
        protected Type _methodReturnType;

        public LevelMethodDescriptor(string name, LevelMethodDelegate method,
         Type methodReturnType)
            : base(name, null)
        {
            _method = method;
            _methodReturnType = methodReturnType;
        }

        public override object GetValue(object component)
        {
            Level l = (Level)component;
            return _method(l);
        }

        public override Type ComponentType
        {
            get { return typeof(Level); }
        }

        public override Type PropertyType
        {
            get { return _methodReturnType; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component) { }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override void SetValue(object component, object value) { }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
