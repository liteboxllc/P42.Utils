﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

namespace P42.Utils.Uno
{
    public static class UIElementExtensions
    {
        public static bool HasPrescribedWidth(this FrameworkElement element) => !double.IsNaN(element.Width) && element.Width >= 0;
        public static bool HasPrescribedHeight(this FrameworkElement element) => !double.IsNaN(element.Height) && element.Height >= 0;


        public static Rect GetBounds(this FrameworkElement element)
        {
            var ttv = element.TransformToVisual(Windows.UI.Xaml.Window.Current.Content);
            var location = ttv.TransformPoint(new Point(0, 0));
            return new Rect(location, new Size(element.ActualWidth, element.ActualHeight));
        }

        public static Rect GetBounds(this UIElement element)
        {
            if (element is FrameworkElement fwElement)
                return fwElement.GetBounds();
            var ttv = element.TransformToVisual(Windows.UI.Xaml.Window.Current.Content);
            var location = ttv.TransformPoint(new Point(0, 0));
            return new Rect(location, new Size(element.DesiredSize.Width, element.DesiredSize.Height));
        }

        public static Rect GetBoundsRelativeTo(this FrameworkElement element, UIElement relativeToElement)
        {
            var ttv = element.TransformToVisual(relativeToElement);
            var location = ttv.TransformPoint(new Point(0, 0));
            return new Rect(location, new Size(element.ActualWidth, element.ActualHeight));
        }

        public static DependencyObject FindAncestor<T>(this FrameworkElement element)
        {
            var parent = element.Parent as FrameworkElement;
            while (parent != null)
            {
                if (parent is T)
                    return parent;
                if (parent is FrameworkElement fe)
                    parent = fe.Parent as FrameworkElement;
                else
                    parent = null;
            }
            return null;
        }

        public static bool IsVisible(this UIElement element)
            => element.Visibility == Visibility.Visible;

        public static bool IsCollapsed(this UIElement element)
            => element.Visibility == Visibility.Collapsed;

        public static DataTemplate AsDataTemplate(this Type type)
        {
            if (type == null || !typeof(FrameworkElement).IsAssignableFrom(type))
                throw new Exception("Cannot convert type [" + type + "] into DataTemplate");
            var markup = $"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:local=\"using:{type.Namespace}\"><local:{type.Name} /></DataTemplate>";
            System.Diagnostics.Debug.WriteLine("BcGroupView.GenerateDatatemplate: markup: " + markup);
            return (DataTemplate)XamlReader.Load(markup);
        }
    }
}
