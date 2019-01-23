/// <summary>
/// ʹ��OLEDB����Excel
/// </summary>
/// <param name="dt">���ݼ�</param>
/// <param name="filepath">�ļ�Ŀ¼���ļ���</param>
/// <param name="tablename">SHEETҳ����</param>
/// <param name="pagecount">ÿҳ��¼��</param>
public static void Export(DataTable dt, string filepath, string tablename, int pagecount)
{
    //excel 2003��ʽ
    string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
    //Excel 2007��ʽ
    //string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0 Xml;";
    try
    {
        using (OleDbConnection con = new OleDbConnection(connString))
        {
            con.Open();
            
            //��ʼ��ҳ
            if (dt.Rows.Count > pagecount)
            {
                int page = dt.Rows.Count / pagecount + 1; //��ҳ��
                for (int i = 0; i < page; i++)
                {
                    //����sheet�ͱ�ͷ
                    StringBuilder strSQL = new StringBuilder();
                    string tabname = tablename + i.ToString();
                    strSQL.Append("CREATE TABLE ").Append("[" + tabname + "]"); //ÿ60000�һҳ
                    strSQL.Append("(");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        strSQL.Append("[" + dt.Columns[j].ColumnName + "] text,");
                    }
                    strSQL = strSQL.Remove(strSQL.Length - 1, 1);
                    strSQL.Append(")");
 
                    OleDbCommand cmd = new OleDbCommand(strSQL.ToString(), con);
                    cmd.ExecuteNonQuery();
 
                    //׼��������������
                    for (int j = i * pagecount; j < (i + 1) * pagecount; j++)
                    {
                        if (i == 0 || j < dt.Rows.Count)
                        {
                            StringBuilder tmp = new StringBuilder();
                            StringBuilder strfield = new StringBuilder();
                            StringBuilder strvalue = new StringBuilder();
                            for (int z = 0; z < dt.Columns.Count; z++)
                            {
                                strfield.Append("[" + dt.Columns[z].ColumnName + "]");
                                strvalue.Append("'" + dt.Rows[j][z].ToString() + "'");
                                if (z != dt.Columns.Count - 1)
                                {
                                    strfield.Append(",");
                                    strvalue.Append(",");
                                }
                                else
                                {
                                }
                            }
                            cmd.CommandText = tmp.Append(" insert into [" + tabname + "]( ")
                                .Append(strfield.ToString())
                                .Append(") values (").Append(strvalue).Append(")").ToString();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            con.Close();
            no = count;
        }
        Console.WriteLine("OK");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    GC.Collect();
}
 
