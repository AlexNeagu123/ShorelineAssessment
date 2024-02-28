namespace RobotWorkflow
{
    /// <summary>
    /// Represents a command center responsible for coordinating the workflow of robots and controllers in each round.
    /// The total score of all the round it's also computed here.
    /// </summary>
    public class CommandCenter
    {
        public int RoundCount { get; }
        public int RobotCount { get; }
        public int ControllerCount { get; }
        private int RobotsPerController => RobotCount / ControllerCount;

        private readonly RobotRegistry _robotRegistry;
        private readonly List<Controller> _controllers = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCenter"/> class.
        /// </summary>
        /// <param name="roundCount">The total number of rounds to be executed.</param>
        /// <param name="robotCount">The total number of robots in the system.</param>
        /// <param name="controllerCount">The total number of controllers managing the robots.</param>
        public CommandCenter(int roundCount, int robotCount, int controllerCount)
        {
            RoundCount = roundCount;
            RobotCount = robotCount;
            ControllerCount = controllerCount;
            _robotRegistry = RobotRegistry.GetInstance();
            _GenerateControllers();
        }

        /// <summary>
        /// Executes the workflow for every round and computes the total score.
        /// Firstly, it updates the status of all robots based on the current round.
        /// Then, all the active robots perform their workload.
        /// After that, the controllers perform their round of interactions with the robots.
        /// </summary>
        /// <returns>The total points accumulated during the execution.</returns>
        public int Run()
        {
            var totalPoints = 0;
            for (var round = 1; round <= RoundCount; round++)
            {
                _robotRegistry.UpdateRobotStatusByRound(round);
                var roundPoints = _robotRegistry.GetPointsFromActiveRobots();
                roundPoints += _controllers.Sum(controller => controller.PerformRound());
                totalPoints += roundPoints;
                Console.WriteLine($"Round #{round}. {roundPoints} generated..");
            }
            return totalPoints;
        }

        /// <summary>
        /// Generates controllers and assigns robots to them.
        /// Disjoint, equally sized segments of robot IDs are assigned to each controller for further interaction.
        /// </summary>
        private void _GenerateControllers()
        {
            for (var i = 0; i < ControllerCount; i++)
            {
                var controller = new Controller();
                foreach (var robotId in _robotRegistry.GetConsecutiveRobots(i * RobotsPerController,
                            i == RobotCount ? RobotCount : (i + 1) * RobotsPerController))
                {
                    controller.AssignRobot(robotId);
                }
                _controllers.Add(controller);
            }
        }
    }
}
