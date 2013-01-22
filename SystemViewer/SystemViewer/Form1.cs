using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;

namespace SystemViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }
        Dictionary<int, string> BChar = new Dictionary<int, string>();
        Dictionary<int, string> PFamily = new Dictionary<int, string>();
        Dictionary<int, string> PArch = new Dictionary<int, string>();
        static ManagementBaseObject mbo;
        public static string newprodet;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            backgroundWorker1.RunWorkerAsync();
        }
        
        #region services
        private void loadservices()
        {
            try
            {
                ManagementObjectSearcher s = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
                listView3.BeginUpdate();
                foreach (ManagementObject service in s.Get())
                {
                    listView3.Items.Add(service["ProcessId"].ToString());
                    listView3.Items[listView3.Items.Count - 1].SubItems.Add(service["Name"].ToString());
                    listView3.Items[listView3.Items.Count - 1].SubItems.Add(service["Status"].ToString());
                    listView3.Items[listView3.Items.Count - 1].SubItems.Add(service["State"].ToString());
                    listView3.Items[listView3.Items.Count - 1].SubItems.Add(service["Caption"].ToString());
                }
                listView3.EndUpdate();
            }
            catch
            {
            }
        }
        #endregion

        #region apps
        private void loadapps()
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                listView2.BeginUpdate();
                foreach (Process pro in processes)
                {
                    if (pro.MainWindowTitle.Length > 0)
                    {
                        listView2.Items.Add(pro.Id.ToString());
                        listView2.Items[listView2.Items.Count - 1].SubItems.Add(pro.MainWindowTitle.ToString());
                        if (pro.Responding == true)
                        {
                            listView2.Items[listView2.Items.Count - 1].SubItems.Add("Running");
                        }
                        else
                        {
                            listView2.Items[listView2.Items.Count - 1].SubItems.Add("Not Responding");
                        }
                    }
                }
                listView2.EndUpdate();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView2.Items.Clear();
                loadapps();
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count >= 1)
            {
                try
                {
                    int pid = Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text.ToString());
                    Process.GetProcessById(pid).Kill();
                    loadapps();
                    loadprocesses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #endregion

        #region processes
        private void loadprocesses()
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                listView1.BeginUpdate();
                foreach (Process pro in processes)
                {
                    listView1.Items.Add(pro.ProcessName.ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(pro.Id.ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(pro.PrivateMemorySize64.ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(pro.WorkingSet64.ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(pro.Threads.Count.ToString());
                }
                listView1.EndUpdate();
            }
            catch
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count >= 1)
            {
                try
                {
                    int pid = Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text.ToString());
                    Process.GetProcessById(pid).Kill();
                    loadapps();
                    loadprocesses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                if (newprodet.Length != 0)
                {
                    if (newprodet.IndexOf("\\") == -1)
                    {
                        string[] newprocdetails = newprodet.Split(' ');
                        if (newprocdetails.Length > 1)
                        {
                            Process newprocess = Process.Start(newprocdetails[0].ToString(), newprocdetails[1].ToString());
                        }
                        else
                        {
                            Process newprocess = Process.Start(newprocdetails[0].ToString());
                        }
                    }
                    else
                    {
                        string procname = newprodet.Substring(newprodet.LastIndexOf("\\") + 1);
                        string[] newprocdetails = procname.Split(' ');
                        if (newprocdetails.Length > 1)
                        {
                            Process newprocess = Process.Start(newprodet.Replace(newprocdetails[1].ToString(), ""), newprocdetails[1].ToString());
                        }
                        else
                        {
                            Process newprocess = Process.Start(newprodet);
                        }

                    }
                }
            }
            catch
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                loadprocesses();
            }
            catch
            {
            }
        }

        #endregion

        #region sysyinfo
        private void sysinfo()
        {
            #region PFamily
            {
                PFamily.Add(1, "Other");
                PFamily.Add(2, "Unknown");
                PFamily.Add(3, "8086");
                PFamily.Add(4, "80286");
                PFamily.Add(5, "Intel386 Processor");
                PFamily.Add(6, "Intel486 Processor");
                PFamily.Add(7, "8087");
                PFamily.Add(8, "80287");
                PFamily.Add(9, "80387");
                PFamily.Add(10, "80487");
                PFamily.Add(11, "Pentium Brand");
                PFamily.Add(12, "Pentium Pro");
                PFamily.Add(13, "Pentium II");
                PFamily.Add(14, "Pentium Processor with MMX Technology");
                PFamily.Add(15, "Celeron");
                PFamily.Add(16, "Pentium II Xeon");
                PFamily.Add(17, "Pentium III");
                PFamily.Add(18, "M1 Family");
                PFamily.Add(19, "M2 Family");
                PFamily.Add(24, "AMD Duron Processor Family");
                PFamily.Add(25, "K5 Family");
                PFamily.Add(26, "K6 Family");
                PFamily.Add(27, "K6-2");
                PFamily.Add(28, "K6-3");
                PFamily.Add(29, "AMD Athlon Processor Family");
                PFamily.Add(30, "AMD2900 Family");
                PFamily.Add(31, "K6-2+");
                PFamily.Add(32, "Power PC Family");
                PFamily.Add(33, "Power PC 601");
                PFamily.Add(34, "Power PC 603");
                PFamily.Add(35, "Power PC 603+");
                PFamily.Add(36, "Power PC 604");
                PFamily.Add(37, "Power PC 620");
                PFamily.Add(38, "Power PC X704");
                PFamily.Add(39, "Power PC 750");
                PFamily.Add(48, "Alpha Family");
                PFamily.Add(49, "Alpha 21064");
                PFamily.Add(50, "Alpha 21066");
                PFamily.Add(51, "Alpha 21164");
                PFamily.Add(52, "Alpha 21164PC");
                PFamily.Add(53, "Alpha 21164a");
                PFamily.Add(54, "Alpha 21264");
                PFamily.Add(55, "Alpha 21364");
                PFamily.Add(64, "MIPS Family");
                PFamily.Add(65, "MIPS R4000");
                PFamily.Add(66, "MIPS R4200");
                PFamily.Add(67, "MIPS R4400");
                PFamily.Add(68, "MIPS R4600");
                PFamily.Add(69, "MIPS R10000");
                PFamily.Add(80, "SPARC Family");
                PFamily.Add(81, "SuperSPARC");
                PFamily.Add(82, "microSPARC II");
                PFamily.Add(83, "microSPARC IIep");
                PFamily.Add(84, "UltraSPARC");
                PFamily.Add(85, "UltraSPARC II");
                PFamily.Add(86, "UltraSPARC IIi");
                PFamily.Add(87, "UltraSPARC III");
                PFamily.Add(88, "UltraSPARC IIIi");
                PFamily.Add(96, "68040");
                PFamily.Add(97, "68xxx Family");
                PFamily.Add(98, "68000");
                PFamily.Add(99, "68010");
                PFamily.Add(100, "68020");
                PFamily.Add(101, "68030");
                PFamily.Add(112, "Hobbit Family");
                PFamily.Add(120, "Crusoe TM5000 Family");
                PFamily.Add(121, "Crusoe TM3000 Family");
                PFamily.Add(122, "Efficeon TM8000 Family");
                PFamily.Add(128, "Weitek");
                PFamily.Add(130, "Itanium Processor");
                PFamily.Add(131, "AMD Athlon 64 Processor Famiily");
                PFamily.Add(132, "AMD Opteron Processor Family");
                PFamily.Add(144, "PA-RISC Family");
                PFamily.Add(145, "PA-RISC 8500");
                PFamily.Add(146, "PA-RISC 8000");
                PFamily.Add(147, "PA-RISC 7300LC");
                PFamily.Add(148, "PA-RISC 7200");
                PFamily.Add(149, "PA-RISC 7100LC");
                PFamily.Add(150, "PA-RISC 7100");
                PFamily.Add(160, "V30 Family");
                PFamily.Add(176, "Pentium III Xeon Processor");
                PFamily.Add(177, "Pentium III Processor with Intel SpeedStep&trade; Technology");
                PFamily.Add(178, "Pentium 4");
                PFamily.Add(179, "Intel Xeon");
                PFamily.Add(180, "AS400 Family");
                PFamily.Add(181, "Intel Xeon Processor MP");
                PFamily.Add(182, "AMD Athlon XP Family");
                PFamily.Add(183, "AMD Athlon MP Family");
                PFamily.Add(184, "Intel Itanium 2");
                PFamily.Add(185, "Intel Pentium M Processor");
                PFamily.Add(190, "K7");
                PFamily.Add(200, "IBM390 Family");
                PFamily.Add(201, "G4");
                PFamily.Add(202, "G5");
                PFamily.Add(203, "G6");
                PFamily.Add(204, "z/Architecture Base");
                PFamily.Add(250, "i860");
                PFamily.Add(251, "i960");
                PFamily.Add(260, "SH-3");
                PFamily.Add(261, "SH-4");
                PFamily.Add(280, "ARM");
                PFamily.Add(281, "StrongARM");
                PFamily.Add(300, "6x86");
                PFamily.Add(301, "MediaGX");
                PFamily.Add(302, "MII");
                PFamily.Add(320, "WinChip");
                PFamily.Add(350, "DSP");
                PFamily.Add(500, "Video Processor");
            }
            #endregion

            #region PArch
            {
                PArch.Add(0, "x86");
                PArch.Add(1, "MIPS");
                PArch.Add(2, "Alpha");
                PArch.Add(3, "PowerPC");
                PArch.Add(6, "Intel Itanium Processor Family (IPF)");
                PArch.Add(9, "x64");
            }
            #endregion
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
                ManagementObjectCollection moc = mos.Get();
                foreach (ManagementObject m in moc)
                {
                    mbo = m;
                    label17.Text = getdata("Manufacturer");
                    label18.Text = getdata("Name");
                    label19.Text = getdata("Model");
                    label21.Text = getdata("Workgroup");
                    label20.Text = getdata("UserName");
                    label22.Text = getdata("Domain");
                    label27.Text = getdata("NumberOfProcessors");
                }
                ManagementObjectSearcher mos1 = new ManagementObjectSearcher("select * from Win32_Processor");
                ManagementObjectCollection moc1 = mos1.Get();
                foreach (ManagementObject m in moc1)
                {
                    mbo = m;
                    label26.Text = getdata("Manufacturer");
                    label25.Text = getdata("Name");
                    label24.Text = getdata("NumberOfCores");
                    label58.Text = getdata("Status");
                    label62.Text = getdata("MaxClockSpeed");
                    label64.Text = getdata("CurrentClockSpeed");
                    label66.Text = getdata("CurrentVoltage");
                    label35.Text = getdata("L2CacheSize");
                    label33.Text = getdata("L2CacheSpeed");
                    label31.Text = getdata("L3CacheSize");
                    label37.Text = getdata("L3CacheSpeed");

                    UInt16 family = (UInt16)m["Family"];
                    try
                    {
                        label28.Text = PFamily[family];
                    }
                    catch
                    {
                        label28.Text = "Unknown";
                    }

                    UInt16 arch = (UInt16)m["Architecture"];
                    try
                    {
                        label39.Text = PArch[arch];
                    }
                    catch
                    {
                        label39.Text = "Unknown";
                    }

                    
                }
                ManagementObjectSearcher mos2 = new ManagementObjectSearcher("select * from Win32_LogicalDisk");
                ManagementObjectCollection moc2 = mos2.Get();
                foreach (ManagementObject m in moc2)
                {
                    mbo = m;
                    label23.Text = getdata("FileSystem");
                }
            }
            catch
            {
            }
        }
        
        public string getdata(string prop)
        {
            try
            {
                return mbo.Properties[prop].Value.ToString();
            }
            catch
            {
                return "Data not Available";
            }
        }
        #endregion

        #region bios
        private void biosinfo()
        {
            #region BChar
            {
                BChar.Add(0,"Reserved");
                BChar.Add(1,"Reserved");
                BChar.Add(2,"Unknown");
                BChar.Add(3,"BIOS Characteristics Not Supported");
                BChar.Add(4,"ISA is supported");
                BChar.Add(5,"MCA is supported");
                BChar.Add(6,"EISA is supported");
                BChar.Add(7,"PCI is supported");
                BChar.Add(8,"PC Card (PCMCIA) is supported");
                BChar.Add(9,"Plug and Play is supported");
                BChar.Add(10,"APM is supported");
                BChar.Add(11,"BIOS is Upgradable (Flash)");
                BChar.Add(12,"BIOS shadowing is allowed");
                BChar.Add(13,"VL-VESA is supported");
                BChar.Add(14,"ESCD support is available");
                BChar.Add(15,"Boot from CD is supported");
                BChar.Add(16,"Selectable Boot is supported");
                BChar.Add(17,"BIOS ROM is socketed");
                BChar.Add(18,"Boot From PC Card (PCMCIA) is supported");
                BChar.Add(19,"EDD (Enhanced Disk Drive) Specification is supported");
                BChar.Add(20,"Int 13h - Japanese Floppy for NEC 9800 1.2mb (3.5, 1k Bytes/Sector, 360 RPM) is supported");
                BChar.Add(21,"Int 13h - Japanese Floppy for Toshiba 1.2mb (3.5, 360 RPM) is supported");
                BChar.Add(22,"Int 13h - 5.25 / 360 KB Floppy Services are supported");
                BChar.Add(23,"Int 13h - 5.25 /1.2MB Floppy Services are supported");
                BChar.Add(24,"Int 13h - 3.5 / 720 KB Floppy Services are supported");
                BChar.Add(25,"Int 13h - 3.5 / 2.88 MB Floppy Services are supported");
                BChar.Add(26,"Int 5h, Print Screen Service is supported");
                BChar.Add(27,"Int 9h, 8042 Keyboard services are supported");
                BChar.Add(28,"Int 14h, Serial Services are supported");
                BChar.Add(29,"Int 17h, printer services are supported");
                BChar.Add(30,"Int 10h, CGA/Mono Video Services are supported");
                BChar.Add(31,"NEC PC-98");
                BChar.Add(32,"ACPI is supported");
                BChar.Add(33,"USB Legacy is supported");
                BChar.Add(34,"AGP is supported");
                BChar.Add(35,"I2O boot is supported");
                BChar.Add(36,"LS-120 boot is supported");
                BChar.Add(37,"ATAPI ZIP Drive boot is supported");
                BChar.Add(38,"1394 boot is supported");
                BChar.Add(39,"Smart Battery is supported");
            }

            #endregion
            try
            {
                ManagementObjectSearcher mos1 = new ManagementObjectSearcher("select * from Win32_BIOS");
                foreach (ManagementObject m in mos1.Get())
                {
                    labelbman.Text = getdata("Manufacturer");
                    labelbdesc.Text = getdata("Description");
                    labelbver.Text = getdata("Version");
                    UInt16[] biosc = (UInt16[])m["BiosCharacteristics"];
                    List<string> BiosChar = new List<string>();
                    foreach (UInt16 uin in biosc)
                    {
                        try
                        {
                            BiosChar.Add(BChar[uin]);
                        }
                        catch
                        {

                        }
                    }
                    foreach (string s in BiosChar)
                    {
                        listBox1.Items.Add(s);
                    }
                }
            }
            catch
            {
            }
        }
        
        #endregion

        #region cpuperf
        private int getPhysicalMemory()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                int physicalmemory = 0;
                foreach (ManagementObject m in mos.Get())
                {
                    physicalmemory = int.Parse(m["TotalPhysicalMemory"].ToString()) / 1024;
                }
                return physicalmemory;
            }
            catch 
            {
                return 0;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int totalvalue = Convert.ToInt32(cpu1.NextValue());
                perfChart1.AddValue(totalvalue);
                label68.Text = totalvalue + "%";

                int core1val = Convert.ToInt32(Core1.NextValue());
                perfChart2.AddValue(core1val);
                label69.Text = core1val + "%";

                int core2val = Convert.ToInt32(Core2.NextValue());
                perfChart3.AddValue(core2val);
                label70.Text = core2val + "%";

                int memval = Convert.ToInt32(mem1.NextValue());
                int pmem = getPhysicalMemory();
                int memuse = ((pmem - memval) * 100) / pmem;
                perfChart4.AddValue(memuse);
                label29.Text = memuse + "%";

                toolStripStatusLabel1.Text = "Cpu Usage: " + totalvalue + "%";
                toolStripStatusLabel2.Text = "Memory Usage: " + memuse + "%";
                toolStripStatusLabel3.Text = "Processes: " + listView1.Items.Count;
            }
            catch
            {
            }
        }

        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            sysinfo();
            getPhysicalMemory();
            biosinfo();
            loadapps();
            loadprocesses();
            loadservices();
            this.Show();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
    }
}
