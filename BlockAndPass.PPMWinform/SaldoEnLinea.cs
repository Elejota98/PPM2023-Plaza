﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class SaldoEnLinea : Form
    {
        public SaldoEnLinea()
        {
            InitializeComponent();
        }

        private void SaldoEnLinea_Load(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblFecha.Text = DateTime.Now.ToLongDateString();
            // TODO: esta línea de código carga datos en la tabla 'dataSetTicketCarga.P_DetalleSaldos' Puede moverla o quitarla según sea necesario.
            this.p_DetalleSaldosTableAdapter.Fill(this.dataSetTicketCarga.P_DetalleSaldos);
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void trmHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
