using UnityEngine;

public class NextLevel : InteractableObject
{
    [SerializeField] private SceneController.SceneName nextScene;

    protected override void Interact()
    {
        base.Interact();
        VariableController variable = ic.GetComponent<VariableController>();
        variable.OffsetLevel(1);
        ic.GetComponent<SceneController>().ChangeScene(variable.GetLevel() == 3 ? SceneController.SceneName.Credits : nextScene);
    }
}
