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
            this.Imagen_Principal = new System.Windows.Forms.Panel();
            this.tabPrincipal.SuspendLayout();
            this.tabMenuPrincipal.SuspendLayout();
            this.Imagen_Principal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEnecabezado
            // 
            this.pnlEnecabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEnecabezado.Location = new System.Drawing.Point(0, 0);
            this.pnlEnecabezado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlEnecabezado.Name = "pnlEnecabezado";
            this.pnlEnecabezado.Size = new System.Drawing.Size(1352, 86);
            this.pnlEnecabezado.TabIndex = 0;
            // 
            // pnlBarraLateralIzquierda
            // 
            this.pnlBarraLateralIzquierda.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarraLateralIzquierda.Location = new System.Drawing.Point(0, 86);
            this.pnlBarraLateralIzquierda.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBarraLateralIzquierda.Name = "pnlBarraLateralIzquierda";
            this.pnlBarraLateralIzquierda.Size = new System.Drawing.Size(153, 776);
            this.pnlBarraLateralIzquierda.TabIndex = 1;
            // 
            // pnlBarraLateralDerecha
            // 
            this.pnlBarraLateralDerecha.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBarraLateralDerecha.Location = new System.Drawing.Point(1199, 86);
            this.pnlBarraLateralDerecha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBarraLateralDerecha.Name = "pnlBarraLateralDerecha";
            this.pnlBarraLateralDerecha.Size = new System.Drawing.Size(153, 776);
            this.pnlBarraLateralDerecha.TabIndex = 2;
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.tabMenuPrincipal);
            this.tabPrincipal.Controls.Add(this.tabEntrada);
            this.tabPrincipal.Controls.Add(this.tabCobrar);
            this.tabPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPrincipal.Location = new System.Drawing.Point(153, 86);
            this.tabPrincipal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.SelectedIndex = 0;
            this.tabPrincipal.Size = new System.Drawing.Size(1046, 776);
            this.tabPrincipal.TabIndex = 3;
            // 
            // tabMenuPrincipal
            // 
            this.tabMenuPrincipal.Controls.Add(this.Imagen_Principal);
            this.tabMenuPrincipal.Location = new System.Drawing.Point(4, 25);
            this.tabMenuPrincipal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabMenuPrincipal.Name = "tabMenuPrincipal";
            this.tabMenuPrincipal.Size = new System.Drawing.Size(1038, 747);
            this.tabMenuPrincipal.TabIndex = 2;
            this.tabMenuPrincipal.Text = "Principal";
            this.tabMenuPrincipal.UseVisualStyleBackColor = true;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblFecha.Location = new System.Drawing.Point(107, 206);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(124, 42);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "label1";
            // 
            // lblHora
            // 
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblHora.Location = new System.Drawing.Point(163, 154);
            this.lblHora.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(291, 54);
            this.lblHora.TabIndex = 0;
            this.lblHora.Text = "lblHoraPago";
            // 
            // tabEntrada
            // 
            this.tabEntrada.Location = new System.Drawing.Point(4, 25);
            this.tabEntrada.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEntrada.Name = "tabEntrada";
            this.tabEntrada.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEntrada.Size = new System.Drawing.Size(1298, 940);
            this.tabEntrada.TabIndex = 0;
            this.tabEntrada.Text = "Entrada";
            this.tabEntrada.UseVisualStyleBackColor = true;
            // 
            // tabCobrar
            // 
            this.tabCobrar.Location = new System.Drawing.Point(4, 25);
            this.tabCobrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCobrar.Name = "tabCobrar";
            this.tabCobrar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCobrar.Size = new System.Drawing.Size(1298, 940);
            this.tabCobrar.TabIndex = 1;
            this.tabCobrar.Text = "Cobrar";
            this.tabCobrar.UseVisualStyleBackColor = true;
            // 
            // Imagen_Principal
            // 
            this.Imagen_Principal.BackgroundImage = global::BlockAndPass.PPMWinform.Properties.Resources.Principal;
            this.Imagen_Principal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Imagen_Principal.Controls.Add(this.lblHora);
            this.Imagen_Principal.Controls.Add(this.lblFecha);
            this.Imagen_Principal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Imagen_Principal.Location = new System.Drawing.Point(0, 0);
            this.Imagen_Principal.Name = "Imagen_Principal";
            this.Imagen_Principal.Size = new System.Drawing.Size(1038, 747);
            this.Imagen_Principal.TabIndex = 2;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 862);
            this.Controls.Add(this.tabPrincipal);
            this.Controls.Add(this.pnlBarraLateralDerecha);
            this.Controls.Add(this.pnlBarraLateralIzquierda);
            this.Controls.Add(this.pnlEnecabezado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.tabPrincipal.ResumeLayout(false);
            this.tabMenuPrincipal.ResumeLayout(false);
            this.Imagen_Principal.ResumeLayout(false);
            this.Imagen_Principal.PerformLayout();
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
        private System.Windows.Forms.Panel Imagen_Principal;
    }
}