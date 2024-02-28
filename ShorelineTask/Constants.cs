namespace RobotWorkflow
{
    public static class Constants
    {
        public const int RobotCount = 10000;
        public const int ControllerCount = 75;
        public const int NeutralScore = 0;
        public const int ControllerInteractionLimit = 100;
        public const int WorkloadThreshold = 90;
        public const int WorkloadUpper = 100;
        public const int UnhealthyPenalty = -10;
        public const int ControllerRoundPenalty = -20;
        public const int HealthyReward = 1;
        public const int RoundCount = 60;
        public const int ActivationUpper = 30;
        public const int DeactivationUpper = 60;
    }
}