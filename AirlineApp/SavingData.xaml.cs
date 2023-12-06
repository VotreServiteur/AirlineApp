using System.Windows;
using System.Windows.Controls;

namespace AirlineApp;

public partial class SavingData : Window
{
    public bool IsForSave { get; set; }

    public SavingData()
    {
        InitializeComponent();
    }

    private void SaveData(object sender, RoutedEventArgs e)
    {
        IsForSave = (sender as Button)!.Content.Equals("Yes");
        DialogResult = true;
    }
}