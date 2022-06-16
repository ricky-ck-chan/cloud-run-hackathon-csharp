﻿public class Util { }
record Position(int X, int Y);
record Range(Position Start, Position End);
static class Extension
{
    public static Position GetPosition(this PlayerState player) => new(player.X, player.Y);
    public static Position GetFrontPosition(this PlayerState player)
    {
        switch (player.Direction.ToUpper())
        {
            case "N":
                return new Position(player.X, player.Y - 1);
            case "E":
                return new Position(player.X + 1, player.Y);
            case "W":
                return new Position(player.X - 1, player.Y);
            case "S":
                return new Position(player.X, player.Y + 1);
            default:
                break;
        }
        return new Position(player.X, player.Y);
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
                result = player.Y != dimX - 1;
                break;
            case "W":
                result = player.X != 0;
                break;
            case "S":
                result = player.X != dimY - 1;
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
                Console.Write("    ");
                for (int i = 0; i < dimX; i++)
                {
                    Console.Write(i.ToString().PadLeft(2, '0') + " ");
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