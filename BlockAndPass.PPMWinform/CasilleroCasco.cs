using BlockAndPass.PPMWinform.ByPServices;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
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
    public partial class CasilleroCasco : Form
    {
        ServicesByP cliente = new ServicesByP();
        private int _IdEstacionamiento = 0;
        private string _IdTransaccion = "";


        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        public CasilleroCasco(string iIdEstacionamiento, string idTransaccion)
        {
            InitializeComponent();
            _IdEstacionamiento = Convert.ToInt32(iIdEstacionamiento);
            _IdTransaccion = idTransaccion;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());
            if (clave != string.Empty)
            {
                InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorIdTransaccion(_IdTransaccion, Convert.ToString( _IdEstacionamiento));

                if (informacionTransaccion.Exito)
                {
                    //if (oCardResponse.cicloActivo)
                    //{
                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(_IdEstacionamiento.ToString(), informacionTransaccion.ModuloEntrada);
                        if (oCarrilxIdModuloResponse.Exito)
                        {
                            string sIdTransaccion = _IdTransaccion;


                            //VALIDAR CASILLERO DISPONIBLE
                            bool bLIBRE = false;
                            InfoTransaccionService lstInfo = cliente.ObtenerCasillero(_IdEstacionamiento.ToString());

                            for (int i = 0; i < lstInfo.LstTransac.Length; i++)
                            {
                                if (lstInfo.LstTransac[i].Casillero == tbCasillero.Text)
                                {
                                    bLIBRE = true;
                                    break;
                                }
                                if (lstInfo.LstTransac[i].Casillero == tbCasillero2.Text)
                                {
                                    bLIBRE = true;
                                    break;
                                }
                            }

                            if (!bLIBRE)
                            {

                                DialogResult result3 = MessageBox.Show("¿Desea adicionar la tarifa de casco?", "Aplicar tarifa casco", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result3 == DialogResult.Yes)
                                {
                                    AplicaCascoResponse oInfo = new AplicaCascoResponse();

                                    oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero.Text);

                                    if (tbCasillero2.Text != string.Empty)
                                    {
                                        oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero2.Text);
                                    }

                                    if (oInfo.Exito)
                                    {
                                        this.DialogResult = DialogResult.OK;
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.None;
                                        MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                                else
                                {
                                    this.DialogResult = DialogResult.None;
                                    MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }
                            else 
                            {
                                this.DialogResult = DialogResult.None;
                                MessageBox.Show("El casillero seleccionado se encuentra ocupado.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    //}
                    //else
                    //{
                    //    this.DialogResult = DialogResult.None;
                    //    MessageBox.Show("Tarjeta sin registro de entrada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    this.Close();
                    //}
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show("Error al leer la tarjeta.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }

            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void btn_Ok_Click_1(object sender, EventArgs e)
        {
            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());
            if (clave != string.Empty)
            {
                InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorIdTransaccion(_IdTransaccion, Convert.ToString(_IdEstacionamiento));

                if (informacionTransaccion.Exito)
                {
                    //if (oCardResponse.cicloActivo)
                    //{
                    CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(_IdEstacionamiento.ToString(), informacionTransaccion.ModuloEntrada);
                    if (oCarrilxIdModuloResponse.Exito)
                    {
                        string sIdTransaccion = _IdTransaccion;


                        //VALIDAR CASILLERO DISPONIBLE
                        bool bLIBRE = false;
                        InfoTransaccionService lstInfo = cliente.ObtenerCasillero(_IdEstacionamiento.ToString());

                        for (int i = 0; i < lstInfo.LstTransac.Length; i++)
                        {
                            if (lstInfo.LstTransac[i].Casillero == tbCasillero.Text)
                            {
                                bLIBRE = true;
                                break;
                            }
                            if (lstInfo.LstTransac[i].Casillero == tbCasillero2.Text)
                            {
                                bLIBRE = true;
                                break;
                            }
                        }

                        if (!bLIBRE)
                        {

                            DialogResult result3 = MessageBox.Show("¿Desea adicionar la tarifa de casco?", "Aplicar tarifa casco", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result3 == DialogResult.Yes)
                            {
                                AplicaCascoResponse oInfo = new AplicaCascoResponse();

                                oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero.Text);

                                if (tbCasillero2.Text != string.Empty)
                                {
                                    oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero2.Text);
                                }

                                if (oInfo.Exito)
                                {
                                    this.DialogResult = DialogResult.OK;
                                }
                                else
                                {
                                    this.DialogResult = DialogResult.None;
                                    MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                            else
                            {
                                this.DialogResult = DialogResult.None;
                                MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show("El casillero seleccionado se encuentra ocupado.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    //}
                    //else
                    //{
                    //    this.DialogResult = DialogResult.None;
                    //    MessageBox.Show("Tarjeta sin registro de entrada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    this.Close();
                    //}
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show("Error al leer la tarjeta.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }

            this.Close();
        }

        private void btn_Cancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
