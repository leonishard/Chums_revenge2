using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;   // assign Header
    public TextMeshProUGUI contentField;  // assign Content
    public LayoutElement layoutElement;
    public int characterWrapLimit = 30;   // max characters before layout expands

    /// <summary>
    /// Set the tooltip content
    /// </summary>
    public void SetText(string header, string content)
    {
        if (headerField != null) headerField.text = header;
        if (contentField != null) contentField.text = content;

        // Enable LayoutElement if text is too long
        if (layoutElement != null)
            layoutElement.enabled = (header.Length > characterWrapLimit || content.Length > characterWrapLimit);
    }
}
