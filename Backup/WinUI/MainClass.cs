using System;
using System.Windows.Forms;

namespace EccGenerator.UI.Windows
{
	/// <summary>
	/// Punto de entrada de la aplicación
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
