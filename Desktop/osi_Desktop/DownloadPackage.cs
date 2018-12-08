using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine;

namespace osi_Desktop
{
    class DownloadPackage:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public int ProgressPercentage
        {
            get { return progressPercentage; }
            set { progressPercentage = value; }
        }
        private int progressPercentage;
        public Beatmapset Beatmapser
        {
            get { return beatmapset; }
        }
        private Beatmapset beatmapset;
        public DownloadPackage(Beatmapset beatmapset)
        {
            this.beatmapset = beatmapset;
        }
    }
}
