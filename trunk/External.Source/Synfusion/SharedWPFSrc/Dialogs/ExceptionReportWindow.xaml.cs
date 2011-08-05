using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;


namespace Syncfusion.Windows.Shared
{
	/// <summary>
	/// Interaction logic for window.
	/// </summary>
	public partial class ExceptionReportWindow : Window
	{
		#region Private members
        /// <summary>
        /// Stores exception.
        /// </summary>
		private Exception m_exception;
		#endregion

		#region Initialization
        /// <summary>
        /// Initializes window.
        /// </summary>
        /// <param name="exception">Given exception.</param>
		public ExceptionReportWindow( Exception exception )
		{
			m_exception = exception;

			InitializeComponent();
			treeView1.Content = exception;
		}
		#endregion

		#region Implementation
        /// <summary>
        /// Invoked when <see cref="Click"/> event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Instance that contains event data.</param>
		private void SendMailButtonClick( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
			Close();
		}
		#endregion
	}
}