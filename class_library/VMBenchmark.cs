using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace class_library
{
    public class VMBenchmark : INotifyPropertyChanged
    {
        public ObservableCollection<VMTime> Time_Coll { get; set; }
        public ObservableCollection<VMAccuracy> Accur_Coll { get; set; }
        public VMBenchmark()
        {
            Time_Coll = new ObservableCollection<VMTime>();
            Accur_Coll = new ObservableCollection<VMAccuracy>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
        private void CheckProperties()
        {
            //double tmp1 = Time_Coll[Time_Coll.Count - 1].Time_HA / Time_Coll[Time_Coll.Count - 1].Time_NO_MKL;
            //double tmp2 = Time_Coll[Time_Coll.Count - 1].Time_EP / Time_Coll[Time_Coll.Count - 1].Time_NO_MKL;
            //if (tmp1 < min1)
            //    MIN_MKL_HA_TO_NO_MKL = tmp1;
            //if (tmp2 < min2)
            //    MIN_MKL_EP_TO_NO_MKL = tmp2;
            //if (tmp1 > max1)
            //    MAX_MKL_HA_TO_NO_MKL = tmp1;
        }
        public void AddVMTime(VMGrid G, VMf f)
        {
            Time_Coll.Add(new VMTime(G, f));
            //CheckProperties();
        }
        public void AddVMTime(VMGrid G, VMf f, double T1, double T2, double T3) // need for ViewData
        {
            Time_Coll.Add(new VMTime(G, f, T1, T2, T3));
            //CheckProperties();
        }
        public void AddVMAccur(VMGrid G, VMf f)
        {
            Accur_Coll.Add(new VMAccuracy(G, f));
        }
        public void AddVMAccur(VMGrid G, VMf f, double MAX, double[] ARG) // need for ViewData 
        {
            Accur_Coll.Add(new VMAccuracy(G, f, MAX, ARG));
        }
        private double min1 = Double.MaxValue;
        private double min2 = Double.MaxValue;
        private double max1 = 0;
        public double MIN_MKL_HA_TO_NO_MKL
        {
            get
            {
                return min1;
            }
            set
            {
                min1 = value;
                OnPropertyChanged("MIN_MKL_HA_TO_NO_MKL");
            }
        }
        public double MIN_MKL_EP_TO_NO_MKL
        {
            get
            {
                return min2;
            }
            set
            {
                min2 = value;
                OnPropertyChanged("MIN_MKL_EP_TO_NO_MKL");
            }
        }
        public double MAX_MKL_HA_TO_NO_MKL
        {
            get
            {
                return max1;
            }
            set
            {
                max1 = value;
                OnPropertyChanged("MAX_MKL_HA_TO_NO_MKL");
            }
        }
    }
}