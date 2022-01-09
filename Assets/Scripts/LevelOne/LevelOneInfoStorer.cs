using DefaultNamespace;

namespace LevelOne
{
    public class LevelOneInfoStorer : NextStageInfoStorer<LevelOneInfoSetterScript>
    {
        public LevelOnePreviousStageInformation CastedInfo
        {
            get => information as LevelOnePreviousStageInformation;
            set => information = value;
        }

        public static LevelOneInfoStorer CastedSingleton => Instance == null ? null : Instance as LevelOneInfoStorer;

        protected override void Start()
        {
            base.Start();
            if (setter != null) return;
            information = new LevelOnePreviousStageInformation();
        }
        
        

    }
}