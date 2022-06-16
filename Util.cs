public class Util { }
record Position(int X, int Y);
record Range(Position Start, Position End);
static class Extension
{
    public static bool HasPlayer(this Position pos, Dictionary<string, PlayerState> State)
    {
        return pos.GetPlayerInPosition(State) != null;
    }
    public static bool IsFacing(this PlayerState player, PlayerState target)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return player.X == target.X && player.Y > target.Y;
            case "E":
                return player.X < target.X && player.Y == target.Y;
            case "W":
                return player.X > target.X && player.Y == target.Y;
            case "S":
                return player.X == target.X && player.Y < target.Y;
            default:
                break;
        }
        return false;
    }
    public static bool LeftIsWall(this PlayerState player, List<int> Dims)
    {
        var dimX = Dims[0] - 1;
        var dimY = Dims[1] - 1;
        switch (player.Direction.ToUpper())
        {
            case "N":
                return player.X == 0;
            case "E":
                return player.Y == 0;
            case "W":
                return player.Y == dimY;
            case "S":
                return player.X == dimX;
            default:
                break;
        }
        return false;
    }
    public static bool RightIsWall(this PlayerState player, List<int> Dims)
    {
        var dimX = Dims[0] - 1;
        var dimY = Dims[1] - 1;
        switch (player.Direction.ToUpper())
        {
            case "N":
                return player.X == dimX;
            case "E":
                return player.Y == dimY;
            case "W":
                return player.Y == 0;
            case "S":
                return player.X == 0;
            default:
                break;
        }
        return false;
    }
    public static bool IsWall(this Position pos, List<int> Dims)
    {
        var dimX = Dims[0] - 1;
        var dimY = Dims[1] - 1;
        return pos.X == 0 || pos.X == dimX || pos.Y == 0 || pos.Y == dimY;
    }
    public static IEnumerable<Position> GetLeftPositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        for (var i = distance; i >= 1; i--)
        {
            poss.Add(player.GetLeftPosition(i));
        }
        return poss;
    }
    public static IEnumerable<Position> GetRightPositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        for (var i = distance; i >= 1; i--)
        {
            poss.Add(player.GetRightPosition(i));
        }
        return poss;
    }
    public static IEnumerable<Position> GetFrontPositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        for (var i = distance; i >= 1; i--)
        {
            poss.Add(player.GetFrontPosition(i));
        }
        return poss;
    }
    public static IEnumerable<Position> GetBackPositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        for (var i = distance; i >= 1; i--)
        {
            poss.Add(player.GetBackPosition(i));
        }
        return poss;
    }
    public static IEnumerable<Position> GetVisablePositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        poss.Add(player.GetLeftPosition(distance));
        poss.Add(player.GetRightPosition(distance));
        return poss;
    }
    public static IEnumerable<Position> GetAllVisablePositions(this PlayerState player, int distance)
    {
        var poss = new List<Position>();
        for (var i = distance; i >= 1; i--)
        {
            poss.AddRange(player.GetVisablePositions(i));
        }
        return poss;
    }
    public static Position GetPosition(this PlayerState player) => new(player.X, player.Y);
    public static Position GetFrontPosition(this PlayerState player, int distance = 1)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Position(player.X, player.Y - distance);
            case "E":
                return new Position(player.X + distance, player.Y);
            case "W":
                return new Position(player.X - distance, player.Y);
            case "S":
                return new Position(player.X, player.Y + distance);
            default:
                break;
        }
        return new Position(player.X, player.Y);
    }
    public static Position GetBackPosition(this PlayerState player, int distance = 1)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Position(player.X, player.Y + distance);
            case "E":
                return new Position(player.X - distance, player.Y);
            case "W":
                return new Position(player.X + distance, player.Y);
            case "S":
                return new Position(player.X, player.Y - distance);
            default:
                break;
        }
        return new Position(player.X, player.Y);
    }
    public static Position GetLeftPosition(this PlayerState player, int distance = 1)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Position(player.X - distance, player.Y);
            case "E":
                return new Position(player.X, player.Y - distance);
            case "W":
                return new Position(player.X, player.Y + distance);
            case "S":
                return new Position(player.X + distance, player.Y);
            default:
                break;
        }
        return new Position(player.X, player.Y);
    }
    public static Position GetRightPosition(this PlayerState player, int distance = 1)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Position(player.X + distance, player.Y);
            case "E":
                return new Position(player.X, player.Y + distance);
            case "W":
                return new Position(player.X, player.Y - distance);
            case "S":
                return new Position(player.X - distance, player.Y);
            default:
                break;
        }
        return new Position(player.X, player.Y);
    }
    public static string MoveArenaCenter(this PlayerState player, List<int> Dims)
    {
        return player.MoveArenaCenter(Dims[0], Dims[1]);
    }
    public static string MoveArenaCenter(this PlayerState player, int dimX, int dimY)
    {
        var centerX = dimX / 2;
        var centerY = dimY / 2;
        switch (player.Direction.ToUpper())
        {
            case "N":
                return player.X > centerX ? "L" : "R";
            case "E":
                return player.Y > centerY ? "L" : "R";
            case "W":
                return player.Y > centerY ? "R" : "L";
            case "S":
                return player.X > centerX ? "R" : "L";
            default:
                break;
        }
        return "R";
    }
    public static Range GetAttackRange(this PlayerState player, int attackDistance)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Range(new Position(player.X, player.Y - 1), new Position(player.X, player.Y - attackDistance));
            case "E":
                return new Range(new Position(player.X + 1, player.Y), new Position(player.X + attackDistance, player.Y));
            case "W":
                return new Range(new Position(player.X - 1, player.Y), new Position(player.X - attackDistance, player.Y));
            case "S":
                return new Range(new Position(player.X, player.Y + 1), new Position(player.X, player.Y + attackDistance));
            default:
                break;
        }
        return new Range(new Position(0, 0), new Position(0, 0));
    }
    public static bool IsWithinRange(this PlayerState player, Range range) => player.GetPosition().IsWithinRange(range);
    public static bool IsWithinRange(this Position pos, Range range)
    {
        var minX = Math.Min(range.Start.X, range.End.X);
        var maxX = Math.Max(range.Start.X, range.End.X);
        var minY = Math.Min(range.Start.Y, range.End.Y);
        var maxY = Math.Max(range.Start.Y, range.End.Y);
        return minX <= pos.X && pos.X <= maxX && minY <= pos.Y && pos.Y <= maxY;
    }
    public static bool IsAnyoneInAttachRange(this PlayerState player, IEnumerable<PlayerState> otherPlayers, int attackDistance)
    {
        return player.GetPlayersInAttackRange(otherPlayers, attackDistance).FirstOrDefault() != null;
    }
    public static IEnumerable<PlayerState> GetPlayersInAttackRange(this PlayerState player, IEnumerable<PlayerState> otherPlayers, int attackDistance)
    {
        var attackRange = player.GetAttackRange(attackDistance);
        return otherPlayers.Where(x => x.IsWithinRange(attackRange));
    }
    public static PlayerState? GetPlayerInPosition(this Position pos, Dictionary<string, PlayerState> State)
    {
        return State.Values.FirstOrDefault(x => x.X == pos.X && x.Y == pos.Y);
    }
    public static bool IsSafePosition(this Position pos, IEnumerable<PlayerState> otherPlayers, int attackDistance)
    {
        return otherPlayers.All(x => !pos.IsWithinRange(x.GetAttackRange(attackDistance)));
    }
    public static bool CanMoveForward(this PlayerState player, List<int> Dims, IEnumerable<PlayerState> otherPlayers)
    {
        var dimX = Dims[0];
        var dimY = Dims[1];
        var result = true;
        var blocker = "wall";
        switch (player.Direction)
        {
            case "N":
                result = player.Y != 0;
                break;
            case "E":
                result = player.X != dimX - 1;
                break;
            case "W":
                result = player.X != 0;
                break;
            case "S":
                result = player.Y != dimY - 1;
                break;
            default:
                break;
        }
        if (result)
        {
            result = otherPlayers.All(x => x.GetPosition() != player.GetFrontPosition());
            if (!result)
                blocker = "player";
        }
        if (!result)
            Console.WriteLine($"Front blocker [{blocker}]");
        return result;
    }
}
class Visual
{
    public static string messageCache = "";
    public static void AddMessageLine(string message = "") => messageCache += message + Environment.NewLine;
    public static void AddMessage(string message = "") => messageCache += message;
    public static void PrintMessage()
    {
        Task.Run(() =>
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(messageCache);
            messageCache = "";
        });
    }
    public static void PrintMessage(ArenaUpdate model, int attackDistance)
    {
        var dimX = model.Arena.Dims[0];
        var dimY = model.Arena.Dims[1];
        var arena = new string[dimX, dimY];
        var direction = new string[] { "N", "E", "W", "S" };
        var meSign = new string[] { "▲", "▶", "◀", "▼" };
        var othersSign = new string[] { "△", "▷", "◁", "▽" };

        var state = model.Arena.State;
        var me = state[model.Links.Self.Href];
        var otherPlayers = state.Values.Where(x => x != me).ToList();
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                arena[i, j] = new Position(i, j).IsSafePosition(otherPlayers, attackDistance) ? "." : "*";
            }
        }
        foreach (var player in otherPlayers)
        {
            arena[player.X, player.Y] = othersSign[Array.IndexOf(direction, player.Direction)];
        }
        arena[me.X, me.Y] = meSign[Array.IndexOf(direction, me.Direction)];

        for (int j = 0; j < dimY; j++)
        {
            if (j == 0)
            {
                AddMessage("    ");
                for (int i = 0; i < dimX; i++)
                {
                    AddMessage(i.ToString().PadLeft(2, '0') + " ");
                }
                AddMessageLine();
            }
            AddMessage(j.ToString().PadLeft(2, '0') + " ");
            for (int i = 0; i < dimX; i++)
            {
                AddMessage("  " + arena[i, j]);
            }
            AddMessageLine();
        }
    }
    public static void PrintConsole(ArenaUpdate model, int attackDistance)
    {
        var dimX = model.Arena.Dims[0];
        var dimY = model.Arena.Dims[1];
        var arena = new string[dimX, dimY];
        var direction = new string[] { "N", "E", "W", "S" };
        var meSign = new string[] { "▲", "▶", "◀", "▼" };
        var othersSign = new string[] { "△", "▷", "◁", "▽" };

        var state = model.Arena.State;
        var me = state[model.Links.Self.Href];
        var otherPlayers = state.Values.Where(x => x != me).ToList();
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                arena[i, j] = new Position(i, j).IsSafePosition(otherPlayers, attackDistance) ? "." : "*";
            }
        }
        foreach (var player in otherPlayers)
        {
            arena[player.X, player.Y] = othersSign[Array.IndexOf(direction, player.Direction)];
        }
        arena[me.X, me.Y] = meSign[Array.IndexOf(direction, me.Direction)];

        for (int j = 0; j < dimY; j++)
        {
            if (j == 0)
            {
                Console.Write("\t");
                for (int i = 0; i < dimX; i++)
                {
                    Console.Write(i.ToString().PadLeft(2, '0') + "\t");
                }
                Console.WriteLine();
            }
            Console.Write(j.ToString().PadLeft(2, '0') + " ");
            for (int i = 0; i < dimX; i++)
            {
                Console.Write("  " + arena[i, j]);
            }
            Console.WriteLine();
        }
    }
}