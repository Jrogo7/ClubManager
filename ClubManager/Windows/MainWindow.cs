using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ClubManager.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private Configuration configuration;

    public MainWindow(Plugin plugin) : base(
        "Club Manager", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(250, 300),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
        this.configuration = plugin.Configuration;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        ImGui.BeginTabBar("Tabs");
        // Render Guests tab if selected 
        if (ImGui.BeginTabItem("Guests")) {

          // Render high level information 
          ImGui.Text(this.configuration.userInHouse ? 
            "You are in a " + TerritoryUtils.getHouseType(this.configuration.territory) + " in " + TerritoryUtils.getHouseLocation(this.configuration.territory) : 
            "You are not in a house.");

          // Clear guest list button 
          if (ImGui.Button("Clear Guest List")) {
            this.configuration.guests = new();
            this.configuration.Save();
          }

          // Draw Guests 
          ImGui.Text("Guests:");
          ImGui.BeginChild(1);
          ImGui.Indent(10);
          foreach (var guest in this.configuration.guests) {
            ImGui.Text(guest.Value.Name);
          }
          ImGui.Unindent(10);
          ImGui.EndChild();

          ImGui.EndTabItem();
        }
        // Render Settings Tab if selected 
        if (ImGui.BeginTabItem("Settings")) {
          // can't ref a property, so use a local copy
          var configValue = this.configuration.showChatAlerts;
          if (ImGui.Checkbox("Show chat alerts", ref configValue))
          {
              this.configuration.showChatAlerts = configValue;
              this.configuration.Save();
          }

          ImGui.EndTabItem();
        }
        ImGui.EndTabBar();
    }
}
