// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuleContext.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Context for the <see cref="IrbisRule"/>.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class RuleContext
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Обрабатываемая запись.
        /// </summary>
        public IrbisRecord Record { get; set; }

        /// <summary>
        /// Формат для краткого библиографического описания.
        /// </summary>
        public string BriefFormat { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RuleContext()
        {
            BriefFormat = "@brief";
        }

        #endregion
    }
}
