using System;
using System.ComponentModel;
using System.Windows;

namespace AirlineApp;

public partial class CreatingPlane : Window
{
    public bool IsPassenger { get; set; }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    public CreatingPlane(bool isPassenger)
    {
        InitializeComponent();
        
        IsPassenger = isPassenger;
        DataContext = this;
    }
    protected void OnPropertyChanged(string property)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
}