using BrownianMotionGraphic.Models;

namespace BrownianMotionGraphic.Services;

public interface IBrownianMotionService
{
    BrownianMotionResult GenerateBrownianMotion(BrownianMotionParameters parameters);
}

