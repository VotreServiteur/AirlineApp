using System.Windows;
using System.Windows.Controls;

namespace AirlineApp;

public partial class SavingData : Window
{
    public SavingData()
    {
        InitializeComponent();
    }

    public bool IsForSave { get; set; }

    private void SaveData(object sender, RoutedEventArgs e)
    {
        IsForSave = (sender as Button)!.Content.Equals("Yes");
        DialogResult = true;
    }
}