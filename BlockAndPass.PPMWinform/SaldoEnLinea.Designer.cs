﻿namespace BlockAndPass.PPMWinform
{
    partial class SaldoEnLinea
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaldoEnLinea));
            this.pDetalleSaldosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetTicketCarga = new BlockAndPass.PPMWinform.Tickets.DataSetTicketCarga();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.p_DetalleSaldosTableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetTicketCargaTableAdapters.P_DetalleSaldosTableAdapter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trmHora = new System.Windows.Forms.Timer(this.components);
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pDetalleSaldosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTicketCarga)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pDetalleSaldosBindingSource
            // 
            this.pDetalleSaldosBindingSource.DataMember = "P_DetalleSaldos";
            this.pDetalleSaldosBindingSource.DataSource = this.dataSetTicketCarga;
            // 
            // dataSetTicketCarga
            // 
            this.dataSetTicketCarga.DataSetName = "DataSetTicketCarga";
            this.dataSetTicketCarga.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSetSaldoEnLinea";
            reportDataSource1.Value = this.pDetalleSaldosBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.ReporteSaldoEnLinea.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(37, 159);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(675, 527);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // p_DetalleSaldosTableAdapter
            // 
            this.p_DetalleSaldosTableAdapter.ClearBeforeFill = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 66);
            this.panel1.TabIndex = 1;
            // 
            // trmHora
            // 
            this.trmHora.Tick += new System.EventHandler(this.trmHora_Tick);
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.Location = new System.Drawing.Point(186, 119);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(64, 24);
            this.lblFecha.TabIndex = 4;
            this.lblFecha.Text = "Fecha";
            // 
            // lblHora
            // 
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora.Location = new System.Drawing.Point(214, 78);
            this.lblHora.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(117, 39);
            this.lblHora.TabIndex = 3;
            this.lblHora.Text = "HORA";
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
            this.btn_Cancel.Location = new System.Drawing.Point(718, 662);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 35);
            this.btn_Cancel.TabIndex = 46;
            this.btn_Cancel.Text = "Cerrar";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 33);
            this.label1.TabIndex = 6;
            this.label1.Text = "Saldo en línea";
            // 
            // SaldoEnLinea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 698);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblHora);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SaldoEnLinea";
            this.Text = "SaldoEnLinea";
            this.Load += new System.EventHandler(this.SaldoEnLinea_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pDetalleSaldosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTicketCarga)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private Tickets.DataSetTicketCarga dataSetTicketCarga;
        private System.Windows.Forms.BindingSource pDetalleSaldosBindingSource;
        private Tickets.DataSetTicketCargaTableAdapters.P_DetalleSaldosTableAdapter p_DetalleSaldosTableAdapter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer trmHora;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblHora;
        private FontAwesome.Sharp.IconButton btn_Cancel;
        private System.Windows.Forms.Label label1;
    }
}