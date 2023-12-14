using System;
using System.Windows;
using System.Windows.Data;
using Airline;
using Planes;
using static System.Convert;

namespace AirlineApp;

public partial class CreatingPlane : Window
{
    
    private PlaneModel? _planeModel;
    public CreatingPlane(bool isPassenger)
    {
        InitializeComponent();
        IsPassenger = isPassenger;
        _planeModel = new PlaneModel();
        _planeModel.IsPassenger = isPassenger;
        DataContext = _planeModel;
        Console.Write(PriceBox.BorderBrush);
    }


    public bool IsPassenger { get; set; }

    public Plane? Airplane { get; set; }

    private void AcceptButton(object sender, RoutedEventArgs e)
    {
        Airplane = IsPassenger
            ? new PassengerPlane(NameBox.Text, ToInt32(CapacityBox.Text), ToInt32(LoadCapacityBox.Text),
                ToInt32(CrewBox.Text), ToInt32(FuelBox.Text), ToInt32(RangeBox.Text), ToInt32(AmountBox.Text))
            : new CargoPlane(NameBox.Text, ToInt32(CapacityBox.Text), ToInt32(LoadCapacityBox.Text),
                ToInt32(CrewBox.Text), ToInt32(FuelBox.Text), ToInt32(RangeBox.Text), ToInt32(PriceBox.Text));
        DialogResult = true;
    }
}