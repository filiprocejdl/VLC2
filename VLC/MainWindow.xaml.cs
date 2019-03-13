using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Vlc.DotNet.Forms;

namespace VLC
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPlayed = true;
        int volume = 50;
        int time;
        string path;


        public MainWindow()
        {
            
            InitializeComponent();
            this.DataContext = time;
            this.DataContext = volume;

            MediaPlayer media = new MediaPlayer();
            path = @"D:\source\test.mp4";

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            vlcPlayer.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
            vlcPlayer.MediaPlayer.EndInit();

            

            vlcPlayer.MediaPlayer.Play(new Uri(path));

            Console.WriteLine(vlcPlayer.MediaPlayer.Audio.Volume);
            
            Thread.Sleep(100);
            vlcPlayer.MediaPlayer.Audio.Volume = 50;
            Console.WriteLine(vlcPlayer.MediaPlayer.Length);
            TimeChanger.Maximum = vlcPlayer.MediaPlayer.Length / 1000;
            VolumeChanger.Value = vlcPlayer.MediaPlayer.Audio.Volume;


            Title.Content = System.IO.Path.GetFileName(path);
            
            //xd
        }
        

        private void Button_Play(object sender, RoutedEventArgs e)
        {
            if (isPlayed)
            {
                vlcPlayer.MediaPlayer.Pause();
                isPlayed = false;
            } else
            {
                vlcPlayer.MediaPlayer.Play();
                isPlayed = true;
            }
            
        }

        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            vlcPlayer.MediaPlayer.Stop();
        }

       

        private void Slider_Volume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume = (int)VolumeChanger.Value;
            vlcPlayer.MediaPlayer.Audio.Volume = volume;
            

            Console.WriteLine(vlcPlayer.MediaPlayer.Audio.Volume);
        }

        private void Slider_Time(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            time = (int)TimeChanger.Value;
            vlcPlayer.MediaPlayer.Time = time * 1000;
            Time.Content = vlcPlayer.MediaPlayer.Time / 1000;

        }

        private void VlcPlayer_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            Time.Content = vlcPlayer.MediaPlayer.Time / 1000;
        }
    }
}
