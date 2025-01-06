using OleksiiStepanov.Data;
using OleksiiStepanov.Gameplay;

namespace OleksiiStepanov.Loading
{
    public class LoadingStepComplete : LoadingStepBase
    {
        public override void Enter()
        {   
            GameplayManager.Instance.Init(Exit);
        }

        public override LoadingStep GetStepType()
        {
            return LoadingStep.Complete;
        }
    }
}
