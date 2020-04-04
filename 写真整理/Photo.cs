using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 写真整理
{
    public class Photo : INotifyPropertyChanged
    {
        private string _Path;
        public string Path
        {
            get
            {
                return this._Path;
            }
            set
            {
                this._Path = value;
                OnPropertyChanged("Path");
            }
        }

        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        private string _ChangeName;
        public string ChangeName
        {
            get
            {
                return this._ChangeName;
            }
            set
            {
                this._ChangeName = value;
                OnPropertyChanged("ChangeName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
