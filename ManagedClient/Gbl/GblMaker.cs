/* GblMaker.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Gbl
{
    public static class GblMaker
    {
        #region Constants

        public const string Filler = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        #endregion

        #region Public methods

        public static GblItem Add
            (
                string tag,
                string value
            )
        {
            return new GblItem
            {
                Command = GblCommand.Add,
                Parameter1 = tag,
                Parameter2 = "*",
                Format1 = value,
                Format2 = Filler
            };
        }

        public static GblItem Replace
            (
                string tag,
                string value
            )
        {
            return new GblItem
            {
                Command = GblCommand.Replace,
                Parameter1 = tag,
                Parameter2 = "*",
                Format1 = value,
                Format2 = Filler
            };
        }

        public static GblItem Change
            (
                string tag,
                string fromText,
                string toText
            )
        {
            return new GblItem
            {
                Command = GblCommand.Change,
                Parameter1 = tag,
                Parameter2 = "*",
                Format1 = fromText,
                Format2 = toText
            };
        }

        public static GblItem Delete
            (
                string tag
            )
        {
            return new GblItem
            {
                Command = GblCommand.Delete,
                Parameter1 = tag,
                Parameter2 = "*",
                Format1 = Filler,
                Format2 = Filler
            };
        }

        public static GblItem DeleteRecord()
        {
            return new GblItem
            {
                Command = GblCommand.DeleteRecord,
                Parameter1 = Filler,
                Parameter2 = Filler,
                Format1 = Filler,
                Format2 = Filler
            };
        }

        #endregion
    }
}
