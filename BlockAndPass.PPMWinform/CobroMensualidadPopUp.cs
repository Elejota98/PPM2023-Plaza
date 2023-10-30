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
    public partial class CobroMensualidadPopUp : Form
    {

        private string _Placa = string.Empty;

        public string Placa
        {
            get { return _Placa; }
            set { _Placa = value; }
        }
        public CobroMensualidadPopUp()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Placa = txtPlaca.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
