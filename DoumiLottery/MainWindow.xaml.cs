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
        
        Task t_Reading, t_Writing;
        public static bool switcher;

        void SetResult(IEnumerable<KeyValuePair<string, string>> resultSet) {
            var sb = new StringBuilder();
            foreach (var i in resultSet) {
                sb.Append(i.Value + " ");
            }
            ResultText.Text = sb.ToString();
        }

        bool IsReadingCompleted {
            get => Core.IsFinishedReading;
        }

        public MainWindow() {
            InitializeComponent();

            // Initialize Timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += Timer_Tick;

            // Initialize Switcher
            if (switcher) {
                Switcher.Content = "停 Stop ！";
            } else {
                Switcher.Content = "开 Start ！";
            }
        }

        private async void Timer_Tick(object sender, EventArgs e) {
            if (!t_Reading.IsCompleted) {
                return;
            }
            if (switcher) {
                DebugText.Text = "抽取中...";
                Core.Draw(2);
                SetResult(Core.results);
            } else {
                DebugText.Text = "结果已产生...写入文件...";
                await Core.WriteResult(@"./out.txt");
                DebugText.Text = "结果已产生...写入完成...";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switcher = true;
            timer.Start();
            DebugText.Text = "正在读取样本列表...";
            t_Reading = Core.ReadSamplesFromFile(@"./samples.txt");
        }

        private void Switcher_Click(object sender, RoutedEventArgs e) {
            switcher = !switcher;
            if (switcher) {
                Switcher.Content = "停 Stop ！";
            } else {
                Switcher.Content = "开 Start ！";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            DebugText.Text = "正常退出";
        }
    }
}
