using BrownianMotionGraphic.Models;

namespace BrownianMotionGraphic.Views;

public class BrownianMotionGraphDrawable : IDrawable
{
    private BrownianMotionResult? _result;
    private readonly Color[] _lineColors = new[]
    {
        Colors.Blue,
        Colors.Red,
        Colors.Green,
        Colors.Orange,
        Colors.Purple,
        Colors.Brown,
        Colors.Pink,
        Colors.Gray,
        Colors.Olive,
        Colors.Navy
    };

    public BrownianMotionResult? Result
    {
        get => _result;
        set => _result = value;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_result == null || !_result.Simulations.Any())
        {
            return;
        }

        DrawBackground(canvas, dirtyRect);
        DrawAxes(canvas, dirtyRect);
        DrawGrid(canvas, dirtyRect);
        DrawSimulations(canvas, dirtyRect);
        DrawLabels(canvas, dirtyRect);
    }

    private void DrawBackground(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(dirtyRect);
    }

    private void DrawAxes(ICanvas canvas, RectF dirtyRect)
    {
        var margin = 60f;
        var graphRect = new RectF(margin, margin, dirtyRect.Width - 2 * margin, dirtyRect.Height - 2 * margin);

        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;

        // Eixo X
        canvas.DrawLine(graphRect.Left, graphRect.Bottom, graphRect.Right, graphRect.Bottom);

        // Eixo Y
        canvas.DrawLine(graphRect.Left, graphRect.Top, graphRect.Left, graphRect.Bottom);
    }

    private void DrawGrid(ICanvas canvas, RectF dirtyRect)
    {
        var margin = 60f;
        var graphRect = new RectF(margin, margin, dirtyRect.Width - 2 * margin, dirtyRect.Height - 2 * margin);

        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1;

        // Linhas verticais da grade
        for (int i = 1; i < 10; i++)
        {
            var x = graphRect.Left + (graphRect.Width * i / 10);
            canvas.DrawLine(x, graphRect.Top, x, graphRect.Bottom);
        }

        // Linhas horizontais da grade
        for (int i = 1; i < 10; i++)
        {
            var y = graphRect.Top + (graphRect.Height * i / 10);
            canvas.DrawLine(graphRect.Left, y, graphRect.Right, y);
        }
    }

    private void DrawSimulations(ICanvas canvas, RectF dirtyRect)
    {
        if (_result == null || !_result.Simulations.Any())
            return;

        var margin = 60f;
        var graphRect = new RectF(margin, margin, dirtyRect.Width - 2 * margin, dirtyRect.Height - 2 * margin);

        var allPrices = _result.Simulations.SelectMany(s => s).ToArray();
        var minPrice = allPrices.Min();
        var maxPrice = allPrices.Max();
        var priceRange = maxPrice - minPrice;

        if (priceRange == 0) priceRange = 1; // Evitar divisão por zero

        var maxDays = _result.TimeSteps.Max();

        canvas.StrokeSize = 2;

        // Desenhar cada simulação
        for (int simIndex = 0; simIndex < _result.Simulations.Count; simIndex++)
        {
            var simulation = _result.Simulations[simIndex];
            canvas.StrokeColor = _lineColors[simIndex % _lineColors.Length];

            for (int i = 1; i < simulation.Length; i++)
            {
                var x1 = graphRect.Left + (graphRect.Width * _result.TimeSteps[i - 1] / maxDays);
                var y1 = graphRect.Bottom - (graphRect.Height * (simulation[i - 1] - minPrice) / priceRange);

                var x2 = graphRect.Left + (graphRect.Width * _result.TimeSteps[i] / maxDays);
                var y2 = graphRect.Bottom - (graphRect.Height * (simulation[i] - minPrice) / priceRange);

                canvas.DrawLine((float)x1, (float)y1, (float)x2, (float)y2);
            }
        }
    }

    private void DrawLabels(ICanvas canvas, RectF dirtyRect)
    {
        if (_result == null)
            return;

        var margin = 60f;
        var graphRect = new RectF(margin, margin, dirtyRect.Width - 2 * margin, dirtyRect.Height - 2 * margin);

        canvas.FontColor = Colors.Black;
        canvas.FontSize = 12;

        var allPrices = _result.Simulations.SelectMany(s => s).ToArray();
        var minPrice = allPrices.Min();
        var maxPrice = allPrices.Max();
        var maxDays = _result.TimeSteps.Max(); // Agora é em dias

        // Labels do eixo Y (preços)
        for (int i = 0; i <= 5; i++)
        {
            var price = minPrice + (maxPrice - minPrice) * i / 5;
            var y = graphRect.Bottom - (graphRect.Height * i / 5);
            canvas.DrawString($"{price:F1}", new RectF(5, y - 10, margin - 10, 20),
                HorizontalAlignment.Right, VerticalAlignment.Center);
        }

        // Labels do eixo X (dias)
        for (int i = 0; i <= 5; i++)
        {
            var days = maxDays * i / 5;
            var x = graphRect.Left + (graphRect.Width * i / 5);
            canvas.DrawString($"{days:F0}", new RectF(x - 20, graphRect.Bottom + 5, 40, 20),
                HorizontalAlignment.Center, VerticalAlignment.Top);
        }

        // Título dos eixos
        canvas.FontSize = 14;
        canvas.DrawString("Tempo (dias)", new RectF(0, dirtyRect.Height - 30, dirtyRect.Width, 20),
            HorizontalAlignment.Center, VerticalAlignment.Center);

        canvas.DrawString("Preço", new RectF(5, 5, 100, 20),
            HorizontalAlignment.Left, VerticalAlignment.Top);
    }
}