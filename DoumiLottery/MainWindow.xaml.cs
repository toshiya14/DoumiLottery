using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DoumiLottery
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :Window
    {


        DispatcherTimer timer;
        string status;
        Task t_Reading, t_Writing;

        bool IsReadingCompleted {
            get => Core.IsFinishedReading;
        }

        public MainWindow() {
            InitializeComponent();

            // Initialize Timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (!IsReadingCompleted) {
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            timer.Start();
            status = "正在读取样本列表...";
            t_Reading = Core.ReadSamplesFromFile(@"./samples.txt");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            status = "等待结果写入完毕...";
            if (t_Writing != null) {
                while (!t_Writing.IsCompleted) {
                    Task.Delay(100);
                }
            }
            status = "正常退出";
        }
    }
}
