// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyProvider.cs
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Morphology
{
    /// <summary>
    /// Base morphology provider.
    /// </summary>
    public class MorphologyProvider
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Flatten the query.
        /// </summary>
        [NotNull]
        public string[] Flatten
            (
                [NotNull] string word,
                [NotNull] MorphologyEntry[] entries
            )
        {
            List<string> result = new List<string>
            {
                word.ToUpper()
            };

            foreach (MorphologyEntry entry in entries)
            {
                result.Add(entry.MainTerm.ToUpper());
                result.AddRange(entry.Forms.Select(w => w.ToUpper()));
            }

            return result
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Find the word in the morphology database.
        /// </summary>
        [NotNull]
        public virtual MorphologyEntry[] FindWord
            (
                [NotNull] string word
            )
        {
            return new MorphologyEntry[0];
        }

        /// <summary>
        /// Rewrite the query using morphology.
        /// </summary>
        [NotNull]
        public virtual string RewriteQuery
            (
                [NotNull] string queryExpression
            )
        {
            return queryExpression;
        }

        #endregion
    }
}
