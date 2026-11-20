namespace NEXUS_WindowsForms
{
    partial class FormInventario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            lstInventario = new ListBox();
            btnUsarObjeto = new Button();
            btnEliminarObjeto = new Button();
            btnCerrarInventario = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 12F, FontStyle.Bold);
            label1.ForeColor = Color.Cyan;
            label1.Location = new Point(50, 18);
            label1.Name = "label1";
            label1.Size = new Size(351, 23);
            label1.TabIndex = 0;
            label1.Text = "INVENTARIO DE OBJETOS EQUIPADOS";
            // 
            // lstInventario
            // 
            lstInventario.BackColor = Color.FromArgb(10, 12, 18);
            lstInventario.Font = new Font("Consolas", 11F);
            lstInventario.ForeColor = Color.White;
            lstInventario.FormattingEnabled = true;
            lstInventario.Location = new Point(17, 56);
            lstInventario.Name = "lstInventario";
            lstInventario.Size = new Size(430, 224);
            lstInventario.TabIndex = 1;
            // 
            // btnUsarObjeto
            // 
            btnUsarObjeto.BackColor = Color.FromArgb(30, 100, 60);
            btnUsarObjeto.FlatStyle = FlatStyle.Flat;
            btnUsarObjeto.ForeColor = Color.White;
            btnUsarObjeto.Location = new Point(16, 297);
            btnUsarObjeto.Name = "btnUsarObjeto";
            btnUsarObjeto.Size = new Size(130, 40);
            btnUsarObjeto.TabIndex = 2;
            btnUsarObjeto.Text = "USAR OBJETO";
            btnUsarObjeto.UseVisualStyleBackColor = false;
            // 
            // btnEliminarObjeto
            // 
            btnEliminarObjeto.BackColor = Color.FromArgb(140, 40, 40);
            btnEliminarObjeto.FlatStyle = FlatStyle.Flat;
            btnEliminarObjeto.ForeColor = Color.White;
            btnEliminarObjeto.Location = new Point(166, 297);
            btnEliminarObjeto.Name = "btnEliminarObjeto";
            btnEliminarObjeto.Size = new Size(130, 40);
            btnEliminarObjeto.TabIndex = 3;
            btnEliminarObjeto.Text = "ELIMINAR";
            btnEliminarObjeto.UseVisualStyleBackColor = false;
            // 
            // btnCerrarInventario
            // 
            btnCerrarInventario.BackColor = Color.FromArgb(40, 50, 65);
            btnCerrarInventario.FlatStyle = FlatStyle.Flat;
            btnCerrarInventario.ForeColor = Color.Cyan;
            btnCerrarInventario.Location = new Point(317, 297);
            btnCerrarInventario.Name = "btnCerrarInventario";
            btnCerrarInventario.Size = new Size(130, 40);
            btnCerrarInventario.TabIndex = 4;
            btnCerrarInventario.Text = "CERRAR";
            btnCerrarInventario.UseVisualStyleBackColor = false;
            // 
            // FormInventario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 20, 30);
            ClientSize = new Size(482, 393);
            Controls.Add(btnCerrarInventario);
            Controls.Add(btnEliminarObjeto);
            Controls.Add(btnUsarObjeto);
            Controls.Add(lstInventario);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormInventario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NEXUS - Inventario del Cadete";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox lstInventario;
        private Button btnUsarObjeto;
        private Button btnEliminarObjeto;
        private Button btnCerrarInventario;
    }
}