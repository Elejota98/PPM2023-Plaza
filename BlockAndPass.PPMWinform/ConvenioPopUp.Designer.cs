namespace BlockAndPass.PPMWinform
{
    partial class ConvenioPopUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvenioPopUp));
            this.cbConvenio = new System.Windows.Forms.ComboBox();
            this.tbDescripcion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.btn_Ok = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // cbConvenio
            // 
            this.cbConvenio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConvenio.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConvenio.FormattingEnabled = true;
            this.cbConvenio.Location = new System.Drawing.Point(17, 94);
            this.cbConvenio.Name = "cbConvenio";
            this.cbConvenio.Size = new System.Drawing.Size(474, 32);
            this.cbConvenio.TabIndex = 3;
            this.cbConvenio.SelectedIndexChanged += new System.EventHandler(this.cbConvenio_SelectedIndexChanged);
            // 
            // tbDescripcion
            // 
            this.tbDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDescripcion.Location = new System.Drawing.Point(17, 169);
            this.tbDescripcion.Multiline = true;
            this.tbDescripcion.Name = "tbDescripcion";
            this.tbDescripcion.ReadOnly = true;
            this.tbDescripcion.Size = new System.Drawing.Size(474, 96);
            this.tbDescripcion.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "Convenio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Convenio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Descripcion";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(64)))), ((int)(((byte)(97)))));
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Icon = FontAwesome.Sharp.IconChar.Times;
            this.btn_Cancel.IconColor = System.Drawing.Color.White;
            this.btn_Cancel.IconSize = 20;
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(271, 284);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 35);
            this.btn_Cancel.TabIndex = 49;
            this.btn_Cancel.Text = "Cerrar";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click_1);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(165)))), ((int)(((byte)(64)))));
            this.btn_Ok.FlatAppearance.BorderSize = 0;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.ForeColor = System.Drawing.Color.White;
            this.btn_Ok.Icon = FontAwesome.Sharp.IconChar.Check;
            this.btn_Ok.IconColor = System.Drawing.Color.White;
            this.btn_Ok.IconSize = 20;
            this.btn_Ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_Ok.Image")));
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(389, 284);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(102, 35);
            this.btn_Ok.TabIndex = 48;
            this.btn_Ok.Text = "Confirmar";
            this.btn_Ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click_1);
            // 
            // ConvenioPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(515, 331);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDescripcion);
            this.Controls.Add(this.cbConvenio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConvenioPopUp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbConvenio;
        private System.Windows.Forms.TextBox tbDescripcion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private FontAwesome.Sharp.IconButton btn_Cancel;
        private FontAwesome.Sharp.IconButton btn_Ok;
    }
}