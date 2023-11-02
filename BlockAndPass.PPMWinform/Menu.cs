using BlockAndPass.PPMWinform.ByPServices;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using BlockAndPass.PPMWinform.Tickets;
using EGlobalT.Device.SmartCard;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ZXing;
using System.Drawing.Imaging;
using System.Windows.Controls.Primitives;

namespace BlockAndPass.PPMWinform
{
    public partial class Menu : Form, IMessageFilter
    {
        CardResponse oCardResponse = new CardResponse();

        #region MoveWindow
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private HashSet<Control> controlsToMove = new HashSet<Control>();
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }
        #endregion

        #region Definiciones
        private bool isButtonPressed = false;
        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }

        private string _IdTransaccion = string.Empty;
        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        private string _CargoUsuario = string.Empty;
        public string CargoUsuario
        {
            get { return _CargoUsuario; }
            set { _CargoUsuario = value; }
        }

        private int _IdSede = 0;
        public int IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        private int _IdEstacionamiento = 0;
        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private int _IdTipoVehiculo = 0;
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }

        private int _IdCarrilEntrada = 0;
        public int IdCarrilEntrada
        {
            get { return _IdCarrilEntrada; }
            set { _IdCarrilEntrada = value; }
        }

        private string _IdTarjeta = string.Empty;

        private int _IdAutorizacion = 0;
        public int IdAutorizacion
        {
            get { return _IdAutorizacion; }
            set { _IdAutorizacion = value; }
        }

        private string _IdModuloEntrada = string.Empty;
        public string IdModuloEntrada
        {
            get { return _IdModuloEntrada; }
            set { _IdModuloEntrada = value; }
        }

        private string _imgUrl = string.Empty;
        public string imgUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }


        string moduloEntrada = ConfigurationManager.AppSettings["IdModulo"].ToString();

        string documentoUsuario = string.Empty;
        string nombresUsuario = string.Empty;
        ServicesByP cliente = new ServicesByP();
        LiquidacionService liquidacion = new LiquidacionService();
        System.Windows.Forms.Timer tmrHora = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer clickTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tmrTimeOutPago = new System.Windows.Forms.Timer();
        int cnt = 0;

        #endregion

        #region Eventos
        void tmrHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss tt");
            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Update();
            lblFecha.Update();


            lblTotalCarrosPrincipal.Text = Convert.ToString(ObtenerCantidadVehiculosActuales());
            lblTotalCarrosPrincipal.Update();
            lblTotalMotosPrincipal.Text = Convert.ToString(ObtenerCantidadMotosActuales());
            lblTotalMotosPrincipal.Update();

            dtpFechaIngreso.Value = DateTime.Now;

            if (tbValorAPagarCobrar.Text != "" || txtPlacaBuscar.Text != "" || tbCodigo.Text != "")
            {
                if (txtPlacaBuscar.Text != "" && tbCodigo.Text.Length < 17)
                {
                    txtPlacaBuscar.Focus();
                }
                else if (tbCodigo.Text.Length < 17)
                {
                    tbCodigo.Text = string.Empty;
                    tbCodigo.Focus();
                }
                else
                {

                }
            }
            else
            {
                tbCodigo.Text = string.Empty;
                tbCodigo.Focus();
            }

            if(txtPlacaBuscar.Text==string.Empty || txtPlacaBuscar.Text == "")
            {
                LimpiarDatosCobrar();
            }
        }

        void tmrTimeOutPago_Tick(object sender, EventArgs e)
        {
            cnt++;

            //int valorfinal = Convert.toInlblTiempoFuera.Text.Replace("Usted dispone de ","").Replace(" segundos para pagar.","");

            //lblTiempoFuera.Text = "Usted dispone de " + (15 - cnt) + " segundos para pagar.";

            if (cnt >= 15)
            {
                tmrTimeOutPago.Stop();
                RestablecerPPM();
                //panelPagar.Enabled = false;
                //btn_Copia.Enabled = true;
            }
        }

        private void tbPlaca_TextChanged(object sender, EventArgs e)
        {
            string texto = tbPlaca.Text;

            try
            {
                if (texto.Length <= 6 && char.IsLetter(texto[texto.Length - 1]))
                {
                    tbTipoVehiculo.Text = "Moto";
                    _IdTipoVehiculo = 2;
                }

                else if (texto.Length >= 6 && char.IsDigit(texto[texto.Length - 1]))
                {
                    tbTipoVehiculo.Text = "Carro";
                    _IdTipoVehiculo = 1;
                }
            }
            catch (Exception ex)
            {

                tbTipoVehiculo.Text = "";
                tbPlaca.Focus();

            }


        }
        #endregion

        #region Funciones 
        public int ObtenerCantidadVehiculosActuales()
        {
            int totalCarros = 0;
            InfoCantidadVehiculosActualesResponse cantidad = cliente.ObtenerCantidadVehiculosActuales();

            totalCarros = cantidad.Cantidad;
            return totalCarros;

        }

        public int ObtenerCantidadMotosActuales()
        {
            int totalMotos = 0;
            InfoCantidadMotosActualesResponse cantidad = cliente.ObtenerCantidadMotosActuales();

            totalMotos = cantidad.Cantidad;
            return totalMotos;

        }
        private void RestablecerPPM()
        {
            //tbHoraIngreso.Text = string.Empty;
            liquidacion = new LiquidacionService();
            tmrTimeOutPago.Stop();
            //panelPagar.Enabled = false;
            //btn_Copia.Enabled = true;
            //chbEstacionamiento.Checked = false;
            ckMensualidadDocumento.Checked = false;
            tbCambioCobrar.Text = "$0";
            //tbIdTarjeta.Text = string.Empty;
            //tbIdTransaccion.Text = string.Empty;
            tbPlaca.Text = string.Empty;
            txtPlacaBuscar.Text = string.Empty;
            tbRecibidoCobrar.Text = "0";
            tbValorAPagarCobrar.Text = string.Empty;
            //lblTiempoFuera.Text = string.Empty;
            tbTiempoCobrar.Text = string.Empty;
            //tbHoraPago.Text = string.Empty;
            tbCasilleroCobrar.Text = string.Empty;
            tbCodigo.Text = string.Empty;
            txtPlacaBuscar.Text = string.Empty;
            ckMensualidadDocumento.Checked = false;
            //txtPlaca.Text = string.Empty;
            _IdTransaccion = string.Empty;
        }

        #endregion

        #region General
        public bool CargaImagenes()
        {
            bool ok = false;
            try
            {
                Imagen_Logo.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\Logo.png"));
                Imagen_Principal.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\Principal.png"));
                Imagen_Principal.BackgroundImageLayout = ImageLayout.Stretch;
                Imagen_FondoCobrar.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\ImagenFondoCobrar.png"));
                Imagen_FondoEntrada.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\ImagenFondoIngreso.png"));

                //Botones


            }
            catch (Exception ex)
            {

                ok = false;
            }
            return ok;
        }

        #region Funcion Reestablecer los botones

        public void ReestablecerBotonesLateralIzquierdo()
        {
            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_Principal.png");
            btn_Principal.Text = "";
            btn_Principal.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Entrada.BackgroundImage = Image.FromFile(@"Media\Png\btn_Entrada.png");
            btn_Entrada.Text = "";
            btn_Entrada.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Cobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cobrar.png");
            btn_Cobrar.Text = "";
            btn_Cobrar.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void ReestablecerBotonesLateralDerechoPrincipal()
        {
            btn_Arqueo.BackgroundImage = Image.FromFile(@"Media\Png\btn_Arqueo.png");
            btn_Arqueo.Text = "";
            btn_Arqueo.BackgroundImageLayout = ImageLayout.Stretch;

            btn_SaldoEnLinea.BackgroundImage = Image.FromFile(@"Media\Png\btn_SaldoEnLinea.png");
            btn_SaldoEnLinea.Text = "";
            btn_SaldoEnLinea.BackgroundImageLayout = ImageLayout.Stretch;

            btn_FacturaContingencia.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaContingencia.png");
            btn_FacturaContingencia.Text = "";
            btn_FacturaContingencia.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Mensualidades.BackgroundImage = Image.FromFile(@"Media\Png\btn_Mensualidades.png");
            btn_Mensualidades.Text = "";
            btn_Mensualidades.BackgroundImageLayout = ImageLayout.Stretch;

            btn_ReportePatios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReportePatios.png");
            btn_ReportePatios.Text = "";
            btn_ReportePatios.BackgroundImageLayout = ImageLayout.Stretch;

        }

        public void ReestablecerBotonesLateralDerechoCobro()
        {
            btn_Cortesia.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cortesia.png");
            btn_Cortesia.Text = "";
            btn_Cortesia.BackgroundImageLayout = ImageLayout.Stretch;

            btn_TarifasEspeciales.BackgroundImage = Image.FromFile(@"Media\Png\btn_TarifasEspeciales.png");
            btn_TarifasEspeciales.Text = "";
            btn_TarifasEspeciales.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Convenios.BackgroundImage = Image.FromFile(@"Media\Png\btn_Convenios.png");
            btn_Convenios.Text = "";
            btn_Convenios.BackgroundImageLayout = ImageLayout.Stretch;

            btn_FacturaElectronica.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaElectronica.png");
            btn_FacturaElectronica.Text = "";
            btn_FacturaElectronica.BackgroundImageLayout = ImageLayout.Stretch;


            btn_Reposicion.BackgroundImage = Image.FromFile(@"Media\Png\btn_Reposicion.png");
            btn_Reposicion.Text = "";
            btn_Reposicion.BackgroundImageLayout = ImageLayout.Stretch;


            btn_Cascos.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cascos.png");
            btn_Cascos.Text = "";
            btn_Cascos.BackgroundImageLayout = ImageLayout.Stretch;

            btn_ConfirmarCobro.BackgroundImage = Image.FromFile(@"Media\Png\btn_Confirmar.png");
            btn_ConfirmarCobro.Text = "";
            btn_ConfirmarCobro.BackgroundImageLayout = ImageLayout.Stretch;

        }

        public void ReestablecerBotonesLateralDerechoEntradas()
        {
            btn_Carro.BackgroundImage = Image.FromFile(@"Media\Png\btn_Carro.png");
            btn_Carro.Text = "";
            btn_Carro.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Motos.BackgroundImage = Image.FromFile(@"Media\Png\btn_Moto.png");
            btn_Motos.Text = "";
            btn_Motos.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Otros.BackgroundImage = Image.FromFile(@"Media\Png\btn_Otro.png");
            btn_Otros.Text = "";
            btn_Otros.BackgroundImageLayout = ImageLayout.Stretch;


        }

        public void ReestablecerBotonInferior()
        {
            btn_Cerrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cerrar.png");
            btn_Cerrar.Text = "";
            btn_Cerrar.BackgroundImageLayout = ImageLayout.Stretch;
        }
        #endregion

        #region Botones 

        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralIzquierdo();
            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_PrincipalPresionado.png");
            tabPrincipal.SelectedTab = tabMenuPrincipal;
        }

        private void btn_Entrada_Click(object sender, EventArgs e)
        {
            string rta = "";
            LimpiarDatosEntrada();
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonesLateralDerechoEntradas();
            btn_Entrada.BackgroundImage = Image.FromFile(@"Media\Png\btn_EntradaPresionado.png");
            tabPrincipal.SelectedTab = tabEntrada;
            rta = ValidarIngreso();
            if (rta.Equals("OK"))
            {
                tbPlaca.Focus();
                tbModuloIngreso.Text = cbPPM.SelectedValue.ToString();

            }
            else
            {
                MessageBox.Show(rta.ToString(), "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        private void btn_Cobrar_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonesLateralDerechoCobro();
            btn_Cobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_CobrarPresionado.png");
            tabPrincipal.SelectedTab = tabCobrar;
            LimpiarDatosCobrar();

        }

        private void btn_Arqueo_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralIzquierdo();
            btn_ReportePatios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ArqueoPresionado.png");
            tabPrincipal.SelectedTab = tabArqueo;
        }

        private void btn_Arqueo_Click_1(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_Arqueo.BackgroundImage = Image.FromFile(@"Media\Png\btn_ArqueoPresionado.png");
            //tabPrincipal.SelectedTab = tabArqueo;

            DialogResult oDialogResult = MessageBox.Show("¿Esta seguro que desea realizar el arqueo?", "Arqueo PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (oDialogResult == DialogResult.Yes)
            {
                RegistrarArqueoResponse rgis = cliente.RegistrarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), _DocumentoUsuario);
                if (rgis.Exito)
                {
                    ArqueoPopUp popup = new ArqueoPopUp(rgis.IdArqueo.ToString());
                    popup.ShowDialog();
                    if (popup.DialogResult == DialogResult.OK)
                    {
                        ConfirmarArqueoResponse confirmacionArqueo = cliente.ConfirmarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), rgis.IdArqueo.ToString(), popup.Valor.ToString(), _DocumentoUsuario);
                        if (confirmacionArqueo.Exito)
                        {
                            ImprimirArqueo(rgis.IdArqueo.ToString());
                        }
                        else
                        {
                            MessageBox.Show(confirmacionArqueo.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            StringBuilder sb = new StringBuilder();
                            sb.Append(confirmacionArqueo.ErrorMessage);
                            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), sb.ToString());
                            sb.Clear();
                        }
                    }
                    else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        MessageBox.Show("Operacion cancelada por el usuario", "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al procesar ventana carga", "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(rgis.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_SaldoEnLinea_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_SaldoEnLinea.BackgroundImage = Image.FromFile(@"Media\Png\btn_SaldoEnLineaPresionado.png");
            //tabPrincipal.SelectedTab = tabSaldoEnLinea;
        }

        private void btn_FacturaContingencia_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_FacturaContingencia.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaContingenciaPresionado.png");
            //tabPrincipal.SelectedTab = tabFacturaContingencia;
        }

        private void btn_Mensualidades_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_Mensualidades.BackgroundImage = Image.FromFile(@"Media\Png\btn_MensualidadesPresionado.png");
            //tabPrincipal.SelectedTab = tabMensualidades;
        }

        private void btn_ReportePatios_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_ReportePatios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReportePatiosPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btn_Carro_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoEntradas();
            btn_Carro.BackgroundImage = Image.FromFile(@"Media\Png\btn_CarroPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
            if (tbTipoVehiculo.Text != "")
            {
                if (tbTipoVehiculo.Text != "Carro")
                {
                    _IdTipoVehiculo = 1;
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Carro", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Carro";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                }
                else
                {
                    ReestablecerBotonesLateralDerechoEntradas();

                }
            }




        }

        private void btn_Motos_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoEntradas();
            btn_Motos.BackgroundImage = Image.FromFile(@"Media\Png\btn_MotoPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
            if (tbTipoVehiculo.Text != "")
            {
                if (tbTipoVehiculo.Text != "Moto")
                {
                    _IdTipoVehiculo = 2;
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Moto", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Moto";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();

                }
                else
                {
                    ReestablecerBotonesLateralDerechoEntradas();
                }
            }
        }

        private void btn_Otros_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoEntradas();
            btn_Otros.BackgroundImage = Image.FromFile(@"Media\Png\btn_OtroPresionado.png");
            TipoVehiculosOtros popup = new TipoVehiculosOtros();
            popup.ShowDialog();

            if (popup.DialogResult == DialogResult.OK)
            {
                if (popup.IdTipoVehiculo == 7)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Zorra 2 Llantas", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Zorra 2 Llantas";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if(popup.IdTipoVehiculo == 8)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Zorra 4 Llantas", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Zorra 4 Llantas";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 9)
                {
                  
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Zorras Grandes", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Zorras Grandes";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 16)
                {
                   
                     MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Moto Carga", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Moto Carga";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 10)
                {  
                     MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Autos - Luv", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Autos - Luv";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 11)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Camioneta", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Camioneta";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 12)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a NHR Sencilla", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "NHR Sencilla";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 13)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a NHR-2", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "NHR-2";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 14)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a NPR-NQR-NHR-3", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "NPR-NQR-NHR-3";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }
                if (popup.IdTipoVehiculo == 15)
                {
                    MessageBox.Show("Se cambio el vehículo con placa " + tbPlaca.Text + " a Remision Transcarnes", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbTipoVehiculo.Text = "Remision Transcarnes";
                    btnConfirmaIngreso.Focus();
                    ReestablecerBotonesLateralDerechoEntradas();
                    _IdTipoVehiculo = popup.IdTipoVehiculo;

                }

            }
            else if(popup.DialogResult == DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoEntradas();
            }
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btn_Cascos_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoEntradas();
            btn_Cascos.BackgroundImage = Image.FromFile(@"Media\Png\btn_CascosPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;

            CascosPoUp popup = new CascosPoUp(cbEstacionamiento.SelectedValue.ToString(), _IdTransaccion);
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                MessageBox.Show("Tarifa casco creada con EXITO", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show("Operacion cancelada por el usuario", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al procesar ventana crear tarifa", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btn_Cortesia_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_Cortesia.BackgroundImage = Image.FromFile(@"Media\Png\btn_CortesiaPresionado.png");
            CortesiaPopUp popup = new CortesiaPopUp(cbEstacionamiento.SelectedValue.ToString());
            popup.ShowDialog();


            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
            if (clave != string.Empty)
            {
                //CardResponse oCardResponse = GetCardInfo(clave);
                //if (!oCardResponse.error)
                //{
                //if (oCardResponse.reposicion)
                //{
                //    Cargando(false);
                //    MessageBox.Show("NO se puede aplicar cortesía a una REPOSICION", "Error Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}  
                if (_IdTransaccion != string.Empty && tbCodigo.Text!=string.Empty)
                {
                    InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorIdTransaccion(_IdTransaccion, cbEstacionamiento.SelectedValue.ToString());
                    if (informacionTransaccion.Exito)
                    {
                        //yyyyMMddHHmmssce

                        string sIdTransaccion = informacionTransaccion.IdTransaccion;

                        //tbIdTarjeta.Text = oCardResponse.idCard;
                        InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                        //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                        if (oInfoTransaccionService.Exito)
                        {
                            AplicarCortesiaResponse oAplicarCortesiaResponse = cliente.AplicarLaCortesia(cbEstacionamiento.SelectedValue.ToString(), popup.Observacion, popup.Motivo.ToString(), oInfoTransaccionService.IdTransaccion, _DocumentoUsuario);
                            if (oAplicarCortesiaResponse.Exito)
                            {
                                AplicarCortesiaTransaccionResponse oAplicarCortesiaTransaccionResponse = cliente.AplicarCotesiaTransaccion(sIdTransaccion, Convert.ToInt32(popup.Motivo.ToString()));
                                if (oAplicarCortesiaTransaccionResponse.Exito)
                                {
                                    //oCardResponse = AplicarCortesia(clave);
                                    if (tbCasilleroCobrar.Text != string.Empty)
                                    {
                                        AplicaCascoResponse oInfo = cliente.LiberarCasco(sIdTransaccion);
                                        if (oInfo.Exito)
                                        {
                                            if (oAplicarCortesiaResponse.Exito)
                                            {

                                                MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                LeerInfoPorPlaca();

                                            }
                                            else
                                            {
                                                MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            //}
                                            //else 
                                            //{
                                            //    Cargando(false);
                                            //    MessageBox.Show(oCardResponse.errorMessage, "Error al liberar casillero PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //}
                                        }
                                        else
                                        {
                                            MessageBox.Show(oAplicarCortesiaResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {

                                        if (oAplicarCortesiaResponse.Exito)
                                        {

                                            MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        }
                                        else
                                        {
                                            MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }

                                }
                                else
                                {
                                    MessageBox.Show(oAplicarCortesiaTransaccionResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else
                        {
                            MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //}
                    //else
                    //{
                    //    Cargando(false);
                    //    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
               else if (txtPlacaBuscar.Text != string.Empty || txtPlacaBuscar.Text!= "------")
                {
                    InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorPlaca(txtPlacaBuscar.Text, cbEstacionamiento.SelectedValue.ToString());
                    if (informacionTransaccion.Exito)
                    {
                        //yyyyMMddHHmmssce

                        string sIdTransaccion = informacionTransaccion.IdTransaccion;

                        //tbIdTarjeta.Text = oCardResponse.idCard;
                        InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                        //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                        if (oInfoTransaccionService.Exito)
                        {
                            AplicarCortesiaResponse oAplicarCortesiaResponse = cliente.AplicarLaCortesia(cbEstacionamiento.SelectedValue.ToString(), popup.Observacion, popup.Motivo.ToString(), oInfoTransaccionService.IdTransaccion, _DocumentoUsuario);
                            if (oAplicarCortesiaResponse.Exito)
                            {
                                AplicarCortesiaTransaccionResponse oAplicarCortesiaTransaccionResponse = cliente.AplicarCotesiaTransaccion(sIdTransaccion, Convert.ToInt32(popup.Motivo.ToString()));
                                if (oAplicarCortesiaTransaccionResponse.Exito)
                                {
                                    //oCardResponse = AplicarCortesia(clave);
                                    if (tbCasilleroCobrar.Text != string.Empty)
                                    {
                                        AplicaCascoResponse oInfo = cliente.LiberarCasco(sIdTransaccion);
                                        if (oInfo.Exito)
                                        {
                                            if (oAplicarCortesiaResponse.Exito)
                                            {

                                                MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                LeerInfoPorPlaca();

                                            }
                                            else
                                            {
                                                MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            //}
                                            //else 
                                            //{
                                            //    Cargando(false);
                                            //    MessageBox.Show(oCardResponse.errorMessage, "Error al liberar casillero PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //}
                                        }
                                        else
                                        {
                                            MessageBox.Show(oAplicarCortesiaResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {

                                        if (oAplicarCortesiaResponse.Exito)
                                        {

                                            MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            LeerInfoPorPlaca();

                                        }
                                        else
                                        {
                                            MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }

                                }
                                else
                                {
                                    MessageBox.Show(oAplicarCortesiaTransaccionResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else
                        {
                            MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //}
                    //else
                    //{
                    //    Cargando(false);
                    //    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }

                else
                {
                    MessageBox.Show("Error al procesar ventana cortesia", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show("Operacion cancelada por el usuario", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al procesar ventana cortesia", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

        private void btn_TarifasEspeciales_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_TarifasEspeciales.BackgroundImage = Image.FromFile(@"Media\Png\btn_TarifasEspecialesPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btn_Convenios_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_Convenios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ConveniosPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btn_FacturaElectronica_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_FacturaElectronica.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaElectronicaPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btn_Reposicion_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_Reposicion.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReposicionPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;
        }

        private void btnConfirmaIngreso_Click(object sender, EventArgs e)
        {
            if (tbPlaca.Text != string.Empty)
            {
                if (chbAutorizado.Checked)
                {

                    //DialogResult result3 = MessageBox.Show("¿Desea crear una entrada de autorizado? \n TENGA EN CUENTA QUE NO PODRA CAMBIAR ESTA SELECCION", "Crear Entrada", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    //if (result3 == DialogResult.Yes)
                    //{
                    //CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, cbEntrada.Text, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), Convert.ToInt32(_IdAutorizacion).ToString());
                    CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, tbModuloIngreso.Text, tbPlaca.Text, dtpFechaIngreso.Value, Convert.ToString(_IdTipoVehiculo), Convert.ToInt32(_IdAutorizacion).ToString());

                    if (oInfo.Exito)
                    {
                        LimpiarDatosEntrada();
                        tbPlaca.Enabled = true;
                        MessageBox.Show("Entrada realizada correctamente", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //}
                    //else
                    //{
                    //    //MessageBox.Show("Coloque la tarjeta en el lector y presione continuar.", "Crear Entrada", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    //    //string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                    //    //CardResponse oCardResponse = CreateAuthEntry(clave, tbPlaca.Text, cbEntrada.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString(), _IdTarjeta);
                    //    //if (!oCardResponse.error)
                    //    //{
                    //    CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, tbModuloIngreso.Text, tbPlaca.Text, dtpFechaIngreso.Value, Convert.ToString(_IdTipoVehiculo), Convert.ToInt32(_IdAutorizacion).ToString());

                    //    if (oInfo.Exito)
                    //    {
                    //        this.DialogResult = DialogResult.OK;
                    //    }
                    //    else
                    //    {
                    //        this.DialogResult = DialogResult.None;
                    //        MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM Clienete Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    //    this.DialogResult = DialogResult.None;
                    //    //    MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    //}
                    //}
                }
                else
                {
                    //MessageBox.Show("Coloque la tarjeta en el lector y presione continuar.", "Crear Entrada", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    //string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                    //CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, cbEntrada.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString());
                    //if (!oCardResponse.error)
                    //{

                    CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, tbModuloIngreso.Text, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);

                    if (oInfo.Exito)
                    {
                        ImprimirTicketEntrada();
                        LimpiarDatosEntrada();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //}
                    //else
                    //{
                    //    this.DialogResult = DialogResult.None;
                    //    MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }

            }
            else
            {
                LimpiarDatosEntrada();
            }
        }

        private void tbPlaca_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                tbPlaca.Focus();
            }
            if (tbPlaca.Text != string.Empty || tbPlaca.Text != "")
            {
                if (e.KeyChar == (char)13)
                {
                    if (EsAutorizado(tbPlaca.Text))
                    {
                        if (!TieneVigencia(tbPlaca.Text))
                        {
                            this.DialogResult = DialogResult.None;
                            DialogResult result3 = MessageBox.Show("El autorizado tiene la vigencia vencida.\n ¿Desea registrar la entrada por horas?", "Crear Entrada", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            //MessageBox.Show("El autorizado tiene la vigencia vencida.", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (result3 == DialogResult.Yes)
                            {
                                CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, tbModuloIngreso.Text, tbPlaca.Text, dtpFechaIngreso.Value, Convert.ToString(_IdTipoVehiculo), string.Empty);
                                if (oInfo.Exito)
                                {
                                    ImprimirTicketEntrada();
                                    LimpiarDatosEntrada();
                                }
                                else
                                {
                                    MessageBox.Show("Error al crear la entrada", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LimpiarDatosEntrada();

                                }
                            }
                            else
                            {
                                LimpiarDatosEntrada();
                            }
                            //this.Close();
                        }
                        else
                        {
                            if (TieneTransaccionAbierta(tbPlaca.Text))
                            {
                                //MessageBox.Show("El autorizado tiene una transaccion pendiente de salida.", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                DialogResult result4 = MessageBox.Show("¿El autorizado tiene una salida pendiente, ¿Desea registrar la salida? " + oCardResponse.placa, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result4 == DialogResult.Yes)
                                {
                                    //bContinuarLiquidacion = false;
                                    CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), tbModuloIngreso.Text);
                                    if (oCarrilxIdModuloResponse.Exito)
                                    {
                                        //string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                        //CardResponse oCardResponseExit = new CardResponse();
                                        //oCardResponseExit = ExitCardAutho(clave, oCardResponse.idCard);
                                        //if (!oCardResponseExit.error)
                                        //{
                                            CreaSalidaResponse resp = cliente.CrearSalida2(cbEstacionamiento.SelectedValue.ToString(), tbPlaca.Text, _IdTransaccion, oCarrilxIdModuloResponse.Carril.ToString(), tbModuloIngreso.Text, _IdTarjeta);

                                            if (resp.Exito)
                                            {
                                                MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            LimpiarDatosEntrada();
                                            tbPlaca.Focus();
                                            tbPlaca.Enabled = true;
                                        }
                                            else
                                            {
                                                this.DialogResult = DialogResult.None;
                                                MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LimpiarDatosEntrada();
                                            tbPlaca.Focus();
                                            tbPlaca.Enabled = true;
                                        }
                                        //}
                                        //else
                                        //{
                                        //    Cargando(false);
                                        //    MessageBox.Show(oCardResponseExit.errorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //}

                                    }
                                    else
                                    {
                                        MessageBox.Show("No fue posible encontrar el carril asociado al modulo.", "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                               
                            }
                            else
                            {
                                chbAutorizado.Checked = true;
                                //chbAutorizado.Visible = true;
                                chbAutorizado.Enabled = false;
                                tbPlaca.Enabled = false;
                                btnConfirmaIngreso.Focus();



                            }
                        }
                    }
                    else
                    {
                        chbAutorizado.Checked = false;
                        chbAutorizado.Visible = false;
                        btnConfirmaIngreso.Focus();

                    }
                }
            }
        }

        private void tbRecibidoCobrar_TextChanged(object sender, EventArgs e)
        {
            tbRecibidoCobrar.Focus();
            tmrHora.Stop();
            clickTimer.Start();
            //MessageBox.Show(tbCambio.Text);
            Int64 pagar = Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", ""));
            //Int64 recibido = Convert.ToInt64(tbRecibido.Text.Replace("$", ""));
            Int64 cambio = Convert.ToInt64(tbCambioCobrar.Text.Replace("$", "").Replace(".", ""));

            Int64 recibido = 0;
            try
            {
                if (Int64.TryParse(tbRecibidoCobrar.Text.Replace("$", "").Replace(".", ""), out recibido))
                {
                    tbRecibidoCobrar.Text = "$" + string.Format("{0:#,##0.##}", recibido);

                    if (recibido > pagar)
                    {
                        //MessageBox.Show(recibido + " " + pagar);
                        tbCambioCobrar.Text = "$" + string.Format("{0:#,##0.##}", (recibido - pagar));
                    }
                    else
                    {
                        tbCambioCobrar.Text = "$0";
                    }
                }
                else
                {
                    tbRecibidoCobrar.Text = string.Empty;
                }
            }
            catch (Exception exe)
            {
                //MessageBox.Show(exe.InnerException.ToString() + " " + exe.Message + " " + );
            }

            tbRecibidoCobrar.SelectionStart = tbRecibidoCobrar.Text.Length; // add some logic if length is 0
            tbRecibidoCobrar.SelectionLength = 0;
        }

        private void tbRecibidoCobrar_MouseClick(object sender, MouseEventArgs e)
        {
            tmrHora.Stop();
            clickTimer.Start();
        }

        private void btn_ConfirmarCobro_Click(object sender, EventArgs e)
        {
            btn_ConfirmarCobro.BackgroundImage = Image.FromFile(@"Media\Png\btn_ConfirmarPresionado.png");
            tmrHora.Stop();
            clickTimer.Start();
            if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
            {
                cnt = 0;
                tmrTimeOutPago.Stop();
                //Cargando(true);
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    #region Estacionamiento
                    if (!ckMensualidadDocumento.Checked)
                    {
                        //CardResponse oCardResponse = new CardResponse();
                        //oCardResponse = PayCard(clave, tbIdTarjeta.Text, cbPPM.SelectedValue.ToString(), DateTime.ParseExact(tbHoraPago.Text, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture));
                        //if (!oCardResponse.error)
                        //{
                        string pagosFinal = "";
                        double sumTotalPagar = 0;
                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            if (pagosFinal == string.Empty)
                            {
                            }
                            else
                            {
                                pagosFinal += ',';
                            }
                            sumTotalPagar += item.Total;
                            pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                        }
                        string fechaPago = Convert.ToString(DateTime.Now);

                        InfoPagoNormalService pagoNormal = cliente.PagarClienteParticular(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), _IdTransaccion, cbPPM.SelectedValue.ToString(), fechaPago, sumTotalPagar.ToString(), documentoUsuario);

                        if (pagoNormal.Exito)
                        {
                            ImprimirPagoNormal(_IdTransaccion);
                            LimpiarDatosCobrar();
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //}
                        //else
                        //{
                        //    Cargando(false);
                        //    MessageBox.Show(oCardResponse.errorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                    }
                    #endregion
                    #region Mensualidad
                    else
                    {
                        //Mensualidad
                        string pagosFinal = "";
                        double sumTotalPagar = 0;
                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            sumTotalPagar += item.Total;
                            pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                        }

                        InfoPagoMensualidadService pagoNormal = cliente.PagarMensualidad(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), txtPlacaBuscar.Text, documentoUsuario);

                        if (pagoNormal.Exito)
                        {
                            CardResponse oCardResponse = new CardResponse();
                            oCardResponse = LimpiarReposicion(clave);
                            if (!oCardResponse.error)
                            {
                                ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                                LimpiarDatosCobrar();
                            }

                            else if (ckMensualidadDocumento.Checked == true && txtPlacaBuscar.Text != null)
                            {

                                ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                                LimpiarDatosCobrar();
                            }
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
            }
            else
            {

                DialogResult result3 = MessageBox.Show("Valor a pagar = 0 ¿Desea crear la salida para la transaccion: " + _IdTransaccion, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    CreaSalidaResponse resp = cliente.CrearSalida3(cbEstacionamiento.SelectedValue.ToString(), _IdTransaccion, cbPPM.SelectedValue.ToString());

                    if (resp.Exito)
                    {
                        MessageBox.Show("Salida registrada exitosamente.", "Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Valor a Pagar = 0, no se registro la salida", "Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                //Cargando(false);

                RestablecerPPM();
            }
        }

        private void txtPlacaBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                LeerInfoPorPlaca();
            }
        }

        private void tbCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

                if (tbCodigo.Text != string.Empty && tbCodigo.Text.Length <= 17)
                {
                    LeerInfoPorPlaca();
                    //_IdTransaccion = tbCodigo.Text;
                }
                else
                {
                    tbCodigo.Text = string.Empty;
                    LeerInfoPorPlaca();

                }
            }
        }

        private void txtPlacaBuscar_MouseClick(object sender, MouseEventArgs e)
        {
            tmrHora.Stop();
            clickTimer.Start();
        }

        private void txtPlacaBuscar_TextChanged(object sender, EventArgs e)
        {
            string texto = txtPlacaBuscar.Text;

            try
            {
                if (texto.Length == 0)
                {

                    LimpiarDatosCobrar();

                }
            }
            catch (Exception ex)
            {
                LimpiarDatosCobrar();
            }
        }



        private void tbRecibidoCobrar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (tbCodigo.Text != string.Empty || txtPlacaBuscar.Text != "")
                {
                    btn_ConfirmarCobro_Click(btn_ConfirmarCobro, EventArgs.Empty);
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show("Error Crear Entrada PPM", "Crear Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

            }
        }

        #endregion

        #region Pantallas

        #region Entradas

        public string ValidarIngreso()
        {
            string rta = string.Empty;
            _IdSede = Convert.ToInt32(cbSede.SelectedValue);
            _IdEstacionamiento = Convert.ToInt32(cbEstacionamiento.SelectedValue);
            _IdModuloEntrada = ConfigurationManager.AppSettings["IdModulo"].ToString();
            CarrilEntradaXEntradaResponse oInfo = cliente.ObtenerListaCarrilEntradaxEstacionamiento(IdSede, _IdEstacionamiento, _IdModuloEntrada);
            if (!oInfo.Exito)
            {
                rta = "No encontro entradas asociadas al estacionamiento id = " + _IdEstacionamiento;
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstCarrillesEntrada)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbEntrada.DataSource = dataSource;
                this.cbEntrada.DisplayMember = "Name";
                this.cbEntrada.ValueMember = "Value";

                rta = "OK";
            }

            return rta;

        }

        private bool EsAutorizado(string sPlaca)
        {
            bool bResultado = false;

            AutorizadoxPlacaResponse resp = cliente.BuscarAutorizadoxPlaca(tbPlaca.Text);

            if (resp.Exito)
            {
                _IdTarjeta = resp.IdTarjeta;
                _IdAutorizacion = Convert.ToInt32(resp.IdAutorizacion);
                tbNombreAutortizado.Text = resp.NombreApellidos.ToString().Trim();
                bResultado = true;
            }

            return bResultado;
        }
        private bool TieneTransaccionAbierta(string sPlaca)
        {
            bool bResultado = false;

            VerificaTransaccionAbiertaAutorizadoResponse resp = cliente.VerificarTransaccionAbiertaAutorizado(_IdTarjeta);
            if (resp.Exito)
            {
                bResultado = true;
            }

            return bResultado;
        }
        private bool TieneVigencia(string sPlaca)
        {
            bool bResultado = false;

            VerificaVigenciaAutorizadoResponse resp = cliente.VerificarVigenciaAutorizado(_IdTarjeta);
            if (resp.Exito)
            {
                bResultado = true;
            }

            return bResultado;
        }
        public void LimpiarDatosEntrada()
        {
            tbPlaca.Text = "";
            tbNombreAutortizado.Text = "";
            tbPlaca.Focus();
            //tbPlaca.Enabled = true;
        }
        #endregion

        #region Cobrar

        public void LeerInfoPorPlaca()
        {
            //liquidacion = cliente.ConsultarValorPagar(true, true, 1, "0", "0EEEC6CB");

            cnt = 0;
            //Cargando(true);
            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
            if (txtPlacaBuscar.Text != string.Empty && !ckMensualidadDocumento.Checked)
            {
                CardResponse oCardResponse = GetCardInfo(clave);

                InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorPlaca(txtPlacaBuscar.Text, cbEstacionamiento.SelectedValue.ToString());


                //if (!oCardResponse.error)
                //{
                bool bContinuarLiquidacion = true;

                #region Estacionamiento
                if (informacionTransaccion.Exito)
                {
                    _IdTransaccion = informacionTransaccion.IdTransaccion;
                    if (informacionTransaccion.IdTransaccion == string.Empty)
                    {
                        DialogResult result3 = MessageBox.Show("¿Desea crear una salida de autorizado? \n PRESIONE NO PARA CONTINUAR CON LA LIQUIDACION.", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result3 == DialogResult.Yes)
                        {

                            DialogResult result4 = MessageBox.Show("¿Esta seguro que desea crear la salida para la placa: " + oCardResponse.placa, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result4 == DialogResult.Yes)
                            {
                                bContinuarLiquidacion = false;
                                CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                                if (oCarrilxIdModuloResponse.Exito)
                                {
                                    string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                    CardResponse oCardResponseExit = new CardResponse();
                                    oCardResponseExit = ExitCardAutho(clave, oCardResponse.idCard);
                                    if (!oCardResponseExit.error)
                                    {
                                        CreaSalidaResponse resp = cliente.CrearSalida2(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.placa, sIdTransaccion, oCarrilxIdModuloResponse.Carril.ToString(), oCardResponse.moduloEntrada, oCardResponse.idCard);

                                        if (resp.Exito)
                                        {
                                            MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            this.DialogResult = DialogResult.None;
                                            MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        //Cargando(false);
                                        MessageBox.Show(oCardResponseExit.errorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("No fue posible encontrar el carril asociado al modulo.", "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }


                    if (bContinuarLiquidacion)
                    {
                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), informacionTransaccion.ModuloEntrada.ToString());
                        if (oCarrilxIdModuloResponse.Exito)
                        {
                            //yyyyMMddHHmmssce

                            string sIdTransaccion = informacionTransaccion.IdTransaccion;

                            //tbIdTarjeta.Text = oCardResponse.idCard;
                            InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                            InfoTransaccionService lstInfo = cliente.ConsultarCascosxId(sIdTransaccion);

                            //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                            if (oInfoTransaccionService.Exito)
                            {
                                if (oInfoTransaccionService.IdTransaccion != string.Empty)
                                {
                                    //tbIdTransaccion.Text = oInfoTransaccionService.IdTransaccion;

                                    //tbPlaca.Text = informacionTransaccion.PlacaEntrada;

                                    //if (oCardResponse.codeAutorizacion1 != 0)
                                    //{
                                    //    cliente.AplicarConvenios(oInfoTransaccionService.IdTransaccion, oCardResponse.codeAutorizacion1, oCardResponse.codeAutorizacion2, oCardResponse.codeAutorizacion3);
                                    //}

                                    DateTime dtDespues = DateTime.Now;
                                    DateTime dtAntes = new DateTime();

                                    //MessageBox.Show(oInfoTransaccionService.HoraTransaccion);

                                    oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("a. m.", "a.m.");
                                    oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("p. m.", "p.m.");

                                    //try
                                    //{
                                    //    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                    //    {
                                    //        if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                    //        {
                                    //            if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                    //            {
                                    //                if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                    //                {
                                    //                    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy H':'mm':'ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                    //                    {
                                    //                        dtAntes = DateTime.ParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture);
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //catch (Exception exe)
                                    //{
                                    //    MessageBox.Show("Tiempo de permanencia no disponible, continue con el pago e informe al desarrollador->" + exe.Message + " / " + oInfoTransaccionService.HoraTransaccion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //}
                                    DateTime? ntes = Convert.ToDateTime(informacionTransaccion.FechaEntrada.ToString());
                                    dtAntes = Convert.ToDateTime(ntes);


                                    tbFechaIngresoCobrar.Text = dtAntes.ToString("dd'/'MM'/'yyyy HH':'mm");
                                    //tbHoraPago.Text = dtDespues.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");

                                    if (lstInfo.LstTransac.Length == 1)
                                    {
                                        tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero;
                                    }
                                    else if (lstInfo.LstTransac.Length == 2)
                                    {
                                        tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero + " y " + lstInfo.LstTransac[1].Casillero;
                                    }
                                    else
                                    {
                                        tbCasilleroCobrar.Text = string.Empty;
                                    }

                                    TimeSpan ts = dtDespues - dtAntes;
                                    tbTiempoCobrar.Text = Convert.ToInt32(ts.TotalMinutes).ToString() + " minutos";
                                    tbTipoVehiculoCobrar.Text= informacionTransaccion.TipoVehiculo.ToString();
                                    tbModuloIngresoCobrar.Text = informacionTransaccion.ModuloEntrada.ToString();
                                    //tbTiempoCobrar.Text = "Día: " + Convert.ToInt32(ts.Days).ToString() + " Horas: " + Convert.ToInt32(ts.Hours).ToString() + " Minutos: " + Convert.ToInt32(ts.Minutes);                



                                    //int tipoVehiculo = 0;
                                    //if (oCardResponse.tipoVehiculo == "AUTOMOBILE")
                                    //{
                                    //    tipoVehiculo = 1;
                                    //}
                                    //else if (oCardResponse.tipoVehiculo == "MOTORCYCLE")
                                    //{
                                    //    tipoVehiculo = 2;
                                    //}
                                    //else if (oCardResponse.tipoVehiculo == "BICYCLE")
                                    //{
                                    //    tipoVehiculo = 3;
                                    //}

                                    liquidacion = cliente.ConsultarValorPagar(false, false, Convert.ToInt32(informacionTransaccion.IdTipoVehiculo), oInfoTransaccionService.IdTransaccion, null);
                                    double sumTotalPagar = 0;
                                    #region Estacionamiento
                                    if (liquidacion.Exito)
                                    {
                                        if (liquidacion.LstLiquidacion.Length > 0)
                                        {
                                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                            {
                                                sumTotalPagar += item.Total;
                                            }

                                            tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                            tbCambioCobrar.Text = "$0";
                                            tbRecibidoCobrar.Text = "0";


                                            //Cargando(false);
                                            //panelPagar.Enabled = true;
                                            tmrTimeOutPago.Start();

                                            //chbEstacionamiento.Checked = true;
                                            //chbMensualidad.Checked = false;

                                            //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";

                                        }
                                        else
                                        {
                                            //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            liquidacion = cliente.ConsultarValorPagar(true, false, 1, "0", null);
                                            sumTotalPagar = 0;
                                            if (liquidacion.Exito)
                                            {
                                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                {
                                                    sumTotalPagar += item.Total;
                                                }

                                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                tbCambioCobrar.Text = "$0";
                                                tbRecibidoCobrar.Text = "0";


                                                //Cargando(false);
                                                //panelPagar.Enabled = true;
                                                tmrTimeOutPago.Start();

                                                //chbEstacionamiento.Checked = false;
                                                //chbMensualidad.Checked = true;

                                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                            }
                                            else
                                            {
                                                //Cargando(false);
                                                MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //tbCodigo.Text = "";
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Mensualidad
                                    else
                                    {
                                        //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                                        sumTotalPagar = 0;
                                        if (liquidacion.Exito)
                                        {
                                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                            {
                                                sumTotalPagar += item.Total;
                                            }

                                            if (sumTotalPagar > 0)
                                            {
                                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                tbCambioCobrar.Text = "$0";
                                                tbRecibidoCobrar.Text = "0";

                                                //Cargando(false);
                                                //panelPagar.Enabled = true;
                                                tmrTimeOutPago.Start();

                                                //chbEstacionamiento.Checked = false;
                                                //chbMensualidad.Checked = true;

                                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                            }
                                            else
                                            {
                                                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                if (result5 == DialogResult.Yes)
                                                {
                                                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                                    if (resp.Exito)
                                                    {
                                                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    else
                                                    {
                                                        this.DialogResult = DialogResult.None;
                                                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    }
                                                }
                                                else
                                                {
                                                    //Cargando(false);
                                                }
                                                //Cargando(false);
                                            }
                                        }
                                        else
                                        {
                                            //Cargando(false);
                                            MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //Cargando(false);
                                    MessageBox.Show("No obtiene infromacion de la transaccion", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                //Cargando(false);
                                MessageBox.Show(oInfoTransaccionService.ErrorMessage + ": " + sIdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Cargando(false);
                        //Camino de salida autorizado
                    }
                }
                #endregion


                #region Mensualidad
                else
                {
                    _IdTransaccion = informacionTransaccion.IdTransaccion;
                    //tbIdTarjeta.Text = oCardResponse.idCard;
                    //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                    //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                    liquidacion = cliente.ConsultarValorPagar(true, false, 1, informacionTransaccion.IdTransaccion, tbPlaca.Text);

                    double sumTotalPagar = 0;
                    if (liquidacion.Exito)
                    {
                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            sumTotalPagar += item.Total;
                        }

                        if (sumTotalPagar > 0)
                        {
                            tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                            tbCambioCobrar.Text = "$0";
                            tbRecibidoCobrar.Text = "0";

                            //Cargando(false);
                            //panelPagar.Enabled = true;
                            tmrTimeOutPago.Start();

                            //chbEstacionamiento.Checked = false;
                            //chbMensualidad.Checked = true;

                            //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                        }
                        else
                        {
                            DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result5 == DialogResult.Yes)
                            {
                                ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                if (resp.Exito)
                                {
                                    MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    this.DialogResult = DialogResult.None;
                                    MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                //Cargando(false);
                            }
                            else
                            {
                                //Cargando(false);
                            }
                        }
                    }
                    else
                    {
                        //Cargando(false);
                        MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
            }
            else
            {
                if (ckMensualidadDocumento.Checked == true)
                {
                    ConsultarIdTarjetaPlacaResponse Documento = cliente.ConsultarIdTarjetaPorPlaca(txtPlacaBuscar.Text);
                    if (Documento.Exito)
                    {
                        string documentoAutorizado = Documento.Documento;
                        //tbIdTarjeta.Text = IdTarjeta.IdTarjetaDescripcion;
                        //oCardResponse.idCard = IdTarjeta.IdTarjetaDescripcion;
                        liquidacion = cliente.ConsultarValorPagar(true, false, 1, documentoAutorizado, txtPlacaBuscar.Text);
                        double sumTotalPagar = 0;
                        if (liquidacion.Exito)
                        {
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                            }

                            if (sumTotalPagar > 0)
                            {
                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                tbCambioCobrar.Text = "$0";
                                tbRecibidoCobrar.Text = "0";

                                //Cargando(false);
                                //panelPagar.Enabled = true;
                                tmrTimeOutPago.Start();

                                //chbEstacionamiento.Checked = false;
                                ckMensualidadDocumento.Checked = true;
                                tbNombreAutorizadoCobrar.Text = Documento.NombreApellidos;

                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                            }
                            else
                            {
                                //DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                //if (result5 == DialogResult.Yes)
                                //{
                                //    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                //    if (resp.Exito)
                                //    {
                                //        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    }
                                //    else
                                //    {
                                //        this.DialogResult = DialogResult.None;
                                //        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    }
                                //    Cargando(false);
                                //}
                                //else
                                //{
                                //    Cargando(false);
                                //}

                                //Cargando(false);
                                MessageBox.Show("Consulte vencimiento de mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show("No fue posible obtener el valor de la mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Cargando(false);
                        MessageBox.Show("La placa no pertenece a un autorizado", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }



                }

                else if (tbCodigo.Text != string.Empty && txtPlacaBuscar.Text == string.Empty)
                {
                    CardResponse oCardResponse = GetCardInfo(clave);

                    InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorIdTransaccion(tbCodigo.Text, cbEstacionamiento.SelectedValue.ToString());


                    //if (!oCardResponse.error)
                    //{
                    bool bContinuarLiquidacion = true;

                    #region Estacionamiento
                    if (informacionTransaccion.Exito)
                    {
                        if (informacionTransaccion.IdTransaccion == string.Empty)
                        {
                            DialogResult result3 = MessageBox.Show("¿Desea crear una salida de autorizado? \n PRESIONE NO PARA CONTINUAR CON LA LIQUIDACION.", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result3 == DialogResult.Yes)
                            {

                                DialogResult result4 = MessageBox.Show("¿Esta seguro que desea crear la salida para la placa: " + oCardResponse.placa, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result4 == DialogResult.Yes)
                                {
                                    bContinuarLiquidacion = false;
                                    CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                                    if (oCarrilxIdModuloResponse.Exito)
                                    {
                                        string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                        CardResponse oCardResponseExit = new CardResponse();
                                        oCardResponseExit = ExitCardAutho(clave, oCardResponse.idCard);
                                        if (!oCardResponseExit.error)
                                        {
                                            CreaSalidaResponse resp = cliente.CrearSalida2(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.placa, sIdTransaccion, oCarrilxIdModuloResponse.Carril.ToString(), oCardResponse.moduloEntrada, oCardResponse.idCard);

                                            if (resp.Exito)
                                            {
                                                MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                this.DialogResult = DialogResult.None;
                                                MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            //Cargando(false);
                                            MessageBox.Show(oCardResponseExit.errorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("No fue posible encontrar el carril asociado al modulo.", "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        if (bContinuarLiquidacion)
                        {
                            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), informacionTransaccion.ModuloEntrada.ToString());
                            if (oCarrilxIdModuloResponse.Exito)
                            {
                                //yyyyMMddHHmmssce

                                string sIdTransaccion = informacionTransaccion.IdTransaccion;

                                //tbIdTarjeta.Text = oCardResponse.idCard;
                                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                InfoTransaccionService lstInfo = cliente.ConsultarCascosxId(sIdTransaccion);

                                //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                                if (oInfoTransaccionService.Exito)
                                {
                                    if (oInfoTransaccionService.IdTransaccion != string.Empty)
                                    {
                                        //tbIdTransaccion.Text = oInfoTransaccionService.IdTransaccion;
                                        _IdTransaccion = oInfoTransaccionService.IdTransaccion;
                                        txtPlacaBuscar.Text = informacionTransaccion.PlacaEntrada;
                                        tbModuloIngresoCobrar.Text = informacionTransaccion.ModuloEntrada;
                                        tbTipoVehiculoCobrar.Text = informacionTransaccion.TipoVehiculo;

                                        //if (oCardResponse.codeAutorizacion1 != 0)
                                        //{
                                        //    cliente.AplicarConvenios(oInfoTransaccionService.IdTransaccion, oCardResponse.codeAutorizacion1, oCardResponse.codeAutorizacion2, oCardResponse.codeAutorizacion3);
                                        //}

                                        DateTime dtDespues = DateTime.Now;
                                        DateTime dtAntes = new DateTime();

                                        //MessageBox.Show(oInfoTransaccionService.HoraTransaccion);

                                        oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("a. m.", "a.m.");
                                        oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("p. m.", "p.m.");

                                        //try
                                        //{
                                        //    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //    {
                                        //        if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //        {
                                        //            if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //            {
                                        //                if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //                {
                                        //                    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy H':'mm':'ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //                    {
                                        //                        dtAntes = DateTime.ParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture);
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        //catch (Exception exe)
                                        //{
                                        //    MessageBox.Show("Tiempo de permanencia no disponible, continue con el pago e informe al desarrollador->" + exe.Message + " / " + oInfoTransaccionService.HoraTransaccion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //}
                                        DateTime? ntes = Convert.ToDateTime(informacionTransaccion.FechaEntrada.ToString());
                                        dtAntes = Convert.ToDateTime(ntes);


                                        tbFechaIngresoCobrar.Text = dtAntes.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");
                                        //tbHoraPago.Text = dtDespues.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");

                                        if (lstInfo.LstTransac.Length == 1)
                                        {
                                            tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero;
                                        }
                                        else if (lstInfo.LstTransac.Length == 2)
                                        {
                                            tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero + " y " + lstInfo.LstTransac[1].Casillero;
                                        }
                                        else
                                        {
                                            tbCasilleroCobrar.Text = string.Empty;
                                        }

                                        TimeSpan ts = dtDespues - dtAntes;
                                        tbTiempoCobrar.Text = Convert.ToInt32(ts.TotalMinutes).ToString() + " minutos";

                                        //int tipoVehiculo = 0;
                                        //if (oCardResponse.tipoVehiculo == "AUTOMOBILE")
                                        //{
                                        //    tipoVehiculo = 1;
                                        //}
                                        //else if (oCardResponse.tipoVehiculo == "MOTORCYCLE")
                                        //{
                                        //    tipoVehiculo = 2;
                                        //}
                                        //else if (oCardResponse.tipoVehiculo == "BICYCLE")
                                        //{
                                        //    tipoVehiculo = 3;
                                        //}

                                        liquidacion = cliente.ConsultarValorPagar(false, false, Convert.ToInt32(informacionTransaccion.IdTipoVehiculo), oInfoTransaccionService.IdTransaccion, null);
                                        double sumTotalPagar = 0;

                                        #region Estacionamiento
                                        if (liquidacion.Exito)
                                        {
                                            if (liquidacion.LstLiquidacion.Length > 0)
                                            {
                                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                {
                                                    sumTotalPagar += item.Total;
                                                }

                                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                tbCambioCobrar.Text = "$0";
                                                tbRecibidoCobrar.Text = "0";


                                                //Cargando(false);
                                                //panelPagar.Enabled = true;
                                                tmrTimeOutPago.Start();

                                                //chbEstacionamiento.Checked = true;
                                                //chbMensualidad.Checked = false;

                                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";

                                            }
                                            else
                                            {
                                                //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                liquidacion = cliente.ConsultarValorPagar(true, false, 1, "0", null);
                                                sumTotalPagar = 0;
                                                if (liquidacion.Exito)
                                                {
                                                    foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                    {
                                                        sumTotalPagar += item.Total;
                                                    }

                                                    tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                    tbCambioCobrar.Text = "$0";
                                                    tbRecibidoCobrar.Text = "0";

                                                    //Cargando(false);
                                                    //panelPagar.Enabled = true;
                                                    tmrTimeOutPago.Start();

                                                    //chbEstacionamiento.Checked = false;
                                                    //chbMensualidad.Checked = true;

                                                    //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                }
                                                else
                                                {
                                                    //Cargando(false);
                                                    MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Mensualidad
                                        else
                                        {
                                            //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                                            sumTotalPagar = 0;
                                            if (liquidacion.Exito)
                                            {
                                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                {
                                                    sumTotalPagar += item.Total;
                                                }

                                                if (sumTotalPagar > 0)
                                                {
                                                    tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                    tbCambioCobrar.Text = "$0";
                                                    tbRecibidoCobrar.Text = "0";

                                                    //Cargando(false);
                                                    //panelPagar.Enabled = true;
                                                    tmrTimeOutPago.Start();

                                                    //chbEstacionamiento.Checked = false;
                                                    //chbMensualidad.Checked = true;

                                                    //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                }
                                                else
                                                {
                                                    DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                    if (result5 == DialogResult.Yes)
                                                    {
                                                        ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                                        if (resp.Exito)
                                                        {
                                                            MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        }
                                                        else
                                                        {
                                                            this.DialogResult = DialogResult.None;
                                                            MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Cargando(false);
                                                    }
                                                    //Cargando(false);
                                                }
                                            }
                                            else
                                            {
                                                //Cargando(false);
                                                MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //Cargando(false);
                                        MessageBox.Show("No obtiene infromacion de la transaccion", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    //Cargando(false);
                                    MessageBox.Show(oInfoTransaccionService.ErrorMessage + ": " + sIdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                //Cargando(false);
                                MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            //Cargando(false);
                            //Camino de salida autorizado
                        }
                    }
                    #endregion

                    #region Mensualidad
                    else
                    {
                        //tbIdTarjeta.Text = oCardResponse.idCard;
                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                        liquidacion = cliente.ConsultarValorPagar(true, false, 1, informacionTransaccion.IdTransaccion, tbPlaca.Text);

                        double sumTotalPagar = 0;
                        if (liquidacion.Exito)
                        {
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                            }

                            if (sumTotalPagar > 0)
                            {
                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                tbCambioCobrar.Text = "$0";
                                tbRecibidoCobrar.Text = "0";

                                //Cargando(false);
                                //panelPagar.Enabled = true;
                                tmrTimeOutPago.Start();

                                //chbEstacionamiento.Checked = false;
                                //chbMensualidad.Checked = true;

                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                            }
                            else
                            {
                                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result5 == DialogResult.Yes)
                                {
                                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                    if (resp.Exito)
                                    {
                                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.None;
                                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    //Cargando(false);
                                }
                                else
                                {
                                    //Cargando(false);
                                }
                            }
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbCodigo.Text = "";
                        }
                    }
                    #endregion

                }
                //else if (ckMensualidadDocumento.Checked == false && tbCodigo.Text != string.Empty)
                //{
                //    InfoTransaccionResponse informacionTransaccion = cliente.ConsultarInfoTransaccionPorIdTransaccion(tbCodigo.Text, cbEstacionamiento.SelectedValue.ToString());

                //    if (informacionTransaccion.Exito)
                //    {

                //        tbIdTarjeta.Text = oCardResponse.idCard;
                //        liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                //        liquidacion = cliente.ConsultarValorPagar(true, false, 1, Documento.IdTarjetaDescripcion, Documento.IdTarjetaDescripcion);
                //        double sumTotalPagar = 0;
                //        if (liquidacion.Exito)
                //        {
                //            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                //            {
                //                sumTotalPagar += item.Total;
                //            }

                //            if (sumTotalPagar > 0)
                //            {
                //                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                //                tbCambioCobrar.Text = "$0";
                //                tbRecibidoCobrar.Text = "0";

                //                Cargando(false);
                //                panelPagar.Enabled = true;
                //                tmrTimeOutPago.Start();

                //                chbEstacionamiento.Checked = false;
                //                ckMensualidadDocumento.Checked = true;

                //                lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                //            }
                //            else
                //            {
                //                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                //                if (result5 == DialogResult.Yes)
                //                {
                //                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                //                    if (resp.Exito)
                //                    {
                //                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                    }
                //                    else
                //                    {
                //                        this.DialogResult = DialogResult.None;
                //                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                    }
                //                    Cargando(false);
                //                }
                //                else
                //                {
                //                    Cargando(false);
                //                }
                //                Cargando(false);
                //                MessageBox.Show("Consulte vencimiento de mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            }
                //        }
                //        else
                //        {
                //            Cargando(false);
                //            MessageBox.Show("Consulte vencimiento de mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }

                //}
                else
                {
                    DialogResult result3 = MessageBox.Show("No fue posible leer la tarjeta, ¿Desea registrar una salida SIN tarjeta?", "Leer PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        CrearSalida popup = new CrearSalida(cbEstacionamiento.SelectedValue.ToString());
                        popup.ShowDialog();
                        if (popup.DialogResult == DialogResult.OK)
                        {
                            //Cargando(false);
                            MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        {
                            //Cargando(false);
                            MessageBox.Show("Operacion cancelada por el usuario", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //Cargando(false);
                            MessageBox.Show("Error al procesar ventana crear salida", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Cargando(false);
                        //MessageBox.Show(oCardResponse.errorMessage, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //}


            //else
            //{
            //    Cargando(false);
            //    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        public void LimpiarDatosCobrar()
        {
            txtPlacaBuscar.Text = "";
            tbTipoVehiculoCobrar.Text = "";
            tbNombreAutorizadoCobrar.Text = "";
            tbConvenioCobrar.Text = "";
            tbCasilleroCobrar.Text = "";
            tbFechaIngresoCobrar.Text = "";
            tbModuloIngresoCobrar.Text = "";
            tbTiempoCobrar.Text = "";
            tbValorAPagarCobrar.Text = "";
            tbRecibidoCobrar.Text = "";
            tbCambioCobrar.Text = "$0";
            ckMensualidadDocumento.Checked = false;
            tbCodigo.Text = "";            
            tbCodigo.Focus();
        }

        #endregion

        #endregion


        #endregion

        #region Formulario

        private void CargarUsuario()
        {
            InfoUsuarioResponse response = cliente.ObtenerInformacionUsuario(_DocumentoUsuario);
            if (response.Exito)
            {
                tbUsuario.Text = response.Usuario;
                tbUsuario.Update();
                documentoUsuario = response.Documento;
                tbNombreUsuario.Text = response.Nombres;
                tbNombreUsuario.Update();
            }
            else
            {
                tbUsuario.Text = "Usuario Desconocido";
            }
        }



        #endregion

        #region Tarjetas
        public CardResponse AplicarCortesia(string password)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Courtesy = true;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse GetCardInfo(string sPass)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        oCardResponse.idCard = resp.CodigoTarjeta;
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(sPass);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                oCardResponse.error = false;
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                oCardResponse.cicloActivo = myTarjeta.ActiveCycle != null ? (bool)myTarjeta.ActiveCycle : false;
                                oCardResponse.codeAutorizacion1 = myTarjeta.CodeAgreement1 != null ? (int)myTarjeta.CodeAgreement1 : 0;
                                oCardResponse.codeAutorizacion2 = myTarjeta.CodeAgreement2 != null ? (int)myTarjeta.CodeAgreement2 : 0;
                                oCardResponse.codeAutorizacion3 = myTarjeta.CodeAgreement3 != null ? (int)myTarjeta.CodeAgreement3 : 0;
                                oCardResponse.cortesia = myTarjeta.Courtesy != null ? (bool)myTarjeta.Courtesy : false;
                                oCardResponse.fechEntrada = myTarjeta.DateTimeEntrance;
                                oCardResponse.moduloEntrada = myTarjeta.EntranceModule;
                                oCardResponse.placa = myTarjeta.EntrancePlate;
                                oCardResponse.fechaPago = myTarjeta.PaymentDateTime.ToString();
                                oCardResponse.moduloPago = myTarjeta.PaymentModule;
                                oCardResponse.reposicion = myTarjeta.Replacement != null ? (bool)myTarjeta.Replacement : false;
                                oCardResponse.tipoTarjeta = myTarjeta.TypeCard.ToString();
                                oCardResponse.tipoVehiculo = myTarjeta.TypeVehicle.ToString();
                                oCardResponse.valet = myTarjeta.ValetParking != null ? (bool)myTarjeta.ValetParking : false;
                                //myTarjeta.

                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }

            return oCardResponse;
        }
        public CardResponse PayCard(string password, string idTarjeta, string moduloPago, DateTime dtFechaPago)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();

            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                //Thread.Sleep(200);
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    //Thread.Sleep(200);
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        //Thread.Sleep(200);
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            //Thread.Sleep(200);
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                Thread.Sleep(2000);
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.PaymentModule = moduloPago;
                                myTarjeta.PaymentDateTime = dtFechaPago;
                                myTarjeta.CodeCard = idTarjeta;
                                myTarjeta.Replacement = false;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    Thread.Sleep(2000);
                                    Rspsta_Leer_Tarjeta_LECTOR respFinal = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                                    if (respFinal.TarjetaLeida)
                                    {
                                        oCardResponse.error = false;
                                        SMARTCARD_PARKING_V1 myTarjetaFinal = (SMARTCARD_PARKING_V1)respFinal.Tarjeta;
                                        oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                                        oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                                        oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                                        oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                                        oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                                        oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance;
                                        oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                                        oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                                        oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
                                        oCardResponse.moduloPago = myTarjetaFinal.PaymentModule;
                                        oCardResponse.reposicion = myTarjetaFinal.Replacement != null ? (bool)myTarjetaFinal.Replacement : false;
                                        oCardResponse.tipoTarjeta = myTarjetaFinal.TypeCard.ToString();
                                        oCardResponse.tipoVehiculo = myTarjetaFinal.TypeVehicle.ToString();
                                        oCardResponse.valet = myTarjetaFinal.ValetParking != null ? (bool)myTarjetaFinal.ValetParking : false;
                                    }
                                    else
                                    {
                                        oCardResponse.error = true;
                                        oCardResponse.errorMessage = "No lee tarjeta.";
                                    }
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse ExitCardAutho(string password, string idTarjeta)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();

            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                //Thread.Sleep(200);
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    //Thread.Sleep(200);
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        //Thread.Sleep(200);
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            //Thread.Sleep(200);
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                Thread.Sleep(2000);
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Replacement = false;
                                myTarjeta.ActiveCycle = false;
                                myTarjeta.DateTimeEntrance = null;
                                myTarjeta.EntranceModule = string.Empty;
                                myTarjeta.EntrancePlate = string.Empty;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse AplicarConvenio(string password, string idConvenio)
        {
            CardResponse oCardResponse = new CardResponse();

            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.CodeAgreement1 = Convert.ToInt32(idConvenio);
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse LimpiarReposicion(string password)
        {
            CardResponse oCardResponse = new CardResponse();

            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Replacement = false;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        #endregion

        #region ClasesAuxiliares
        public class CardResponse
        {
            public bool error { set; get; }
            public string errorMessage { set; get; }
            public string idCard { set; get; }

            public bool cicloActivo { get; set; }
            public bool cortesia { get; set; }
            public bool reposicion { get; set; }
            public bool valet { get; set; }

            public int codeAutorizacion1 { get; set; }
            public int codeAutorizacion2 { get; set; }
            public int codeAutorizacion3 { get; set; }

            public DateTime? fechEntrada { get; set; }
            public string fechaPago { get; set; }

            public string moduloEntrada { get; set; }
            public string moduloPago { get; set; }
            public string placa { get; set; }
            public string tipoTarjeta { get; set; }
            public string tipoVehiculo { set; get; }
        }
        #endregion

        #region Impresion
        private void ImprimirPagoNormal(string idTransaccion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFactura(idTransaccion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicket(oInfoFacturaResponse.LstItems.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //RestablecerPPM();
                //Cargando(false);
            }
            else
            {
                RestablecerPPM();
                //Cargando(false);
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        private void ImprimirTicketEntrada()
        {
            InfoEntradaResponse oInfoFacturaEntradaResponse = cliente.ObtenerDatosFacturaEntrada(moduloEntrada);
            if (oInfoFacturaEntradaResponse.Exito)
            {
                bool resultado = PrintTicketEntrada(oInfoFacturaEntradaResponse.LstItems.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //RestablecerPPM();
                //Cargando(false);
            }
            else
            {
                //RestablecerPPM();
                //Cargando(false);
                LimpiarDatosEntrada();
                MessageBox.Show(oInfoFacturaEntradaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketEntrada(List<InfoItemsFacturaEntradaResponse> datos)
        {

            bool bPrint = false;

            try
            {

                List<List<InfoItemsFacturaEntradaResponse>> facturas = new List<List<InfoItemsFacturaEntradaResponse>>();
                foreach (InfoItemsFacturaEntradaResponse item in datos)
                {
                    bool find = false;
                    if (facturas.Count > 0)
                    {
                        foreach (List<InfoItemsFacturaEntradaResponse> item2 in facturas)
                        {
                            if (item2[0].IdTransaccion == item.IdTransaccion)
                            {
                                find = true;
                                item2.Add(item);
                            }
                        }

                        if (!find)
                        {
                            List<InfoItemsFacturaEntradaResponse> otraFactura = new List<InfoItemsFacturaEntradaResponse>();
                            otraFactura.Add(item);
                            facturas.Add(otraFactura);
                        }
                        find = false;
                    }
                    else
                    {
                        List<InfoItemsFacturaEntradaResponse> primeraFactura = new List<InfoItemsFacturaEntradaResponse>();
                        primeraFactura.Add(item);
                        facturas.Add(primeraFactura);
                    }
                }



                if (facturas.Count > 0)
                {
                    CapturaRutaBarras();
                    foreach (var item in facturas)
                    {
                        ReportDataSource datasource = new ReportDataSource();
                        LocalReport oLocalReport = new LocalReport();

                        datasource = new ReportDataSource("DataSetTicketEntrada", (DataTable)GenerarTicketEntrada(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketEntrada"));
                        ReportParameter urlImage = new ReportParameter("imgUrl", new Uri(Convert.ToString(_imgUrl)).AbsoluteUri);
                        oLocalReport.EnableExternalImages = true;
                        oLocalReport.SetParameters(new ReportParameter[] { urlImage });
                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();

                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        ore.PrintController = new StandardPrintController();
                        ore.Print();

                        oLocalReport.Dispose();
                        oLocalReport = null;
                        ore.Dispose();
                        ore = null;
                        EliminarCodigoBarras();
                    }
                }
                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }

        public bool PrintTicket(List<InfoItemsFacturaResponse> datos)
        {
            bool bPrint = false;

            try
            {

                List<List<InfoItemsFacturaResponse>> facturas = new List<List<InfoItemsFacturaResponse>>();
                foreach (InfoItemsFacturaResponse item in datos)
                {
                    bool find = false;
                    if (facturas.Count > 0)
                    {
                        foreach (List<InfoItemsFacturaResponse> item2 in facturas)
                        {
                            if (item2[0].NumeroFactura == item.NumeroFactura)
                            {
                                find = true;
                                item2.Add(item);
                            }
                        }

                        if (!find)
                        {
                            List<InfoItemsFacturaResponse> otraFactura = new List<InfoItemsFacturaResponse>();
                            otraFactura.Add(item);
                            facturas.Add(otraFactura);
                        }
                        find = false;
                    }
                    else
                    {
                        List<InfoItemsFacturaResponse> primeraFactura = new List<InfoItemsFacturaResponse>();
                        primeraFactura.Add(item);
                        facturas.Add(primeraFactura);
                    }
                }



                if (facturas.Count > 0)
                {
                    foreach (var item in facturas)
                    {
                        ReportDataSource datasource = new ReportDataSource();
                        LocalReport oLocalReport = new LocalReport();

                        datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPago(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPago"));


                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();

                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        ore.PrintController = new StandardPrintController();
                        ore.Print();

                        oLocalReport.Dispose();
                        oLocalReport = null;
                        ore.Dispose();
                        ore = null;
                    }
                }
                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }

        private DataSetTicketEntrada GenerarTicketEntrada(List<InfoItemsFacturaEntradaResponse> infoTicket)
        {
            DataSetTicketEntrada facturacion = new DataSetTicketEntrada();

            double total = 0;
            foreach (var item in infoTicket)
            {
                //total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketEntrada.TablaTicketEntradaRow rowDatosFactura = facturacion.TablaTicketEntrada.NewTablaTicketEntradaRow();

                // Crear una instancia de BarcodeWriter
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.CODE_128;

                // Generar el código de barras como un objeto Bitmap
                Bitmap barcodeBitmap = barcodeWriter.Write(Convert.ToString(item.IdTransaccion));
                string rutaGuardar = _imgUrl;
                // Guardar el código de barras en un archivo con el nombre IdTransaccion
                string codigoBarrasFileName = rutaGuardar + "\\" + item.IdTransaccion + ".png";
                barcodeBitmap.Save(codigoBarrasFileName);
                _imgUrl = codigoBarrasFileName;
                // Limpieza
                barcodeBitmap.Dispose();

                //rowDatosFactura.IdTransaccion = item.IdTransaccion;
                //panelCodigo.Visible = false;
                //panelCodigo1.BackgroundImage = Codigo.Encode(BarcodeLib.TYPE.CODE128B,item.IdTransaccion,Color.Black,Color.White,400,100);      
                rowDatosFactura.PlacaEntrada = item.PlacaEntrada;
                rowDatosFactura.FechaEntrada = item.FechaEntrada;
                rowDatosFactura.TipoVehiculo = item.TipoVehiculo;
                rowDatosFactura.Rut = "NIT 804003167 - 1";
                rowDatosFactura.Nombre = "EDIFICIO PLAZA CENTRAL PH";
                rowDatosFactura.Telefono = "6700040";
                rowDatosFactura.Direccion = "Cra 16 # 33-44 Edif. Plaza Central PH";
                //rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                //rowDatosFactura.Modulo = item.Modulo;
                //rowDatosFactura.Nombre = item.Nombre;
                //rowDatosFactura.NumeroFactura = item.NumeroFactura;
                //rowDatosFactura.Placa = item.Placa;
                //rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                //rowDatosFactura.Resolucion = item.NumeroResolucion;
                //rowDatosFactura.Rut = "NIT 900.554.696 -8";
                //rowDatosFactura.Telefono = item.Telefono;
                //rowDatosFactura.TotalFinal = total;
                //rowDatosFactura.Total = Convert.ToDouble(item.Total);
                //rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                //rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                //rowDatosFactura.TipoPago = item.Tipo;
                //rowDatosFactura.Fecha2 = item.FechaEntrada;
                //rowDatosFactura.Vehiculo = item.TipoVehiculo;
                //rowDatosFactura.VigenciaFactura = item.Vigencia;

                facturacion.TablaTicketEntrada.AddTablaTicketEntradaRow(rowDatosFactura);
            }

            return facturacion;
        }
        public void CapturaRutaBarras()
        {


            string Placa = string.Empty;
            string rutaBarras = string.Empty;

            rutaBarras = cliente.ObtenerRutaCodigoBarras(Convert.ToString(_IdEstacionamiento));

            _imgUrl = rutaBarras;

        }
        public void EliminarCodigoBarras()
        {  //LEER OLD
            //TextReader leer = new StreamReader("" + rutaPlaca + "" + "" + Globales.sSerial + "" + ".txt");
            //Placa = leer.ReadToEnd();
            //_sPlaca = Placa.TrimEnd();
            string rutaPlacaGuardada = imgUrl;
            if (File.Exists(rutaPlacaGuardada))
            {
                File.Delete(rutaPlacaGuardada);
            }

        }
        private DataSetTicketPago GenerarTicketPago(List<InfoItemsFacturaResponse> infoTicket)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 804003167 - 1";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.Fecha2 = item.FechaEntrada;
                rowDatosFactura.Vehiculo = item.TipoVehiculo;
                rowDatosFactura.VigenciaFactura = item.Vigencia;
                rowDatosFactura.NombreUsuario = nombresUsuario;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }

        private byte[] GetBytes(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }


        private void ImprimirPagoMensualidad(string idTransaccion, string idAutorizacion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFacturaMensualidad(idTransaccion, idAutorizacion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketMensualidad(oInfoFacturaResponse.LstItemsMensualidad.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket Mensualidad", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RestablecerPPM();
                //Cargando(false);
            }
            else
            {
                RestablecerPPM();
                //Cargando(false);
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketMensualidad(List<InfoItemsFacturaMensualidadResponse> datos)
        {
            bool bPrint = false;

            try
            {

                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoMensualidad(datos).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPagoM"));


                oLocalReport.DataSources.Add(datasource);
                oLocalReport.Refresh();

                ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                ore.PrintController = new StandardPrintController();
                ore.Print();

                oLocalReport.Dispose();
                oLocalReport = null;
                ore.Dispose();
                ore = null;

                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketPagoMensualidad GenerarTicketPagoMensualidad(List<InfoItemsFacturaMensualidadResponse> infoTicket)
        {
            DataSetTicketPagoMensualidad facturacion = new DataSetTicketPagoMensualidad();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPagoMensualidad.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 800064936 - 5";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.NombreAutorizacion = item.NombreAutorizacion;
                rowDatosFactura.Documento = item.Documento;
                rowDatosFactura.VigenciaFactura = item.Vigencia;
                rowDatosFactura.NombreUsuario = nombresUsuario;
                rowDatosFactura.NombreApellidos = item.NombreApellidos;
                rowDatosFactura.Placa1 = item.Placa1;

                //NEW FIELDS
                rowDatosFactura.Nit = item.NombreEmpresa;
                rowDatosFactura.NombreEmpresa = item.Nit;
                //

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        private void ImprimirArqueo(string idArqueo)
        {
            InfoArqueoResponse oInfoFacturaResponse = cliente.ObtenerDatosComprobanteArqueo(idArqueo);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketArqueo(oInfoFacturaResponse.LstInfoArqueos.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool PrintTicketArqueo(List<InfoItemsArqueoResponse> datos)
        {
            bool bPrint = false;

            try
            {
                foreach (InfoItemsArqueoResponse item in datos)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketArqueo", (DataTable)GenerarTicketArqueo(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketArqueo"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    ore.PrintController = new StandardPrintController();
                    ore.Print();

                    oLocalReport.Dispose();
                    oLocalReport = null;
                    ore.Dispose();
                    ore = null;

                    bPrint = true;
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketArqueo GenerarTicketArqueo(InfoItemsArqueoResponse infoTicket)
        {
            DataSetTicketArqueo facturacion = new DataSetTicketArqueo();

            DataSetTicketArqueo.TablaTicketArqueoRow rowDatosFactura = facturacion.TablaTicketArqueo.NewTablaTicketArqueoRow();

            rowDatosFactura.Valor = infoTicket.Valor != "" ? Convert.ToDouble(infoTicket.Valor) : 0;
            rowDatosFactura.Producido = Convert.ToDouble(infoTicket.Producido);
            rowDatosFactura.Direccion = infoTicket.Direccion;
            rowDatosFactura.Fecha = infoTicket.Fecha;
            rowDatosFactura.IdArqueo = infoTicket.IdArqueo;
            rowDatosFactura.Modulo = infoTicket.Modulo;
            rowDatosFactura.Nombre = infoTicket.Nombre;
            rowDatosFactura.Telefono = infoTicket.Telefono;
            rowDatosFactura.Cantidad = infoTicket.CantTransacciones;
            rowDatosFactura.IdUsuario = infoTicket.IdUsuario;
            rowDatosFactura.Conteo = Convert.ToDouble(infoTicket.Conteo);


            facturacion.TablaTicketArqueo.AddTablaTicketArqueoRow(rowDatosFactura);

            return facturacion;
        }
        private void ImprimirCarga(string idCarga)
        {
            InfoCargaResponse oInfoFacturaResponse = cliente.ObtenerDatosComprobanteCarga(idCarga);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketCarga(oInfoFacturaResponse.LstInfoCargas.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool PrintTicketCarga(List<InfoItemsCargaResponse> datos)
        {
            bool bPrint = false;

            try
            {
                foreach (InfoItemsCargaResponse item in datos)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketCarga", (DataTable)GenerarTicketCarga(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketCarga"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    ore.PrintController = new StandardPrintController();
                    ore.Print();

                    oLocalReport.Dispose();
                    oLocalReport = null;
                    ore.Dispose();
                    ore = null;

                    bPrint = true;
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketCarga GenerarTicketCarga(InfoItemsCargaResponse infoTicket)
        {
            DataSetTicketCarga facturacion = new DataSetTicketCarga();

            DataSetTicketCarga.TablaTicketCargaRow rowDatosFactura = facturacion.TablaTicketCarga.NewTablaTicketCargaRow();

            rowDatosFactura.Valor = Convert.ToDouble(infoTicket.Valor);
            rowDatosFactura.Direccion = infoTicket.Direccion;
            rowDatosFactura.Fecha = infoTicket.Fecha;
            rowDatosFactura.IdCarga = infoTicket.IdCarga;
            rowDatosFactura.Modulo = infoTicket.Modulo;
            rowDatosFactura.Nombre = infoTicket.Nombre;
            rowDatosFactura.Telefono = infoTicket.Telefono;
            rowDatosFactura.IdEstacionamiento = infoTicket.IdEstacionamiento;
            rowDatosFactura.IdUsuario = infoTicket.IdUsuario;


            facturacion.TablaTicketCarga.AddTablaTicketCargaRow(rowDatosFactura);

            return facturacion;
        }
        #endregion

        public Menu(string sDocumento, string sCargo, InfoPPMService oInfoPPMService)
        {
            InitializeComponent();
            tbPlaca.TextChanged += tbPlaca_TextChanged; ;

            _DocumentoUsuario = sDocumento;
            _CargoUsuario = sCargo;
            Application.AddMessageFilter(this);

            controlsToMove.Add(this);
            tmrTimeOutPago.Start();
            tmrHora.Start();
            tmrHora.Interval = 1000;
            tmrHora.Tick += tmrHora_Tick;
            CargarUsuario();
            CargaImagenes();
            ReestablecerBotonesLateralDerechoPrincipal();
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonInferior();

            //Funcion ClickPlacaBuscar 

            clickTimer = new System.Windows.Forms.Timer();
            clickTimer.Interval = 15000; 
            clickTimer.Tick += (s, e) =>
            {
                clickTimer.Stop(); 
                tmrHora.Start(); 
            };


            var dataSource = new List<Object>();
            dataSource.Add(new { Name = oInfoPPMService.Sede, Value = oInfoPPMService.IdSede });

            //Setup data binding
            this.cbSede.DataSource = dataSource;
            this.cbSede.DisplayMember = "Name";
            this.cbSede.ValueMember = "Value";

            var dataSource2 = new List<Object>();
            dataSource2.Add(new { Name = oInfoPPMService.Estacionamiento, Value = oInfoPPMService.IdEstacionamiento });

            //Setup data binding
            this.cbEstacionamiento.DataSource = dataSource2;
            this.cbEstacionamiento.DisplayMember = "Name";
            this.cbEstacionamiento.ValueMember = "Value";

            var dataSource3 = new List<Object>();
            dataSource3.Add(new { Name = oInfoPPMService.Modulo, Value = oInfoPPMService.Modulo });

            //Setup data binding
            this.cbPPM.DataSource = dataSource3;
            this.cbPPM.DisplayMember = "Name";
            this.cbPPM.ValueMember = "Value";


        }

        private void Menu_Load(object sender, EventArgs e)
        {
            tabPrincipal.SelectedTab = tabMenuPrincipal;
            tabPrincipal.Appearance = TabAppearance.FlatButtons;
            tabPrincipal.ItemSize = new Size(0, 1);
            tabPrincipal.SizeMode = TabSizeMode.Fixed;
            ReestablecerBotonesLateralIzquierdo();
            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_PrincipalPresionado.png");


        }




    }
}
