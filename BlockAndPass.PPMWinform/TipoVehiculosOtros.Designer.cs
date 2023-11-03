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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipoVehiculosOtros));
            this.pnl_LogoTipoVehiculosOtros = new System.Windows.Forms.Panel();
            this.cboTipoVehiculo = new System.Windows.Forms.ComboBox();
            this.lblTipovehiculo = new System.Windows.Forms.Label();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.btn_Ok = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // pnl_LogoTipoVehiculosOtros
            // 
            this.pnl_LogoTipoVehiculosOtros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.pnl_LogoTipoVehiculosOtros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_LogoTipoVehiculosOtros.Location = new System.Drawing.Point(0, 0);
            this.pnl_LogoTipoVehiculosOtros.Name = "pnl_LogoTipoVehiculosOtros";
            this.pnl_LogoTipoVehiculosOtros.Size = new System.Drawing.Size(567, 81);
            this.pnl_LogoTipoVehiculosOtros.TabIndex = 0;
            // 
            // cboTipoVehiculo
            // 
            this.cboTipoVehiculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTipoVehiculo.FormattingEnabled = true;
            this.cboTipoVehiculo.Location = new System.Drawing.Point(197, 157);
            this.cboTipoVehiculo.Name = "cboTipoVehiculo";
            this.cboTipoVehiculo.Size = new System.Drawing.Size(354, 37);
            this.cboTipoVehiculo.TabIndex = 1;
            // 
            // lblTipovehiculo
            // 
            this.lblTipovehiculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipovehiculo.ForeColor = System.Drawing.Color.Gray;
            this.lblTipovehiculo.Location = new System.Drawing.Point(12, 157);
            this.lblTipovehiculo.Name = "lblTipovehiculo";
            this.lblTipovehiculo.Size = new System.Drawing.Size(179, 39);
            this.lblTipovehiculo.TabIndex = 6;
            this.lblTipovehiculo.Text = "Tipo Vehiculo";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(64)))), ((int)(((byte)(97)))));
            this.btn_Cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Icon = FontAwesome.Sharp.IconChar.Times;
            this.btn_Cancel.IconColor = System.Drawing.Color.White;
            this.btn_Cancel.IconSize = 20;
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(230, 265);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 35);
            this.btn_Cancel.TabIndex = 47;
            this.btn_Cancel.Text = "Cerrar";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(165)))), ((int)(((byte)(64)))));
            this.btn_Ok.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Ok.FlatAppearance.BorderSize = 0;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.ForeColor = System.Drawing.Color.White;
            this.btn_Ok.Icon = FontAwesome.Sharp.IconChar.Check;
            this.btn_Ok.IconColor = System.Drawing.Color.White;
            this.btn_Ok.IconSize = 20;
            this.btn_Ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_Ok.Image")));
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(341, 265);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(90, 35);
            this.btn_Ok.TabIndex = 46;
            this.btn_Ok.Text = "Ingresar";
            this.btn_Ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // TipoVehiculosOtros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 325);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
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
        private FontAwesome.Sharp.IconButton btn_Cancel;
        private FontAwesome.Sharp.IconButton btn_Ok;
    }
}