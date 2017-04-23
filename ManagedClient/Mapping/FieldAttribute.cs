// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldAttribute.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Mapping
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public class FieldAttribute
        : Attribute
    {
        #region Properties

        public string Tag { get; set; }

        public int Occurence { get; set; }

        #endregion

        #region Construction

        public FieldAttribute 
            (
                string tag 
            )
        {
            Tag = tag;
        }

        public FieldAttribute 
            ( 
                string tag, 
                int occurence 
            )
        {
            Tag = tag;
            Occurence = occurence;
        }

        #endregion
    }
}
