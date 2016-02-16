/* SubFieldAttribute.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// Задаёт отображение подполя на свойство.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public sealed class SubFieldAttribute
        : Attribute
    {
        #region Properties

        public char Code { get; set; }

        public int Occurence { get; set; }

        #endregion

        #region Construction

        public SubFieldAttribute
            (
                char code
            )
        {
            Code = code;
        }

        public SubFieldAttribute
            (
                char code,
                int occurence
            )
        {
            Code = code;
            Occurence = occurence;
        }

        #endregion
    }
}
