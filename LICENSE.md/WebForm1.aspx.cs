using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;

namespace payroll
{
    public partial class WebForm1 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        string condition;
        List<string> listM = new List<string>();
        List<string> listE = new List<string>();
        List<string> listF = new List<string>();
        List<string> listA = new List<string>();
        List<string> listB = new List<string>();
        List<string> listC = new List<string>();
        List<string> listD = new List<string>();
        List<int> listG = new List<int>();
        string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\PayrollData.mdf;Integrated Security=True";
        int k = 0;
        protected void Button1_Click(object sender, EventArgs e)
  
        {

            if (filename != "")
            {
            
                using (TextFieldParser csvParser = new TextFieldParser(@filename))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    csvParser.ReadLine();

                  

                  
                   

                
                using (var reader = new StreamReader(@filename))

                {

                    while (!reader.EndOfStream)
                    {


                        var splits = reader.ReadLine().Split(',');

                        listE.Add(splits[0]);
                        listF.Add(splits[1]);

                        k++;
                    }

                }
            }
                int i = 1;

                k = listF.Count;
                using (var rdd = new StreamReader(@filename))
                {
                    while (i < k)
                    {


                        var splits = rdd.ReadLine().Split(',');

                        listA.Add(splits[0]);
                        listB.Add(splits[1]);
                        listC.Add(splits[2]);
                        listD.Add(splits[3]);

                        i++;

                    }

                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        
                        try
                        {
                            con.Open();
                           
                            using (SqlCommand command = new SqlCommand("INSERT INTO ReportIDs(IDs) VALUES(@ID)", con))

                            {
                                command.Parameters.AddWithValue("@ID", Int32.Parse(listF[k - 1]));

                                command.ExecuteNonQuery();

                            }


                        }


                        catch (Exception ex)

                        {
                            condition = "Attention!!! file already processed";
                        }
                    }
                    if (condition == "Attention!!! file already processed")
                    {
                        MessageBox.Show(condition);
                    }
                    else
                    {


                        getReport();

                    }

                }
            }
            else
            {

                getReport();
            }
            
        }

        public void getReport()
        {
            int A, B, hr;

            Double C, D;
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            hr = 0;

            k = listA.Count;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                for (int j = 1; j < k; j++)

                {

                    using (SqlCommand command = new SqlCommand("INSERT INTO PayrollData(PayDate, HoursWorked, employeeID,JobGrp) VALUES(@PayDate, @HoursWorked, @employeeID,@JobGrp)", con))

                    {
                        command.Parameters.AddWithValue("@PayDate", listA[j]);
                        command.Parameters.AddWithValue("@HoursWorked", listB[j]);
                        command.Parameters.AddWithValue("@employeeID", listC[j]);
                        command.Parameters.AddWithValue("@JobGrp", listD[j]);
                        command.ExecuteNonQuery();

                    }
                }
            }


            using (SqlConnection con = new SqlConnection(conStr))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                List<string> listC = new List<string>();
                List<string> listD = new List<string>();

                using (SqlCommand command = new SqlCommand("Select * from PayrollData", con))
                {
                    con.Open();
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        listA.Add(dr["PayDate"].ToString());
                        listB.Add(dr["HoursWorked"].ToString());
                        listC.Add(dr["EmployeeID"].ToString());
                        listD.Add(dr["JobGrp"].ToString());

                    }
                }

                StringBuilder csvcont = new StringBuilder();
                csvcont.AppendLine(("Employee ID, Pay Period, Amount Paid"));
                k = listB.Count;

                string filepath = @"C:\Users\winuser\Documents\PayrollReport.CSV";
                try
                {
                    File.AppendAllText(filepath, csvcont.ToString());
                
                    using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
                    FileMode.Create, FileAccess.Write)))
                    {


                        writer.WriteLine("Employee ID, Pay Period, Amount Paid");

                        for (int z = 0; z < k; z++)
                        {
                            string t = (listA[z]).Substring(1, 1);

                            if (t == "/")
                            {

                                listA[z] = "0" + listA[z];

                            }
                        }

                        k = listC.Count;

                        string m;
                        m = "";
                        for (int i = 1; i < k; i++)
                        {
                            if (listM.Contains(listC[i]))
                            {

                                m = "nothing yet";
                            }
                            else
                            {
                                listM.Add(listC[i]);

                            }

                        }
                        for (int j = 0; j < k; j++)
                        {
                            if (listD[j] == "A")
                            {
                                listG.Add(30);
                            }
                            else
                            {
                                listG.Add(20);
                            }
                        }

                        int count = 1;
                        int amount = listM.Count + 1;

                        k = listC.Count;

                        while (count < amount)
                        {
                            for (int j = 0; j < k; j++)
                            {

                                if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) <= 15)
                                {

                                    A++;

                                    C += Convert.ToDouble(listB[j]);

                                    hr = listG[j];


                                }
                                if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) > 15)
                                {

                                    B++;
                                    D += Convert.ToDouble(listB[j]);

                                    hr = listG[j];


                                }



                            }
                            count++;

                            if (A != 0)
                            {
                                writer.WriteLine((count - 1).ToString() + "," + "01/MM/YYYY - 15/MM/YYYY" + "," + (C * hr).ToString());
                            }
                            if (B != 0)
                            {
                                writer.WriteLine((count - 1).ToString() + "," + "16/MM/YYYY - 30/MM/YYYY" + "," + (D * hr).ToString());
                            }

                            A = 0;
                            B = 0;
                            C = 0;
                            D = 0;
                        }

                        MessageBox.Show("Find your file in the documents folder");
                    }
                }
                catch(Exception ex) {
                    MessageBox.Show(ex.Message);

                }
        }
        }


    private static string filename="";
       
        static void ThreadMethod()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            filename = dlg.FileName;
           
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int k = 0;
            try
            {
                using (TextFieldParser csvParser = new TextFieldParser(@filename))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    csvParser.ReadLine();



                    using (var reader = new StreamReader(@filename))

                    {
                        List<string> listM = new List<string>();



                        while (!reader.EndOfStream)
                        {


                            var splits = reader.ReadLine().Split(',');


                            listM.Add(splits[1]);

                            k++;
                        }


                        string filepath = @"C:\Users\winuser\Documents\PayrollReport.CSV";


                        if (File.Exists(filepath))
                        {


                            MessageBox.Show("Test Passed file found");
                        }
                        else
                        {
                            MessageBox.Show("Test Failed file not found");
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Upload a CSV file");
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
           
            Thread newThread = new Thread(new ThreadStart(ThreadMethod));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
           
        }
    }
}



















        
  
        
    

