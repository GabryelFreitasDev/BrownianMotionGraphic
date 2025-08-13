using BrownianMotionGraphic.Services;
using BrownianMotionGraphic.ViewModels;
using BrownianMotionGraphic.Views;

namespace BrownianMotionGraphic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Obter a MainPage do provedor de serviços para garantir que as dependências sejam injetadas
            return new Window(MauiProgram.Services.GetService<MainPage>());
        }
    }
}