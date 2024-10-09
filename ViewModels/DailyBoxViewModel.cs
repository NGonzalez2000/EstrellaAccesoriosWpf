using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EstrellaAccesoriosWpf.Common;
using EstrellaAccesoriosWpf.Filters.FiltersViewModels;
using EstrellaAccesoriosWpf.Models;
using EstrellaAccesoriosWpf.Popups;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace EstrellaAccesoriosWpf.ViewModels;

public partial class DailyBoxViewModel : ViewModel
{
    private readonly EstrellaDbContext _dbContext;
    private readonly ISnackbarMessageQueue _messageQueue;
    private ICollectionView collectionView = null!;

    [ObservableProperty]
    private MoneyMovement? selectedItem;

    [ObservableProperty]
    private MoneyMovementFilterViewModel filterViewModel = null!;

    [ObservableProperty]
    private ObservableCollection<MoneyMovement> moneyMovements;

    [ObservableProperty]
    private DateOnly boxDate;

    [ObservableProperty]
    private DateOnly dateStart;

    [ObservableProperty]
    private DateOnly dateEnd;

    [ObservableProperty]
    private CashClose actualCashClose = null!;

    [ObservableProperty]
    private CashClose previousCashClose = null!;

    [ObservableProperty]
    private decimal balance = 0m;

    public DailyBoxViewModel(EstrellaDbContext dbContext, ISnackbarMessageQueue SnackbarMessageQueue)
    {
        _dbContext = dbContext;
        _messageQueue = SnackbarMessageQueue;
        MoneyMovements = [];
    }

    private void OnFilterViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }

    protected override async Task LoadAsync()
    {
        DateStart = await _dbContext.CashCloses.MinAsync(cc => cc.Date);

        BoxDate = DateEnd = DateOnly.FromDateTime(DateTime.Now);

        GetBalance();

        SetFilter();
    }

    private void GetBalance()
    {
        CashClose cashClose = GetCashClose(DateEnd.AddDays(-1));
        Balance = cashClose.Balance;
        MoneyMovements = new([.. _dbContext.MoneyMovements.Include(mm => mm.MoneyMovementType).Include(mm => mm.PaymentMethod).Where(mm => mm.Date == DateEnd)]);

        foreach(var movement in MoneyMovements)
        {
            if (movement.MoneyMovementType.Description == "RETIRO")
                Balance -= movement.Amount;
            else
            {
                if(movement.PaymentMethod is not PaymentMethod method)
                    Balance += movement.Amount;
                else if(method.ModifyCash)
                    Balance += movement.Amount;
            }
        }
    }

    partial void OnBoxDateChanged(DateOnly value)
    {
        ActualCashClose = GetCashClose(value);

        var previousDate = value.AddDays(-1);

        if (value <= DateStart)
            PreviousCashClose = ActualCashClose;
        else
            PreviousCashClose = GetCashClose(previousDate);

        MoneyMovements = new(_dbContext.MoneyMovements.Include(mm => mm.MoneyMovementType).Where(mm => mm.Date == BoxDate).ToList());
        FilterViewModel = new();
        FilterViewModel.PropertyChanged += OnFilterViewModelPropertyChanged;
    }

    private CashClose GetCashClose(DateOnly value)
    {
        var cashClose = _dbContext.CashCloses.FirstOrDefault(c => c.Date == value);
        if(cashClose is not null) return cashClose;

        var previousDate = value.AddDays(-1);
        var previousCashClose = GetCashClose(previousDate);

        if (value == DateEnd)
        {
            return CashClose.Create(value, 0m);
        }

        cashClose = CashClose.Create(value, previousCashClose.Balance);

        List<MoneyMovement> movements = [.. _dbContext.MoneyMovements.Include(c => c.MoneyMovementType).Include(c => c.PaymentMethod).Where(mm => mm.Date == value)];

        foreach(MoneyMovement movement in movements)
        {
            if (movement.MoneyMovementType.Description == "RETIRO")
                cashClose.RemoveBalance(movement.Amount);
            else
            {
                if(movement.PaymentMethod is not PaymentMethod method)
                {
                    cashClose.AddBalance(movement.Amount);
                }
                else if(method.ModifyCash)
                {
                    cashClose.AddBalance(movement.Amount);
                }

            }
        }

        _dbContext.CashCloses.Add(cashClose);
        _dbContext.SaveChanges();

        return cashClose;

    }

    protected override void Refresh()
    {
        collectionView.SortDescriptions.Clear();
        collectionView.SortDescriptions.Add(FilterViewModel.GetSortDescription());
        collectionView.Refresh();
    }
    protected override async Task UnloadAsync()
    {
        await Task.CompletedTask;
        MoneyMovements = [];
    }
    private void SetFilter()
    {
        collectionView = CollectionViewSource.GetDefaultView(MoneyMovements);
        collectionView.Filter = FilterViewModel.Filter;
    }
    [RelayCommand]
    private async Task Create()
    {
        var movementTypes = _dbContext.MoneyMovementTypes.Where(mt => mt.Description != "VENTA").ToList();

        MoneyMovement newItem = MoneyMovement.Create("", 0m, movementTypes[0]);
        MoneyMovementPopup popup = new(newItem, _dbContext);
        bool response = await popup.Create();
        if (!response)
        {
            return;
        }


        _dbContext.MoneyMovements.Add(newItem);
        await _dbContext.SaveChangesAsync();

        MoneyMovements.Add(newItem);
        SetFilter();

        if (newItem.MoneyMovementType.Description == "RETIRO")
            Balance -= newItem.Amount;
        else
            Balance += newItem.Amount;

        _messageQueue.Enqueue("Operacion creada con éxito");
    }
}
