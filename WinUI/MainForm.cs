using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using EccGenerator.Sql;
using EccGenerator.Structures;
using EccGenerator.Patterns;
using EccGenerator.Binding;

namespace EccGenerator.UI.Windows
{
	/// <summary>
	/// Descripción breve de MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lTarea1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvCampos;
		private System.Windows.Forms.ColumnHeader colAlias;
		private System.Windows.Forms.ColumnHeader colCampo;
		private System.Windows.Forms.ColumnHeader colTipo;
		private System.Windows.Forms.Panel panCampos;
		private System.Windows.Forms.FolderBrowserDialog browseDialog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tDirectorio;
		private System.Windows.Forms.Button bBuscarDir;
		private System.Windows.Forms.Button bGenerar;
		private System.Windows.Forms.ComboBox lTabla;
		private System.Windows.Forms.Panel panEntidad;
		private System.Windows.Forms.Panel panDir;
		private System.Windows.Forms.TextBox tEntidad;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tAplicacion;
		/// <summary>
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Necesario para admitir el Diseñador de Windows Forms
			//
			InitializeComponent();			
		}

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Código generado por el Diseñador de Windows Forms
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.lTabla = new System.Windows.Forms.ComboBox();
			this.lTarea1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panCampos = new System.Windows.Forms.Panel();
			this.lvCampos = new System.Windows.Forms.ListView();
			this.colAlias = new System.Windows.Forms.ColumnHeader();
			this.colCampo = new System.Windows.Forms.ColumnHeader();
			this.colTipo = new System.Windows.Forms.ColumnHeader();
			this.browseDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panEntidad = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.tAplicacion = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tEntidad = new System.Windows.Forms.TextBox();
			this.panDir = new System.Windows.Forms.Panel();
			this.bBuscarDir = new System.Windows.Forms.Button();
			this.tDirectorio = new System.Windows.Forms.TextBox();
			this.bGenerar = new System.Windows.Forms.Button();
			this.panCampos.SuspendLayout();
			this.panEntidad.SuspendLayout();
			this.panDir.SuspendLayout();
			this.SuspendLayout();
			// 
			// lTabla
			// 
			this.lTabla.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lTabla.ItemHeight = 13;
			this.lTabla.Location = new System.Drawing.Point(283, 16);
			this.lTabla.Name = "lTabla";
			this.lTabla.Size = new System.Drawing.Size(208, 21);
			this.lTabla.TabIndex = 1;
			this.lTabla.SelectedIndexChanged += new System.EventHandler(this.lTabla_SelectedIndexChanged);
			// 
			// lTarea1
			// 
			this.lTarea1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lTarea1.Location = new System.Drawing.Point(8, 16);
			this.lTarea1.Name = "lTarea1";
			this.lTarea1.Size = new System.Drawing.Size(176, 23);
			this.lTarea1.TabIndex = 0;
			this.lTarea1.Text = "1. Escoja Tabla";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(464, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "2. Establezca Alias para los Campos";
			// 
			// panCampos
			// 
			this.panCampos.Controls.Add(this.lvCampos);
			this.panCampos.Enabled = false;
			this.panCampos.Location = new System.Drawing.Point(11, 88);
			this.panCampos.Name = "panCampos";
			this.panCampos.Size = new System.Drawing.Size(482, 123);
			this.panCampos.TabIndex = 4;
			this.panCampos.EnabledChanged += new System.EventHandler(this.panCampos_EnabledChanged);
			// 
			// lvCampos
			// 
			this.lvCampos.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.lvCampos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvCampos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.colAlias,
																					   this.colCampo,
																					   this.colTipo});
			this.lvCampos.GridLines = true;
			this.lvCampos.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvCampos.HideSelection = false;
			this.lvCampos.LabelEdit = true;
			this.lvCampos.LabelWrap = false;
			this.lvCampos.Location = new System.Drawing.Point(0, 0);
			this.lvCampos.MultiSelect = false;
			this.lvCampos.Name = "lvCampos";
			this.lvCampos.Size = new System.Drawing.Size(480, 120);
			this.lvCampos.TabIndex = 4;
			this.lvCampos.View = System.Windows.Forms.View.Details;
			this.lvCampos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvCampos_KeyDown);
			this.lvCampos.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvCampos_AfterLabelEdit);
			// 
			// colAlias
			// 
			this.colAlias.Text = "Alias";
			this.colAlias.Width = 176;
			// 
			// colCampo
			// 
			this.colCampo.Text = "Campo";
			this.colCampo.Width = 150;
			// 
			// colTipo
			// 
			this.colTipo.Text = "Tipo";
			this.colTipo.Width = 152;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 328);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(392, 23);
			this.label3.TabIndex = 5;
			this.label3.Text = "4. Escoja Directorio de Salida";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 219);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(424, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "3.Especifique los datos de las clases";
			// 
			// panEntidad
			// 
			this.panEntidad.Controls.Add(this.label2);
			this.panEntidad.Controls.Add(this.tAplicacion);
			this.panEntidad.Controls.Add(this.label5);
			this.panEntidad.Controls.Add(this.tEntidad);
			this.panEntidad.Enabled = false;
			this.panEntidad.Location = new System.Drawing.Point(11, 248);
			this.panEntidad.Name = "panEntidad";
			this.panEntidad.Size = new System.Drawing.Size(482, 72);
			this.panEntidad.TabIndex = 7;
			this.panEntidad.EnabledChanged += new System.EventHandler(this.panEntidad_EnabledChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 23);
			this.label2.TabIndex = 9;
			this.label2.Text = "Aplicación";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.UseMnemonic = false;
			// 
			// tAplicacion
			// 
			this.tAplicacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tAplicacion.Location = new System.Drawing.Point(64, 43);
			this.tAplicacion.Name = "tAplicacion";
			this.tAplicacion.Size = new System.Drawing.Size(408, 20);
			this.tAplicacion.TabIndex = 8;
			this.tAplicacion.Text = "";
			this.tAplicacion.Leave += new System.EventHandler(this.tEntidad_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 23);
			this.label5.TabIndex = 7;
			this.label5.Text = "Entidad";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label5.UseMnemonic = false;
			// 
			// tEntidad
			// 
			this.tEntidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tEntidad.Location = new System.Drawing.Point(64, 10);
			this.tEntidad.Name = "tEntidad";
			this.tEntidad.Size = new System.Drawing.Size(408, 20);
			this.tEntidad.TabIndex = 6;
			this.tEntidad.Text = "";
			this.tEntidad.Leave += new System.EventHandler(this.tEntidad_TextChanged);
			// 
			// panDir
			// 
			this.panDir.Controls.Add(this.bBuscarDir);
			this.panDir.Controls.Add(this.tDirectorio);
			this.panDir.Enabled = false;
			this.panDir.Location = new System.Drawing.Point(8, 360);
			this.panDir.Name = "panDir";
			this.panDir.Size = new System.Drawing.Size(488, 40);
			this.panDir.TabIndex = 8;
			this.panDir.EnabledChanged += new System.EventHandler(this.panDir_EnabledChanged);
			// 
			// bBuscarDir
			// 
			this.bBuscarDir.Image = ((System.Drawing.Image)(resources.GetObject("bBuscarDir.Image")));
			this.bBuscarDir.Location = new System.Drawing.Point(448, 8);
			this.bBuscarDir.Name = "bBuscarDir";
			this.bBuscarDir.Size = new System.Drawing.Size(32, 24);
			this.bBuscarDir.TabIndex = 8;
			this.bBuscarDir.Click += new System.EventHandler(this.bBuscarDir_Click);
			// 
			// tDirectorio
			// 
			this.tDirectorio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tDirectorio.Location = new System.Drawing.Point(8, 11);
			this.tDirectorio.Name = "tDirectorio";
			this.tDirectorio.Size = new System.Drawing.Size(432, 20);
			this.tDirectorio.TabIndex = 7;
			this.tDirectorio.Text = "";
			this.tDirectorio.TextChanged += new System.EventHandler(this.tDirectorio_Leave);
			// 
			// bGenerar
			// 
			this.bGenerar.Enabled = false;
			this.bGenerar.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bGenerar.Location = new System.Drawing.Point(8, 408);
			this.bGenerar.Name = "bGenerar";
			this.bGenerar.Size = new System.Drawing.Size(488, 48);
			this.bGenerar.TabIndex = 9;
			this.bGenerar.Text = "Generar";
			this.bGenerar.Click += new System.EventHandler(this.bGenerar_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(506, 463);
			this.Controls.Add(this.bGenerar);
			this.Controls.Add(this.panDir);
			this.Controls.Add(this.panEntidad);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panCampos);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lTabla);
			this.Controls.Add(this.lTarea1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Generador de Clases ECC  [G2154.2007]";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.panCampos.ResumeLayout(false);
			this.panEntidad.ResumeLayout(false);
			this.panDir.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			string[] tables = SqlExtractor.GetTables();
			
			ArrayList arrl = new ArrayList(tables);

			arrl.Insert(0,"");

			lTabla.DataSource = arrl;
		}

		private void lTabla_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string tabla = (string)lTabla.SelectedItem;

			if(tabla != "")
			{
				CargaCampos(tabla);

				panCampos.Enabled = true;
			}
			else
			{
				panCampos.Enabled = false;
			}
		}

		private void panCampos_EnabledChanged(object sender, System.EventArgs e)
		{
			panEntidad.Enabled = false;

		}

		private void panEntidad_EnabledChanged(object sender, System.EventArgs e)
		{
			if(!panEntidad.Enabled) panDir.Enabled = false;
		}

		private void panDir_EnabledChanged(object sender, System.EventArgs e)
		{
			if(!panDir.Enabled) bGenerar.Enabled = false;
		}

		private void CargaCampos(string tabla)
		{
			lvCampos.Items.Clear();

			FieldArray fields = SqlExtractor.GetTableFields(tabla);

			foreach(Field field in fields)
			{
				string[] items = new string[] { "", field.Name, field.Type.Name };

				ListViewItem lvi = new ListViewItem(items);
				lvi.Tag = field;

				lvCampos.Items.Add(lvi);
			}
		}

		private FieldArray RecuperaCampos()
		{
			FieldArray ret = new FieldArray();

			foreach(ListViewItem lvi in lvCampos.Items)
			{
				ret.Add((Field)lvi.Tag);
			}

			return ret;
		}

		private void lvCampos_AfterLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
		{
			ListViewItem lvi = lvCampos.Items[e.Item];
			Field field = (Field)lvi.Tag;

			if(!AliasValido(e.Label))
			{
				if(!(e.Label == null || e.Label == ""))
				{
					e.CancelEdit = true;

					MessageBox.Show("'" + e.Label + "' no es un alias válido");
				}
			}
			else
			{
			
				lvi.Text = e.Label;			
				field.Alias = lvi.Text;

				VerificaCamposCubiertos();
			}
		}

		private void VerificaCamposCubiertos()
		{
			ArrayList arr = new ArrayList();

			int count = 0;
			foreach(ListViewItem item in lvCampos.Items)
			{
				if(item.Text != "")
				{				
					if(arr.IndexOf(item.Text) == -1)
					{
						arr.Add(item.Text);
						count++;
					}
					else
					{
						MessageBox.Show("No puede haber alias repetidos ('" + item.Text + "')");
					}
				}
			}

			panEntidad.Enabled = lvCampos.Items.Count == count;
		}

		private bool AliasValido(string campo)
		{
			if(campo == null || campo == "") return false;

			bool valido = true;
			bool numeric = true;

			foreach(char chr in campo)
			{
				if(!char.IsDigit(chr) && numeric) numeric = false;

				if(numeric || char.IsControl(chr) || char.IsPunctuation(chr) || chr == ' ')
				{
					valido = false;
					break;
				}
					
			}

			return valido;
		}

		private void tEntidad_TextChanged(object sender, System.EventArgs e)
		{
			TextBox text = (TextBox)sender;

			if(!AliasValido(text.Text))
			{				
				MessageBox.Show("'" + tEntidad.Text + "' no es un nombre de entidad válido");

				text.Text = "";

				panDir.Enabled = false;
			}
			else
			{
				if(tAplicacion.Text != "" && tEntidad.Text != "") panDir.Enabled = true;
			}

			// En caso de que esté generando una segunda entidad, el directorio ya está seleccionado
			tDirectorio_Leave(sender,e);
		}

		private void tDirectorio_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void lvCampos_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.F2 && lvCampos.SelectedItems.Count > 0)
				lvCampos.SelectedItems[0].BeginEdit();
		}

		private void bBuscarDir_Click(object sender, System.EventArgs e)
		{
			browseDialog.ShowDialog();

			tDirectorio.Text = browseDialog.SelectedPath;
		}

		private void tDirectorio_Leave(object sender, System.EventArgs e)
		{
			if(tDirectorio.Text != "")
				bGenerar.Enabled = true;
			else
				bGenerar.Enabled = false;
		}

		private void bGenerar_Click(object sender, System.EventArgs e)
		{
			const string ENG_FNAME = "{0}\\Eng{1}.cs";
			const string CLS_FNAME = "{0}\\C{1}.cs";
			const string SP_FNAME = "{0}\\{1}.sql";

			FieldArray fields = RecuperaCampos();
			string	entidad = tEntidad.Text,
					aplicacion = tAplicacion.Text,
					path = tDirectorio.Text,
					table = lTabla.SelectedValue.ToString();

			string	spnameselect = SqlExtractor.GetSPName(aplicacion,entidad,SPType.Select),
					spselect = SqlExtractor.CreateSelectProcedure(aplicacion,table,fields,entidad),

					spnameupdate = SqlExtractor.GetSPName(aplicacion,entidad,SPType.Update),
					spupdate = SqlExtractor.CreateUpdateProcedure(aplicacion,table,fields,entidad),

					spnameinsert = SqlExtractor.GetSPName(aplicacion,entidad,SPType.Insert),
					spinsert = SqlExtractor.CreateInsertProcedure(aplicacion,table,fields,entidad),

					spnamedelete = SqlExtractor.GetSPName(aplicacion,entidad,SPType.Delete),
					spdelete = SqlExtractor.CreateDeleteProcedure(aplicacion,table,fields,entidad);

			Hashtable extInfo = new Hashtable();

			extInfo.Add("aplicacion",aplicacion);
			extInfo.Add("spselect",spnameselect);
			extInfo.Add("spupdate",spnameupdate);
			extInfo.Add("spinsert",spnameinsert);
			extInfo.Add("spdelete",spnamedelete);
			extInfo.Add("primarykeys",SqlExtractor.GetPrimaryKeys(table));

			string strclass = Binder.BindClass(entidad,fields,extInfo);
			string strengine = Binder.BindEngine(entidad,fields,extInfo);			

			StreamWriter sw = new StreamWriter(string.Format(CLS_FNAME,path,entidad),false);
			sw.Write(strclass);
			sw.Close();


			sw = new StreamWriter(string.Format(ENG_FNAME,path,entidad),false);
			sw.Write(strengine);
			sw.Close();
			
			sw = new StreamWriter(string.Format(SP_FNAME,path,spnameselect),false);
			sw.Write(spselect);
			sw.Close();

			sw = new StreamWriter(string.Format(SP_FNAME,path,spnameupdate),false);
			sw.Write(spupdate);
			sw.Close();

			sw = new StreamWriter(string.Format(SP_FNAME,path,spnameinsert),false);
			sw.Write(spinsert);
			sw.Close();

			sw = new StreamWriter(string.Format(SP_FNAME,path,spnamedelete),false);
			sw.Write(spdelete);
			sw.Close();

			MessageBox.Show("Archivos generados correctamente");

			// Preparo para generar nuevas entidades
			tEntidad.Text = "";
            panEntidad.Enabled = false;
		}
	}
}