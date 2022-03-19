using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace class_library
{
    public class VMTime
    {
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll_Lib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        bool Get_MKL_TIMES(int n, double[] a, double[] y, double[] z, double[] p, ref double time_HA, ref double time_EP, ref double time_NO_MKL, VMf f);
        //
        public VMGrid CurGrid { get; set; }
        public double Time_HA { get; set; }
        public double Time_EP { get; set; }
        public double Time_NO_MKL { get; set; }
        public VMTime(VMGrid G, VMf f)
        {
            CurGrid = G;
            Fun_Name = f;
            double[] time = Get_Times();
            Time_HA = time[0];
            Time_EP = time[1];
            Time_NO_MKL = time[2];
        }
        public VMTime(VMGrid G, VMf f, double T1, double T2, double T3)
        {
            CurGrid = G;
            Fun_Name = f;
            Time_HA = T1;
            Time_EP = T2;
            Time_NO_MKL = T3;
        }

        public VMf Fun_Name { get; set; }
        private double[] Get_Times()
        {
            double[] output_HA = new double[CurGrid.Length];
            double[] output_EP = new double[CurGrid.Length];
            double[] output_NO_MKL = new double[CurGrid.Length];
            double[] input = new double[CurGrid.Length];
            double time_HA = 0, time_EP = 0, time_NO_MKL = 0;
            for (int i = 0; i < CurGrid.Length; i++)
            {
                input[i] = CurGrid.Begin + CurGrid.Step * i;
            }
            try
            {
                Get_MKL_TIMES(CurGrid.Length, input, output_HA, output_EP, output_NO_MKL, ref time_HA, ref time_EP, ref time_NO_MKL, Fun_Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Console.WriteLine("input vector");
            //foreach (double item in input)
            //{
            //    Console.WriteLine(item + " ");
            //}
            //Console.WriteLine("output vector HA");
            //foreach (double item in output_HA)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("output vector EP");
            //foreach (double item in output_EP)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("output vector NO MKL");
            //foreach (double item in output_NO_MKL)
            //{
            //    Console.WriteLine(item);
            //}

            return new double[3] { time_HA, time_EP, time_NO_MKL };
        }
        public override string ToString()
        {
            string S = $"Grid is:\t From {CurGrid.Begin} To {CurGrid.End} With Step = {CurGrid.Step.ToString("F6")} ({CurGrid.Length} numbers overall)\n";
            return S + $"Function: {Fun_Name}\nThe times:\t {Time_HA} seconds (HA) ; {Time_EP} seconds (EP) ; {Time_NO_MKL} seconds (NO_MKL) \n";
        }
    }
}
