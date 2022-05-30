using UnityEngine;

public class NextLevel : InteractableObject
{
    [SerializeField] private SceneController.SceneName nextScene;

    protected override void Interact()
    {
        base.Interact();
        ic.GetComponent<SceneController>().ChangeScene(nextScene);
        ic.GetComponent<VariableController>().OffsetLevel(1);
    }
}
