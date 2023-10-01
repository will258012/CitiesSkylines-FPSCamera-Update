namespace FPSCamera.Configuration
{
    using CSkyL.Config;
    using System.Collections.Generic;
    using Offset = CSkyL.Transform.Offset;

    public class CamOffset : Base
    {
        private const string defaultPath = "FPSCameraOffset.xml";
        public static readonly CamOffset G = new CamOffset();  // G: Global config

        public CamOffset() : this(defaultPath) { }
        public CamOffset(string filePath) : base(filePath) { }

        public static CamOffset Load(string path = defaultPath) => Load<CamOffset>(path);

        public override void Assign(Base other)
        {
            if (other is CamOffset otherOffset) _offsets = otherOffset._offsets;
            else CSkyL.Log.Warn($"Config: cannot assign <{other.GetType().Name}> to <CamOffset>");
        }

        public Offset this[string key] {
            get {
                if (_offsets.TryGetValue(key, out var offset)) return offset.AsOffSet;
                return (_offsets[key] = _DefaultFor<CfOffset>()).AsOffSet;
            }
            set => _offsets[key].Assign(value);
        }

        protected override TConfig _DefaultFor<TConfig>()
        {
            if (typeof(TConfig) == typeof(CfOffset))
                return (TConfig) (object)
                       new CfOffset(new CfFloat(0f), new CfFloat(0f), new CfFloat(0f));
            return default;
        }

        private static CfOffset _CreateOffset(float forward, float up, float right = 0f,
                                       float yaw = 0f, float pitch = 0f)
        {
            var offset = new CfOffset(new CfFloat(0f), new CfFloat(0f), new CfFloat(0f));
            offset.Assign(new Offset(
                new CSkyL.Transform.LocalMovement { forward = forward, up = up, right = right },
                new CSkyL.Transform.DeltaAttitude(yaw, pitch)));
            return offset;
        }


        private Dictionary<string, CfOffset> _offsets = new Dictionary<string, CfOffset>
        {
            ["巴士"] = _CreateOffset(2.55f, .42f),
            ["生物燃料巴士"] = _CreateOffset(2.1f, .2f),
            ["校车"] = _CreateOffset(.75f, .3f),

            ["有轨电车"] = _CreateOffset(1.04f, .23f),
            ["Tram Middle"] = _CreateOffset(-6.24f, -1.32f, 1.82f, .6f, -3.6f),
            ["Tram End"] = _CreateOffset(-5f, -2.76f, 0f, 180f),

            ["地铁"] = _CreateOffset(4.35f, .76f),
            ["地铁乘客"] = _CreateOffset(.11f, -1.05f, 2.43f, -6.3f, -8f),
            ["单轨乘客"] = _CreateOffset(-.32f,-1.7f,1.2f),
            ["客运火车牵引机"] = _CreateOffset(3.84f, .9f),
            ["客运火车旅客"] = _CreateOffset(-2.44f, -.45f, 1.78f, -4.5f, -7.35f),
            ["货运火车牵引机"] = _CreateOffset(3.19f, .9f),
            ["货运火车"] = _CreateOffset(0f, -.59f),

            ["自行车"] = _CreateOffset(-3f, -.4f),
            ["私人电动交通工具"] = _CreateOffset(-2f, -.4f),

            ["露营车"] = _CreateOffset(0f, -2.2f),

            ["拖拉机"] = _CreateOffset(-.46f, .7f),
            ["Forest Forwarder 01"] = _CreateOffset(1.16f, .96f),
            ["Farm Truck 01"] = _CreateOffset(-1f, .5f),
        };
    }
}
