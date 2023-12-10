using System.Windows;

namespace AirlineApp;

public partial class TypeChoosing : Window
{
    public TypeChoosing()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}