﻿public class Flow
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

        var frontPositions = me.GetFrontPositions(attackDistance);
        var isAnyoneInAttachRange = frontPositions.Any(x => x.GetPlayerInPosition(state) != null);
        if (isAnyoneInAttachRange)
        {
            Visual.AddMessageLine("Someone in my attack range");
            return "T";
        }
        else
            Visual.AddMessageLine("No one in my attack range");

        var canMoveForward = me.CanMoveForward(model.Arena.Dims, otherPlayers);
        Visual.AddMessageLine($"I can {(canMoveForward ? "" : "NOT ")}move forward");

        var leftPositions = me.GetLeftPositions(attackDistance);
        var leftPlayers = leftPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isLeftPlayerFacingMe = leftPlayers.Any(x => x.IsFacing(me));
        if (isLeftPlayerFacingMe)
            Visual.AddMessageLine("Someone facing me in left");


        var rightPositions = me.GetRightPositions(attackDistance);
        var rightPlayers = rightPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isRightPlayerFacingMe = rightPlayers.Any(x => x.IsFacing(me));
        if (isRightPlayerFacingMe)
            Visual.AddMessageLine("Someone facing me in right");

        var backPositions = me.GetBackPositions(attackDistance);
        var backPlayers = backPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isBackPlayerFacingMe = backPlayers.Any(x => x.IsFacing(me));
        if (isBackPlayerFacingMe)
            Visual.AddMessageLine("Someone facing me in back");

        var inAttackRange = isBackPlayerFacingMe || isLeftPlayerFacingMe || isRightPlayerFacingMe;
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
            if (isLeftPlayerFacingMe)
                return "R";
            if (isRightPlayerFacingMe)
                return "L";
            if (leftPlayers.Count() > 0)
                return "L";
            if (rightPlayers.Count() > 0)
                return "R";
            return me.MoveArenaCenter(model.Arena.Dims);
        }

        var isAnyoneInMyFrontAttachRange = me.IsAnyoneInAttachRange(otherPlayers, attackDistance + 1);
        if (isAnyoneInMyFrontAttachRange)
        {
            Visual.AddMessageLine("Hunt front player");
            return "F";
        }

        return "F";
    }
}
