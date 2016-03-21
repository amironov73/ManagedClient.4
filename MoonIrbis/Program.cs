/* Program.cs
 */

#region Using directives

using System;

using ManagedClient;
using ManagedClient.Scripting;

using MoonSharp.Interpreter;

#endregion

namespace MoonIrbis
{
    class Program
    {
        private static IrbisScript _script;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            string scriptText = null;
            string fileName = args[0];
            if ((fileName == "-e")
                && (args.Length >= 2))
            {
                scriptText = args[1];
                fileName = null;
                if (args.Length == 3)
                {
                    fileName = args[2];
                }
            }

            try
            {
                using (_script = new IrbisScript())
                {
                    if (!string.IsNullOrEmpty(scriptText))
                    {
                        _script.DoString
                            (
                                scriptText
                            );
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        _script.DoFile
                            (
                                fileName
                            );
                    }
                }
            }
            catch (ScriptRuntimeException sre)
            {
                Console.WriteLine(sre.DecoratedMessage);
                Console.WriteLine(sre.ToString());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}
