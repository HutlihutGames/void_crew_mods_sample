using UnityEngine;

namespace VC.Common
{
    public class VoidCrewAsset : MonoBehaviour
    {
        [SerializeField][HideInInspector] private string assetGuid;
        public string AssetGuid => assetGuid;

#if UNITY_EDITOR
        public void SetAssetGUID(string guid)
        {
            assetGuid = guid;
        }
#endif
    }
}