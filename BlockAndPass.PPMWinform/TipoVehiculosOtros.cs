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
    public partial class TipoVehiculosOtros : Form
    {

        private int _IdTipoVehiculo = 0;
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }
        public TipoVehiculosOtros()
        {
            InitializeComponent();

            List<KeyValuePair<int, string>> miLista = new List<KeyValuePair<int, string>>();
            miLista.Add(new KeyValuePair<int, string>(7, "Zorra 2 Llantas"));
            miLista.Add(new KeyValuePair<int, string>(8, "Zorra 4 Llantas"));
            miLista.Add(new KeyValuePair<int, string>(9, "Zorras Grandes"));
            miLista.Add(new KeyValuePair<int, string>(16, "Moto Carga"));
            miLista.Add(new KeyValuePair<int, string>(10, "Autos-Luv"));
            miLista.Add(new KeyValuePair<int, string>(11, "Camioneta"));
            miLista.Add(new KeyValuePair<int, string>(12, "NHR Sencilla"));
            miLista.Add(new KeyValuePair<int, string>(13, "NHR-2"));
            miLista.Add(new KeyValuePair<int, string>(14, "NPR-NQR-NHR-3"));
            miLista.Add(new KeyValuePair<int, string>(15, "Remision Transcarnes"));
            //miLista.Add(new KeyValuePair<int, string>(1, "Carro"));
            //miLista.Add(new KeyValuePair<int, string>(2, "Moto"));
            //miLista.Add(new KeyValuePair<int, string>(3, "Bicicleta"));

            cboTipoVehiculo.DataSource = miLista;
            cboTipoVehiculo.DisplayMember = "Value";
            cboTipoVehiculo.ValueMember = "Key";
        }

        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_Confirmar_Click(object sender, EventArgs e)
        {
            _IdTipoVehiculo = Convert.ToInt32(cboTipoVehiculo.SelectedValue.ToString());
            this.DialogResult =DialogResult.OK;
        }
    }
}
