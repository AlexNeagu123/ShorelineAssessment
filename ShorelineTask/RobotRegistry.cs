namespace RobotWorkflow
{
    /// <summary>
    /// A singleton designed for managing a collection of robots.
    /// This singleton keeps track of the mapping from robot ids to the actual objects.
    /// It exposes only the robot ids.
    /// Based on a robot id, several operations can be performed, such as checking if the robot is active or healthy, or repairing it.
    /// </summary>
    public class RobotRegistry
    {
        private static RobotRegistry? _instance;
        private readonly List<Guid> _robotIds = [];
        private readonly Dictionary<Guid, Robot> _robots;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotRegistry"/> class.
        /// </summary>
        /// <param name="robotCount">The number of robots to generate.</param>
        /// <param name="activationUpper">The upper bound for activation round generation.</param>
        /// <param name="deactivationUpper">The upper bound for deactivation round generation.</param>
        private RobotRegistry(int robotCount, int activationUpper, int deactivationUpper)
        {
            _robots = new Dictionary<Guid, Robot>();
            _GenerateRobots(robotCount, activationUpper, deactivationUpper);
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="RobotRegistry"/>.
        /// </summary>
        /// <returns>The singleton instance of <see cref="RobotRegistry"/>.</returns>
        public static RobotRegistry GetInstance()
        {
            return _instance ??= new RobotRegistry(Constants.RobotCount, Constants.ActivationUpper,
                Constants.DeactivationUpper);
        }

        /// <summary>
        /// Gets a list of consecutive robot IDs within the specified range.
        /// </summary>
        /// <param name="startsFrom">The starting index of the range.</param>
        /// <param name="endsIn">The ending index of the range (exclusive).</param>
        /// <returns>A list of consecutive robot IDs.</returns>
        public List<Guid> GetConsecutiveRobots(int startsFrom, int endsIn)
        {
            return _robotIds.GetRange(startsFrom, endsIn - startsFrom);
        }

        /// <summary>
        /// Updates the active status of all robots based on the current round.
        /// </summary>
        /// <param name="round">The current round number.</param>
        public void UpdateRobotStatusByRound(int round)
        {
            foreach (var robot in _robots.Values)
            {
                robot.UpdateByRound(round);
            }
        }

        public bool IsRobotActive(Guid robotId)
        {
            return _robots[robotId].IsActive;
        }

        public bool IsRobotHealthy(Guid robotId)
        {
            return _robots[robotId].IsHealthy;
        }

        public void RepairRobot(Guid robotId)
        {
            _robots[robotId].IsHealthy = true;
        }

        /// <summary>
        /// Calculates and returns the total points from all active robots' workload.
        /// </summary>
        /// <returns>The total points from active robots' workload.</returns>
        public int GetPointsFromActiveRobots()
        {
            return _robots.Values.Where(robot => robot.IsActive).Sum(robot => robot.PerformWorkload());
        }

        /// <summary>
        /// Generates robots with random activation and deactivation rounds and adds them to the registry.
        /// </summary>
        /// <param name="robotCount">The number of robots to generate.</param>
        /// <param name="activationUpper">The upper bound for activation round generation.</param>
        /// <param name="deactivationUpper">The upper bound for deactivation round generation.</param>
        private void _GenerateRobots(int robotCount, int activationUpper, int deactivationUpper)
        {
            var random = new Random();
            for (var i = 0; i < robotCount; i++)
            {
                var activationRound = random.Next(1, activationUpper + 1);
                var deactivationRound = random.Next(activationUpper, deactivationUpper + 1);
                var robot = Robot.Create(activationRound, deactivationRound);

                _robots.Add(robot.Id, robot);
                _robotIds.Add(robot.Id);
            }
        }
    }
}
