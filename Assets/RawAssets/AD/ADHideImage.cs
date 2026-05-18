namespace AD
{
    [UnityEngine.RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ADHideImage : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            image.enabled = false;
            image.color = UnityEngine.Color.clear;
        }
    }
}