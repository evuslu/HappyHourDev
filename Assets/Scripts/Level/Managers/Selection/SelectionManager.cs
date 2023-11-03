namespace Evu.Level{

    using UnityEngine;

    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] LayerMask layerMaskSelectibles;
        
        private Camera cam = null;

        private PlayerCharacterController selectedCharacter = null;

        private void Awake()
        {
            cam = Camera.main;

            InputManager.OnClick += OnClick;
        }

        private void OnDestroy()
        {
            InputManager.OnClick -= OnClick;
        }

        private void OnClick()
        {
            if (!GameManager.Instance.IsInputActive)
                return;

            Ray ray = cam.ScreenPointToRay(InputManager.Instance.TouchPos);

            RaycastHit hitInfo;

            if (!Physics.Raycast(ray, out hitInfo, 100f, layerMaskSelectibles.value))
                return;

            GameObject go = hitInfo.collider.gameObject;

            PlayerCharacterController character = go.GetComponent<PlayerCharacterController>();

            if (character != null)
            {
                if (!character.HasStateAuthority)
                    return;//network player

                SelectCharacter(character);
                
                return;
            }

            //ground hit
            if (selectedCharacter == null)
                return;

            selectedCharacter.MoveToTarget(hitInfo.point);
        }

        private void DeselectCharacter()
        {
            if (selectedCharacter == null)
                return;

            selectedCharacter.SetSelectionIndcatorActive(false);
        }

        private void SelectCharacter(PlayerCharacterController character)
        {
            DeselectCharacter();

            selectedCharacter = character;

            selectedCharacter.SetSelectionIndcatorActive(true);
        }
    }

}