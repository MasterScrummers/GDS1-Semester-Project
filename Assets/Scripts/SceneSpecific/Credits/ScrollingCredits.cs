using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScrollingCredits : MonoBehaviour
{
    [System.Serializable]
    private class CreditPart
    {
        [System.Serializable]
        private class CreditPeople
        {
            [SerializeField] private string name;
            [SerializeField] private string link;
            [SerializeField] private bool isLink = false;

            public string Name { get => name; set => name = value; }
            public string Link { get => link; set => link = value; }
            public bool IsLink { get => isLink; set => isLink = value; }
        }

        [SerializeField] private string heading;
        [SerializeField] private CreditPeople[] people;

        public string Heading { get => heading; set => heading = value; }
        public string[] GetPeople()
        {
            string[] contributors = new string[people.Length];
            for (int i = 0; i < people.Length; i++)
            {
                CreditPeople person = people[i];
                contributors[i] = person.IsLink ? "<link=" + person.Link + "><u>" + person.Name + "</u></link>" : person.Name;
            }
            return contributors;
        }
    }

    [SerializeField] private string gameName;
    private RectTransform rect;
    private Vector2 startPos;
    private TextMeshProUGUI text;
    [SerializeField] private Vector2 speed = new Vector3(0, 1);
    [SerializeField] private CreditPart[] credits;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
        text.text = "<size=120%>" + gameName + "</size>";

        foreach(CreditPart part in credits)
        {
            AddToCredits(part.Heading, part.GetPeople());
        }

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, text.preferredHeight);
        startPos = rect.anchoredPosition;
    }

    private void AddToCredits(string heading, string[] names)
    {
        text.text += "\n\n" + heading;
        foreach(string name in names)
        {
            text.text += "\n<size=80%>" + name + "</size>";
        }
    }

    void Update()
    {
        rect.Translate(speed * Time.deltaTime);
        if (rect.anchoredPosition.y > text.preferredHeight)
        {
            rect.anchoredPosition = startPos;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        int index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, Camera.main);
        if (index == -1)
        {
            return;
        }

        Application.OpenURL(text.textInfo.linkInfo[index].GetLinkID());
    }
}
