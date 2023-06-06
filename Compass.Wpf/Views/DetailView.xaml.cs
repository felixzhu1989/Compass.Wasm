﻿using System.Diagnostics;
using System.Windows.Navigation;

namespace Compass.Wpf.Views;

/// <summary>
/// DetailView.xaml 的交互逻辑
/// </summary>
public partial class DetailView : UserControl
{
    public DetailView()
    {
        InitializeComponent();
    }
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        // for .NET Core you need to add UseShellExecute = true
        // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
        var startInfo = new ProcessStartInfo(e.Uri.AbsoluteUri)
            { UseShellExecute =true };
        Process.Start(startInfo);
        e.Handled = true;
    }
}