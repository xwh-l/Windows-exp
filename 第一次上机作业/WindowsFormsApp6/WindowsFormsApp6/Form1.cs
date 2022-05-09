using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {

        int curCnt = 0;
        int totalCnt = 0;
        int remainedCnt = 0;
        static Mutex mutex = new Mutex();
        const int BUFFER_SIZE = 20;
        static Semaphore produce = new Semaphore(BUFFER_SIZE, BUFFER_SIZE);
        static Semaphore consume = new Semaphore(0, BUFFER_SIZE);

        public Form1()
        {
            InitializeComponent();
        }



        //生产者线程
        private void produceItem(string name,int rate)
        {
            while (true)
            {
                while (!produce.WaitOne(10))
                {
                    Console.WriteLine(name + " wants to produce an item, but the buffer is full");
                    update(name + " wants to produce an item, but the buffer is full");

                }

                mutex.WaitOne();
                ++curCnt;
                remainedCnt++;

                //new Thread(() => { addlist1(name); }).Start();
                this.listBox1.Invoke(new Action(() => {
                    listBox1.Items.Add(name + " produces an item, totally " + curCnt + ", now there are " + remainedCnt + " items in the buffer");
                }));
                

                Console.WriteLine(name + " produces an item, totally " + curCnt + ", now there are " + remainedCnt + " items in the buffer");                
                mutex.ReleaseMutex();
                consume.Release();
                Thread.Sleep(rate * 1000);
                //Thread.Sleep(1000/rate);
                if (curCnt >= totalCnt)
                {
                    //Thread.CurrentThread.Abort();
                    Console.WriteLine(name + "exit.");
                    break;
                }

            }
        }

        
        //消费者线程
        private void consumeItem(string name, int rate)
        {
            while (true)
            {
                while (!consume.WaitOne(10))
                {
                    Console.WriteLine(name + " wants to consume an item, but the buffer is empty");
                }
                mutex.WaitOne();
                remainedCnt--;
                //new Thread(() => { addlist2(name); }).Start();
                listBox1.Invoke(new Action(() => {
                    listBox1.Items.Add(name + " consumes an item, now there are " + remainedCnt + " items in the buffer");
                }));
                
                Console.WriteLine(name + " consumes an item, now there are " + remainedCnt + " items");
                //update(name + " consumes an item, now there are " + remainedCnt + " items");
                mutex.ReleaseMutex();
                produce.Release();
                //Thread.Sleep(1000/rate);
                Thread.Sleep(rate * 1000);
                if (curCnt >= totalCnt)
                {
                    //Thread.CurrentThread.Abort();
                    Console.WriteLine(name + "exit.");
                    break;
                }
            }
        }
        
        void ProducerConsumer(int producerCnt, int consumerCnt, int productRate, int consumeRate
    , int buffer_size, int _totalProductNum)
        {
            
            clearComments();
            showComment("begin of producer consumer problem");
            totalCnt = _totalProductNum;

            Thread[] producers = new Thread[producerCnt];
            for (int i = 0; i < producerCnt; i++)
            {
                string name = "producer" + i;
                producers[i] = new Thread(() => { produceItem(name, productRate); });
                producers[i].IsBackground = true;
                producers[i].Start();
            }
            Thread.Sleep(2000);

            Thread[] consumers = new Thread[consumerCnt];
            for (int i = 0; i < consumerCnt; i++)
            {
                string name = "consumer" + i;
                consumers[i] = new Thread(() => { consumeItem(name, consumeRate); });
                //consumers[i] = new Thread(() => { consumeItem("consumer" + i, consumeRate); });
                consumers[i].IsBackground = true;
                consumers[i].Start();
            }

            
            Console.WriteLine("End of producer consumer problem");
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            int pro_num = int.Parse(textBox1.Text);//the number of producers
            int cos_num = int.Parse(textBox2.Text);//the number of cosumers

            int warehouse_size = int.Parse(textBox3.Text);//the size of the warehouse

            int pro_rate = int.Parse(textBox4.Text);
            int cos_rate = int.Parse(textBox5.Text);

            button1.Enabled = false;
            ProducerConsumer(pro_num, cos_num, pro_rate, cos_rate, BUFFER_SIZE, warehouse_size);
            button1.Enabled = true;

        }
        //定义回调
        private delegate void updateDelegate(string comment);
        public void update(string comment)
        {
            showComment(comment);
            

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string name = (string)e.Argument;
            listBox1.Items.Add(name + " produces an item, totally " + curCnt + ", now there are " + remainedCnt + " items in the buffer");
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string name = (string)e.Argument;
            listBox1.Items.Add(name + " consumes an item, now there are " + remainedCnt + " items in the buffer");
        }
        private void clearComments()
        {
            listBox1.Items.Clear();
        }

        private void showComment(string comment)
        {
            if (comment.Length==0)
            {                
                return;
            }

            listBox1.Items.Add(comment);
        }
    }
}

