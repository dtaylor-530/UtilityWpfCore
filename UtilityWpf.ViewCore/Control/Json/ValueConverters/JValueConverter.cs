﻿//using System;
//using System.Globalization;
//using System.Windows.Data;
//using Newtonsoft.Json.Linq;

//namespace UtilityWpf.View.Json.ValueConverters
//{
//    public sealed class JValueConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (value is JValue jval)
//            {
//                switch (jval.Type)
//                {

//                    case JTokenType.Null:
//                        return "Null";
//                    default:
//                        return jval.Value;
//                }
//            }

//            return value;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
//        }
//    }
//}
