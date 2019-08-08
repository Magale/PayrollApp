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
        int k = 0;
        string condition;
        
        //string filename;
        protected void Button1_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(filename);
            //var path = @filename;
            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\PayrollData.mdf;Integrated Security=True";

            try
            {
                using (TextFieldParser csvParser = new TextFieldParser(@filename))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    csvParser.ReadLine();


                    // List<string> termsListA = new List<string>();
                    //List<string> termsListB = new List<string>();
                    //List<string> termsListC = new List<string>();
                    //List<string> termsListD = new List<string>();

                    using (var reader = new StreamReader(@filename))

                    {
                        List<string> listM = new List<string>();
                        List<string> listE = new List<string>();
                        List<string> listF = new List<string>();
                        List<string> listA = new List<string>();
                        List<string> listB = new List<string>();
                        List<string> listC = new List<string>();
                        List<string> listD = new List<string>();
                        List<string> listG = new List<string>();


                        while (!reader.EndOfStream)
                        {

                            //var line = reader.ReadLine();
                            var splits = reader.ReadLine().Split(',');

                            listE.Add(splits[0]);
                            listF.Add(splits[1]);
                            //listC.Add(splits[2]);
                            k++;
                        }
                       

                        int i = 1;
                        using (var rd = new StreamReader(@filename))
                        {
                            while (i < k)
                            {

                                //var line = reader.ReadLine();
                                var splits = rd.ReadLine().Split(',');

                                listA.Add(splits[0]);
                                listB.Add(splits[1]);
                                listC.Add(splits[2]);
                                listD.Add(splits[3]);
                                // MessageBox.Show((listC.Count).ToString());
                                i++;

                            }
                            k = listF.Count;
                            string repoID = "Payroll" + listF[k - 1];
                            using (SqlConnection con = new SqlConnection(conStr))
                            {
                                try
                                {
                                    con.Open();
                                    using (SqlCommand command = new SqlCommand("CREATE TABLE " + repoID + "( PayDate char(50), HoursWorked char(50),employeeID Char(50), JobGrp Char(50) );", con))
                                    {
                                        command.ExecuteNonQuery();

                                    }
                                    for (int j = 1; j < k - 1; j++)

                                    {
                                        //using (SqlCommand command = new SqlCommand("INSERT INTO " + repoID + "(PayDate, HoursWorked, employeeID,JobGrp) VALUES(" + listA[j] + "," + listB[j] + "," + listC[j] + "," + listD[j] + ");", con))
                                        using (SqlCommand command = new SqlCommand("INSERT INTO " + repoID + "(PayDate, HoursWorked, employeeID,JobGrp) VALUES(@PayDate, @HoursWorked, @employeeID,@JobGrp)", con))

                                        {
                                            command.Parameters.AddWithValue("@PayDate", listA[j]);
                                            command.Parameters.AddWithValue("@HoursWorked", listB[j]);
                                            command.Parameters.AddWithValue("@employeeID", listC[j]);
                                            command.Parameters.AddWithValue("@JobGrp", listD[j]);
                                            command.ExecuteNonQuery();

                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                              condition = "";
                                }
                            }
                            k = listC.Count;
                          
                            string m;
                            m = "";
                            for (i = 1; i < k; i++)
                            {
                                if (listM.Contains(listC[i]))
                                {

                                    m = "nothig yet";
                                }
                                else
                                {
                                    listM.Add(listC[i]);
                                    //MessageBox.Show((listM[k]));
                                }


                            }

                            //MessageBox.Show((listM.Count).ToString());
                            //MessageBox.Show(k.ToString());
                            //MessageBox.Show(k.ToString());
                        }

                        //termsListC.Add("K");
                        //termsListC.Add("K");
                        //termsListA.Add(fields[0]);
                        //termsListB.Add(fields[1]);
                        //termsListC.Add(fields[2]);
                        //termsListC.Add(fields[3]);
                        //int k = termsListA.Count;
                        //MessageBox.Show(k.ToString());

                        //int size = fields[2].Length;
                        int A, B, hr;
                        string hrs;
                        Double C, D;
                        A = 0;
                        B = 0;
                        C = 0;
                        D = 0;
                        hr = 0;
                        hrs = "";
                        if (condition == "Attention!!! file aready processed")
                        {
                            MessageBox.Show(condition);
                        }
                        else
                        {
                            StringBuilder csvcont = new StringBuilder();
                            csvcont.AppendLine(("Employee ID, Pay Period, Amount Paid"));
                            k = listB.Count;
                            string filepath = @"C:\Users\winuser\Documents\report" + listF[k] + ".CSV";

                            File.AppendAllText(filepath, csvcont.ToString());
                            using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
                            FileMode.Create, FileAccess.Write)))
                            {

                                // writer.WriteLine("sep=,");
                                writer.WriteLine("Employee ID, Pay Period, Amount Paid");

                                for (int z = 0; z < k; z++)
                                {
                                    string t = (listA[z]).Substring(1, 1);
                                    //MessageBox.Show(t);
                                    if (t == "/")
                                    {

                                        listA[z] = "0" + listA[z];

                                    }
                                }


                                int count = 0;

                                while (count < 5)
                                {
                                    for (int j = 1; j < k; j++)
                                    {
                                        //int key = 0;
                                        if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) <= 15)
                                        {

                                            A++;
                                            //MessageBox.Show((listA[j]).Substring(0, 2));
                                            C += Convert.ToDouble(listB[j]);
                                            //k++;
                                            hrs = listD[j];
                                        }
                                        if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) > 15)
                                        {


                                            //MessageBox.Show((listA[j]).Substring(0, 2));
                                            B++;
                                            D += Convert.ToDouble(listB[j]);
                                            //k++;
                                            hrs = listD[j];
                                        }


                                    }
                                    count++;
                                    if (hrs == "A")
                                    {
                                        hr = 20;
                                    }
                                    else
                                    {
                                        hr = 30;
                                    }


                                    if (A != 0)
                                    {
                                        writer.WriteLine((count - 1).ToString() + "," + "1/11/2016 - 15/11/2016" + "," + (C * hr).ToString());
                                    }
                                    if (B != 0)
                                    {
                                        writer.WriteLine((count - 1).ToString() + "," + "16/11/2016 - 30/11/2016" + "," + (D * hr).ToString());
                                    }
                                    A = 0;
                                    B = 0;
                                    C = 0;
                                    D = 0;
                                }
                                MessageBox.Show("Find your file in the documents folder");
                            }
                            //  MessageBox.Show(A.ToString());
                            // MessageBox.Show(B.ToString());

                        }

                        // MessageBox.Show(count.ToString());
                        // MessageBox.Show(A.ToString());
                        //MessageBox.Show(B.ToString());
                        //MessageBox.Show(C.ToString());
                        //MessageBox.Show(D.ToString());
                    }
                }



            }
            catch (Exception ex)
            {
                StringBuilder csvcont = new StringBuilder();
                csvcont.AppendLine(("Employee ID, Pay Period, Amount Paid"));
                //k = listB.Count;
                string filepath = @"C:\Users\winuser\Documents\report00.CSV";

                File.AppendAllText(filepath, csvcont.ToString());
                using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
                FileMode.Create, FileAccess.Write)))
                {

                    // writer.WriteLine("sep=,");
                    writer.WriteLine("Employee ID, Pay Period, Amount Paid");
                }
                MessageBox.Show("Find your file in the documents folder");
            }
        }


        private static string filename;
        protected void Button2_Click(object sender, EventArgs e)
        {

            Thread newThread = new Thread(new ThreadStart(ThreadMethod));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
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


                    // List<string> termsListA = new List<string>();
                    //List<string> termsListB = new List<string>();
                    //List<string> termsListC = new List<string>();
                    //List<string> termsListD = new List<string>();

                    using (var reader = new StreamReader(@filename))

                    {
                        List<string> listM = new List<string>();



                        while (!reader.EndOfStream)
                        {

                            //var line = reader.ReadLine();
                            var splits = reader.ReadLine().Split(',');


                            listM.Add(splits[1]);
                            //listC.Add(splits[2]);
                            k++;
                        }

                        string path = @"C:\Users\winuser\Documents\report00.CSV";

                        string filepath = @"C:\Users\winuser\Documents\report" + listM[k - 1] + ".CSV";
                     

                        if (File.Exists(filepath) || File.Exists(path))
                        {

                            // writer.WriteLine("sep=,");

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
    }
}
        
        
  
        
    

