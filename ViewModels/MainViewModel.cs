using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Popups;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class MainViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<TabbedPage> tabs;

    [ObservableProperty]
    private string searchKeyword = string.Empty;

    [ObservableProperty]
    private TabbedPage? selectedItem;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(MoveNextCommand))]
    [NotifyCanExecuteChangedFor(nameof(MovePrevCommand))]
    private int selectedIndex;

    [ObservableProperty]
    private bool controlsEnabled = true;

    private readonly ICollectionView _collectionView;

    public MainViewModel(IEnumerable<View> views) 
    {
        Tabs = [];
        foreach (var view in views.OrderBy(x => x.Title))
        {
            Tabs.Add(new(view));
        }
        
        for (int i = 0; i < Tabs.Count; i++) 
        {
            if (Tabs[i].Content!.Title == "INICIO") 
            {
                Tabs.Move(i, 0);
            }
            if (Tabs[i].Content!.Title == "CONFIGURACIONES")
            {
                Tabs.Move(i, Tabs.Count - 1);
            }
        }

        SelectedItem = Tabs.FirstOrDefault();
        _collectionView = CollectionViewSource.GetDefaultView(Tabs);
        _collectionView.Filter = CollectionFilter;
    }
    protected override async Task UnloadAsync()
    {
        await Task.CompletedTask;
    }

    protected override async Task LoadAsync()
    {
        await Task.CompletedTask;
    }
    [RelayCommand]
    private void Home()
    {
        SearchKeyword = string.Empty;
        SelectedIndex = 0;
    }
    [RelayCommand(CanExecute = nameof(MovePrevCanExecute))]
    private void MovePrev()
    {
        if(!string.IsNullOrEmpty(SearchKeyword))
        {
            SearchKeyword = string.Empty;
        }
        SelectedIndex--;
    }
    private bool MovePrevCanExecute() => SelectedIndex > 0;
    
    [RelayCommand(CanExecute = nameof(MoveNextCanExecute))]
    private void MoveNext()
    {
        if (!string.IsNullOrEmpty(SearchKeyword))
        {
            SearchKeyword = string.Empty;
        }
        SelectedIndex++;
    }
    private bool MoveNextCanExecute() => SelectedIndex < Tabs.Count - 1;

    public bool CollectionFilter(object o)
    {
        if (o is not TabbedPage page) return false;
        if (page.Content is null) return false;
        return page.Content.Title.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase);
    }
    partial void OnSearchKeywordChanged(string value)
    {
        _collectionView.Refresh();
    }
    async partial void OnSelectedItemChanged(TabbedPage? oldValue, TabbedPage? newValue)
    {
        if (oldValue is not null && oldValue.Content is not null)
            await oldValue.Content.Unload();

        if (newValue is not null && newValue.Content is not null)
            await newValue.Content.LoadAsync();
    }

    protected override void Refresh()
    {
    }
}
