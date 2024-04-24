using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace cs2_xray
{
    // [MinimumApiVersion(160)]
    public class cs2_xray : BasePlugin
    {
        public override string ModuleName => "CS2-Xray";
        public override string ModuleDescription => "";
        public override string ModuleAuthor => "heartbreakhotel";
        public override string ModuleVersion => "0.0.1";

        public Dictionary<int, int> aostatus = new Dictionary<int, int>();

        public override void Load(bool hotReload)
        {
            Console.WriteLine($"{ModuleName} loaded successfully!");

            RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);

            RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        }

        public HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo @info)
        {
            CCSPlayerController player = @event.Userid;

            if (player.IsValid && !player.IsBot && !player.IsHLTV)
            {
                aostatus[player.Slot] = 0;
            }
            return HookResult.Continue;
        }

        public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo @info)
        {
            CCSPlayerController player = @event.Userid;

            if (player.IsValid && !player.IsBot && !player.IsHLTV)
            {
                aostatus.Remove(player.Slot);
            }
            return HookResult.Continue;
        }

        [RequiresPermissions("#css/admin")]
        [ConsoleCommand("css_xray", "Enable/disable xray")]
        [CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        public void OnXray(CCSPlayerController? player, CommandInfo command)
        {
            if (player!.IsValid && !player.IsBot)
            {
                if (aostatus[player.Slot] == 0)
                {
                    aostatus[player.Slot] = 1;
                    player.ExecuteClientCommand("r_aoproxy_show 1");
                }
                else
                {
                    aostatus[player.Slot] = 0;
                    player.ExecuteClientCommand("r_aoproxy_show 0");
                }
            }
        }

    }
}