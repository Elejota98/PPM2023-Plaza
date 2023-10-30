using BlockAndPass.PPMWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class Menu : Form, IMessageFilter
    {

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
          

        }
        #endregion
        public Menu(string sDocumento, string sCargo, InfoPPMService oInfoPPMService)
        {
            InitializeComponent();

            _DocumentoUsuario = sDocumento;
            _CargoUsuario = sCargo;
            Application.AddMessageFilter(this);

            controlsToMove.Add(this);
            //tmrTimeOutPago.Start();
            tmrHora.Interval = 1000;
            tmrHora.Tick += tmrHora_Tick;
            tmrHora.Start();

        }

        private void Menu_Load(object sender, EventArgs e)
        {
            tabPrincipal.SelectedTab = tabMenuPrincipal;
            tabPrincipal.Appearance = TabAppearance.FlatButtons;
            tabPrincipal.ItemSize = new Size(0, 1);
            tabPrincipal.SizeMode = TabSizeMode.Fixed;
        }
    }
}
