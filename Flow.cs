public class Flow
{
    static readonly int attackDistance = 3;
    internal static string Process(ArenaUpdate model)
    {
        Visual.AddMessageLine(nameof(Process));
        Visual.PrintMessage(model, attackDistance);
        var commands = new List<string> { "F", "R", "L", "T" };
        var state = model.Arena.State;
        var dims = model.Arena.Dims;
        if (!state.ContainsKey(model.Links.Self.Href)) { Visual.AddMessageLine("Cannot find me"); return "T"; }

        var me = state[model.Links.Self.Href];
        var otherPlayers = state.Values.Where(x => x != me).ToList();

        var canMoveForward = me.CanMoveForward(dims, otherPlayers);
        Visual.AddMessageLine($"I can {(canMoveForward ? "" : "NOT ")}move forward");
        var frontPositions = me.GetFrontPositions(attackDistance);
        var isAnyoneInAttachRange = frontPositions.Any(x => x.GetPlayerInPosition(state) != null);

        if (me.IsInCorner(dims))
        {
            Visual.AddMessageLine("I'm in corner");
            if (canMoveForward)
                return "F";
            Visual.AddMessageLine("Cannot move forward");
            if (me.RightIsWall(dims))
            {
                Visual.AddMessageLine("Wall in right");
                var leftPosition = me.GetLeftPosition(attackDistance);
                if (leftPosition.HasPlayer(state))
                {
                    Visual.AddMessageLine("Other player in left");
                    if (isAnyoneInAttachRange)
                        return "T";
                }
                return "L";
            }
            if (me.LeftIsWall(dims))
            {
                Visual.AddMessageLine("Wall in left");
                var rightPosition = me.GetRightPosition(attackDistance);
                if (rightPosition.HasPlayer(state))
                {
                    Visual.AddMessageLine("Other player in right");
                    if (isAnyoneInAttachRange)
                        return "T";
                }
                return "R";
            }
        }

        var numberPlayersFacingMe = 0;

        var leftPositions = me.GetLeftPositions(attackDistance);
        var leftPlayers = leftPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isLeftPlayerFacingMe = leftPlayers.Any(x => x.IsFacing(me));
        if (isLeftPlayerFacingMe)
        {
            numberPlayersFacingMe++;
            Visual.AddMessageLine("Someone facing me in left");
        }


        var rightPositions = me.GetRightPositions(attackDistance);
        var rightPlayers = rightPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isRightPlayerFacingMe = rightPlayers.Any(x => x.IsFacing(me));
        if (isRightPlayerFacingMe)
        {
            numberPlayersFacingMe++;
            Visual.AddMessageLine("Someone facing me in right");
        }

        var backPositions = me.GetBackPositions(attackDistance);
        var backPlayers = backPositions.Select(x => x.GetPlayerInPosition(state)).OfType<PlayerState>().ToList();
        var isBackPlayerFacingMe = backPlayers.Any(x => x.IsFacing(me));
        if (isBackPlayerFacingMe)
        {
            numberPlayersFacingMe++;
            Visual.AddMessageLine("Someone facing me in back");
        }

        Visual.AddMessageLine($"{numberPlayersFacingMe} player facing me");

        var inAttackRange = isBackPlayerFacingMe || isLeftPlayerFacingMe || isRightPlayerFacingMe;
        Visual.AddMessageLine($"I'm {(inAttackRange ? "" : "NOT ")}in attack range");
        var frontInAttackRange = !me.GetFrontPosition().IsSafePosition(otherPlayers, attackDistance);
        Visual.AddMessageLine($"My front {(frontInAttackRange ? "" : "NOT ")}in attack range");

        var frontIsWall = me.IsFacingWall(dims);
        if (!canMoveForward && isLeftPlayerFacingMe && isRightPlayerFacingMe && isBackPlayerFacingMe)
        {
            if (frontIsWall)
                return me.MoveArenaCenter(dims);
            else
                return "T";
        }

        if (inAttackRange)
        {
            var canEscape = false;
            if (canMoveForward)
            {
                if (!frontInAttackRange)
                    canEscape = true;
                if (numberPlayersFacingMe > 1)
                    canEscape = true;
            }
            Visual.AddMessageLine($"I can {(canEscape ? "" : "NOT ")}escape");
            if (canEscape)
                return "F";


            if (me.RightIsWall(dims))
                return "L";
            if (me.LeftIsWall(dims))
                return "R";

            if (isLeftPlayerFacingMe)
                return "R";
            if (isRightPlayerFacingMe)
                return "L";

            if (!isLeftPlayerFacingMe && leftPlayers.Count > 0)
                return "L";
            if (!isRightPlayerFacingMe && rightPlayers.Count > 0)
                return "R";

            if (isLeftPlayerFacingMe && isRightPlayerFacingMe && canMoveForward)
                return "F";

            return me.MoveArenaCenter(dims);
        }

        if (isAnyoneInAttachRange)
        {
            Visual.AddMessageLine("Someone in my attack range");
            return "T";
        }
        else
            Visual.AddMessageLine("No one in my attack range");

        var isAnyoneInMyFrontAttachRange = me.IsAnyoneInAttachRange(otherPlayers, attackDistance + 1);
        if (isAnyoneInMyFrontAttachRange)
        {
            Visual.AddMessageLine("Hunt front player");
            return "F";
        }
        if (leftPlayers.Count > 0)
        {
            Visual.AddMessageLine("Hunt left player");
            return "L";
        }
        if (rightPlayers.Count > 0)
        {
            Visual.AddMessageLine("Hunt right player");
            return "R";
        }

        if (canMoveForward && !me.GetFrontPosition().IsNearWall(dims) && !frontInAttackRange)
        {
            Visual.AddMessageLine("Pactrol");
            return "F";
        }
        Visual.AddMessageLine("Center");
        return me.MoveArenaCenter(dims);
    }
}
