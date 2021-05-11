using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// ת�������������ߴ�
    /// </summary>
    public class NodeLevelIndentConverter : IValueConverter
    {
        /// <summary>
        /// ������λ�ߴ�
        /// </summary>
        private const double IndentUnitSize = 14.0;

        /// <summary>
        /// ת��
        /// </summary>
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            int level/*���ڵ㼶��*/ = System.Convert.ToInt32(value);
            double indentSize/*�����ߴ�*/ = level * IndentUnitSize;
            Thickness margin = new Thickness(indentSize, 0, 0, 0);

            return margin;
        }

        /// <summary>
        /// ת����
        /// </summary>
        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
