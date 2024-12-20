using OleksiiStepanov.Game;

namespace OleksiiStepanov.Loading
{
    public class LoadingStepComplete : LoadingStepBase
    {
        public override void Enter()
        {   
            Exit();
        }

        public override LoadingStep GetStepType()
        {
            return LoadingStep.Complete;
        }
    }
}
