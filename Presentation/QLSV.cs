using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BusinessLogic;
using DataAccess;
using System.IO;
namespace Presentation
{
    public partial class QLSV : DevExpress.XtraEditors.XtraForm
    {
        string path = "../../Hinh";
        XL_SINHVIEN Bang_SINHVIEN;
        XL_LOP Bang_LOP;
        BindingManagerBase DSSV;
        public QLSV()
        {
            InitializeComponent();
        }

        private void QLSV_Load(object sender, EventArgs e)
        {
            Bang_SINHVIEN = new XL_SINHVIEN();
            Bang_LOP = new XL_LOP();

            loadCBOLop();
            loadDGVHocSinh();
            txtMaSV.DataBindings.Add("text", Bang_SINHVIEN, "MaSV", true);
            txtHoTen.DataBindings.Add("text", Bang_SINHVIEN, "HoTen", true);
            dateNgaySinh.DataBindings.Add("text", Bang_SINHVIEN, "NgaySinh", true);
            radNam.DataBindings.Add("checked", Bang_SINHVIEN, "GioiTinh", true);
            cboLop.DataBindings.Add("SelectedValue", Bang_SINHVIEN, "MaLop", true);
            txtQueQuan.DataBindings.Add("text", Bang_SINHVIEN, "DiaChi", true);
            pHinh.DataBindings.Add("ImageLocation", Bang_SINHVIEN, "Hinh", true);

            DSSV = this.BindingContext[Bang_SINHVIEN];
            enabledNutLenh(false);
        }

        private void loadDGVHocSinh()
        {
            dgvDSSV.AutoGenerateColumns = false;
            dgvDSSV.DataSource = Bang_SINHVIEN;
        }

        private void loadCBOLop()
        {
            cboLop.DataSource = Bang_LOP;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        private void enabledNutLenh(bool p)
        {
            btnThem.Enabled = !p;
            btnXoa.Enabled = !p;
            btnSua.Enabled = !p;
            btnThoat.Enabled = !p;
            btnLuu.Enabled = p;
            btnHuy.Enabled = p;
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radNam_CheckedChanged(object sender, EventArgs e)
        {
            radNu.Checked = !radNam.Checked;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DSSV.AddNew();
            enabledNutLenh(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                Bang_SINHVIEN.Ghi();
                enabledNutLenh(false);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DSSV.CancelCurrentEdit();
            Bang_SINHVIEN.RejectChanges();
            enabledNutLenh(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DSSV.RemoveAt(DSSV.Position);
            if(!Bang_SINHVIEN.Ghi())
            {
                MessageBox.Show("Xóa thất bại !");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                Bang_SINHVIEN.Ghi();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow r = Bang_SINHVIEN.Select("MaSV='" + txtTimKiem.Text + "' ")[0];
                DSSV.Position = Bang_SINHVIEN.Rows.IndexOf(r);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTimKiem_MouseDown(object sender, MouseEventArgs e)
        {
            txtTimKiem.Text = "";
        }

        private void btnHinh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Files|*.jpg|PNG Files|*.png|All Files|*.*";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string fileName = openFileDialog1.SafeFileName;
                string pathFile = path + "/" + fileName;
                if(!File.Exists(pathFile))
                File.Copy(openFileDialog1.FileName, pathFile);
                pHinh.ImageLocation= pathFile;
            }
        }
    }
}