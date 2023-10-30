namespace BlockAndPass.PPMWinform
{
    partial class Menu
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
            this.pnlEnecabezado = new System.Windows.Forms.Panel();
            this.pnlBarraLateralIzquierda = new System.Windows.Forms.Panel();
            this.pnlBarraLateralDerecha = new System.Windows.Forms.Panel();
            this.tabPrincipal = new System.Windows.Forms.TabControl();
            this.tabMenuPrincipal = new System.Windows.Forms.TabPage();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            this.tabEntrada = new System.Windows.Forms.TabPage();
            this.tabCobrar = new System.Windows.Forms.TabPage();
            this.tabPrincipal.SuspendLayout();
            this.tabMenuPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEnecabezado
            // 
            this.pnlEnecabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEnecabezado.Location = new System.Drawing.Point(0, 0);
            this.pnlEnecabezado.Name = "pnlEnecabezado";
            this.pnlEnecabezado.Size = new System.Drawing.Size(1014, 70);
            this.pnlEnecabezado.TabIndex = 0;
            // 
            // pnlBarraLateralIzquierda
            // 
            this.pnlBarraLateralIzquierda.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarraLateralIzquierda.Location = new System.Drawing.Point(0, 70);
            this.pnlBarraLateralIzquierda.Name = "pnlBarraLateralIzquierda";
            this.pnlBarraLateralIzquierda.Size = new System.Drawing.Size(115, 630);
            this.pnlBarraLateralIzquierda.TabIndex = 1;
            // 
            // pnlBarraLateralDerecha
            // 
            this.pnlBarraLateralDerecha.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBarraLateralDerecha.Location = new System.Drawing.Point(899, 70);
            this.pnlBarraLateralDerecha.Name = "pnlBarraLateralDerecha";
            this.pnlBarraLateralDerecha.Size = new System.Drawing.Size(115, 630);
            this.pnlBarraLateralDerecha.TabIndex = 2;
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.tabMenuPrincipal);
            this.tabPrincipal.Controls.Add(this.tabEntrada);
            this.tabPrincipal.Controls.Add(this.tabCobrar);
            this.tabPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPrincipal.Location = new System.Drawing.Point(115, 70);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.SelectedIndex = 0;
            this.tabPrincipal.Size = new System.Drawing.Size(784, 630);
            this.tabPrincipal.TabIndex = 3;
            // 
            // tabMenuPrincipal
            // 
            this.tabMenuPrincipal.Controls.Add(this.lblFecha);
            this.tabMenuPrincipal.Controls.Add(this.lblHora);
            this.tabMenuPrincipal.Location = new System.Drawing.Point(4, 22);
            this.tabMenuPrincipal.Name = "tabMenuPrincipal";
            this.tabMenuPrincipal.Size = new System.Drawing.Size(776, 604);
            this.tabMenuPrincipal.TabIndex = 2;
            this.tabMenuPrincipal.Text = "Principal";
            this.tabMenuPrincipal.UseVisualStyleBackColor = true;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.Location = new System.Drawing.Point(21, 63);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(99, 33);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "label1";
            // 
            // lblHora
            // 
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora.Location = new System.Drawing.Point(63, 21);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(235, 42);
            this.lblHora.TabIndex = 0;
            this.lblHora.Text = "lblHoraPago";
            // 
            // tabEntrada
            // 
            this.tabEntrada.Location = new System.Drawing.Point(4, 22);
            this.tabEntrada.Name = "tabEntrada";
            this.tabEntrada.Padding = new System.Windows.Forms.Padding(3);
            this.tabEntrada.Size = new System.Drawing.Size(776, 604);
            this.tabEntrada.TabIndex = 0;
            this.tabEntrada.Text = "Entrada";
            this.tabEntrada.UseVisualStyleBackColor = true;
            // 
            // tabCobrar
            // 
            this.tabCobrar.Location = new System.Drawing.Point(4, 22);
            this.tabCobrar.Name = "tabCobrar";
            this.tabCobrar.Padding = new System.Windows.Forms.Padding(3);
            this.tabCobrar.Size = new System.Drawing.Size(776, 604);
            this.tabCobrar.TabIndex = 1;
            this.tabCobrar.Text = "Cobrar";
            this.tabCobrar.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 700);
            this.Controls.Add(this.tabPrincipal);
            this.Controls.Add(this.pnlBarraLateralDerecha);
            this.Controls.Add(this.pnlBarraLateralIzquierda);
            this.Controls.Add(this.pnlEnecabezado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.tabPrincipal.ResumeLayout(false);
            this.tabMenuPrincipal.ResumeLayout(false);
            this.tabMenuPrincipal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEnecabezado;
        private System.Windows.Forms.Panel pnlBarraLateralIzquierda;
        private System.Windows.Forms.Panel pnlBarraLateralDerecha;
        private System.Windows.Forms.TabControl tabPrincipal;
        private System.Windows.Forms.TabPage tabEntrada;
        private System.Windows.Forms.TabPage tabCobrar;
        private System.Windows.Forms.TabPage tabMenuPrincipal;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Label lblFecha;
    }
}