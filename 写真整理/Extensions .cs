using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace 写真整理
{
    public static class Extensions
    {
        public static int GetItemIndexAtPoint<T>(this ItemsControl ic, Point pt)
            where T : DependencyObject
        {
            HitTestResult result = VisualTreeHelper.HitTest(ic, pt);
            DependencyObject obj = result.VisualHit;

            while (obj != null && !(obj is T))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            if (obj != null)
            {
                return ic.Items.IndexOf((obj as dynamic).Content);
            }

            return -1;
        }
    }
}
