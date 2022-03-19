using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_APP
{
    public class MultiConverters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string Length, Start, End;
                Length = values[0].ToString();
                Start = values[1].ToString();
                End = values[2].ToString();
                return Length + ";" + Start + ";" + End;
            }
            catch (Exception ex)
            {
                return " ; ; ";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string st = value.ToString();
                string [] stt = st.Split(";", StringSplitOptions.RemoveEmptyEntries);
                return new object[] { int.Parse(stt[0]), Double.Parse(stt[1]), Double.Parse(stt[2]) };
            }
            catch (Exception ex)
            {
                return new object[3];
            }
        }
    }
    public class Convert_Time : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double Time_HA, Time_EP, Time_NO_MKL;
                Time_HA = Double.Parse(values[0].ToString());
                Time_EP = Double.Parse(values[1].ToString());
                Time_NO_MKL = Double.Parse(values[2].ToString());
                return $"Time_HA = {Time_HA.ToString("F7")} ; Time_EP = {Time_EP.ToString("F7")}\nTime_NO_MKL = {Time_NO_MKL.ToString("F7")}\nHA / NO_MKL = {(Time_HA / Time_NO_MKL).ToString("F3")}" +
                    $"\nEP / NO_MKL = {(Time_EP / Time_NO_MKL).ToString("F3")}";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[3] { 0, 0, 0}; // doesn't matter what to return cause of One-Way binding
        }
    }

    public class Convert_Accur : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double MaxDiff, Arg, Val_HA, Val_EP;
                MaxDiff = Double.Parse(values[0].ToString());
                Arg = Double.Parse(values[1].ToString());
                Val_HA = Double.Parse(values[2].ToString());
                Val_EP = Double.Parse(values[3].ToString());
                return $"Max difference = {MaxDiff.ToString()}\nReaches with argument = {Arg.ToString()}\nValues in modes:\nHA = {Val_HA.ToString()}\nEP = {Val_EP.ToString()}";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[3] { 0, 0, 0 }; // doesn't matter what to return cause of One-Way binding
        }
    }
    public class Display_Zero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double tmp = Double.Parse(value.ToString());
                if (tmp == 0 || tmp == double.MaxValue)
                    return "No data";
                else return tmp.ToString("F3");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
