namespace BlockAndPass.PPMWinform
{
    partial class ReportePatios
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportePatios));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.P_ReportePatiosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetTicketCarga = new BlockAndPass.PPMWinform.Tickets.DataSetTicketCarga();
            this.pReportePatiosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.p_ReportePatiosTableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetTicketCargaTableAdapters.P_ReportePatiosTableAdapter();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.P_ReportePatiosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTicketCarga)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pReportePatiosBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_ReportePatiosBindingSource
            // 
            this.P_ReportePatiosBindingSource.DataMember = "P_ReportePatios";
            this.P_ReportePatiosBindingSource.DataSource = this.dataSetTicketCarga;
            // 
            // dataSetTicketCarga
            // 
            this.dataSetTicketCarga.DataSetName = "DataSetTicketCarga";
            this.dataSetTicketCarga.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pReportePatiosBindingSource
            // 
            this.pReportePatiosBindingSource.DataMember = "P_ReportePatios";
            this.pReportePatiosBindingSource.DataSource = this.dataSetTicketCarga;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 76);
            this.panel1.TabIndex = 2;
            // 
            // p_ReportePatiosTableAdapter
            // 
            this.p_ReportePatiosTableAdapter.ClearBeforeFill = true;
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
            this.btn_Cancel.Location = new System.Drawing.Point(475, 570);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 35);
            this.btn_Cancel.TabIndex = 47;
            this.btn_Cancel.Text = "Cerrar";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // reportViewer1
            // 
            reportDataSource2.Name = "DataSetReportePatios";
            reportDataSource2.Value = this.P_ReportePatiosBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.ReportePatios.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 82);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(520, 482);
            this.reportViewer1.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 33);
            this.label1.TabIndex = 6;
            this.label1.Text = "Reporte de patios";
            // 
            // ReportePatios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 609);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReportePatios";
            this.Text = "ReportePatios";
            this.Load += new System.EventHandler(this.ReportePatios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.P_ReportePatiosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTicketCarga)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pReportePatiosBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource pReportePatiosBindingSource;
        private Tickets.DataSetTicketCarga dataSetTicketCarga;
        private Tickets.DataSetTicketCargaTableAdapters.P_ReportePatiosTableAdapter p_ReportePatiosTableAdapter;
        private FontAwesome.Sharp.IconButton btn_Cancel;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource P_ReportePatiosBindingSource;
        private System.Windows.Forms.Label label1;
    }
}