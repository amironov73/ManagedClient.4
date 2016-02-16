/* IrbisUserInfo.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
    public sealed class IrbisUserInfo
    {
        #region Properties

        public string Number { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Cataloger { get; set; }

        public string Reader { get; set; }

        public string Circulation { get; set; }

        public string Acquisitions { get; set; }

        public string Provision { get; set; }

        public string Administrator { get; set; }

        #endregion

        #region Private members

        private string FormatPair
            (
                string prefix,
                string value,
                string defaultValue
            )
        {
            if ( string.Compare ( value, defaultValue, StringComparison.OrdinalIgnoreCase ) == 0 )
            {
                return string.Empty;
            }
            return string.Format
                (
                    "{0}={1};",
                    prefix,
                    value
                );

        }

        #endregion

        #region Public methods

        public static IrbisUserInfo[] Parse ( string[] text )
        {
            List <IrbisUserInfo> result = new List < IrbisUserInfo > ();

            for ( int index = 3; index < (text.Length - 9); index += 9 )
            {
                IrbisUserInfo user = new IrbisUserInfo
                                         {
                                             Number = text[index],
                                             Name = text[index+1],
                                             Password = text[index+2],
                                             Cataloger = text[index+3],
                                             Reader = text[index+4],
                                             Circulation = text[index+5],
                                             Acquisitions = text[index+6],
                                             Provision = text[index+7],
                                             Administrator = text[index+8]
                                         };
                result.Add ( user );
            }

            return result.ToArray ();
        }

        public string Encode ()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    FormatPair ( "C", Cataloger, "irbisc.ini" ),
                    FormatPair ( "R", Reader, "irbisr.ini" ),
                    FormatPair ( "B", Circulation, "irbisb.ini" ),
                    FormatPair ( "M", Acquisitions, "irbisp.ini" ),
                    FormatPair ( "K", Provision, "irbisk.ini" ),
                    FormatPair ( "A", Administrator, "irbisa.ini" )
                );
        }

        #endregion

        #region Object members

        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Number: {0}, Name: {1}, Password: {2}, "
                  + "Cataloguer: {3}, Reader: {4}, Circulation: {5}, "
                  + "Acquisitions: {6}, Provision: {7}, Administrator: {8}", 
                    Number, 
                    Name, 
                    Password, 
                    Cataloger, 
                    Reader, 
                    Circulation, 
                    Acquisitions, 
                    Provision, 
                    Administrator 
                );
        }

        #endregion
    }
}
