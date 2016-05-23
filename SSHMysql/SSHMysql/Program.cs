using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Web;
using System.Threading;
//Add MySql Library
using MySql.Data.MySqlClient;
using MySql.Data.Types;

// SSH
using Renci.SshNet;
using Renci.SshNet.Common;

namespace MySQL_Console
{
    class MainClass
    {

        public static void testc()
        {
            Chilkat.SshTunnel sshTunnel = new Chilkat.SshTunnel();

            bool success;
        success = sshTunnel.UnlockComponent("30-day trial");
if (success != true) {
    //textBox1.Text += sshTunnel.LastErrorText + "\r\n";
    Console.WriteLine("fail trial");
    return;
}

//  The destination host/port is the database server.
//  The DestHostname may be the domain name or
//  IP address (in dotted decimal notation) of the database
//  server.
sshTunnel.DestPort = 3306;
sshTunnel.DestHostname = "219.245.64.4";

//  Provide information about the location of the SSH server,
//  and the authentication to be used with it. This is the
//  login information for the SSH server (not the database server).
sshTunnel.SshHostname = "219.245.64.4";
sshTunnel.SshPort = 22;
sshTunnel.SshLogin = "root";
sshTunnel.SshPassword = "123456";

//  Start accepting connections in a background thread.
//  The SSH tunnels are autonomously run in a background
//  thread.  There is one background thread for accepting
//  connections, and another for managing the tunnel pool.
int listenPort;
listenPort = 9999;
            success = sshTunnel.BeginAccepting(listenPort);
if (success != true) {
    Console.WriteLine(sshTunnel.LastErrorText);
    return;
}
        }


        public static void test2()
        {
            ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;

            Process process = Process.Start(psi);
            string cmdForTunnel = @"F:\program\putty\plink -i F:\program\putty\keyprivate.ppk -l root -load mysshtunel";
            process.StandardInput.WriteLine(cmdForTunnel);
            // process.WaitForExit();
           
           // string output = process.StandardOutput.ReadToEnd();

           // Console.WriteLine(output);
            Console.WriteLine("OK,wait");

        }
        public static void test3()
        {
            using (var client = new SshClient("219.245.64.4", "root", "123456"))
            {
                try
                {
                    Console.WriteLine("Trying SSH connection...");
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Console.WriteLine("SSH connection is active: {0}", client.ConnectionInfo.ToString());
                    }
                    else
                    {
                        Console.WriteLine("SSH connection has failed: {0}", client.ConnectionInfo.ToString());
                    }

                    Console.WriteLine("\r\nTrying port forwarding...");
                    var port = new ForwardedPortLocal("127.0.0.1", Convert.ToUInt32(19999), "219.245.64.4", Convert.ToUInt32(3306));

                    //ForwardedPortDynamic port = new ForwardedPortDynamic("127.0.0.1", 3306);
                    client.AddForwardedPort(port);
                    port.Start();
                    if (port.IsStarted)
                    {
                        Console.WriteLine("Port forwarded: {0}", port.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Port forwarding has failed.");
                    }

                }
                catch (SshException e)
                {
                    Console.WriteLine("SSH client connection error: {0}", e.Message);
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine("Socket connection error: {0}", e.Message);
                }

            }
        }
        public static void Main(string[] args)
        {
            //PrivateKeyFile file = new PrivateKeyFile(@"F:\program\putty\keyprivate.ppk");
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("219.245.64.4", "root", "123456");
            //connectionInfo.Timeout = TimeSpan.FromSeconds(30);
           // testc();
           // Console.WriteLine("Sleep for ssh tunnel");
          //  Thread.Sleep(10000);
            //test2();
            //test3();

            ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;

            Process process = Process.Start(psi);
            string cmdForTunnel = @"F:\program\putty\plink -i F:\program\putty\keyprivate.ppk -l root -load mysshtunel";
            process.StandardInput.WriteLine(cmdForTunnel);
            // process.WaitForExit();

            // string output = process.StandardOutput.ReadToEnd();

            // Console.WriteLine(output);
            Console.WriteLine("OK,wait");
            
            Console.WriteLine("\r\nTrying database connection...");
           // Console.ReadLine();
           // DBConnect dbConnect = new DBConnect("localhost", "wg", "wyg", "123456", "9999");
           // Console.WriteLine("Connect db success");
          //  var ct = dbConnect.Count("terminals");
          //  Console.WriteLine(ct.ToString());

            //string constr = "server=219.245.64.4;User Id=wyg;password=123456;Database=wygdb";
            Console.ReadLine();
            string constr = "server=127.0.0.1;user=wyg;password=123456;Database=wygdb;port=19999";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand("insert into stu values('03111298', '小王','23')", mycon);
            if (mycmd.ExecuteNonQuery() > 0)
            {
               Console.WriteLine("数据插入成功！");
            }
          //  Console.ReadLine();
            mycon.Close();

        }
    }

    // MySQL DB class
    class DBConnect
    {
        private MySqlConnection connection;

        private string server;
        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        private string database;
        public string Database
        {
            get
            {
                return this.database;
            }
            set
            {
                this.database = value;
            }
        }

        private string uid;
        public string Uid
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        private string port;
        public string Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }

        //Constructor
        public DBConnect(string server, string database, string uid, string password, string port = "3306")
        {
            this.server = server;

            this.database = database;
            this.uid = uid;
            this.password = password;
            this.port = port;

            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
           
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                Console.WriteLine("MySQL connected.");
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                Console.WriteLine(ex);
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;

                    default:
                        Console.WriteLine("Unhandled exception: {0}.", ex.Message);
                        break;

                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string tableName, List<KeyValuePair<string, string>> setArgs, List<KeyValuePair<string, string>> whereArgs)
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string tableName, List<KeyValuePair<string, string>> whereArgs)
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string> Select(string queryString)
        {
            string query = queryString;

            //Create a list to store the result
            List<string> list = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                int fieldCOunt = dataReader.FieldCount;
                while (dataReader.Read())
                {
                    for (int i = 0; i < fieldCOunt; i++)
                    {
                        list.Add(dataReader.GetValue(i).ToString());
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }

            return list;

        }

        //Count statement
        public int Count(string tableName)
        {
            string query = "SELECT Count(*) FROM " + tableName;
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }

            return Count;

        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("Error {0}, unable to backup!", e.Message);
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("Error {0}, unable to Restore!", e.Message);
            }
        }
    }
}
