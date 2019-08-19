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
        List<string> listN = new List<string>();
        List<int> listP = new List<int>();
   
        List<int> listG = new List<int>();
        List<List<string>> listOflist = new List<List<string>>(); 
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

                using (SqlCommand command = new SqlCommand("Select * from PayrollData ORDER BY employeeID", con))
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
                        k = listB.Count;
                        for (int z = 0; z < k; z++)
                        { 
                            string b = (listA[z].Substring(4).Substring(0,1));
                         
                          
                            if (b == "/")
                            {

                                listA[z] = listA[z].Substring(0, 2) + "/0" + listA[z].Substring(3).Substring(0, 3).Substring(0, 1) + listA[z].Substring(4);

                            }

                        }
                        k = listB.Count;
                      
                        //MessageBox.Show(listA[32]);


                        k = listC.Count;

                        string m;
                        m = "";
                        for (int i = 0; i < k; i++)
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
                        k = listA.Count;
                        for (int i = 0; i < k; i++)
                        {
                       

                            if (listP.Contains(Int32.Parse(listA[i].Substring(6))))
                            {

                                m = "nothing yet";
                            }
                            else
                            {
                                listP.Add(Int32.Parse(listA[i].Substring(6)));

                            }

                        }

                       

                        k = listA.Count;
                        for (int i =0; i < k; i++)
                        {
                           

                            if (listN.Contains(listA[i].Substring(3).Substring(0, 2)))
                            {

                                m = "nothing yet";
                            }
                            else
                            {
                                listN.Add(listA[i].Substring(3).Substring(0, 2));

                            }
                        }
                        // MessageBox.Show((listN.Count).ToString());
                        k = listD.Count;
                        string val = "A";
                        for (int j = 0; j < k; j++)
                        {
                           
                        
                        if (listD[j].Substring(0,1) == val)
                            {
                                listG.Add(20);
                            }
                            else
                            {
                                listG.Add(30); 
                            }
                            //MessageBox.Show(listG[j].ToString());
                        }


                        string rep,rep1;
                        rep = "";
                        rep1 = "";
                        k = listB.Count;
                        int lim1 = listP.Count;
                        //MessageBox.Show(listN[2]);
                        for (int t = 0; t < lim1; t++)
                        {
                            int lim = listN.Count;
                            for (int x = 0; x < lim; x++)
                            {
                                int count = 1;
                                int amount = listM.Count + 1;

                                while (count < amount)
                                {
                                    for (int j = 0; j < k; j++)
                                    {

                                        if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) <= 15 && listN[x] == listA[j].Substring(3).Substring(0, 2) && listP[t] == Int32.Parse(listA[j].Substring(6)))
                                        {

                                            A++;

                                            C += Convert.ToDouble(listB[j]);

                                            hr = listG[j];

                                            rep = "01/" + listA[j].Substring(3).Substring(0, 2) + "/" + listA[j].Substring(6) + "-" + "15/" + listA[j].Substring(3).Substring(0, 2) + "/" + listA[j].Substring(6);
                                        }
                                        if (Int32.Parse(listC[j]) == count && Int32.Parse((listA[j]).Substring(0, 2)) > 15 && listN[x] == listA[j].Substring(3).Substring(0, 2) && listP[t] == Int32.Parse(listA[j].Substring(6)))
                                        {

                                            B++;
                                            D += Convert.ToDouble(listB[j]);

                                            hr = listG[j];
                                           rep1 = "16/"+ listA[j].Substring(3).Substring(0, 2)+"/"+ listA[j].Substring(6) +"-"+ "30/" + listA[j].Substring(3).Substring(0, 2) + "/" + listA[j].Substring(6);

                                        }


                                    }
                                    count++;


                                    if (A != 0)
                                    {
                                        writer.WriteLine((count - 1).ToString() + "," + rep+ "," + (C * hr).ToString());
                                        //listP.Add((count - 1).ToString());
                                    }
                                    if (B != 0)
                                    {
                                        writer.WriteLine((count - 1).ToString() + "," + rep1+ "," + (D * hr).ToString());
                                        //listP.Add((count - 1).ToString());
                                    }

                                    A = 0;
                                    B = 0;
                                    C = 0;
                                    D = 0;
                                }



                            }
                            
                        }
                        MessageBox.Show("Find your file in the documents folder");
                    }
                }
                catch (Exception ex)
                {
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














        
  
        
    

