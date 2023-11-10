using BlockAndPass.PPMWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class InfoClientePopUp : Form
    {

        ServicesByP cliente = new ServicesByP();

        private int _Nit = 0;
        public int Nit
        {
            get { return _Nit; }
            set { _Nit = value; }
        }

        public InfoClientePopUp()
        {
            InitializeComponent();
            lblRtaCliente.Text = "";

        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("¿Desea generar la factura electrónica al cliente con el nit " + bNitCliente.Text + " ?", "Facturación Electronica", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
                else
                {
                    this.DialogResult = DialogResult.None;
                }
            }

        private void bNitCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (ValidarNitCliente(Convert.ToInt32(bNitCliente.Text)))
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        public bool ValidarNitCliente(int nit)
        {
            bool ok = false;
            try
            {
                InfoClienteFacturacionElectronica rta = cliente.ValidarClientePorNit(nit);
                if (rta.Exito)
                {
                    if (rta.Nombre != string.Empty)
                    {
                        lblRtaCliente.Text = "¡ " + rta.Nombre.ToString() + " !";
                        lblRtaCliente.Update();
                        _Nit = nit;
                        btn_Ok.Visible = true;
                    }
                }
                else
                {
                    lblRtaCliente.Text = "El nit ingresado no está registrado como cliente";
                    lblRtaCliente.Update();
                    Thread.Sleep(3000);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                return ok;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        }
    }

