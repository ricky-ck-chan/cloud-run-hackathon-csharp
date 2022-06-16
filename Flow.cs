public class Flow
{
    static readonly int attackDistance = 3;
    internal static string Process(ArenaUpdate model)
    {
        Visual.AddMessageLine(nameof(Process));
        Visual.PrintMessage(model, attackDistance);
        var commands = new List<string> { "F", "R", "L", "T" };
        var state = model.Arena.State;
        if (!state.ContainsKey(model.Links.Self.Href)) { Visual.AddMessageLine("Cannot find me"); return "T"; }

        var me = state[model.Links.Self.Href];
        var otherPlayers = state.Values.Where(x => x != me).ToList();

        var isAnyoneInAttachRange = me.IsAnyoneInAttachRange(otherPlayers, attackDistance);
        if (isAnyoneInAttachRange)
        {
            Visual.AddMessageLine("Someone in my attack range");
            return "T";
        }
        else
            Visual.AddMessageLine("No one in my attack range");

        var canMoveForward = me.CanMoveForward(model.Arena.Dims, otherPlayers);
        Visual.AddMessageLine($"I can {(canMoveForward ? "" : "NOT ")}move forward");

        var inAttackRange = !me.GetPosition().IsSafePosition(otherPlayers, attackDistance);
        Visual.AddMessageLine($"I'm {(inAttackRange ? "" : "NOT ")}in attack range");
        if (inAttackRange)
        {
            var canEscape = false;
            if (canMoveForward)
            {
                var frontInAttackRange = !me.GetFrontPosition().IsSafePosition(otherPlayers, attackDistance);
                Visual.AddMessageLine($"My front {(frontInAttackRange ? "" : "NOT ")}in attack range");
                if (!frontInAttackRange)
                    canEscape = true;
            }
            Visual.AddMessageLine($"I can {(canEscape ? "" : "NOT ")}escape");
            if (canEscape)
                return "F";
        }

        var isAnyoneInMyFrontAttachRange = me.IsAnyoneInAttachRange(otherPlayers, attackDistance + 1);
        if (isAnyoneInMyFrontAttachRange)
        {
            Visual.AddMessageLine("Hunt front player");
            return "F";
        }



        if (!canMoveForward) commands.Remove("F");

        return "T";
    }
}
