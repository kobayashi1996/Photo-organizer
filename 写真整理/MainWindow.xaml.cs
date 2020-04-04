using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace 写真整理
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Photo> PhotoList;

        public MainWindow()
        {
            InitializeComponent();

            PhotoList = new ObservableCollection<Photo>();
            PhotoListView.ItemsSource = PhotoList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Control ctl && ctl.DataContext is Photo p)
            {
                Photo tmp = new Photo();
                tmp.Path = p.Path;
                tmp.ChangeName = p.ChangeName;
                Window1 w = new Window1();
                w.OkText.Text = "変更";
                w.DataContext = tmp;

                w.ShowDialog();
                if (!w.IsCancel)
                {
                    p.ChangeName = tmp.ChangeName;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Control ctl && ctl.DataContext is Photo p)
            {
                PhotoList.Remove(p);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Photo p = new Photo();
            p.Path = @"./Resources/close.png";

            Window1 w = new Window1();
            w.DataContext = p;

            w.ShowDialog();
            if (!w.IsCancel)
            {
                PhotoList.Add(p);
            }
        }

        private void Border_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Photo)))
            {
                e.Effects = System.Windows.DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Border_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Photo)))
            {
                Photo source = (Photo)e.Data.GetData(typeof(Photo));
                if (sender is Border bdr && bdr.DataContext is Photo target)
                {
                    if (source != target)
                    {
                        PhotoList.Remove(source);
                        PhotoList.Insert(PhotoList.IndexOf(target), source);
                    }
                }
            }
            else if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                var dropFiles = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
                if (dropFiles == null) return;

                if (sender is Border bdr && bdr.DataContext is Photo p)
                {
                    if (string.Compare(System.IO.Path.GetExtension(dropFiles[0]), ".jpg", true) == 0)
                    {
                        p.Path = dropFiles[0];
                    }
                    if (string.Compare(System.IO.Path.GetExtension(dropFiles[0]), ".jpeg", true) == 0)
                    {
                        p.Path = dropFiles[0];
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();

            dlg.Filter = "リスト保存ファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(dlg.FileName))
                {
                    foreach(Photo p in PhotoList)
                    {
                        sw.WriteLine(p.ChangeName);
                    }
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();

            dlg.Filter = "リスト保存ファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        Photo p = new Photo();
                        p.Path = @"./Resources/close.png";
                        p.ChangeName = line;

                        PhotoList.Add(p);
                    }
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            PhotoList.Clear();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();

            dlg.Filter = "対比表保存ファイル(*.csv)|*.csv";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        foreach (Photo p in PhotoList)
                        {
                            if (p.Path != @"./Resources/close.png")
                            {
                                string cname = System.IO.Path.GetFileNameWithoutExtension(p.ChangeName) + System.IO.Path.GetExtension(p.Path);
                                string cpath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(p.Path), cname);
                                if (System.IO.File.Exists(cpath))
                                {
                                    System.IO.File.Delete(cpath);
                                }
                                System.IO.File.Copy(p.Path, cpath, true);
                                sw.WriteLine(string.Format("{0},{1}", p.Path, cname));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void PhotoListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border bdr && bdr.DataContext is Photo p)
            {
                DragDrop.DoDragDrop(bdr, p, DragDropEffects.Move);
            }
        }

        private void PhotoListView_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Photo)))
            {
                Photo source = (Photo)e.Data.GetData(typeof(Photo));
                PhotoList.Remove(source);
                PhotoList.Add(source);
            }
        }

        private void PhotoListView_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Photo)))
            {
                e.Effects = System.Windows.DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}
