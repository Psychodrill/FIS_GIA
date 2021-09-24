using FBS.CompositionsPathGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBS.ComposisionFix
{
    public partial class MainFrm : Form
    {
        public Dictionary<string, ERBDCompositionInfo> compdataFromFiles = null;
        private DataTable compMissingData;
        private DataTable erbdData;
        

        public void GetMissingData(int a_inst_id)
        {
            SqlConnectionStringBuilder strConn = new SqlConnectionStringBuilder();
            strConn.ConnectionString = "server = 10.0.3.8; user id = akazantsev; " +
            "password= Unc541l_11;initial catalog=gvuz_start_2016";
            using (SqlConnection connection = new SqlConnection(strConn.ConnectionString))
            {
                var bindingSource = new BindingSource();
                string missingCompositionsQry = FBS.CompositionsPathGenerator.FbsCompExtensions.GetMissingPaths();
                SqlCommand cmd = new SqlCommand(missingCompositionsQry, connection);
                cmd.Parameters.Add("@inst_id", SqlDbType.Int).Value = a_inst_id;
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    try
                    {
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        bindingSource.DataSource = table;
                        compMissingData = table;
                        dgvMissingCompPaths.ReadOnly = true;
                        dgvMissingCompPaths.DataSource = bindingSource;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "ERROR Loading");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }



        public void GetErbdCompInfo()
        {
            SqlConnectionStringBuilder strConn = new SqlConnectionStringBuilder();
            strConn.ConnectionString = "server = 10.0.3.2; user id = akazantsev; " +
            "password= Qwerty123;initial catalog=ERBD_2015";
            using (SqlConnection connection = new SqlConnection(strConn.ConnectionString))
            {
                var bindingSource = new BindingSource();
                string erdbCompoInfoQry = FBS.CompositionsPathGenerator.FbsCompExtensions.GetErbdCompositionsInfo();
                SqlCommand cmd = new SqlCommand(erdbCompoInfoQry, connection);
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    try
                    {
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        bindingSource.DataSource = table;
                        erbdData = table;
                        dgvErbdCompInfo.ReadOnly = true;
                        dgvErbdCompInfo.DataSource = bindingSource;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "ERROR Loading");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}


