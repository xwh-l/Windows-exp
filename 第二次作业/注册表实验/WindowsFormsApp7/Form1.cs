using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {


        static readonly IntPtr Mydefine = new IntPtr(unchecked((int)0x80000001));
        static IntPtr currenthKey = Mydefine;
        static int STANDARD_RIGHTS_ALL = (0x001F0000);
        static int KEY_QUERY_VALUE = (0x0001);
        static int KEY_SET_VALUE = (0x0002);
        static int KEY_CREATE_SUB_KEY = (0x0004);
        static int KEY_ENUMERATE_SUB_KEYS = (0x0008);
        static int KEY_NOTIFY = (0x0010);
        static int KEY_CREATE_LINK = (0x0020);
        static int SYNCHRONIZE = (0x00100000);
        static int REG_OPTION_NON_VOLATILE = (0x00000000);
        static int KEY_ALL_ACCESS = (STANDARD_RIGHTS_ALL | KEY_QUERY_VALUE | KEY_SET_VALUE |
                                     KEY_CREATE_SUB_KEY | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY | KEY_CREATE_LINK) & (~SYNCHRONIZE);


        [DllImport("advapi32.dll", EntryPoint = "RegCreateKey")]//创建
        public static extern int RegCreate(IntPtr hkey, string lpSubKey,out IntPtr phkResult);

        [DllImport("advapi32.dll", EntryPoint = "RegDeleteKey")]//删除
        public static extern int RegDelete(IntPtr hkey, string lpSubKey);

        [DllImport("advapi32.dll", EntryPoint = "RegOpenKeyEx")]//打开
        public static extern int RegOpen(IntPtr h, string subkey, uint opt, int desired,out IntPtr res);
        
        [DllImport("advapi32.dll", EntryPoint = "RegCloseKey")]//关闭
        public static extern int RegClose(IntPtr h);

        [DllImport("advapi32.dll", EntryPoint = "RegRestoreKey")]//读取
        public static extern int RegRestore(IntPtr h, string file, int flags);

        //设置Key值
        [DllImport("Advapi32.dll", EntryPoint = "RegSetValueEx")]
        private static extern int RegSetValue(IntPtr hKey, string lpValueName, uint unReserved, uint unType, byte[] lpData, uint dataCount);

        //删除键
        [DllImport("Advapi32.dll", EntryPoint = "RegDeleteKeyValue")]
        private static extern int RegDeleteKeyValue(IntPtr hKey, string lpSubKey, string lpValueName);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr phk = IntPtr.Zero;
            int ret = RegCreate(currenthKey, textBox2.Text,out phk);
            if (ret == 0)
            {
                MessageBox.Show("创建成功！");
                
            }
            else
            {
                MessageBox.Show("创建失败！");
            }
            RegClose(phk);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ret = RegOpen(currenthKey, textBox2.Text, 0, KEY_ALL_ACCESS, out IntPtr phkResult);
            if (ret == 0)
            {
                MessageBox.Show("打开项成功！");

            }
            else
            {
                MessageBox.Show("没有找到子项！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (0 == RegDelete(currenthKey, textBox2.Text))
            {
                MessageBox.Show("删除项成功！");
            }
            else
            {
                MessageBox.Show("删除项失败！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IntPtr pHKey = IntPtr.Zero;//输出创建后的句柄
            int lpdwDisposition = 0;
            int ret = RegCreate(currenthKey, textBox2.Text, out pHKey);
            //设置访问的Key值
            uint REG_SZ = 1;
            //要存储的数据
            byte[] data = Encoding.Unicode.GetBytes(textBox3.Text);
            int success = RegSetValue(pHKey, textBox1.Text, 0, REG_SZ, data, (uint)data.Length);
            MessageBox.Show("创建键成功！");
            RegClose(pHKey);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (0 == RegDeleteKeyValue(currenthKey, textBox2.Text, textBox1.Text))
            {
                MessageBox.Show("删除键成功！");
            }
            else
            {
                MessageBox.Show("删除键失败！");
            }
        }


    }
}
