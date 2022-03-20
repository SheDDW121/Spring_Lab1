using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using class_library;

namespace WPF_APP
{
    public class ViewData : INotifyPropertyChanged
    {
        public VMBenchmark BM { get; set; }
        public VMGrid Grid_V { get; set; } = new VMGrid(0, 0, 0);

        //public int L { get; set; } // needed for previous version of binding
        //public double B { get; set; }
        //public double E { get; set; }

        public VMf MFun { get; set; } //for binding
        public ViewData(VMBenchmark bm)
        {
            BM = bm;
            BM.Time_Coll.CollectionChanged += Time_Coll_CollectionChanged;
            BM.Accur_Coll.CollectionChanged += Accur_Coll_CollectionChanged;
        }

        public ViewData()
        {
            BM = new VMBenchmark();
            BM.Time_Coll.CollectionChanged += Time_Coll_CollectionChanged;
            BM.Accur_Coll.CollectionChanged += Accur_Coll_CollectionChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Time_Coll_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (WasChanged == false)
            {
                WasChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs("WasChanged"));
            }
            try
            {
                double tmp1 = BM.Time_Coll[BM.Time_Coll.Count - 1].Time_HA / BM.Time_Coll[BM.Time_Coll.Count - 1].Time_NO_MKL;
                double tmp2 = BM.Time_Coll[BM.Time_Coll.Count - 1].Time_EP / BM.Time_Coll[BM.Time_Coll.Count - 1].Time_NO_MKL;
                if (tmp1 < BM.MIN_MKL_HA_TO_NO_MKL)
                    BM.MIN_MKL_HA_TO_NO_MKL = tmp1;
                if (tmp2 < BM.MIN_MKL_EP_TO_NO_MKL)
                    BM.MIN_MKL_EP_TO_NO_MKL = tmp2;
                if (tmp1 > BM.MAX_MKL_HA_TO_NO_MKL)
                    BM.MAX_MKL_HA_TO_NO_MKL = tmp1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Accur_Coll_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (WasChanged == false)
            {
                WasChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs("WasChanged"));
            }
        }

        public void AddVMTime(VMGrid G, VMf f)
        {
            BM.AddVMTime(G, f);
        }
        public void AddVMTime(VMGrid G, VMf f, double T1, double T2, double T3)
        {
            BM.AddVMTime(G, f, T1, T2, T3);
        }
        public void AddVMAccur(VMGrid G, VMf f)
        {
            BM.AddVMAccur(G, f);
        }
        public void AddVMAccur(VMGrid G, VMf f, double MAX, double[] ARG)
        {
            BM.AddVMAccur(G, f, MAX, ARG);
        }

        public bool WasChanged { get; set; } = false;
        public bool Save(string filename)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filename);
                {
                    sw.WriteLine(BM.Time_Coll.Count); //number of time in collection, will be needed for load
                    foreach (VMTime item in BM.Time_Coll) // time collection
                    {
                        sw.WriteLine(item.CurGrid.Length + " " + item.CurGrid.Begin + " " + item.CurGrid.End); //Grid
                        sw.WriteLine(item.Fun_Name); //Fun
                        sw.WriteLine(item.Time_HA + " " + item.Time_EP + " " + item.Time_NO_MKL); //times
                    }

                    sw.WriteLine(BM.Accur_Coll.Count);
                    foreach (VMAccuracy item in BM.Accur_Coll) // time collection
                    {
                        sw.WriteLine(item.CurGrid.Length + " " + item.CurGrid.Begin + " " + item.CurGrid.End); //Grid
                        sw.WriteLine(item.Fun_Name); //Fun
                        sw.WriteLine(item.MAX_DIFF_HA_AND_EP + " " + item.ARG_FOR_MAX_DIFF[0] + " " + item.ARG_FOR_MAX_DIFF[1] + " " +
                            item.ARG_FOR_MAX_DIFF[2]); //times
                    }
                    sw.WriteLine(BM.MIN_MKL_HA_TO_NO_MKL + " " + BM.MIN_MKL_EP_TO_NO_MKL + " " + BM.MAX_MKL_HA_TO_NO_MKL);
                }
                WasChanged = false;
                PropertyChanged(this, new PropertyChangedEventArgs("WasChanged"));
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error in Save\n{ex.Message}");
                //return false;
                throw ex;
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }
            return true;
        }
        public bool Load(string filename)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filename);
                {
                    BM = new VMBenchmark();
                    char[] separator = { ' ' };        //for Parse

                    int N = int.Parse(sr.ReadLine()); //number of VMTime elements

                    for (int i = 0; i < N; i++)
                    {
                        string[] st = sr.ReadLine().Split(separator); //for Grid
                        VMGrid TmpG = new VMGrid(int.Parse(st[0]), Double.Parse(st[1]), Double.Parse(st[2]));

                        VMf TmpF = VMf.vmdTan;  //for Fun
                        st[0] = sr.ReadLine();
                        if (st[0] != "vmdTan")
                            TmpF = VMf.vmdErfInv;

                        st = sr.ReadLine().Split(separator); //for times
                        double[] tmpT = new double[3];
                        tmpT[0] = Double.Parse(st[0]); tmpT[1] = Double.Parse(st[1]); tmpT[2] = Double.Parse(st[2]);

                        AddVMTime(TmpG, TmpF, tmpT[0], tmpT[1], tmpT[2]);
                    }

                    N = int.Parse(sr.ReadLine()); //number of VMAccuracy elements

                    for (int i = 0; i < N; i++)
                    {
                        string[] st = sr.ReadLine().Split(separator); //for Grid
                        VMGrid TmpG = new VMGrid(int.Parse(st[0]), Double.Parse(st[1]), Double.Parse(st[2]));

                        VMf TmpF = VMf.vmdTan;  //for Fun
                        st[0] = sr.ReadLine();
                        if (st[0] != "vmdTan")
                            TmpF = VMf.vmdErfInv;

                        st = sr.ReadLine().Split(separator); //for times

                        double MAX = 0;
                        double[] tmpT = new double[3];
                        MAX = Double.Parse(st[0]);
                        tmpT[0] = Double.Parse(st[1]); tmpT[1] = Double.Parse(st[2]); tmpT[2] = Double.Parse(st[3]);

                        AddVMAccur(TmpG, TmpF, MAX, tmpT);
                    }
                    string [] stt = sr.ReadLine().Split(separator); // for properties ViewData
                    BM.MIN_MKL_HA_TO_NO_MKL = Double.Parse(stt[0]);
                    BM.MIN_MKL_EP_TO_NO_MKL = Double.Parse(stt[1]);
                    BM.MAX_MKL_HA_TO_NO_MKL = Double.Parse(stt[2]);

                    BM.Time_Coll.CollectionChanged += Time_Coll_CollectionChanged;
                    BM.Accur_Coll.CollectionChanged += Accur_Coll_CollectionChanged;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine($"Error in Load\n{ex.Message}");
                //return false;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
            return true;
        }
    }
}
