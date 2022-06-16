public class Flow
{
    static readonly int attackDistance = 3;
    internal static string Process(ArenaUpdate model)
    {
        Console.WriteLine(nameof(Process));
        Visual.PrintConsole(model,attackDistance);
        var commands = new List<string> { "F", "R", "L", "T" };
        var state = model.Arena.State;
        if (!state.ContainsKey(model.Links.Self.Href)) { Console.WriteLine("Cannot find me"); return "T"; }

        var me = state[model.Links.Self.Href];
        var otherPlayers = state.Values.Where(x => x != me).ToList();
        var myFront = me.GetFrontPosition();

        if (me.GetPlayersInAttackRange(otherPlayers, attackDistance).Any(x => x.WasHit != true))
        {
            Console.WriteLine("Someone in my attack range");
            return "T";
        }
        else
            Console.WriteLine("No one in my attack range");

        var canMoveForward = me.CanMoveForward(model.Arena.Dims, otherPlayers);
        Console.WriteLine($"I can {(canMoveForward ? "" : "NOT ")}move forward");

        var inAttackRange = !me.GetPosition().IsSafePosition(otherPlayers, attackDistance);
        Console.WriteLine($"I'm {(inAttackRange ? "" : "NOT ")}in attack range");
        if (inAttackRange)
        {
            var canEscape = false;
            if (canMoveForward)
            {
                var frontInAttackRange = !me.GetFrontPosition().IsSafePosition(otherPlayers, attackDistance);
                Console.WriteLine($"My front {(frontInAttackRange ? "" : "NOT ")}in attack range");
                if (!frontInAttackRange)
                    canEscape = true;
            }
            Console.WriteLine($"I can {(canEscape ? "" : "NOT ")}escape");
            if (canEscape)
                return "F";
        }



        if (!canMoveForward) commands.Remove("F");

        return "T";
    }
}
