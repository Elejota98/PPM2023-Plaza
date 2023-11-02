namespace BlockAndPass.PPMWinform
{
    partial class TipoVehiculosOtros
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
            this.pnl_LogoTipoVehiculosOtros = new System.Windows.Forms.Panel();
            this.cboTipoVehiculo = new System.Windows.Forms.ComboBox();
            this.lblTipovehiculo = new System.Windows.Forms.Label();
            this.btn_Confirmar = new System.Windows.Forms.Button();
            this.btn_Cancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnl_LogoTipoVehiculosOtros
            // 
            this.pnl_LogoTipoVehiculosOtros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.pnl_LogoTipoVehiculosOtros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_LogoTipoVehiculosOtros.Location = new System.Drawing.Point(0, 0);
            this.pnl_LogoTipoVehiculosOtros.Name = "pnl_LogoTipoVehiculosOtros";
            this.pnl_LogoTipoVehiculosOtros.Size = new System.Drawing.Size(632, 81);
            this.pnl_LogoTipoVehiculosOtros.TabIndex = 0;
            // 
            // cboTipoVehiculo
            // 
            this.cboTipoVehiculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTipoVehiculo.FormattingEnabled = true;
            this.cboTipoVehiculo.Location = new System.Drawing.Point(223, 167);
            this.cboTipoVehiculo.Name = "cboTipoVehiculo";
            this.cboTipoVehiculo.Size = new System.Drawing.Size(232, 28);
            this.cboTipoVehiculo.TabIndex = 1;
            // 
            // lblTipovehiculo
            // 
            this.lblTipovehiculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipovehiculo.ForeColor = System.Drawing.Color.Gray;
            this.lblTipovehiculo.Location = new System.Drawing.Point(25, 160);
            this.lblTipovehiculo.Name = "lblTipovehiculo";
            this.lblTipovehiculo.Size = new System.Drawing.Size(179, 39);
            this.lblTipovehiculo.TabIndex = 6;
            this.lblTipovehiculo.Text = "Tipo Vehiculo";
            // 
            // btn_Confirmar
            // 
            this.btn_Confirmar.FlatAppearance.BorderSize = 0;
            this.btn_Confirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Confirmar.Location = new System.Drawing.Point(323, 260);
            this.btn_Confirmar.Name = "btn_Confirmar";
            this.btn_Confirmar.Size = new System.Drawing.Size(120, 53);
            this.btn_Confirmar.TabIndex = 7;
            this.btn_Confirmar.Text = "Confirmar";
            this.btn_Confirmar.UseVisualStyleBackColor = true;
            this.btn_Confirmar.Click += new System.EventHandler(this.btn_Confirmar_Click);
            // 
            // btn_Cancelar
            // 
            this.btn_Cancelar.FlatAppearance.BorderSize = 0;
            this.btn_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancelar.Location = new System.Drawing.Point(141, 260);
            this.btn_Cancelar.Name = "btn_Cancelar";
            this.btn_Cancelar.Size = new System.Drawing.Size(120, 53);
            this.btn_Cancelar.TabIndex = 8;
            this.btn_Cancelar.Text = "Cancelar";
            this.btn_Cancelar.UseVisualStyleBackColor = true;
            this.btn_Cancelar.Click += new System.EventHandler(this.btn_Cancelar_Click);
            // 
            // TipoVehiculosOtros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 325);
            this.Controls.Add(this.btn_Cancelar);
            this.Controls.Add(this.btn_Confirmar);
            this.Controls.Add(this.lblTipovehiculo);
            this.Controls.Add(this.cboTipoVehiculo);
            this.Controls.Add(this.pnl_LogoTipoVehiculosOtros);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TipoVehiculosOtros";
            this.Text = "TipoVehiculosOtros";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_LogoTipoVehiculosOtros;
        private System.Windows.Forms.ComboBox cboTipoVehiculo;
        private System.Windows.Forms.Label lblTipovehiculo;
        private System.Windows.Forms.Button btn_Confirmar;
        private System.Windows.Forms.Button btn_Cancelar;
    }
}