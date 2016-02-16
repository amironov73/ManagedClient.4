/* SearchTermInfo.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о поисковом термине.
    /// </summary>
    [Serializable]
    public sealed class SearchTermInfo
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public methods
        
        public static SearchTermInfo[] Parse
            (
                IEnumerable<string> answer
            )
        {
            List <SearchTermInfo> result = new List < SearchTermInfo > ();
            
            Regex regex = new Regex(@"^(\d+)\#(.+)$");
            foreach ( string line in answer )
            {
                Match match = regex.Match ( line );
                if ( match.Success )
                {
                    SearchTermInfo item = new SearchTermInfo 
                                              {
                                                  Count = int.Parse(match.Groups[1].Value),
                                                  Text = match.Groups[2].Value
                                              };
                    result.Add ( item );
                }
            }

            return result.ToArray ();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "{0}#{1}", 
                    Count, 
                    Text 
                );
        }

        #endregion
    }
}
