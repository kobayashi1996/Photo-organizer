using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 写真整理
{
    public class OkText : INotifyPropertyChanged
    {
        private string _Text;
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
                OnPropertyChanged("Text");
            }
        }

        public OkText()
        {
            Text = "追加";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
