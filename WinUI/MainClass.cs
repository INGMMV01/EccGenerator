using System;
using System.Windows.Forms;

namespace EccGenerator.UI.Windows
{
	/// <summary>
	/// Punto de entrada de la aplicaci�n
	/// </summary>
	public sealed class MainClass
	{
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}
	}
}
