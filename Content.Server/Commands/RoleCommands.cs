using Content.Server.Administration;
using Content.Server.Players;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Commands.Roles;

[AdminCommand(AdminFlags.Admin)]
public sealed class UnlockAllRolesCommand : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    // Если IRoleUnlockManager не существует, используйте соответствующую систему вашей версии
    [Dependency] private readonly IRoleUnlockManager _roleUnlockManager = default!;

    public string Command => "unlockallroles";
    public string Description => "Unlocks all role requirements for specified player";
    public string Help => "Usage: unlockallroles <username>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError("Expected exactly one argument!");
            return;
        }

        if (!_playerManager.TryGetSessionByUsername(args[0], out var targetSession))
        {
            shell.WriteError($"Unable to find player with username {args[0]}");
            return;
        }

        var playerData = targetSession.ContentData();
        if (playerData == null)
        {
            shell.WriteError("Unable to get player data!");
            return;
        }

        var allRoles = _roleUnlockManager.GetAllPlayableRoles(); // Метод может называться иначе

        foreach (var role in allRoles)
        {
            _roleUnlockManager.SetRoleUnlocked(playerData, role, true); // Метод может отличаться
        }

        shell.WriteLine($"Unlocked all {allRoles.Count()} roles for player {args[0]}");
    }
}