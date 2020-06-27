﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKTX.DB;

namespace QLKTX.BS
{
    public partial class BS_layer
    {
        public DB_Main db = null;
        string error = "";

        public BS_layer()
        {
            db = new DB_Main();
        }

        public enum TableName
        {
            SinhVien,
            NhanVien,
            PhieuDK,
            HoaDon,
            Phong,
            DichVu,
            LoaiPhong,
            SDDV
        }

        public DataTable Select(ref string error, TableName table, object selectType, string strValue = "")
        {
            string strTableName = table.ToString();
            string strType = selectType.ToString();
            string sql = "";
            SqlParameter[] sqlParameters = new SqlParameter[] { };
            switch (selectType)
            {
                case EnumConst.NhanVien.All:
                case EnumConst.SinhVien.All:
                case EnumConst.Phong.All:
                case EnumConst.PhieuDK.All:
                case EnumConst.HoaDon.All:
                case EnumConst.DichVu.All:
                case EnumConst.LoaiPhong.All:
                    sql = $"SELECT * FROM {strTableName}";
                    break;
                case EnumConst.SinhVien.HoTen:
                case EnumConst.NhanVien.HoTen:
                    strValue = "%" + strValue + "%";
                    sql = $"SELECT * FROM {strTableName} WHERE {strType} LIKE @Value";
                    sqlParameters = new SqlParameter[]
                    {
                        new SqlParameter("Value", strValue)
                    };
                    break;
                default:
                    sql = $"SELECT * FROM {strTableName} WHERE {strType} = @Value";
                    sqlParameters = new SqlParameter[]
                    {
                        new SqlParameter("Value", strValue)
                    };
                    break;
            }
            return db.ExecuteQuery(sql, sqlParameters, CommandType.Text, ref error);
        }

        public bool Delete(TableName tableName, string strKey, ref string err)
        {
            string strTableName = tableName.ToString();
            string key = "";
            switch (tableName)
            {
                case TableName.SinhVien:    key = "MSSV"; break;
                case TableName.NhanVien:    key = "MaNV"; break;
                case TableName.HoaDon:      key = "MaHD"; break;
                case TableName.PhieuDK:     key = "MaPDK";break;
                case TableName.LoaiPhong:   key = "MaLoaiPhong"; break;
                case TableName.DichVu:      key = "MaDV"; break;
                    //case Phong
            }    
            string sql = $"DELETE FROM {strTableName} WHERE {key}=@Key";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("Key", strKey)
            };
            return db.ExecuteNonQuery(sql, sqlParameters, CommandType.Text, ref err);
        }
    }
}