using System;
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
    public partial class ReportePatios : Form
    {
        public ReportePatios()
        {
            InitializeComponent();
        }

        private void ReportePatios_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataSetTicketCarga.P_ReportePatios' Puede moverla o quitarla según sea necesario.
            this.p_ReportePatiosTableAdapter.Fill(this.dataSetTicketCarga.P_ReportePatios);

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
