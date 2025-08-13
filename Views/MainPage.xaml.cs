using BrownianMotionGraphic.ViewModels;

namespace BrownianMotionGraphic.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;
    private readonly BrownianMotionGraphDrawable _graphDrawable;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = _viewModel;
        
        _graphDrawable = new BrownianMotionGraphDrawable();
        ChartGraphicsView.Drawable = _graphDrawable;

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainPageViewModel.CurrentResult))
        {
            _graphDrawable.Result = _viewModel.CurrentResult;
            ChartGraphicsView.Invalidate();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
}

