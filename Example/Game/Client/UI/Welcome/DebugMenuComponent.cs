using System;
using Godot;
using Framework;
using Framework.Game;
using Framework.Network;
namespace Shooter.Client.UI.Welcome
{
    public partial class DebugMenuComponent : CanvasLayer, IChildComponent<GameLogic>
    {

        public GameLogic BaseComponent { get; set; }

        [Export]
        public NodePath LogMessagePath { get; set; }

        [Export]
        public NodePath FPSPath { get; set; }

        [Export]
        public NodePath PingPath { get; set; }

        [Export]
        public NodePath PackageLoosePath { get; set; }

        [Export]
        public NodePath PackageDataPath { get; set; }

        [Export]
        public NodePath IdleTimePath { get; set; }

        [Export]
        public NodePath PhysicsTimePath { get; set; }

        [Export]
        public float updateTimeLabel = 0.5f;

        private float currentUpdateTime = 0f;

        public override void _EnterTree()
        {
            Logger.OnLogMessage += (string message) =>
        {
            this.GetNode<Label>(this.LogMessagePath).Text = message;
        };
        }


        public override void _Process(float delta)
        {
            base._Process(delta);

            if (currentUpdateTime >= updateTimeLabel)
            {
                var componnent = this.BaseComponent as IGameLogic;
                var netService = componnent.Services.Get<ClientNetworkService>();
                if (netService != null)
                {
                    this.GetNode<Label>(this.PackageDataPath).Text = "Send: " + (netService.BytesSended / 1000) + "kB/s, " + "Rec: " + (netService.BytesReceived / 1000) + "kB/s";
                    this.GetNode<Label>(this.PackageLoosePath).Text = netService.PackageLoss + " (" + netService.PackageLossPercent + "%" + ")";
                    this.GetNode<Label>(this.PingPath).Text = netService.Ping.ToString() + "ms";
                }

                this.GetNode<Label>(this.FPSPath).Text = Engine.GetFramesPerSecond().ToString();

                this.GetNode<Label>(this.IdleTimePath).Text = Math.Round(this.GetProcessDeltaTime() * 1000, 6) + "ms";
                this.GetNode<Label>(this.PhysicsTimePath).Text = Math.Round(this.GetPhysicsProcessDeltaTime() * 1000, 6) + "ms";

                currentUpdateTime = 0f;
            }
            else
            {
                currentUpdateTime += delta;
            }
        }
    }
}
