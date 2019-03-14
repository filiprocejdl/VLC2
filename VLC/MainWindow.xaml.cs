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
using Vlc.DotNet.Core;
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
            //path = @"C:\Users\ROCCA\Videos\test.mp4";

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            vlcPlayer.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
            vlcPlayer.MediaPlayer.EndInit();          
            VolumeChanger.Value = vlcPlayer.MediaPlayer.Audio.Volume;

            Console.WriteLine(vlcPlayer.MediaPlayer.Audio.Volume);

           // vlcPlayer.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;

        }

       /* private void MediaPlayer_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TimeChanger.Value = e.NewPosition;
            });
        } */
        
        //Play video from file
                private void Button_OpenVideo(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog NewPath = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = NewPath.ShowDialog();

            if (result == true) 
            {
                path = NewPath.FileName;
                isPlayed = true;
                PlayVideo();
                
            }
        }

        public void PlayVideo()
        {                      
            Title.Content = System.IO.Path.GetFileName(path);
            vlcPlayer.MediaPlayer.Play(new Uri(path));
            vlcPlayer.MediaPlayer.Audio.Volume = 25;
            VolumeChanger.Value = vlcPlayer.MediaPlayer.Audio.Volume;
            TimeChanger.Maximum = vlcPlayer.MediaPlayer.Length / 1000;
            Thread.Sleep(100);
            TimeEnd.Content = TimeChanger.Maximum = vlcPlayer.MediaPlayer.Length / 1000;
        }

        //Play X Pause
        private void Button_Play(object sender, RoutedEventArgs e)
        {
            if (isPlayed)
            {
                vlcPlayer.MediaPlayer.Pause();
                isPlayed = false;
                PlayButton.Content = "Play";
            } else
            {
                vlcPlayer.MediaPlayer.Play();
                isPlayed = true;
                PlayButton.Content = "Pause";
            }

        }

        private void MediaPlayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            vlcPlayer.MediaPlayer.Time = e.NewTime;
        }

        //Stop
        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            vlcPlayer.MediaPlayer.Stop();
        }

       

        //Volume
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
            TimeCurrent.Content = vlcPlayer.MediaPlayer.Time / 1000;

        }

       

        private void VlcPlayer_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            TimeCurrent.Content = vlcPlayer.MediaPlayer.Time / 1000;
        }

        private void Button_PretocZpet(object sender, RoutedEventArgs e)
        {
            

            if (vlcPlayer.MediaPlayer.Time - 5000 <= 0 )
            {
                vlcPlayer.MediaPlayer.Time = 0;
                TimeCurrent.Content = 0;
            } else
            {
                vlcPlayer.MediaPlayer.Time = vlcPlayer.MediaPlayer.Time - 5000;
                TimeCurrent.Content = vlcPlayer.MediaPlayer.Time / 1000;
                TimeChanger.Value = vlcPlayer.MediaPlayer.Time / 1000;
            }
        }

        private void Button_PretocDopredu(object sender, RoutedEventArgs e)
        {
            if (vlcPlayer.MediaPlayer.Time + 30000 >= vlcPlayer.MediaPlayer.Length)
            {
                TimeCurrent.Content = vlcPlayer.MediaPlayer.Length;
            } else
            {
                vlcPlayer.MediaPlayer.Time = vlcPlayer.MediaPlayer.Time + 30000;
                TimeCurrent.Content = vlcPlayer.MediaPlayer.Time / 1000;
                TimeChanger.Value = vlcPlayer.MediaPlayer.Time / 1000;
            }
            
            
        }

       
    }
}
