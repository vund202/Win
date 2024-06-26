﻿using QLKTX.BS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX.UI
{
    public partial class FrmTimKiem : Form
    {
        public enum SearchType
        {
            NhanVien,
            SinhVien,
            PhieuDK,
            Phong,
            HoaDon
        }


        SearchType searchType;

        private TextBox txtKey = new TextBox()
        {
            Location = new Point(300, 28),
            Size = new Size(630, 25),
            Font = new Font("Segoe UI", 10)
        };

        private string error = "";

        public FrmTimKiem()
        {
            InitializeComponent();
            pnInput.Controls.Add(txtKey);
            txtKey.TextChanged += new EventHandler(txtKey_TextChanged);
            txtKey.Enter += new EventHandler(txtKey_Enter);
        }

        private void txtKey_Enter(object sender, EventArgs e)
        {
            txtKey.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BS_layer.TableName tableName = BS_layer.TableName.SinhVien;
            object selectType = (EnumConst.SinhVien)cmbMucTimKiem.SelectedIndex;
            string[] headerText = EnumConst.SinhVienHeaderText;

            switch (searchType)
            {
                case SearchType.SinhVien:
                    tableName = BS_layer.TableName.SinhVien;
                    selectType = (EnumConst.SinhVien)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.SinhVienHeaderText;
                    break;
                case SearchType.NhanVien:
                    tableName = BS_layer.TableName.NhanVien;
                    selectType = (EnumConst.NhanVien)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.NhanVienHeaderText;
                    break;
                case SearchType.PhieuDK:
                    tableName = BS_layer.TableName.PhieuDK;
                    selectType = (EnumConst.PhieuDK)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.PhieuDKHeaderText;
                    break;
                case SearchType.HoaDon:
                    tableName = BS_layer.TableName.HoaDon;
                    selectType = (EnumConst.HoaDon)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.HoaDonHeaderText;
                    break;
                case SearchType.Phong:
                    tableName = BS_layer.TableName.Phong;
                    selectType = (EnumConst.Phong)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.PhongHeaderText;
                    break;
                default:
                    tableName = BS_layer.TableName.HoaDon;
                    selectType = (EnumConst.HoaDon)cmbMucTimKiem.SelectedIndex;
                    headerText = EnumConst.HoaDonHeaderText;
                    break;
            }

            if (cmbMucTimKiem.Text != "Khu phòng")
                dgv.DataSource = FrmMain.bS_Layer.Select(ref error, tableName, selectType, txtKey.Text.Trim());
            else
                dgv.DataSource = FrmMain.bS_Layer.Select(ref error, tableName, strMaKhu: txtKhu.Text.Trim(), strMaPhong: txtPhong.Text.Trim());

            //Xóa cột AnhChanDung
            if (tableName == BS_layer.TableName.SinhVien || tableName == BS_layer.TableName.NhanVien)
                dgv.Columns.RemoveAt(dgv.Columns.Count - 1);

            for (int i = 0; i < dgv.Columns.Count; i++)
                dgv.Columns[i].HeaderText = headerText[i];
        }

        private void TimTheoLoai_Click(object sender, EventArgs e)
        {
            btnNhanVien.BackColor = SystemColors.Control;
            btnSinhVien.BackColor = SystemColors.Control;
            btnPhieuDK.BackColor = SystemColors.Control;
            btnPhong.BackColor = SystemColors.Control;
            btnHoaDon.BackColor = SystemColors.Control;
            var btn = sender as Button;
            btn.BackColor = Color.Gold;

            dgv.DataSource = null;
            cmbMucTimKiem.Items.Clear();
            cmbMucTimKiem.Text = "";
            txtKey.Clear();
            txtKhu.Clear();
            txtPhong.Clear();

            txtKey.Visible = true;

            if (btn.Name == "btnNhanVien")
            {
                searchType = SearchType.NhanVien;
                cmbMucTimKiem.Items.AddRange(EnumConst.SearchNhanVien);
            }
            else if (btn.Name == "btnSinhVien")
            {
                searchType = SearchType.SinhVien;
                cmbMucTimKiem.Items.AddRange(EnumConst.SearchSinhVien);
            }   
            else if (btn.Name == "btnPhieuDK")
            {
                searchType = SearchType.PhieuDK;
                cmbMucTimKiem.Items.AddRange(EnumConst.SearchPhieuDK);
            }   
            else if (btn.Name == "btnPhong")
            {
                searchType = SearchType.Phong;
                cmbMucTimKiem.Items.AddRange(EnumConst.SearchPhong);
            }   
            else if (btn.Name == "btnHoaDon")
            {
                searchType = SearchType.HoaDon;
                cmbMucTimKiem.Items.AddRange(EnumConst.SearchHoaDon);
            }    
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            if (txtKey.Text.EndsWith(" ")) 
                btnSearch_Click(sender, e);
        }

        private void cmbMucTimKiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv.DataSource = null;
            if (cmbMucTimKiem.Text == "Khu phòng")
            {
                txtKey.Visible = false;
                pnKey.Visible = true;
            } 
            else
            {
                txtKey.Visible = true;
                pnKey.Visible = false;
            }
            if (cmbMucTimKiem.SelectedIndex >= 0)
                btnSearch.Enabled = true;
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            switch (this.searchType)
            {
                case SearchType.NhanVien:
                    FrmNhanVien nhanVien = new FrmNhanVien(dgv.Rows[r].Cells[0].Value.ToString());
                    nhanVien.ShowDialog();
                    break;
                case SearchType.SinhVien:
                    FrmSinhVien sinhVien = new FrmSinhVien(dgv.Rows[r].Cells[0].Value.ToString());
                    sinhVien.ShowDialog();
                    break;
                case SearchType.PhieuDK:
                    FrmDangKyPhong frmDangKy = new FrmDangKyPhong(dgv.Rows[r].Cells[0].Value.ToString());
                    frmDangKy.ShowDialog();
                    break;
                case SearchType.Phong:
                    FrmPhong phong = new FrmPhong(
                        Khu: dgv.Rows[r].Cells[1].Value.ToString(),
                        MaPhong: dgv.Rows[r].Cells[0].Value.ToString());
                    phong.ShowDialog();
                    break;
                case SearchType.HoaDon:
                    FrmHoaDon hoaDon = new FrmHoaDon(MaHD: dgv.Rows[r].Cells[0].Value.ToString());
                    hoaDon.ShowDialog();
                    break;
            }

            //cập nhật lại những thay đổi, nếu có
            btnSearch_Click(btnSearch, new EventArgs());
        }
    }
}