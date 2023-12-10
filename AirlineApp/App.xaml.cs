using System;
using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace AirlineApp;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
}

public class AppDragDrop : IDropTarget
{
    public void DragOver(IDropInfo dropInfo)
    {
        throw new NotImplementedException();
    }

    public void Drop(IDropInfo dropInfo)
    {
        throw new NotImplementedException();
    }
}