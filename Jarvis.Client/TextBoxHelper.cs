using System;
using System.Windows;
using System.Windows.Controls;

namespace Jarvis.Client
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.RegisterAttached(
            "SelectedText",
            typeof(string),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedTextChanged));

        public static readonly DependencyProperty SelectionStartProperty = DependencyProperty.RegisterAttached(
            "SelectionStart",
            typeof(int),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionStartChanged));

        public static readonly DependencyProperty SelectionLengthProperty = DependencyProperty.RegisterAttached(
            "SelectionLength",
            typeof(int),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionLengthChanged));

        static void SelectionLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            TextBox tb = d as TextBox;
            if(tb != null) {
                if(e.OldValue == null && e.NewValue != null) tb.SelectionChanged += tb_SelectionChanged;
                else if(e.OldValue != null && e.NewValue == null) tb.SelectionChanged -= tb_SelectionChanged;

                var newValue = (int)e.NewValue;

                if(newValue != tb.SelectionLength) tb.SelectionLength = newValue;
            }
        }

        static void SelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            TextBox tb = d as TextBox;
            if(tb != null) {
                if(e.OldValue == null && e.NewValue != null) tb.SelectionChanged += tb_SelectionChanged;
                else if(e.OldValue != null && e.NewValue == null) tb.SelectionChanged -= tb_SelectionChanged;

                var newValue = (int)e.NewValue;

                if(newValue != tb.SelectionStart) tb.SelectionStart = newValue;
            }
        }

        // Using a DependencyProperty as the backing store for SelectedText.  This enables animation, styling, binding, etc...
        static void SelectedTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            TextBox tb = obj as TextBox;
            if(tb != null) {
                if(e.OldValue == null && e.NewValue != null) tb.SelectionChanged += tb_SelectionChanged;
                else if(e.OldValue != null && e.NewValue == null) tb.SelectionChanged -= tb_SelectionChanged;

                string newValue = e.NewValue as string;

                if(newValue != null && newValue != tb.SelectedText) tb.SelectedText = newValue as string;
            }
        }

        public static string GetSelectedText(DependencyObject obj) {
            return (string)obj.GetValue(SelectedTextProperty);
        }

        public static void SetSelectedText(DependencyObject obj, string value) {
            obj.SetValue(SelectedTextProperty, value);
        }

        public static int GetSelectionStart(DependencyObject obj) {
            return (int)obj.GetValue(SelectionStartProperty);
        }

        public static void SetSelectionStart(DependencyObject obj, int value) {
            obj.SetValue(SelectionStartProperty, value);
        }

        public static int GetSelectionLength(DependencyObject obj) {
            return (int)obj.GetValue(SelectionLengthProperty);
        }

        public static void SetSelectionLength(DependencyObject obj, int value) {
            obj.SetValue(SelectionLengthProperty, value);
        }

        static void tb_SelectionChanged(object sender, RoutedEventArgs e) {
            TextBox tb = sender as TextBox;
            if(tb != null) SetSelectedText(tb, tb.SelectedText);
            if(tb != null) SetSelectionStart(tb, tb.SelectionStart);
            if(tb != null) SetSelectionLength(tb, tb.SelectionLength);
        }
    }
}