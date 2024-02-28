namespace RobotWorkflow
{
    /// <summary>
    /// Represents a controller responsible for managing interactions with robots.
    /// </summary>
    public class Controller()
    {
        private List<Guid> _robotIds = [];

        private readonly RobotRegistry _robotRegistry = RobotRegistry.GetInstance();
        public void AssignRobot(Guid robotId)
        {
            _robotIds.Add(robotId);
        }

        /// <summary>
        /// Performs a round of interactions with the assigned robots.
        /// Firstly, it shuffles the robot IDs to randomize the order of interactions.
        /// Then, it tries to repair unhealthy robots if there are enough interactions left.
        /// It is worth noting that the controller knows only the robot id, not the robot object itself.
        /// </summary>
        /// <returns>The penalty for the controller round.</returns>
        public int PerformRound()
        {
            var random = new Random();
            _robotIds = _robotIds.OrderBy(_ => random.Next()).ToList();

            var remainingInteractions = Constants.ControllerInteractionLimit;

            foreach (var robotId in _robotIds.Where(_ => remainingInteractions >= 3))
            {
                if (!_robotRegistry.IsRobotActive(robotId))
                {
                    remainingInteractions--;
                    continue;
                }

                if (_robotRegistry.IsRobotHealthy(robotId))
                {
                    remainingInteractions--;
                    continue;
                }

                _robotRegistry.RepairRobot(robotId);
                remainingInteractions--;
            }

            return Constants.ControllerRoundPenalty;
        }
    }
}
