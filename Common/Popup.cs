using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.Common;

public class Popup : UserControl
{
    Grid MainGrid = null!;
    TextBlock Header = null!;
    Button AcceptButton = null!;
    Button CancelButton = null!;
    ContentControl ContentArea = null!;
    ScrollViewer ScrollViewer = null!;

    public object PopupContent
    {
        get { return (object)GetValue(PopupContentProperty); }
        set { SetValue(PopupContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for PopupContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PopupContentProperty =
        DependencyProperty.Register("PopupContent", typeof(object), typeof(Popup), new PropertyMetadata(null));


    public Popup()
    {
        InitializeComponent();
        MinWidth = 300;
        MinHeight = 100;
        MaxWidth = 1200;
        MaxHeight = 700;
    }
    protected void SetValues(object content, string header, string button)
    {
        Header.Text = header;
        ContentArea.Content = content;
        AcceptButton.Content = button;
    }
    private void InitializeComponent()
    {
        MainGrid = new Grid();
        MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        MainGrid.RowDefinitions.Add(new RowDefinition());
        MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Header = new TextBlock
        {
            Name = "HeaderText",
            Text = "Header",
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };
        Grid.SetRow(Header, 0);
        MainGrid.Children.Add(Header);

        ContentArea = new ContentControl
        {
            Name = "ContentArea",
            Margin = new Thickness(10)
        };

        ScrollViewer = new ScrollViewer()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };
        ScrollViewer.SetBinding(ContentProperty, new Binding("PopupContent") { Source = this });

        ContentArea.Content = ScrollViewer;
        Grid.SetRow(ContentArea, 1);
        MainGrid.Children.Add(ContentArea);

        var buttonsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };

        AcceptButton = new Button { Name = "AcceptButton", Content = "ACEPTAR", Width = 120, Margin = new Thickness(5)};
        AcceptButton.Click += AcceptButton_Click;

        CancelButton = new Button { Name = "CancelButton", Content = "CANCELAR", Width = 120, Margin = new Thickness(5)};
        CancelButton.Click += CancelButton_Click;

        buttonsPanel.Children.Add(AcceptButton);
        buttonsPanel.Children.Add(CancelButton);
        Grid.SetRow(buttonsPanel, 2);
        MainGrid.Children.Add(buttonsPanel);

       
        Content = MainGrid;
    }

    protected void SetHeader(string header)
    {
        Header.Text = header;
    }
    protected virtual void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        DialogHost.CloseDialogCommand.Execute(true, this);
    }
    protected virtual void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogHost.CloseDialogCommand.Execute(false, this);
    }
}
