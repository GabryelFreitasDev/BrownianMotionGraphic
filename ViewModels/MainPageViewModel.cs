using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BrownianMotionGraphic.Models;
using BrownianMotionGraphic.Services;

namespace BrownianMotionGraphic.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IBrownianMotionService _brownianMotionService;

    [ObservableProperty]
    private double initialPrice = 100.0;

    [ObservableProperty]
    private double volatility = 20.0;

    [ObservableProperty]
    private double mean = 1.0;

    [ObservableProperty]
    private int numberOfDays = 252;

    [ObservableProperty]
    private int numberOfSimulations = 1;

    [ObservableProperty]
    private bool isGenerating = false;

    [ObservableProperty]
    private BrownianMotionResult? currentResult;

    [ObservableProperty]
    private string statusMessage = "Pronto para gerar simulação";

    [ObservableProperty]
    private bool showAdvancedOptions = false;

    public MainPageViewModel(IBrownianMotionService brownianMotionService)
    {
        _brownianMotionService = brownianMotionService;
    }

    [RelayCommand]
    private async Task GenerateSimulation()
    {
        try
        {
            IsGenerating = true;
            StatusMessage = "Gerando simulação...";

            var parameters = new BrownianMotionParameters
            {
                InitialPrice = InitialPrice,
                Volatility = Volatility / 100,
                Mean = Mean / 100,
                NumberOfDays = NumberOfDays,
                NumberOfSimulations = NumberOfSimulations
            };

            await Task.Run(() =>
            {
                CurrentResult = _brownianMotionService.GenerateBrownianMotion(parameters);
            });

            StatusMessage = $"Simulação concluída - {NumberOfSimulations} {(NumberOfSimulations == 1 ? "simulação gerada" : "simulações geradas")}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erro ao gerar simulação: {ex.Message}";
        }
        finally
        {
            IsGenerating = false;
        }
    }

    [RelayCommand]
    private void ResetParameters()
    {
        InitialPrice = 100.0;
        Volatility = 20.0;
        Mean = 1.0;
        NumberOfDays = 252;
        NumberOfSimulations = 1;
        CurrentResult = null;
        StatusMessage = "Parâmetros resetados";
    }

    [RelayCommand]
    private void ToggleAdvancedOptions()
    {
        ShowAdvancedOptions = !ShowAdvancedOptions;
    }

    partial void OnNumberOfDaysChanged(int value)
    {
        if (value > 1000)
        {
            NumberOfDays = 1000;
            StatusMessage = "Número máximo de dias limitado a 1000";
        }
        else if (value < 1)
        {
            NumberOfDays = 1;
        }
    }

    partial void OnVolatilityChanged(double value)
    {
        Volatility = Math.Round(value, 2);

        if (value < 0)
        {
            Volatility = 0;
        }
        else if (value > 200.0)
        {
            Volatility = 200.0;
            StatusMessage = "Volatilidade limitada a 200%";
        }
    }

    partial void OnMeanChanged(double value)
    {
        Mean = Math.Round(value, 2);

        if (value < -50.0)
        {
            Mean = -50.0;
            StatusMessage = "Retorno médio mínimo é -50%";
        }
        else if (value > 50.0)
        {
            Mean = 50.0;
            StatusMessage = "Retorno médio limitado a 50%";
        }
    }

    partial void OnInitialPriceChanged(double value)
    {
        if (value <= 0)
        {
            InitialPrice = 1.0;
            StatusMessage = "Preço inicial deve ser maior que zero";
        }
    }

    partial void OnNumberOfSimulationsChanged(int value)
    {
        if (value > 10)
        {
            NumberOfSimulations = 10;
            StatusMessage = "Número máximo de simulações limitado a 10";
        }
        else if (value < 1)
        {
            NumberOfSimulations = 1;
        }
    }
}