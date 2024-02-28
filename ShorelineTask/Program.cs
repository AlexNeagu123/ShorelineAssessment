using RobotWorkflow;

var commandCenter = new CommandCenter(Constants.RoundCount, Constants.RobotCount, Constants.ControllerCount);
var totalPoints = commandCenter.Run();
Console.WriteLine($"Total number of points obtained: {totalPoints}");