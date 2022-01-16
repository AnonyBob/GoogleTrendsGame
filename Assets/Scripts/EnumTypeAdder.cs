using OwlAndJackalope.UX.Runtime.Data.Serialized.Enums;
using UnityEngine;

namespace GoogleTrends
{
    public static class EnumTypeAdder
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        public static void InitializeEnums()
        {
            SerializedDetailEnumCache.AddEnumType(20, new EnumDetailCreator<GameState>(x => (GameState)x));
        }
    }
}