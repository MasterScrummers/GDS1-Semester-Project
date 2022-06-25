using UnityEngine;

public class NextLevel : InteractableObject
{
    [SerializeField] private SceneController.SceneName nextScene;

    protected override void Interact()
    {
        base.Interact();
        VariableController variable = ic.GetComponent<VariableController>();
        ic.GetComponent<SceneController>().ChangeScene(variable.finalLevel ? SceneController.SceneName.Credits : nextScene);
    }
}
