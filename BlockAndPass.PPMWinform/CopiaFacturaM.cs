﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CopiaFacturaM : Form
    {
        string _NumFact = string.Empty;
        string _IdModulo = string.Empty;

        public CopiaFacturaM(string NUM, string IDMOD)
        {
            _NumFact = NUM;
            _IdModulo = IDMOD;
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string NUMFACT = tbnumerofactura.Text;
            string IDMODULO = cboIdModulo.SelectedItem.ToString();
            this.dataTable2TableAdapter1.Fill(this.dataSetCopia.DataTable2, NUMFACT , IDMODULO );
            //this.DATA

            this.reportViewer1.RefreshReport();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CopiaFacturaM_Load(object sender, EventArgs e)
        {
            tbnumerofactura.Text = _NumFact;
        }

        private void btn_Ok_Click_1(object sender, EventArgs e)
        {
            string NUMFACT = tbnumerofactura.Text;
            string IDMODULO = cboIdModulo.SelectedItem.ToString();
            this.dataTable2TableAdapter1.Fill(this.dataSetCopia.DataTable2, NUMFACT, IDMODULO);
            //this.DATA

            this.reportViewer1.RefreshReport();
        }

        private void btn_Cancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
