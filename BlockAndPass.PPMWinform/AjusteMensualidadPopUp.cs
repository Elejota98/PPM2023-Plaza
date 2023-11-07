using BlockAndPass.PPMWinform.ByPServices;
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
    public partial class AjusteMensualidadPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        public AjusteMensualidadPopUp()
        {
            InitializeComponent();
        }

        private void CargarDatosDataTable()
        {
           
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            InfoDatosAutorizadoMensualidadResponse oInfoAutorizado = cliente.ObtenerInformacionAutorizadoMensualidad(txtDocumentoBuscar.Text, txtPlaca1Buscar.Text);

            if (oInfoAutorizado.Exito)
            {
                DvgListadoPersonasAutorizadas.DataSource = oInfoAutorizado.lstInfoDatosAutorizadoResponse;
            }
        }

        private void AjusteMensualidadPopUp_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ActualizarDatosMensualidadResponse oActualizarDatosMensualidadResponse = cliente.ActualizaDatosMensualidad(txtDocumento.Text, txtPlaca1.Text, txtPlaca2.Text, DtmFechaInicio.Text, DtmFechaFin.Text);
            if (oActualizarDatosMensualidadResponse.Exito)
            {
                MessageBox.Show("Datos guardados exitosamente", "Actualizar datos mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void DvgListadoPersonasAutorizadas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == DvgListadoPersonasAutorizadas.Columns["Editar"].Index)
                {
                    DataGridViewCheckBoxCell ChkEditar = (DataGridViewCheckBoxCell)DvgListadoPersonasAutorizadas.Rows[e.RowIndex].Cells["Editar"];
                    ChkEditar.Value = !Convert.ToBoolean(ChkEditar.Value);
                    if (Convert.ToBoolean(ChkEditar.Value) == true)
                    {
                            txtDocumento.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Documento"].Value);
                        txtDocumento.Enabled = false;
                            //CargarAutorizacionesPorId();
                            txtNombreApellidos.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["NombreApellidos"].Value);
                            //txtEmpresa.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["NombreEmpresa"].Value);
                            //TxtNit.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Nit"].Value);
                            //txtTelefono.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Telefono"].Value);
                            //txtEmail.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Email"].Value);
                            txtPlaca1.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Placa1"].Value);
                            txtPlaca2.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Placa2"].Value);
                            txtPlaca3.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Placa3"].Value);
                            //txtPlaca4.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Placa4"].Value);
                            //txtPlaca5.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Placa5"].Value);
                            DtmFechaInicio.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["FechaInicio"].Value);
                            DtmFechaFin.Text = Convert.ToString(DvgListadoPersonasAutorizadas.CurrentRow.Cells["FechaFin"].Value);
                            //Boolean Estado = Convert.ToBoolean(DvgListadoPersonasAutorizadas.CurrentRow.Cells["Estado"].Value);
                            //if (Estado == true)
                            //{
                            //    chkEstado.Checked = true;
                            //}
                            //else
                            //{
                            //    chkEstado.Checked = false;
                            //}
                            //Bloquear();
                            cboAutorizados.Enabled = true;
                            //DtmFechaInicio.Enabled = true;
                            //DtmFechaFin.Enabled = true;
                            //btnActualizar.Visible = true;
                            //btnGuardar.Visible = false;
                            txtDocumento.Enabled = false;

                    }


                }

            }
            catch (Exception ex)
            {

                txtEmpresa.Text = "0";
                TxtNit.Text = "0";
            }
        }
    }
}
