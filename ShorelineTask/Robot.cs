namespace RobotWorkflow
{

    /// <summary>
    /// Represents a robot entity with activation, deactivation, health, and activity status.
    /// </summary>
    public class Robot(int activationRound, int deactivationRound)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int ActivationRound { get; } = activationRound;
        public int DeactivationRound { get; } = deactivationRound;
        public bool IsActive { get; private set; }
        public bool IsHealthy { get; set; } = true;

        /// <summary>
        /// If all the arguments are validated, a new instance of the <see cref="Robot"/> class is created and returned.
        /// </summary>
        /// <param name="activationRound">The round when the robot is activated.</param>
        /// <param name="deactivationRound">The round when the robot is deactivated.</param>
        /// <returns>A new instance of the <see cref="Robot"/> class.</returns>
        /// <exception cref="ArgumentException">Thrown when activationRound or deactivationRound is out of valid range.</exception>
        public static Robot Create(int activationRound, int deactivationRound)
        {
            if (activationRound is < 1 or > 30)
            {
                throw new ArgumentException("Activation round should be between 1 and 30");
            }

            if (deactivationRound is < 30 or > 60)
            {
                throw new ArgumentException("Deactivation round should be between 30 and 60");
            }

            return new Robot(activationRound, deactivationRound);
        }

        /// <summary>
        /// Updates the activity status of the robot based on the current round.
        /// </summary>
        /// <param name="round">The current round number.</param>
        public void UpdateByRound(int round)
        {
            if (round == ActivationRound)
            {
                IsActive = true;
            }
            else if (round == DeactivationRound)
            {
                IsActive = false;
            }
        }

        /// <summary>
        /// Simulates a workload for a given robot.
        /// </summary>
        /// <returns>The workload score after finishing.</returns>
        public int PerformWorkload()
        {
            if (!IsActive)
            {
                return Constants.NeutralScore;
            }

            if (!IsHealthy)
            {
                return Constants.UnhealthyPenalty;
            }

            var random = new Random();
            var workloadResult = random.Next(1, Constants.WorkloadUpper + 1);

            if (workloadResult <= Constants.WorkloadThreshold)
            {
                return Constants.HealthyReward;
            }

            IsHealthy = false;
            return Constants.NeutralScore;
        }
    }
}
