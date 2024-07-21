using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace EstrellaAccesoriosWpf.Common;

public abstract class View : UserControl
{
    public static readonly DependencyProperty ButtonsContentProperty =
        DependencyProperty.Register("ButtonsContent", typeof(object), typeof(View), new PropertyMetadata(null));

    public static readonly DependencyProperty FilterContentProperty =
        DependencyProperty.Register("FilterContent", typeof(object), typeof(View), new PropertyMetadata(null));

    public static readonly DependencyProperty MainContentProperty =
        DependencyProperty.Register("MainContent", typeof(object), typeof(View), new PropertyMetadata(null));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(View), new PropertyMetadata(string.Empty));
    public object ButtonsContent
    {
        get { return (object)GetValue(ButtonsContentProperty); }
        set { SetValue(ButtonsContentProperty, value); }
    }
    public object FilterContent
    {
        get { return (object)GetValue(FilterContentProperty); }
        set { SetValue(FilterContentProperty, value); }
    }
    public object MainContent
    {
        get { return (object)GetValue(MainContentProperty); }
        set { SetValue(MainContentProperty, value); }
    }
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }



    public View(ViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void InitializeComponent()
    {
        var mainGrid = new Grid();
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        mainGrid.RowDefinitions.Add(new RowDefinition());
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        var buttonsContent = new ContentControl()
        {
            Name = "ButtonsContent",
            Margin = new Thickness(10)
        };
        buttonsContent.SetBinding(ContentControl.ContentProperty, new Binding("ButtonsContent") { Source = this });
        mainGrid.Children.Add(buttonsContent);

        var filterContent = new ContentControl()
        {
            Name = "FilterContent",
            Margin = new Thickness(10)
        };
        filterContent.SetBinding(ContentControl.ContentProperty, new Binding("FilterContent") { Source = this });
        Grid.SetColumn(filterContent, 1);
        mainGrid.Children.Add(filterContent);

        var separator = new Separator() { Height = 2 };
        Grid.SetColumnSpan(separator, 2);
        Grid.SetRow(separator, 1);
        mainGrid.Children.Add(separator);

        var mainContent = new ContentControl()
        {
            Name = "MainContent",
            Margin = new Thickness(10)
        };
        mainContent.SetBinding(ContentControl.ContentProperty, new Binding("MainContent") { Source = this });
        Grid.SetColumnSpan(mainContent, 2);
        Grid.SetRow(mainContent, 2);
        mainGrid.Children.Add(mainContent);

        Content = mainGrid;
    }

    public Task LoadAsync()
    {
        if (DataContext is ViewModel vm)
        {
            return vm.LoadCommand.ExecuteAsync(this);
        }
        return Task.CompletedTask;
    }
    public async Task Unload()
    {
        if (DataContext is ViewModel vm)
        {
            await vm.UnloadCommand.ExecuteAsync(this);
        }
    }
    

}
