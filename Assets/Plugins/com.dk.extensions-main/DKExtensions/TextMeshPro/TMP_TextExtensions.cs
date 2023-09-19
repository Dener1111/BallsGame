public static partial class TMP_TextExtensions
{
    public static void SetText(this TMPro.TMP_Text tmpText, object obj)
    {
        tmpText.SetText(obj.ToString());
    }

    public static void Clear(this TMPro.TMP_Text tmpText)
    {
        tmpText.text = "";
    }
}
