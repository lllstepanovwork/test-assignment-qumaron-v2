using OleksiiStepanov.Game;

namespace OleksiiStepanov.Loading
{
    public class LoadingStepAppInit : LoadingStepBase
    {
        public override void Enter()
        {
            Exit();
        }

        public override LoadingStep GetStepType()
        {
            return LoadingStep.AppInit;
        }
    }
}
