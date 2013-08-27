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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCForge.Util {

    public sealed class MCForgeScripter {

        private static readonly CompilerParameters _settings = new CompilerParameters(new [] {"mscorlib.dll", "MCForge_.dll", "MCForge.exe"}) {
            GenerateInMemory = true
        };

        /// <summary>
        /// Compiles the specified source code.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="language">The language.</param>
        /// <returns>A result from the compilation</returns>
        public static CompileResult Compile ( string text, ScriptLanguage language ) {
            CodeDomProvider provider = null;

            switch ( language ) {
                case ScriptLanguage.CSharp:
                    provider = CodeDomProvider.CreateProvider( "CSharp" );
                    break;
                case ScriptLanguage.VB:
                    provider = CodeDomProvider.CreateProvider( "VisualBasic" );
                    break;
                case ScriptLanguage.JavaScript:
                    throw new NotImplementedException( "This language interface has not been implemented yet." );

            }

            if ( provider == null ) {
                throw new NotImplementedException( "You must have .net developer tools. (You need a visual studio)" );
            }

            var compile = provider.CompileAssemblyFromSource( _settings, text );

            if ( compile.Errors.Count > 0 ) {
                return new CompileResult( null, compile.Errors );
            }

            var assembly = compile.CompiledAssembly;
            var list = new List<Command>();

            foreach ( Command command in from type in assembly.GetTypes()
                                         where type.BaseType == typeof( Command )
                                         select Activator.CreateInstance( type ) as Command ) {
                list.Add( command );
            }

            return new CompileResult( list.ToArray(), null );
        }

        public static Command[] FromAssemblyFile ( string file ) {
            Assembly lib = Assembly.LoadFile ( file );
            var list = new List<Command>();

            foreach ( var instance in lib.GetTypes().Where( t => t.BaseType == typeof( Command ) ).Select( Activator.CreateInstance ) ) {
                list.Add( (Command) instance );
            }

            return list.ToArray ();
        }

    }

    public sealed class CompileResult {

        /// <summary>
        /// Array of errors, if any.
        /// </summary>
        public CompilerErrorCollection CompilerErrors { get; internal set; }

        /// <summary>
        /// Gets the command object.
        /// </summary>
        public Command[] Commands { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompileResult"/> class.
        /// </summary>
        /// <param name="commands">The command object.</param>
        /// <param name="errors">The errors.</param>
        public CompileResult ( Command[] commands, CompilerErrorCollection errors ) {
            Commands = commands;
            CompilerErrors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompileResult"/> class.
        /// </summary>
        public CompileResult () { }
    }

    public enum ScriptLanguage {

        /// <summary>
        /// C#.net Scripting Interface Language
        /// </summary>
        CSharp,

        /// <summary>
        /// VB.net Scripting Interface Language
        /// </summary>
        VB,

        /// <summary>
        /// JavaScript Scripting Interface Language.
        /// NOTE: Not yet implemented
        /// </summary>
        JavaScript

    }
}
