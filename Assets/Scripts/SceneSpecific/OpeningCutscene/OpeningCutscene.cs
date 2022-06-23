using System.Collections.Generic;
using UnityEngine;

public class OpeningCutscene : MonoBehaviour
{
    private int flags = 0;
    private Dictionary<string, GameObject> children;
    private VectorLerper lerper;
    private VectorLerper lerper2;
    [SerializeField] Animator kirbyAnim;

    void Start()
    {
        children = new Dictionary<string, GameObject>();
        foreach (Transform child in DoStatic.GetChildren(transform))
        {
            children.Add(child.name, child.gameObject);
        }

        lerper = new(children["CutsceneKirby"].transform.position, new Vector2(-6, -5.5f), 3);
        lerper2 = new();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        lerper.Update(delta);
        lerper2.Update(delta);

        switch(flags)
        {
            case 0:
                children["CutsceneKirby"].transform.position = lerper.currentValue;
                if (!lerper.isLerping)
                {
                    flags++;
                    kirbyAnim.SetTrigger("Next");
                    lerper.SetValues(Vector2.one, new Vector2(3, 3), 1f);
                }
                return;

            case 1:
                children["Cube"].transform.localScale = lerper.currentValue;
                if (!lerper.isLerping)
                {
                    flags++;
                    kirbyAnim.SetTrigger("Next");
                    children["CutsceneKirby"].AddComponent<Rotate>();
                    Transform kirby = children["CutsceneKirby"].transform;
                    lerper.SetValues(kirby.position, children["Cube"].transform.position, 1f);
                    lerper2.SetValues(kirby.localScale, Vector2.zero, 1f);
                }
                return;

            case 2:
                Transform kirby2 = children["CutsceneKirby"].transform;
                kirby2.position = lerper.currentValue;
                kirby2.localScale = lerper2.currentValue;
                if (!lerper.isLerping)
                {
                    SceneController sceneController = DoStatic.GetGameController<SceneController>();
                    sceneController.ChangeScene(sceneController.GetComponent<VariableController>().GetScene());
                }
                return;
        }
    }
}
