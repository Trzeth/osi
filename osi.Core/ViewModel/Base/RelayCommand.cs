using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace osi.Core
{
	public class RelayCommand : ICommand
	{
		#region Private Member

		/// <summary>
		/// The action to run
		/// </summary>
		private Action mAction;

		#endregion

		#region Public Events

		public event EventHandler CanExecuteChanged = (sender,e) => { };

		#endregion

		#region Command Methods
		/// <summary>
		/// A relay command can always execute
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			mAction();
		}

		#endregion

		#region Constructor

		public RelayCommand(Action action)
		{
			mAction = action;
		}

		#endregion
	}
}
