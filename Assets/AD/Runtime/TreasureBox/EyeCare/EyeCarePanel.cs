using UnityEngine;
using UnityEngine.UI;

namespace AD.Runtime.TreasureBox.EyeCare
{
    public class EyeCarePanel : MonoBehaviour
    {
        [SerializeField] private Button confirmBtn;

        // Start is called before the first frame update
        void Start()
        {
            confirmBtn.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        private void OnDestroy()
        {
            confirmBtn.onClick.RemoveAllListeners();
        }
    }
}