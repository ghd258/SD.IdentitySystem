using SD.Infrastructure.WPF.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SD.Infrastructure.WPF.Converters
{
    /// <summary>
    /// TreeListView�����ߴ�ת����
    /// </summary>
    public class TreeListViewIndentConverter : IValueConverter
    {
        /// <summary>
        /// ������λ�ߴ�
        /// </summary>
        private const double IndentUnitSize = 14.0;

        /// <summary>
        /// ת��TreeListView�����ߴ�
        /// </summary>
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            TreeListViewItem treeListViewItem = (TreeListViewItem)value;
            int level/*���ڵ㼶��*/ = treeListViewItem.Level;
            double indentSize/*�����ߴ�*/ = level * IndentUnitSize;
            Thickness margin = new Thickness(indentSize, 0, 0, 0);

            return margin;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
