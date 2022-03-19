using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace class_library
{
    public class VMAccuracy
    {
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll_Lib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        bool Get_MKL_ACCUR(int n, double[] a, double[] y, double[] z, ref double MAX_DIFF, double[] ARG_FOR_MAX_DIFF, VMf f);
        //
        public VMGrid CurGrid { get; set; }
        public double MAX_DIFF_HA_AND_EP { get; set; }
        public double[] ARG_FOR_MAX_DIFF { get; set; }
        public VMAccuracy(VMGrid G, VMf f)
        {
            CurGrid = G;
            Fun_Name = f;
            double[] Output = Get_Accur();
            MAX_DIFF_HA_AND_EP = Output[0];
            ARG_FOR_MAX_DIFF = new double[3] { Output[1], Output[2], Output[3] };
        }
        public VMAccuracy(VMGrid G, VMf f, double MAX, double[] ARG)
        {
            CurGrid = G;
            Fun_Name = f;
            MAX_DIFF_HA_AND_EP = MAX;
            ARG_FOR_MAX_DIFF = ARG;
        }

        public VMf Fun_Name { get; set; }
        private double[] Get_Accur()
        {
            double[] output_HA = new double[CurGrid.Length];
            double[] output_EP = new double[CurGrid.Length];
            double[] input = new double[CurGrid.Length];
            double max_diff = -1; double[] arg_max_diff = new double[3];
            for (int i = 0; i < CurGrid.Length; i++)
            {
                input[i] = CurGrid.Begin + CurGrid.Step * i;
            }
            Get_MKL_ACCUR(CurGrid.Length, input, output_HA, output_EP, ref max_diff, arg_max_diff, Fun_Name);
            return new double[4] { max_diff, arg_max_diff[0], arg_max_diff[1], arg_max_diff[2] };
        }
        public override string ToString()
        {
            string S = $"Grid is:    \t From {CurGrid.Begin} To {CurGrid.End} With Step = {CurGrid.Step.ToString("F6")} ({CurGrid.Length} numbers overall)\n";
            return S + $" Function: {Fun_Name}\nAccuracy is:\t Max difference in HA and EP modes = {MAX_DIFF_HA_AND_EP}\nIt reaches with argument " +
                $"= {ARG_FOR_MAX_DIFF[0]} \n(in HA mode = {ARG_FOR_MAX_DIFF[1]})\n(in EP mode = {ARG_FOR_MAX_DIFF[2]})\n";
        }
    }
}
