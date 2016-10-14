using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameProgramming2D.GUI
{
	/// <summary>
	/// Represents dialog
	/// </summary>
	public class Dialog : MonoBehaviour
	{
		#region Delegates

		// Defines the type of method which can be called when dialog is closed
		// (no return value, no parameters)
		public delegate void DialogClosedDelegate();

		#endregion

		#region Unity fields
		// Refers to the headline component.
		[SerializeField] private Text _headline;
		// Refers the the text component.
		[SerializeField] private Text _text;
		// Refers to the OK button.
		[SerializeField] private Button _okButton;
		// Referes to the cancel button.
		[SerializeField] private Button _cancelButton;

		#endregion

		#region Private fields

		// The original position of the OK button. We want to set the button back to this position
		// if are using the dialog again and last time it was used as a one button dialog and this time
		// as two button dialog.
		private Vector3 _okButtonPosition;
		// The delegate which should contain the reference to the method which is invoked when ok button is clicked.
		private UnityAction _okButtonClick;
		// The delegate which should contain the reference to the method which is invoked when cancel button is clicked.
		private UnityAction _cancelButtonClick;

		#endregion

		#region Unity messages

		private void Awake()
		{
			// Store the original position of the OK button so we can restore it if needed.
			_okButtonPosition = _okButton.transform.localPosition;
		}

		#endregion

		#region Public interface

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		public void Show()
		{
			gameObject.SetActive( true );
		}

		/// <summary>
		/// Calls dialogClosedDelegate and closes dialog after that.
		/// </summary>
		/// <param name="dialogClosedDelegate">A reference to a method which is called when dialog is closed.</param>
		/// <param name="destroyAfterClose">If true, dialog is destroyed when it is closed.</param>
		public void CloseDialog( DialogClosedDelegate dialogClosedDelegate = null,
			bool destroyAfterClose = true )
		{
			// If the dialogClosedDelegate is not null (contains a reference to a method which
			// should be invoked when dialog is closed) we should invoke it.
			if ( dialogClosedDelegate != null )
			{
				dialogClosedDelegate();
			}

			// If destroyAfterClose is true, we should destroy the dialog when it is closed. Otherwise
			// we should just deactivate it.
			if ( destroyAfterClose )
			{
				Destroy( gameObject );
			}
			else
			{
				gameObject.SetActive( false );
			}
		}

		/// <summary>
		/// Sets the action which should be executed when OK button is pressed. Always closes the dialog at the same time.
		/// </summary>
		/// <param name="callback">Reference to a method which is invoked when dialog is closed.</param>
		/// <param name="destroyAfterClose">True if dialog should be destroyed when it is closed.</param>
		public void SetOnOKClicked( DialogClosedDelegate callback = null,
			bool destroyAfterClose = true )
		{
			// Anonymous method. Syntax (param1, param2,...) => { Implementation of method };
			// For more information, see: https://msdn.microsoft.com/en-us/library/bb397687.aspx
			_okButtonClick = () => CloseDialog( callback, destroyAfterClose ); 
			SetButtonOnClick( _okButton, _okButtonClick );
		}

		/// <summary>
		/// Sets the action which should be executed when OK button is pressed. Always closes the dialog at the same time.
		/// </summary>
		/// <param name="callback">Reference to a method which is invoked when dialog is closed.</param>
		/// <param name="destroyAfterClose">True if dialog should be destroyed when it is closed.</param>
		public void SetOnCancelClicked( DialogClosedDelegate callback = null,
			bool destroyAfterClose = true )
		{
			_cancelButtonClick = () => CloseDialog( callback, destroyAfterClose );
			SetButtonOnClick( _cancelButton, _cancelButtonClick );
		}

		/// <summary>
		/// Sets the headline of the dialod.
		/// </summary>
		/// <param name="text"></param>
		public void SetHeadline( string text )
		{
			_headline.text = text;
		}

		/// <summary>
		/// Sets the text of the dialog.
		/// </summary>
		/// <param name="text"></param>
		public void SetText( string text )
		{
			_text.text = text;
		}

		/// <summary>
		/// Sets if cancel button should be shown or not.
		/// </summary>
		/// <param name="showCancel">True, if cancel button should be shown. False otherwise.</param>
		public void SetShowCancel( bool showCancel )
		{
			_cancelButton.gameObject.SetActive( showCancel );

			if ( !showCancel )
			{
				// Is this is supposed to be a one button dialog, move ok button to center of the dialog along the x axis.
				var okPosition = _okButton.transform.localPosition;
				okPosition.x = 0;
				_okButton.transform.localPosition = okPosition;
			}
			else
			{
				// Reset the position of the ok button (two button version).
				_okButton.transform.localPosition = _okButtonPosition;
			}
		}

		/// <summary>
		/// Sets the text of the OK button.
		/// </summary>
		/// <param name="text">Text of the OK button.</param>
		public void SetOKButtonText( string text )
		{
			SetButtonText( _okButton, text );
		}

		/// <summary>
		/// Sets the text of the cancel button.
		/// </summary>
		/// <param name="text">Text of the cancel button.</param>
		public void SetCancelButtonText( string text )
		{
			SetButtonText( _cancelButton, text );
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Sets text to Buttons child text component
		/// </summary>
		/// <param name="button">The button which text we want to set</param>
		/// <param name="text">The text which we want to set to the button</param>
		private void SetButtonText( Button button, string text )
		{
			var label = button.GetComponentInChildren< Text >();
			label.text = text;
		}

		/// <summary>
		/// Adds callback delegate to buttons onClick event.
		/// </summary>
		private void SetButtonOnClick( Button button, UnityAction callback )
		{
			button.onClick.AddListener( callback );
		}

		#endregion
	}
}
