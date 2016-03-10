/* SelectionPanel.cs -- панель для отбора записей
 */

#region Using directives

using System;
using System.Windows.Forms;

using ManagedClient;
using ManagedClient.SubBasing;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Панель для отбора записей.
    /// </summary>
    public partial class SelectionPanel 
        : UserControl
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SelectionPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Настройка
        /// </summary>
        public void Setup
            (
                SubBasingSettings settings
            )
        {
            if (ReferenceEquals(settings, null))
            {
                throw new ArgumentNullException("settings");
            }

            _databaseBox.Items.Clear();
            _databaseBox.Items.AddRange(settings.Databases);
            _databaseBox.SelectedIndex = settings.DatabaseIndex;

            _criteriaBox.Items.Clear();
            _criteriaBox.Items.AddRange(settings.Criteria);
            _criteriaBox.SelectedIndex = settings.CriteriaIndex;

            _statementBox.Text = settings.Statement;
        }

        /// <summary>
        /// Получение пользовательского выбора.
        /// </summary>
        /// <returns></returns>
        public SelectionQuery GetQuery()
        {
            SelectionQuery result = new SelectionQuery();

            IrbisDatabaseInfo database 
                = (IrbisDatabaseInfo) _databaseBox.SelectedItem;
            result.Database = database.Name;

            SelectionQuery query 
                = (SelectionQuery) _criteriaBox.SelectedItem;
            result.SelectionType = query.SelectionType;
            result.Prefix = query.Prefix;
            result.Description = query.Description;
            
            result.Statement = _statementBox.Text.Trim();

            return result;
        }

        #endregion
    }
}
